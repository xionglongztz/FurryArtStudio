' FurryArtStudio - 本地稿件管理工具
' Copyright 2026 xionglongztz/PawLaboratory
'
' Licensed under the Apache License, Version 2.0 (the "License");
' you may not use this file except in compliance with the License.
' You may obtain a copy of the License at
'
'     http://www.apache.org/licenses/LICENSE-2.0
'
' Unless required by applicable law or agreed to in writing, software
' distributed under the License is distributed on an "AS IS" BASIS,
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' See the License for the specific language governing permissions and
' limitations under the License.
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Printing
Imports System.Globalization
Imports System.IO
Imports System.IO.Compression
Imports System.Runtime.InteropServices
Imports System.Text
Imports Krypton.Toolkit
Imports Ookii.Dialogs.WinForms
Imports SysThreading = System.Threading

Public Class MainForm
    Implements IThemeChangeable, ILocalizable
#Region "变量与常量"
    '库
    Private _libraryManager As LibraryManager '当前稿件库管理器实例
    '打印
    Private _currentPrintImages As List(Of String) = Nothing '当前要打印的图片文件路径
    Private _currentPrintIndex As Integer = 0 '当前要打印的图片序号
    Private _artworkCount As Integer = 0 '当前实例稿件总数
    '图片
    Private _imageList As New List(Of Artwork) '图片列表
    Public Event LibraryClosed As EventHandler '定义库关闭事件
    Private _openViewForms As New List(Of ViewForm) '跟踪打开的图片窗口
    '菜单
    Private Const SC_ALWAYSONTOP = 1 '置顶
    Private Const SC_NEWMANUSCRIPT = 2 '新建稿件
    Private Const SC_REFRESH = 3 '刷新
    Private Const SC_SETTINGS = 4 '选项
    Private Const SC_ABOUT = 5 '关于
    '用于主题消息变更消抖
    Private WithEvents _themeDebounceTimer As New Timer With {.Interval = 300}
#End Region

#Region "窗体事件处理"
    ''' <summary>
    ''' 程序启动时调用
    ''' </summary>
    Private Async Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StatusLabel.Text = My.Resources.Stat_Init '正在初始化
        _libraryManager = LibraryManager.Instance '启动稿件库管理器实例
        ResizeControl() '调整组件尺寸
        SysMenuInit() '设置系统菜单
        Dim titleFont As New Font(LblTitle.Font, FontStyle.Bold)
        LblTitle.Font = titleFont
        Dim diffFont As New Font(LblTitle.Font, FontStyle.Italic)
        LblDeltas.Font = diffFont
        If IsAdmin() Then MnuRunAsElevated.Enabled = False
        If ImageGalleryMain IsNot Nothing Then
            RegisterUIPIDragDropFilter(ImageGalleryMain.Handle)
        End If
#If DEBUG Then
        MnuDevTools.Visible = True '显示并启用开发者工具选项
        MnuDevTools.Enabled = True
#Else
        MnuDevTools.Enabled = False
#End If
        Dim autoChangeLang As Boolean = IsFirstRun() '判断程序是否首次启动, 并存储状态
        Dim settings = AppSettings.Load() '读取设置项
        If autoChangeLang Then
            Dim systemCulture = CultureInfo.InstalledUICulture '检测当前系统语言, 并自动设置
            Select Case systemCulture.Name '下次重构时, 考虑改为单独的函数
                Case "zh-CN", "zh-SG"
                    settings.Appearance.Language = AppSettings.LanguageOption.ChineseSimplified
                Case "zh-TW", "zh-HK", "zh-MO"
                    settings.Appearance.Language = AppSettings.LanguageOption.ChineseTraditional
                Case Else
                    settings.Appearance.Language = AppSettings.LanguageOption.English
            End Select
        End If
        Select Case settings.Appearance.Language
            Case AppSettings.LanguageOption.English
                SysThreading.Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
            Case AppSettings.LanguageOption.ChineseSimplified
                SysThreading.Thread.CurrentThread.CurrentUICulture = New CultureInfo("zh-Hans")
            Case AppSettings.LanguageOption.ChineseTraditional
                SysThreading.Thread.CurrentThread.CurrentUICulture = New CultureInfo("zh-Hant")
            Case Else '回退
                SysThreading.Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        End Select
        UpdateFormLang() '更新语言
        MenuInit() '初始化菜单
        SystemThemeChange() '设置主题
        If settings.Appearance.ShowThemeColor Then '修改标题栏颜色(win11生效)
            SetTitleBarColor(Handle, Color.FromArgb(settings.Appearance.ThemeColorArgb))
        End If
        Icon = Icon.FromHandle(My.Resources.Icons.FurryArtStudio.GetHicon) '设置图标
        ImageGalleryMain.SelectionAccentColor = Color.FromArgb(settings.Appearance.SelectionAccentColorArgb)
        ImageGalleryMain.BadgeColor = Color.FromArgb(settings.Appearance.BadgeColorArgb)
        TlStrip.Visible = settings.Appearance.ShowToolBar
        MnuShowToolBar.Checked = settings.Appearance.ShowToolBar
        StaStrip.Visible = settings.Appearance.ShowStatusBar
        MnuShowStatusBar.Checked = settings.Appearance.ShowStatusBar
        Select Case settings.Appearance.Theme
            Case AppSettings.ThemeMode.Light
                MnuThemeLight.Checked = True
            Case AppSettings.ThemeMode.Dark
                MnuThemeDark.Checked = True
            Case AppSettings.ThemeMode.FollowSystem
                MnuThemeSystem.Checked = True
        End Select
        If autoChangeLang Then settings.Save() '首次运行时保存配置文件
        Me.AllowDrop = True
        If settings.Startup.ShowHitokoto Then '调用异步过程显示一言
            StatusLabel.Text = My.Resources.Stat_ShowHitokoto
            Await ShowHitokoto()
        End If
        NotifyIco.Icon = Me.Icon
        NotifyIco.Text = My.Resources.FurryArtStudio
        NotifyIco.Visible = False
        StatusLabel.Text = My.Resources.Stat_Ready '就绪
        MnuExtension.Visible = False
        CreateGlobalMutex()
        If Not IsSingleInstance() Then '激活已有单例
            ActivateExistingInstance()
            Dispose()
        End If
    End Sub
    ''' <summary>
    ''' 显示一言
    ''' </summary>
    Private Async Function ShowHitokoto() As Task
        Dim buttonInfo As New TaskDialogButton(My.Resources.Hitokoto_MoreInfo)
        Dim buttonCancel As New TaskDialogButton(ButtonType.Cancel)
        Dim hitokotoInfo = Await HitokotoHandler.GetHitokoto()
        Dim expandInfo As New StringBuilder
        With expandInfo
            .Append($"ID: {hitokotoInfo.ID}" & vbCrLf)
            .Append($"UUID: {hitokotoInfo.UUID}" & vbCrLf)
            .Append($"{My.Resources.Hitokoto_Type} {hitokotoInfo.Type}" & vbCrLf)
            .Append($"{My.Resources.Hitokoto_Creator} {hitokotoInfo.Creator}" & vbCrLf)
            .Append($"{My.Resources.Hitokoto_Reviewer} {hitokotoInfo.Reviewer}" & vbCrLf)
            .Append($"{My.Resources.Hitokoto_Createdat} {hitokotoInfo.CreatedAt}" & vbCrLf)
        End With
        Using dlg As New TaskDialog With {
                    .WindowTitle = My.Resources.Hitokoto,
                    .MainInstruction = hitokotoInfo.Content,
                    .Content = $"——{hitokotoInfo.FromWho}「{hitokotoInfo.From}」",
                    .MainIcon = TaskDialogIcon.Information,
                    .ExpandedInformation = expandInfo.ToString
                    }
            dlg.Buttons.Add(buttonInfo)
            dlg.Buttons.Add(buttonCancel)
            Dim result As TaskDialogButton = dlg.ShowDialog()
            If result Is buttonInfo Then
                Process.Start($"https://hitokoto.cn/?uuid={hitokotoInfo.UUID}") '打开一言链接
            End If
        End Using
    End Function
    ''' <summary>
    ''' 关闭时释放资源
    ''' </summary>
    Private Sub MainForm_Closing(sender As Object, e As ComponentModel.CancelEventArgs) Handles Me.Closing
        CloseLibrary()
        LibraryManager.CloseAllLibrary() '关闭时释放资源
        DestroyGlobalMutex()
    End Sub
    ''' <summary>
    ''' 窗体消息处理
    ''' </summary>
    ''' <param name="m">Windows消息</param>
    Protected Overrides Sub WndProc(ByRef m As Message) '窗体消息处理函数
        If m.Msg = WM_SYSCOMMAND Then '窗体响应菜单
            Dim hMenu = GetSystemMenu(Handle, False)
            Select Case m.WParam.ToInt32'对应菜单标号
                Case SC_ALWAYSONTOP '窗口置顶
                    SetOnTop()
                Case SC_NEWMANUSCRIPT
                    NewManuscript()
                Case SC_REFRESH
                    RefreshLib()
                Case SC_SETTINGS
                    StatusLabel.Text = My.Resources.Stat_OpenProp
                    PropertiesForm.ShowDialog()
                    StatusLabel.Text = My.Resources.Stat_Ready
                Case SC_ABOUT
                    AboutForm.ShowDialog()
            End Select
        End If
        If m.Msg = WM_SETTINGCHANGE Then '主题发生改变时
            '(注: 使用 WM_DWMCOLORIZATIONCOLORCHANGED 是个不靠谱的野路子办法,踩过坑)
            If m.LParam <> IntPtr.Zero Then
                Dim s = Marshal.PtrToStringUni(m.LParam)
                If s = "ImmersiveColorSet" Then
                    Dim settings = AppSettings.Load()
                    If settings.Appearance.Theme = AppSettings.ThemeMode.FollowSystem Then
                        _themeDebounceTimer.Stop()
                        _themeDebounceTimer.Start() '消抖
                    End If
                End If
            End If
        End If
        If m.Msg = WM_SHOWME Then '从托盘图标状态恢复
            If Me.WindowState = FormWindowState.Minimized Then
                Me.WindowState = FormWindowState.Normal
            End If
            Me.Activate()
            If NotifyIco IsNot Nothing Then
                NotifyIco.Visible = False
            End If
            SetForegroundWindow(Handle)
        End If
        MyBase.WndProc(m) '循环监听消息
    End Sub
    Private Sub _themeDebounceTimer_Tick() Handles _themeDebounceTimer.Tick
        _themeDebounceTimer.Stop()
        BeginInvoke(New MethodInvoker(Sub()
                                          Dim settings = AppSettings.Load()
                                          If settings.Appearance.Theme = AppSettings.ThemeMode.FollowSystem Then
                                              UpdateFormTheme() '当主题设置为跟随系统时,主题变更通过接口发送给全部窗体
                                          End If
                                      End Sub))
    End Sub
    ''' <summary>
    ''' 分割线调整时触发
    ''' </summary>
    Private Sub ArtworkListSplitContainer_SplitterMoved(sender As Object, e As SplitterEventArgs) Handles ArtworkListSplitContainer.SplitterMoved
        ResizeControl()
    End Sub
    ''' <summary>
    ''' 调整组件的大小
    ''' </summary>
    Private Sub ResizeControl()
        Dim p2Width = ArtworkListSplitContainer.Panel2.Width - 10
        PiChkThumb.Height = PiChkThumb.Width '保持为方形
    End Sub
    ''' <summary>
    ''' 当窗口句柄创建或重建时，注入UIPI消息过滤器
    ''' </summary>
    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        RegisterUIPIDragDropFilter(Me.Handle)
        If ImageGalleryMain IsNot Nothing AndAlso ImageGalleryMain.IsHandleCreated Then
            RegisterUIPIDragDropFilter(ImageGalleryMain.Handle)
        End If
    End Sub
#End Region

#Region "辅助方法"
    ''' <summary>
    ''' 设置置顶状态
    ''' </summary>
    Private Sub SetOnTop()
        If TopMost = False Then
            TopMost = True
            CheckMenuItem(GetSystemMenu(Handle, False), SC_ALWAYSONTOP, MF_CHECKED) '窗口置顶
            MnuOnTop.Checked = True
            TSBtnOnTop.Checked = True
        Else
            TopMost = False
            CheckMenuItem(GetSystemMenu(Handle, False), SC_ALWAYSONTOP, MF_UNCHECKED) '取消置顶
            MnuOnTop.Checked = False
            TSBtnOnTop.Checked = False
        End If
    End Sub
    ''' <summary>
    ''' 语言变更
    ''' </summary>
    Private Sub LanguageChange() Implements ILocalizable.LanguageChange
        '文件
        MnuFile.Text = My.Resources.Mnu_File
        MnuOnTop.Text = My.Resources.Mnu_AlwaysOnTop
        MnuPrivacyProtect.Text = My.Resources.Mnu_PrivacyProtect
        MnuDevTools.Text = My.Resources.Mnu_DevTools
        If IsAdmin() Then
            MnuRunAsElevated.Text = My.Resources.Mnu_Elevated
        Else
            MnuRunAsElevated.Text = My.Resources.Mnu_RunAsElevated
        End If
        MnuRunTerminal.Text = My.Resources.Mnu_OpenTerminal
        MnuProperties.Text = My.Resources.Mnu_Options
        MnuOpenPath.Text = My.Resources.Mnu_OpenFolder
        MnuCreateShortcut.Text = My.Resources.Mnu_CreateShortcut
        MnuExit.Text = My.Resources.Mnu_Exit
        '稿件库
        MnuLibrary.Text = My.Resources.Mnu_Lib
        MnuLibList.Text = My.Resources.Mnu_CurrentLib
        MnuLibRefresh.Text = My.Resources.Mnu_Refresh
        MnuLibNew.Text = My.Resources.Mnu_New
        MnuLibImport.Text = My.Resources.Mnu_Import
        MnuLibExport.Text = My.Resources.Mnu_Export
        MnuLibExportCSV.Text = My.Resources.Mnu_ExportCSV
        MnuLibClone.Text = My.Resources.Mnu_Clone
        MnuLibOpenFolder.Text = My.Resources.Mnu_OpenPath
        MnuLibCopy.Text = My.Resources.Mnu_Copy
        MnuLibCopyPath.Text = My.Resources.Mnu_CopyPath
        MnuLibClose.Text = My.Resources.Mnu_Close
        MnuLibRename.Text = My.Resources.Mnu_Rename
        MnuLibDelete.Text = My.Resources.Mnu_Delete
        MnuLibStatistics.Text = My.Resources.Mnu_Properties
        '稿件
        MnuManuscript.Text = My.Resources.Mnu_Ms
        MnuMsNew.Text = My.Resources.Mnu_New
        MnuMsImport.Text = My.Resources.Mnu_Import
        MnuMsView.Text = My.Resources.Mnu_View
        MnuMsEdit.Text = My.Resources.Mnu_Edit
        MnuMsExport.Text = My.Resources.Mnu_ExportMs
        MnuMsPrint.Text = My.Resources.Mnu_Print
        MnuMsDelete.Text = My.Resources.Mnu_Delete
        MnuMsOpenFolder.Text = My.Resources.Mnu_OpenPath
        MnuMsCopy.Text = My.Resources.Mnu_Copy
        MnuMsCopyPath.Text = My.Resources.Mnu_CopyPath
        '视图
        MnuViews.Text = My.Resources.Mnu_Views
        MnuShowStatusBar.Text = My.Resources.View_ShowStatusBar
        MnuShowToolBar.Text = My.Resources.View_ShowToolBar
        MnuTheme.Text = My.Resources.View_Theme
        MnuThemeDark.Text = My.Resources.View_Dark
        MnuThemeLight.Text = My.Resources.View_Light
        MnuThemeSystem.Text = My.Resources.View_System
        MnuViewPlay.Text = My.Resources.Mnu_Play
        MnuSelectAll.Text = My.Resources.Mnu_SelectAll
        MnuSelectReverse.Text = My.Resources.Mnu_SelectReverse
        MnuSearch.Text = My.Resources.Mnu_Search
        MnuAdvancedSearch.Text = My.Resources.Mnu_AdvancedSearch
        MnuPageUp.Text = My.Resources.Mnu_Prev
        MnuPageDown.Text = My.Resources.Mnu_Next
        '帮助
        MnuHelp.Text = My.Resources.Mnu_Help
        MnuHelpTutorial.Text = My.Resources.Mnu_Tutorial
        MnuHelpWebsite.Text = My.Resources.Mnu_Website
        MnuHelpGithub.Text = My.Resources.Mnu_GitHub
        MnuHelpWhatsNew.Text = My.Resources.Mnu_WhatsNew
        MnuCheckUpdate.Text = My.Resources.Mnu_CheckUpdate
        MnuHelpLicense.Text = My.Resources.Mnu_License
        MnuHelpPrivacy.Text = My.Resources.Mnu_Privacy
        MnuFeedback.Text = My.Resources.Mnu_Feedback
        MnuBugReport.Text = My.Resources.Mnu_BugReport
        MnuSuggestions.Text = My.Resources.Mnu_Suggestions
        MnuMyFeedback.Text = My.Resources.Mnu_MyFeedback
        MnuHelpDonate.Text = My.Resources.Mnu_Donate
        MnuOpenAfdian.Text = My.Resources.Mnu_OpenAfdian
        MnuLoadSponsors.Text = My.Resources.Mnu_LoadSponsors
        MnuTerms.Text = My.Resources.Mnu_Terms
        MnuHelpAbout.Text = My.Resources.Mnu_About
        '窗体
        UpdateMenuItem()
        Dim settings = AppSettings.Load()
        If settings.Appearance.MenuUppercase Then '首字母大写
            MnuFile.Text = MnuFile.Text.ToUpper
            MnuLibrary.Text = MnuLibrary.Text.ToUpper
            MnuManuscript.Text = MnuManuscript.Text.ToUpper
            MnuViews.Text = MnuViews.Text.ToUpper
            MnuHelp.Text = MnuHelp.Text.ToUpper
        End If
        Dim tf As Font
        Select Case SysThreading.Thread.CurrentThread.CurrentUICulture.Name'根据语言设置菜单栏字体
            Case "zh-Hant"
                tf = New Font("Microsoft JhengHei", SystemFonts.MenuFont.Size) '繁体
            Case "zh-Hans"
                tf = New Font("Microsoft YaHei", SystemFonts.MenuFont.Size) '简体
            Case Else
                tf = SystemFonts.MenuFont
        End Select
        For Each item As ToolStripMenuItem In MnuStrip.Items.OfType(Of ToolStripMenuItem)()
            SetMenuFont(item, tf)
        Next
        If _libraryManager.GetCurrentLibrary Is Nothing Then
            ArtworkStatusLabel.Text = My.Resources.Main_LblNoLib
            SelectStatusLabel.Text = My.Resources.Main_LblNoMs
            StorageStatusLabel.Text = My.Resources.Main_LblNoStorage
            PageStatusLabel.Text = My.Resources.Main_LblNoPage
        End If
    End Sub
    ''' <summary>
    ''' 递归设置字体
    ''' </summary>
    ''' <param name="menu">菜单项</param>
    ''' <param name="f">字体</param>
    Private Sub SetMenuFont(menu As ToolStripMenuItem, f As Font)
        menu.Font = f
        For Each subItem As ToolStripItem In menu.DropDownItems
            Dim menuItem As ToolStripMenuItem = TryCast(subItem, ToolStripMenuItem)
            If menuItem IsNot Nothing Then
                SetMenuFont(menuItem, f)
            End If
        Next
    End Sub
    ''' <summary>
    ''' 系统主题发生变化时调用以更新
    ''' </summary>
    Private Sub SystemThemeChange() Implements IThemeChangeable.SystemThemeChange
        '颜色常量
        Dim bgColor As Color
        Dim frColor As Color
        '获取控件集合
        Dim controlList As List(Of Control) = GetAllControls(Me)
        '判断颜色
        If IsDarkMode() Then
            bgColor = BgColorDark
            frColor = FrColorDark
            ImageGalleryMain.DisplayMode = GalleryDisplayMode.Dark '设置图片墙主题
            KryptonMgrMain.GlobalPaletteMode = PaletteMode.MaterialDark '设置菜单栏主题
            InitializeMenuImages(True) '设置菜单图标主题
        Else
            bgColor = BgColorLight
            frColor = FrColorLight
            ImageGalleryMain.DisplayMode = GalleryDisplayMode.Normal
            KryptonMgrMain.GlobalPaletteMode = PaletteMode.MaterialLight
            InitializeMenuImages()
        End If
        For Each control In controlList
            control.ForeColor = frColor
            control.BackColor = bgColor
        Next
        ForeColor = frColor
        BackColor = bgColor
        'WinAPI
        DwmSetWindowAttribute(Handle, DwmWindowAttribute.UseImmersiveDarkMode, IsDarkMode(), Marshal.SizeOf(Of Integer))
        SetPreferredAppMode(If(IsDarkMode(), PreferredAppMode.AllowDark, PreferredAppMode.ForceLight))
        FlushMenuThemes()
    End Sub
    ''' <summary>
    ''' 设置缩放图标
    ''' </summary>
    ''' <typeparam name="T">ToolStripItem 泛型</typeparam>
    ''' <param name="items">图标映射元素列表</param>
    Private Sub SetScaledIcon(Of T As ToolStripItem)(items As List(Of (Item As T, BaseName As String)))
        For Each item In items
            Dim resourceName = item.BaseName & If(IsDarkMode(), "Dark", "Light")
            Using img As Image = DirectCast(My.Resources.Icons.ResourceManager.GetObject(resourceName), Image)
                Dim size As Integer = 32 '图标尺寸
                Using thumb As New Bitmap(size, size)
                    Using g = Graphics.FromImage(thumb)
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic
                        g.SmoothingMode = SmoothingMode.HighQuality
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality
                        g.CompositingQuality = CompositingQuality.HighQuality
                        g.DrawImage(img, 0, 0, size, size)
                    End Using
                    item.Item.Image = thumb.Clone '使用高质量缩略图
                End Using
            End Using
        Next
    End Sub
    ''' <summary>
    ''' 初始化菜单图标
    ''' </summary>
    ''' <param name="isDarkMode">(可选)设置深色主题, 默认为浅色</param>
    Private Sub InitializeMenuImages(Optional isDarkMode As Boolean = False)
        Dim menuIcons As New List(Of (MenuItem As ToolStripMenuItem, BaseName As String)) From {
            (MnuOnTop, "MenuPin"),
            (MnuPrivacyProtect, "MenuPrivacyProtect"),
            (MnuDevTools, "MenuDevTools"),
            (MnuRunAsElevated, "MenuShield"),
            (MnuRunTerminal, "MenuTerminal"),
            (MnuProperties, "MenuSettings"),
            (MnuOpenPath, "MenuFolderOpen"),
            (MnuCreateShortcut, "MenuCreateShortcut"),
            (MnuTray, "MenuTray"),
            (MnuExit, "MenuClose"),
            (MnuLibList, "MenuFolders"),
            (MnuLibRefresh, "MenuRefresh"),
            (MnuLibNew, "MenuFolderNew"),
            (MnuLibImport, "MenuFolderInput"),
            (MnuLibExport, "MenuFolderOutput"),
            (MnuLibExportCSV, "MenuExportCsv"),
            (MnuLibClone, "MenuClone"),
            (MnuLibOpenFolder, "MenuFolderOpen"),
            (MnuLibCopy, "MenuCopy"),
            (MnuLibClose, "MenuFolderClose"),
            (MnuLibRename, "MenuFolderEdit"),
            (MnuLibDelete, "MenuFolderDel"),
            (MnuLibStatistics, "MenuProperties"),
            (MnuMsNew, "MenuFileNew"),
            (MnuMsImport, "MenuFileInput"),
            (MnuMsView, "MenuView"),
            (MnuMsEdit, "MenuEdit"),
            (MnuMsExport, "MenuFileOutput"),
            (MnuMsPrint, "MenuPrint"),
            (MnuMsDelete, "MenuDelete"),
            (MnuMsOpenFolder, "MenuFolderOpen"),
            (MnuMsCopy, "MenuCopy"),
            (MnuShowStatusBar, "MenuStatusBar"),
            (MnuShowToolBar, "MenuToolBar"),
            (MnuThemeDark, "MenuDarkMode"),
            (MnuThemeLight, "MenuLightMode"),
            (MnuThemeSystem, "MenuSystem"),
            (MnuAdvancedSearch, "MenuAdvSearch"),
            (MnuViewPlay, "MenuImagePlay"),
            (MnuSearch, "MenuSearch"),
            (MnuPageUp, "MenuPrevious"),
            (MnuPageDown, "MenuNext"),
            (MnuHelpTutorial, "MenuTutorial"),
            (MnuHelpGithub, "MenuGithub"),
            (MnuOpenAfdian, "MenuSupport"),
            (MnuHelpWebsite, "MenuWebsite"),
            (MnuHelpAbout, "MenuInfo"),
            (MnuBugReport, "MenuBugReport"),
            (MnuSuggestions, "MenuSuggestions"),
            (MnuCheckUpdate, "MenuCheckUpdate"),
            (MnuHelpWhatsNew, "MenuStar"),
            (ConMnuMsView, "MenuView"),
            (ConMnuMsEdit, "MenuEdit"),
            (ConMnuMsExport, "MenuFileOutput"),
            (ConMnuMsPrint, "MenuPrint"),
            (ConMnuMsDelete, "MenuDelete"),
            (ConMnuMsOpenFolder, "MenuFolderOpen"),
            (ConMnuMsCopy, "MenuCopy")
        }
        Dim toolIcons As New List(Of (ToolItem As ToolStripButton, BaseName As String)) From {
            (TSBtnOnTop, "MenuPin"),
            (TSBtnProperties, "MenuSettings"),
            (TSBtnRefresh, "MenuRefresh"),
            (TSBtnLibNew, "MenuFolderNew"),
            (TSBtnLibImport, "MenuFolderInput"),
            (TSBtnLibExport, "MenuFolderOutput"),
            (TSBtnLibRename, "MenuFolderEdit"),
            (TSBtnLibClose, "MenuFolderClose"),
            (TSBtnMsNew, "MenuFileNew"),
            (TSBtnMsImport, "MenuFileInput"),
            (TSBtnMsExport, "MenuFileOutput"),
            (TSBtnMsEdit, "MenuEdit"),
            (TSBtnMsPrint, "MenuPrint"),
            (TSBtnMsOpenFolder, "MenuFolderOpen"),
            (TSBtnPlay, "MenuImagePlay"),
            (TSBtnSearch, "MenuSearch"),
            (TSBtnPrevPage, "MenuPrevious"),
            (TSBtnNextPage, "MenuNext")
        }
        SetScaledIcon(menuIcons) '设置菜单项图标
        SetScaledIcon(toolIcons) '设置工具栏图标
        Dim menuHandle = GetSystemMenu(Handle, False) '设置窗体菜单
        If isDarkMode Then
            ApplyMenuIcon(menuHandle, SC_ALWAYSONTOP, My.Resources.Icons.MenuPinDark, True)
            ApplyMenuIcon(menuHandle, SC_NEWMANUSCRIPT, My.Resources.Icons.MenuFileNewDark, True)
            ApplyMenuIcon(menuHandle, SC_REFRESH, My.Resources.Icons.MenuRefreshDark, True)
            ApplyMenuIcon(menuHandle, SC_SETTINGS, My.Resources.Icons.MenuSettingsDark, True)
            ApplyMenuIcon(menuHandle, SC_ABOUT, My.Resources.Icons.MenuInfoDark, True)
        Else
            ApplyMenuIcon(menuHandle, SC_ALWAYSONTOP, My.Resources.Icons.MenuPinLight)
            ApplyMenuIcon(menuHandle, SC_NEWMANUSCRIPT, My.Resources.Icons.MenuFileNewLight)
            ApplyMenuIcon(menuHandle, SC_REFRESH, My.Resources.Icons.MenuRefreshLight)
            ApplyMenuIcon(menuHandle, SC_SETTINGS, My.Resources.Icons.MenuSettingsLight)
            ApplyMenuIcon(menuHandle, SC_ABOUT, My.Resources.Icons.MenuInfoLight)
        End If
    End Sub
    ''' <summary>
    ''' 初始化菜单项
    ''' </summary>
    Private Sub MenuInit()
        Text = My.Resources.FurryArtStudio
        MnuLibOpenFolder.Enabled = False
        MnuLibCopy.Enabled = False
        MnuLibCopyPath.Enabled = False
        MnuLibClone.Enabled = False
        MnuLibExport.Enabled = False
        MnuLibClose.Enabled = False
        MnuLibRename.Enabled = False
        MnuLibDelete.Enabled = False
        MnuLibStatistics.Enabled = False
        MnuMsNew.Enabled = False
        MnuMsImport.Enabled = False
        MnuMsView.Enabled = False
        MnuMsEdit.Enabled = False
        MnuMsDelete.Enabled = False
        MnuMsExport.Enabled = False
        MnuMsPrint.Enabled = False
        MnuMsOpenFolder.Enabled = False
        MnuMsCopy.Enabled = False
        MnuMsCopyPath.Enabled = False
        MnuAdvancedSearch.Enabled = False
        MnuSelectAll.Enabled = False
        MnuSelectReverse.Enabled = False
        MnuViewPlay.Enabled = False
        MnuPageDown.Enabled = False
        MnuPageUp.Enabled = False
        MnuSearch.Enabled = False
        ConMnuMsView.Enabled = False
        ConMnuMsEdit.Enabled = False
        ConMnuMsDelete.Enabled = False
        ConMnuMsExport.Enabled = False
        ConMnuMsPrint.Enabled = False
        ConMnuMsOpenFolder.Enabled = False
        ConMnuMsCopy.Enabled = False
        ConMnuMsCopyPath.Enabled = False
        LblDeltas.Visible = False
        RefreshLibListMenu()
        ImageGalleryMain.ClearImages() '清空所有图片
        ImageGalleryMain.Enabled = False
        MnuSearchTxtbox.Focus()
        If _imageList.Count <> 0 Then _imageList.Clear()
        ClearMetadata()
        MnuSearchTxtbox.Enabled = False
        MnuSearchTxtbox.Text = ""
        StatusLabel.Text = My.Resources.Stat_Ready
        ArtworkStatusLabel.Text = My.Resources.Main_LblNoLib
        _artworkCount = 0
        SelectStatusLabel.Text = My.Resources.Main_LblNoMs
        StorageStatusLabel.Text = My.Resources.Main_LblNoStorage
        PageStatusLabel.Text = My.Resources.Main_LblNoPage
        PageStatusLabel.Visible = False
        TSSep2.Visible = False
        TSSep1.Visible = False
        SearchStatusLabel.Visible = False
        MnuLibExportCSV.Enabled = False
        ArtworkListSplitContainer.TabStop = False '避免启动时出现虚线
        Dim menuHandle = GetSystemMenu(Handle, False) '获取菜单句柄
        EnableMenuItem(menuHandle, SC_NEWMANUSCRIPT, MF_GRAYED)
        GC.Collect()
        TSBtnLibExport.Enabled = False
        TSBtnLibRename.Enabled = False
        TSBtnLibClose.Enabled = False
        TSBtnMsNew.Enabled = False
        TSBtnMsImport.Enabled = False
        TSBtnMsExport.Enabled = False
        TSBtnMsEdit.Enabled = False
        TSBtnMsPrint.Enabled = False
        TSBtnMsOpenFolder.Enabled = False
        TSBtnPlay.Enabled = False
        TSBtnSearch.Enabled = False
        TSBtnPrevPage.Enabled = False
        TSBtnNextPage.Enabled = False
    End Sub
    ''' <summary>
    ''' 初始化系统菜单
    ''' </summary>
    Private Sub SysMenuInit()
        Dim menuHandle = GetSystemMenu(Handle, False) '获取菜单句柄
        '添加新菜单
        InsertMenu(menuHandle, 0, MF_BYPOSITION Or MF_STRING, SC_ALWAYSONTOP, My.Resources.Mnu_AlwaysOnTop)
        InsertMenu(menuHandle, 1, MF_BYPOSITION Or MF_SEPARATOR, 0, Nothing)
        InsertMenu(menuHandle, 8, MF_BYPOSITION Or MF_STRING, SC_NEWMANUSCRIPT, My.Resources.Mnu_NewMs)
        InsertMenu(menuHandle, 9, MF_BYPOSITION Or MF_STRING, SC_REFRESH, My.Resources.Mnu_Refresh)
        InsertMenu(menuHandle, 10, MF_BYPOSITION Or MF_SEPARATOR, 0, Nothing)
        InsertMenu(menuHandle, 11, MF_BYPOSITION Or MF_STRING, SC_SETTINGS, My.Resources.Mnu_Options)
        InsertMenu(menuHandle, 12, MF_BYPOSITION Or MF_STRING, SC_ABOUT, My.Resources.Mnu_About)
        InsertMenu(menuHandle, 13, MF_BYPOSITION Or MF_SEPARATOR, 0, Nothing)
        '设置菜单快捷键
        UpdateMenuItem()
        '设置菜单可用状态
        EnableMenuItem(menuHandle, SC_NEWMANUSCRIPT, MF_GRAYED)
    End Sub
    ''' <summary>
    ''' 更新窗体菜单项
    ''' </summary>
    Private Sub UpdateMenuItem()
        Dim menuHandle = GetSystemMenu(Handle, False) '获取菜单句柄
        SetMenuItemWithShortcut(menuHandle, 0, SC_ALWAYSONTOP, My.Resources.Mnu_AlwaysOnTop, "Alt+T")
        SetMenuItemWithShortcut(menuHandle, 8, SC_NEWMANUSCRIPT, My.Resources.Mnu_NewMs, "Ctrl+N")
        SetMenuItemWithShortcut(menuHandle, 9, SC_REFRESH, My.Resources.Mnu_Refresh, "F5")
        SetMenuItemWithShortcut(menuHandle, 11, SC_SETTINGS, My.Resources.Mnu_Options, "Ctrl+K")
        SetMenuItemWithShortcut(menuHandle, 12, SC_ABOUT, My.Resources.Mnu_About, "Ctrl+F1")
    End Sub
    ''' <summary>
    ''' 载入数据并设置图片墙
    ''' </summary>
    Private Sub LoadArtworks()
        Text = $"{_libraryManager.GetCurrentLibrary.LibraryName} - {My.Resources.FurryArtStudio}"
        StatusLabel.Text = My.Resources.Stat_Loading
        ArtworkListSplitContainer.UseWaitCursor = True
        '设置菜单
        MnuLibOpenFolder.Enabled = True
        MnuLibCopy.Enabled = True
        MnuLibCopyPath.Enabled = True
        MnuLibClone.Enabled = True
        MnuLibExport.Enabled = True
        MnuLibClose.Enabled = True
        MnuLibRename.Enabled = True
        MnuLibDelete.Enabled = True
        MnuLibStatistics.Enabled = True
        MnuMsNew.Enabled = True
        MnuMsImport.Enabled = True
        MnuAdvancedSearch.Enabled = True
        MnuSelectAll.Enabled = True
        MnuSelectReverse.Enabled = True
        MnuViewPlay.Enabled = True
        PageStatusLabel.Visible = True
        TSSep2.Visible = True
        MnuLibExportCSV.Enabled = True
        MnuSearch.Enabled = True
        ImageGalleryMain.Enabled = True
        ArtworkListSplitContainer.TabStop = True
        TSBtnLibExport.Enabled = True
        TSBtnLibRename.Enabled = True
        TSBtnLibClose.Enabled = True
        TSBtnMsNew.Enabled = True
        TSBtnMsImport.Enabled = True
        TSBtnPlay.Enabled = True
        TSBtnSearch.Enabled = True
        Dim menuHandle = GetSystemMenu(Handle, False) '获取菜单句柄
        EnableMenuItem(menuHandle, SC_NEWMANUSCRIPT, MF_ENABLED)
        '设置UI
        ClearMetadata()
        LblTitle.Text = My.Resources.Main_LblNoSelect
        MnuSearchTxtbox.Enabled = True
        '设置图片墙
        ImageGalleryMain.ClearImages() '清空所有图片
        ArtworkStatusLabel.Text = String.Format(My.Resources.Main_LblLibName,
                                                _libraryManager.GetCurrentLibrary.LibraryName) '稿件库名称
        Dim artworks = _libraryManager.GetCurrentLibrary.GetAllArtworksComplete '获取当前库全部数据
        SelectStatusLabel.Text = String.Format(My.Resources.Main_LblMs, artworks.Count) '稿件数量
        '遍历所有稿件
        Dim libraryPath = _libraryManager.GetCurrentLibrary.LibraryPath
        SetGallery(artworks, libraryPath) '载入稿件
        _artworkCount = artworks.Count
        _imageList = artworks '用作预览器参数
        _imageList.Reverse() '反转顺序
        '设置状态栏
        Dim result = GetFolderInfo(libraryPath)
        StorageStatusLabel.Text = String.Format(My.Resources.Main_LblStorage,
                                                result.sizeString, result.fileCount)
        Dim page As Integer = Math.Max(1, Math.Ceiling(_artworkCount / ImageGalleryMain.PageSize))
        MnuPageDown.Enabled = page > 1
        TSBtnNextPage.Enabled = page > 1
        PageStatusLabel.Text = String.Format(My.Resources.Main_LblPage1, page) '在初始化阶段暂时获得不到准确的页码
        ArtworkListSplitContainer.UseWaitCursor = False
        StatusLabel.Text = My.Resources.Stat_Ready
    End Sub
    ''' <summary>
    ''' 设置图片墙显示内容
    ''' </summary>
    Private Sub SetGallery(artworks As List(Of Artwork), libraryPath As String)
        For Each artwork In artworks
            Dim artworkPath As String = Path.Combine(libraryPath, artwork.UUID.ToString())
            If Not Directory.Exists(artworkPath) Then '保证文件夹存在
                Directory.CreateDirectory(artworkPath)
            End If
            Dim fileCount As Integer = artwork.FilePaths.Length
            Dim gi As New GalleryImage With {
                .Title = artwork.Title,
                .UUID = artwork.UUID.ToString,
                .ID = artwork.ID,
                .Count = fileCount,
                .Thumbnail = artwork.Thumbnail
            }
            ImageGalleryMain.AddImage(gi)
        Next
    End Sub
    ''' <summary>
    ''' 查看属性信息
    ''' </summary>
    Private Sub StorageStatusLabel_Click(sender As Object, e As EventArgs) Handles StorageStatusLabel.Click
        MnuLibStatistics.PerformClick()
    End Sub
    ''' <summary>
    ''' 搜索内容时触发
    ''' </summary>
    Private Sub MnuSearchTxtbox_TextChanged(sender As Object, e As EventArgs) Handles MnuSearchTxtbox.TextChanged
        SearchArtwork()
    End Sub
    ''' <summary>
    ''' 搜索稿件
    ''' </summary>
    Private Sub SearchArtwork()
        StatusLabel.Text = My.Resources.Stat_Searching
        ClearSelect()
        If MnuSearchTxtbox.Text = "" Then
            RefreshLib()
            TSSep1.Visible = False
            SearchStatusLabel.Visible = False
            If _libraryManager.GetCurrentLibrary IsNot Nothing Then Text = $"{_libraryManager.GetCurrentLibrary.LibraryName} - {My.Resources.FurryArtStudio}"
        Else
            ImageGalleryMain.ClearImages()
            Dim resultArtwork As List(Of Artwork) = _libraryManager.GetCurrentLibrary.SearchArtworks(MnuSearchTxtbox.Text)
            Dim libraryPath = _libraryManager.GetCurrentLibrary.LibraryPath
            SetGallery(resultArtwork, libraryPath) '载入稿件
            TSSep1.Visible = True
            SearchStatusLabel.Visible = True
            SearchStatusLabel.Text = String.Format(My.Resources.Main_LblSearchedMs, ImageGalleryMain.TotalImageCount) '已搜到的图片数量
            Dim page As Integer = Math.Max(1, Math.Ceiling(ImageGalleryMain.TotalImageCount / ImageGalleryMain.PageSize))
            MnuPageDown.Enabled = page > 1
            PageStatusLabel.Text = String.Format(My.Resources.Main_LblPage1, page) '在初始化阶段暂时获得不到准确的页码
            SelectStatusLabel.Text = String.Format(My.Resources.Main_LblMs, _artworkCount) '稿件总数量
            Text = String.Format(My.Resources.Main_LblMs, ImageGalleryMain.TotalImageCount) & " - " & _libraryManager.GetCurrentLibrary.LibraryName & $" - {My.Resources.FurryArtStudio}"
        End If
        StatusLabel.Text = My.Resources.Stat_Ready
    End Sub

    ''' <summary>
    ''' 清除右侧面板显示元数据字段
    ''' </summary>
    Private Sub ClearMetadata()
        LblTitle.Text = ""
        LblAuthor.Text = ""
        LblTags.Text = ""
        LblCharacters.Text = ""
        LblNotes.Text = ""
        PiChkThumb.Image = Nothing
        PiChkThumb.Visible = False
    End Sub
#End Region

#Region "菜单项"

#Region "文件菜单项"
    Private Sub MnuDevTools_Click(sender As Object, e As EventArgs) Handles MnuDevTools.Click
        DevToolsForm.Show()
    End Sub
    Private Sub MnuRunAsElevated_Click(sender As Object, e As EventArgs) Handles MnuRunAsElevated.Click
        If RunAsElevated() Then
            Me.Close()
        End If
    End Sub
    Private Sub MnuRunTerminal_Click(sender As Object, e As EventArgs) Handles MnuRunTerminal.Click
        Dim startPath As String
        Try
            If _libraryManager.GetCurrentLibrary IsNot Nothing Then
                startPath = _libraryManager.GetCurrentLibrary.LibraryPath
            Else
                startPath = AppDomain.CurrentDomain.BaseDirectory
            End If
            Dim psi As New ProcessStartInfo With {
                .FileName = "cmd.exe",
                .Arguments = $"/K cd /D {startPath}",
                .UseShellExecute = False,
                .CreateNoWindow = False
            }
            Process.Start(psi)
        Catch ex As Exception
            ShowErrorDialog(ex, My.Resources.Msg_TerminalFailed)
        End Try
    End Sub
    Private Sub MnuProperties_Click(sender As Object, e As EventArgs) Handles MnuProperties.Click
        StatusLabel.Text = My.Resources.Stat_OpenProp
        PropertiesForm.ShowDialog()
        StatusLabel.Text = My.Resources.Stat_Ready
    End Sub
    Private Sub MnuCreateShortcut_Click(sender As Object, e As EventArgs) Handles MnuCreateShortcut.Click
        CreateDesktopShortcut(
            targetPath:=Application.ExecutablePath,
            shortcutName:=My.Resources.FurryArtStudio,
            workingDirectory:=Application.StartupPath,
            description:=My.Resources.FAS_Description,
            iconLocation:=$"{Application.ExecutablePath},0"
        )
    End Sub
    Private Sub MnuExit_Click(sender As Object, e As EventArgs) Handles MnuExit.Click
        Me.Close()
    End Sub
    Private Sub MnuOpenPath_Click(sender As Object, e As EventArgs) Handles MnuOpenPath.Click
        Shell($"explorer {Application.StartupPath}", 1)
    End Sub
    Private Sub MnuTray_Click(sender As Object, e As EventArgs) Handles MnuTray.Click
        Hide()
        NotifyIco.Visible = True
    End Sub
    Private Sub NotifyIco_MouseClick(sender As Object, e As MouseEventArgs) Handles NotifyIco.MouseClick
        Show()
        NotifyIco.Visible = False
    End Sub
#End Region

#Region "稿件库菜单项"
    ''' <summary>
    ''' 刷新当前库菜单列表
    ''' </summary>
    Private Sub RefreshLibListMenu()
        MnuLibList.DropDownItems.Clear()
        Dim artworksPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Artworks")
        If Not Directory.Exists(artworksPath) Then Directory.CreateDirectory(artworksPath) '检查 Artworks 文件夹是否存在
        Dim libraryFolders = Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Artworks"))
        If libraryFolders.Length = 0 Then
            Dim artworkMenuItem As New ToolStripMenuItem() With {
                .Text = My.Resources.Main_LblNoLib,
                .Enabled = False
            }
            MnuLibList.DropDownItems.Add(artworkMenuItem)
            Return
        End If
        Dim menuCount As Integer = 0
        For Each foldername In libraryFolders '将稿件库文件夹添加到'当前库'菜单中
            menuCount += 1
            Dim artworkMenuItem As New ToolStripMenuItem With {
                .Tag = Path.GetFileName(foldername)
            }
            artworkMenuItem.Text = $"&{menuCount}. {artworkMenuItem.Tag}"
            If menuCount < 9 Then
                artworkMenuItem.ShortcutKeys = Keys.Alt + Keys.D0 + menuCount
            End If
            MnuLibList.DropDownItems.Add(artworkMenuItem)
            AddHandler artworkMenuItem.Click, AddressOf OnArtworkMenuClick
        Next
    End Sub
    ''' <summary>
    ''' 处理新增的稿件库菜单项
    ''' </summary>
    Private Sub OnArtworkMenuClick(sender As Object, e As EventArgs)
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        If menuItem IsNot Nothing Then
            Dim foldername As String = TryCast(menuItem.Tag, String)
            If Not String.IsNullOrEmpty(foldername) Then
                _libraryManager.AddLibrary(foldername) '新增并切换到稿件库
                _libraryManager.SwitchLibrary(foldername)
                LoadArtworks() '重新载入稿件库
            End If
        End If
    End Sub
    Private Sub MnuLibNew_Click(sender As Object, e As EventArgs) Handles MnuLibNew.Click
        Using inputForm As New InputDialogForm With {
            .Text = My.Resources.Input_NewLibTitle
        }
            inputForm.InputTxtbox.Text = My.Resources.Input_TxtNewLib
            StatusLabel.Text = My.Resources.Stat_CreatingLib
            If inputForm.ShowDialog() = DialogResult.OK Then '显示对话框并获取结果
                Dim newLib As String = inputForm.InputValue
                _libraryManager.AddLibrary(newLib)
                _libraryManager.SwitchLibrary(newLib)
                RefreshLib() '刷新
            Else
            End If
            StatusLabel.Text = My.Resources.Stat_Ready
        End Using
    End Sub
    Private Sub MnuLibImport_Click(sender As Object, e As EventArgs) Handles MnuLibImport.Click
        Using pawFileDlg As New OpenFileDialog() With {
            .Filter = My.Resources.Main_FileFilterPAW
            }
            If pawFileDlg.ShowDialog() = DialogResult.OK Then
                'Using archive As ZipFile = ZipFile.OpenRead(pawFileDlg.FileName)
                '    For Each entry As ZipArchiveEntry In archive.Entries
                '        ' 构建完整目标路径（保留 zip 内相对目录结构）
                '        Dim destinationPath As String = Path.Combine(extractPath, entry.FullName)
                '        Dim destinationDir As String = Path.GetDirectoryName(destinationPath)

                '        ' 创建目标目录（如果不存在）
                '        If Not String.IsNullOrEmpty(destinationDir) Then
                '            Directory.CreateDirectory(destinationDir)
                '        End If

                '        ' 跳过表示目录的条目（以 '/' 结尾）
                '        If entry.Name = "" Then
                '            Continue For
                '        End If

                '        ' 提取文件，并覆盖同名文件（True 表示覆盖）
                '        entry.ExtractToFile(destinationPath, True)
                '    Next
                'End Using

            End If
        End Using
    End Sub
    Private Sub MnuLibExport_Click(sender As Object, e As EventArgs) Handles MnuLibExport.Click
        StatusLabel.Text = My.Resources.Stat_Exporting
        Using pawFileDlg As New SaveFileDialog() With {
            .Filter = My.Resources.Main_FileFilterPAW,
            .FileName = $"{_libraryManager.GetCurrentLibrary.LibraryName}.paw"
            }
            If pawFileDlg.ShowDialog() = DialogResult.OK Then
                Try
                    Dim libPath As String = _libraryManager.GetCurrentLibrary.LibraryPath
                    MnuLibClose.PerformClick()
                    Dim targetFileName As String = pawFileDlg.FileName
                    If File.Exists(targetFileName) Then '当出现重名文件时删除
                        File.Delete(targetFileName)
                    End If
                    ZipFile.CreateFromDirectory(libPath, targetFileName, CompressionLevel.Fastest, True)
                    Using dlg As New TaskDialog With {
                        .WindowTitle = My.Resources.FurryArtStudio,
                        .Content = My.Resources.Msg_ExportComplete,
                        .MainIcon = TaskDialogIcon.Information
                        }
                        dlg.Buttons.Add(New TaskDialogButton(ButtonType.Ok))
                        dlg.ShowDialog()
                    End Using
                Catch ex As Exception
                    ShowErrorDialog(ex, My.Resources.Msg_ExportFailed)
                End Try
            End If
        End Using
        StatusLabel.Text = My.Resources.Stat_Ready
    End Sub
    Private Sub MnuLibExportCSV_Click(sender As Object, e As EventArgs) Handles MnuLibExportCSV.Click
        StatusLabel.Text = My.Resources.Stat_ExportCSV
        Using csvFileDialog As New SaveFileDialog() With {
            .Filter = My.Resources.Main_FileFilterCSV,
            .FileName = $"{_libraryManager.GetCurrentLibrary.LibraryName}.csv"
            }
            If csvFileDialog.ShowDialog() = DialogResult.OK Then
                Try
                    _libraryManager.GetCurrentLibrary.ExportTableToCSV(csvFileDialog.FileName)
                    Using dlg As New TaskDialog With {
                        .WindowTitle = My.Resources.FurryArtStudio,
                        .Content = My.Resources.Msg_ExportComplete,
                        .MainIcon = TaskDialogIcon.Information
                        }
                        dlg.Buttons.Add(New TaskDialogButton(ButtonType.Ok))
                        dlg.ShowDialog()
                    End Using
                Catch ex As Exception
                    ShowErrorDialog(ex, My.Resources.Msg_CreateCSVFailed)
                End Try
            End If
        End Using
        StatusLabel.Text = My.Resources.Stat_Ready
    End Sub
    Private Sub MnuLibRefresh_Click(sender As Object, e As EventArgs) Handles MnuLibRefresh.Click
        RefreshLib()
    End Sub
    ''' <summary>
    ''' 刷新菜单列表, 并重新载入数据
    ''' </summary>
    Private Sub RefreshLib()
        CloseLibrary() '为了避免编辑稿件后图片浏览器出现问题, 所以必须关闭库重载
        RefreshLibListMenu()
        If _libraryManager.GetCurrentLibrary IsNot Nothing Then LoadArtworks()
        If MnuSearchTxtbox.Text <> "" Then SearchArtwork()
    End Sub
    Private Sub MnuLibClone_Click(sender As Object, e As EventArgs) Handles MnuLibClone.Click
        Using inputForm As New InputDialogForm With {
            .Text = My.Resources.Input_CloneLibTitle
        }
            inputForm.InputTxtbox.Text = String.Format(My.Resources.Input_TxtClone, _libraryManager.GetCurrentLibrary.LibraryName)
            StatusLabel.Text = My.Resources.Stat_Cloning
            If inputForm.ShowDialog() = DialogResult.OK Then '显示对话框并获取结果
                Dim newLib As String = inputForm.InputValue
                Dim newPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Artworks", newLib)
                Try
                    FileIO.FileSystem.CopyDirectory(_libraryManager.GetCurrentLibrary.LibraryPath, newPath, FileIO.UIOption.AllDialogs)
                Catch ex As OperationCanceledException
                    Return '操作被取消, 忽略即可
                End Try
                _libraryManager.AddLibrary(newLib)
                _libraryManager.SwitchLibrary(newLib)
                RefreshLib() '刷新
            Else
            End If
            StatusLabel.Text = My.Resources.Stat_Ready
        End Using
    End Sub
    Private Sub MnuLibOpenFolder_Click(sender As Object, e As EventArgs) Handles MnuLibOpenFolder.Click
        Shell($"explorer /select,{_libraryManager.GetCurrentLibrary.LibraryPath}", 1)
    End Sub
    Private Sub MnuLibCopy_Click(sender As Object, e As EventArgs) Handles MnuLibCopy.Click
        CopyDirectoryToClipboard(_libraryManager.GetCurrentLibrary.LibraryPath)
    End Sub
    Private Sub MnuLibCopyPath_Click(sender As Object, e As EventArgs) Handles MnuLibCopyPath.Click
        Clipboard.SetDataObject(_libraryManager.GetCurrentLibrary.LibraryPath)
    End Sub
    Private Sub MnuLibClose_Click(sender As Object, e As EventArgs) Handles MnuLibClose.Click
        StatusLabel.Text = My.Resources.Stat_Closing
        CloseLibrary()
        _libraryManager.CloseLibrary(_libraryManager.GetCurrentLibrary.LibraryName)
        MenuInit()
    End Sub
    Private Sub MnuLibRename_Click(sender As Object, e As EventArgs) Handles MnuLibRename.Click
        Dim oldLib As String = _libraryManager.GetCurrentLibrary.LibraryName
        Dim oldPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Artworks", oldLib)
        Using inputForm As New InputDialogForm With {
            .Text = My.Resources.Input_RenameLibTitle
        }
            inputForm.InputTxtbox.Text = _libraryManager.GetCurrentLibrary.LibraryName
            StatusLabel.Text = My.Resources.Stat_Renaming
            If inputForm.ShowDialog() = DialogResult.OK Then '显示对话框并获取结果
                Try
                    Dim newLib As String = inputForm.InputValue
                    _libraryManager.CloseLibrary(_libraryManager.GetCurrentLibrary.LibraryName) '先释放数据库资源
                    Dim newPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Artworks", newLib)
                    Directory.Move(oldPath, newPath) '重命名
                    _libraryManager.AddLibrary(newLib) '载入新稿件库
                    _libraryManager.SwitchLibrary(newLib)
                Catch ex As Exception
                    ShowErrorDialog(ex, My.Resources.Msg_CreateFailed)
                End Try
                RefreshLib() '更名后重新载入数据库
            Else
            End If
            StatusLabel.Text = My.Resources.Stat_Ready
        End Using
    End Sub
    Private Sub MnuLibDelete_Click(sender As Object, e As EventArgs) Handles MnuLibDelete.Click
        Dim isShiftPressed As Boolean = My.Computer.Keyboard.ShiftKeyDown
        Dim nowPath As String = _libraryManager.GetCurrentLibrary.LibraryPath
        Dim nowLib As String = _libraryManager.GetCurrentLibrary.LibraryName
        '对话框按钮
        Dim buttonDeletePermanently As New TaskDialogButton(My.Resources.Msg_IKnowWhatIamDoing)
        Dim buttonDelete As New TaskDialogButton(My.Resources.Msg_DeleteLibConfirm)
        Dim buttonCancel As New TaskDialogButton(ButtonType.Cancel)
        _libraryManager.CloseLibrary(nowLib) '先释放数据库资源, 再尝试删除文件
        Try
            StatusLabel.Text = My.Resources.Stat_DeletingLib
            If isShiftPressed Then
                Using dlg As New TaskDialog With {
                    .WindowTitle = My.Resources.FurryArtStudio,
                    .MainInstruction = String.Format(My.Resources.Msg_DeleteLibPermanently, nowLib),
                    .Content = My.Resources.Msg_CannotBeIrreversible,
                    .MainIcon = TaskDialogIcon.Warning
                    }

                    dlg.Buttons.Add(buttonDeletePermanently)
                    dlg.Buttons.Add(buttonCancel)
                    Dim result As TaskDialogButton = dlg.ShowDialog()
                    If result Is buttonDeletePermanently Then
                        Directory.Delete(nowPath, True)
                    Else
                        Throw New OperationCanceledException(My.Resources.Main_StrOperationCancelled)
                    End If
                End Using
            Else
                Using dlg As New TaskDialog With {
                    .WindowTitle = My.Resources.FurryArtStudio,
                    .MainInstruction = String.Format(My.Resources.Msg_DeleteLib, nowLib),
                    .MainIcon = TaskDialogIcon.Information
                    }

                    dlg.Buttons.Add(buttonDelete)
                    dlg.Buttons.Add(buttonCancel)
                    Dim result As TaskDialogButton = dlg.ShowDialog()
                    If result Is buttonDelete Then
                        FileIO.FileSystem.DeleteDirectory(nowPath,
                        FileIO.UIOption.AllDialogs,
                        FileIO.RecycleOption.SendToRecycleBin)
                    Else
                        Throw New OperationCanceledException(My.Resources.Main_StrOperationCancelled)
                    End If
                End Using
            End If
            MenuInit()
            CloseLibrary()
        Catch ex As OperationCanceledException '操作被取消, 回滚操作
            _libraryManager.AddLibrary(nowLib)
            _libraryManager.SwitchLibrary(nowLib)
        Catch ex As Exception
            ShowErrorDialog(ex, My.Resources.Msg_LibDeleteFailed)
        End Try
        StatusLabel.Text = My.Resources.Stat_Ready
    End Sub
    Private Sub MnuLibStatistics_Click(sender As Object, e As EventArgs) Handles MnuLibStatistics.Click
        Dim sb As New StringBuilder
        Dim library = _libraryManager.GetCurrentLibrary
        sb.Append(String.Format(My.Resources.Main_StrPropLib, library.LibraryName) & vbCrLf)
        sb.Append(String.Format(My.Resources.Main_StrPropLibPath, library.LibraryPath) & vbCrLf)
        sb.Append(String.Format(My.Resources.Main_StrPropMsCount, library.GetAllArtworksComplete.Count) & vbCrLf)
        Dim result = GetFolderInfo(library.LibraryPath)
        sb.Append(String.Format(My.Resources.Main_StrPropStorage, result.sizeString, result.fileCount) & vbCrLf)
        sb.Append(String.Format(My.Resources.Main_StrPropNowTime, Now))
        Using dlg As New TaskDialog With {
            .WindowTitle = My.Resources.FurryArtStudio,
            .Content = sb.ToString,
            .MainIcon = TaskDialogIcon.Information
            }
            dlg.Buttons.Add(New TaskDialogButton(ButtonType.Ok))
            dlg.ShowDialog()
        End Using
    End Sub
#End Region

#Region "稿件菜单项"
    Private Sub MnuMsNew_Click(sender As Object, e As EventArgs) Handles MnuMsNew.Click
        NewManuscript()
    End Sub
    Private Sub NewManuscript()
        StatusLabel.Text = My.Resources.Stat_NewMs
        Dim initArtwork As New Artwork With {
            .CreateTime = Now
        }
        Using editForm As New EditDialogForm(initArtwork, _libraryManager.GetCurrentLibrary.LibraryPath)
            If editForm.ShowDialog() = DialogResult.OK Then
                Dim newArtwork As Artwork = editForm.EditedArtwork '获得新建的稿件对象
                _libraryManager.GetCurrentLibrary.AddArtwork(newArtwork)

                RefreshLib()
            End If
        End Using
        StatusLabel.Text = My.Resources.Stat_Ready
    End Sub
    Private Sub MnuMsImport_Click(sender As Object, e As EventArgs) Handles MnuMsImport.Click

    End Sub
    Private Sub MnuMsView_Click(sender As Object, e As EventArgs) Handles MnuMsView.Click
        ViewImage(Guid.Parse(ImageGalleryMain.SelectedImages(0).UUID))
    End Sub
    Private Sub MnuMsEdit_Click(sender As Object, e As EventArgs) Handles MnuMsEdit.Click
        StatusLabel.Text = My.Resources.Stat_EditMs
        Dim nowArtwork As Artwork = _libraryManager.GetCurrentLibrary.GetArtworkByUUID(Guid.Parse(ImageGalleryMain.SelectedImages(0).UUID))
        Using editForm As New EditDialogForm(nowArtwork, _libraryManager.GetCurrentLibrary.LibraryPath)
            If editForm.ShowDialog() = DialogResult.OK Then
                Dim newArtwork As Artwork = editForm.EditedArtwork '获得新建的稿件对象
                _libraryManager.GetCurrentLibrary.UpdateArtwork(newArtwork)
                RefreshLib()
            End If
        End Using
        StatusLabel.Text = My.Resources.Stat_Ready
    End Sub
    Private Sub MnuMsDelete_Click(sender As Object, e As EventArgs) Handles MnuMsDelete.Click
        Dim isShiftPressed As Boolean = My.Computer.Keyboard.ShiftKeyDown
        StatusLabel.Text = My.Resources.Stat_DeletingMs
        Dim imgList As List(Of GalleryImage) = ImageGalleryMain.SelectedImages()
        Dim uuidList As List(Of Guid) = imgList.Select(Function(c) Guid.Parse(c.UUID)).ToList()
        Dim titleList As List(Of String) = imgList.Select(Function(c) c.Title).ToList()
        Dim nowPath As String = _libraryManager.GetCurrentLibrary.LibraryPath
        '对话框按钮
        Dim buttonDelete As New TaskDialogButton(My.Resources.Msg_DeleteMsConfirm)
        Dim buttonDeletePermanently As New TaskDialogButton(My.Resources.Msg_DeleteMsPermanentlyConfirm)
        Dim buttonCancel As New TaskDialogButton(ButtonType.Cancel)
        Try
            If isShiftPressed Then
                Using dlg As New TaskDialog With {
                    .WindowTitle = My.Resources.FurryArtStudio,
                    .MainInstruction = String.Format(My.Resources.Msg_DeleteMsPermanently, titleList.Count),
                    .Content = String.Join(vbCrLf, titleList.Take(5)) &
                                             If(titleList.Count > 5, vbCrLf &
                                             String.Format(My.Resources.Msg_DeleteMsCount, titleList.Count), ""),
                    .MainIcon = TaskDialogIcon.Warning
                    }
                    dlg.Buttons.Add(buttonDeletePermanently)
                    dlg.Buttons.Add(buttonCancel)
                    Dim result As TaskDialogButton = dlg.ShowDialog()
                    If result Is buttonCancel Then
                        StatusLabel.Text = My.Resources.Stat_Ready
                        Return
                    End If
                    For Each uuid In uuidList
                        Directory.Delete(Path.Combine(nowPath, uuid.ToString), True) '永久删除数据
                        _libraryManager.GetCurrentLibrary.SoftDeleteArtwork(uuid) '标记为软删除
                    Next
                End Using
            Else
                Using dlg As New TaskDialog With {
                    .WindowTitle = My.Resources.FurryArtStudio,
                    .MainInstruction = String.Format(My.Resources.Msg_DeleteMs, titleList.Count),
                    .Content = String.Join(vbCrLf, titleList.Take(5)) &
                                             If(titleList.Count > 5, vbCrLf &
                                             String.Format(My.Resources.Msg_DeleteMsCount, titleList.Count), ""),
                    .MainIcon = TaskDialogIcon.Information
                    }
                    dlg.Buttons.Add(buttonDelete)
                    dlg.Buttons.Add(buttonCancel)
                    Dim result As TaskDialogButton = dlg.ShowDialog()
                    If result Is buttonCancel Then
                        StatusLabel.Text = My.Resources.Stat_Ready
                        Return
                    End If
                    For Each uuid In uuidList
                        FileIO.FileSystem.DeleteDirectory(Path.Combine(nowPath, uuid.ToString),
                                                          FileIO.UIOption.OnlyErrorDialogs,
                                                          FileIO.RecycleOption.SendToRecycleBin) '移动到回收站
                        _libraryManager.GetCurrentLibrary.SoftDeleteArtwork(uuid) '标记为软删除
                    Next
                End Using
            End If
            RefreshLib()
        Catch ex As OperationCanceledException
            '忽略
        End Try
        StatusLabel.Text = My.Resources.Stat_Ready
    End Sub
    Private Sub MnuMsExport_Click(sender As Object, e As EventArgs) Handles MnuMsExport.Click
        StatusLabel.Text = "正在导出稿件"
        Dim artworks As New List(Of Artwork)
        For Each gi In ImageGalleryMain.SelectedImages
            artworks.Add(_libraryManager.GetCurrentLibrary.GetArtworkByUUID(Guid.Parse(gi.UUID)))
        Next
        Using exportForm As New ExportForm(artworks)
            If exportForm.ShowDialog() = DialogResult.OK Then

            End If
        End Using
        StatusLabel.Text = My.Resources.Stat_Ready
    End Sub
    Private Sub MnuMsPrint_Click(sender As Object, e As EventArgs) Handles MnuMsPrint.Click
        StatusLabel.Text = My.Resources.Stat_PreparePrint
        '获取选中的图片列表
        Dim selectedImages As New List(Of String)()
        Dim selectedItem As List(Of GalleryImage) = ImageGalleryMain.SelectedImages
        '获取图片目录
        Dim artworkDir = Path.Combine(_libraryManager.GetCurrentLibrary.LibraryPath, selectedItem(0).UUID)
        '获取目录下所有图片文件
        If Directory.Exists(artworkDir) Then
            '支持的图片格式
            Dim imageExtensions() As String = {".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".webp"}
            Dim files = Directory.GetFiles(artworkDir, "*.*")
            For Each file In files
                Dim fileName As String = Path.GetFileName(file)
                If imageExtensions.Contains(Path.GetExtension(file).ToLower()) Then
                    selectedImages.Add(file)
                End If
            Next
        End If
        '检查是否有图片
        If selectedImages.Count = 0 Then
            Using dlg As New TaskDialog With {
                    .WindowTitle = My.Resources.FurryArtStudio,
                    .MainInstruction = My.Resources.Msg_NoPrintFile,
                    .MainIcon = TaskDialogIcon.Information
                    }
                dlg.Buttons.Add(New TaskDialogButton(ButtonType.Ok))
                dlg.ShowDialog()
            End Using
            StatusLabel.Text = My.Resources.Stat_Ready
            Return
        End If
        Dim isShiftPressed As Boolean = My.Computer.Keyboard.ShiftKeyDown
        Dim nowArtwork As Artwork = _libraryManager.GetCurrentLibrary.GetArtworkByUUID(Guid.Parse(selectedItem(0).UUID))
        Dim docName As String = $"{nowArtwork.Title} - {nowArtwork.Author}" '这里存在编码转换问题
        If isShiftPressed Then
            Dim pd As New PrintDocument With {
                .DocumentName = docName,'标题 - 作者
                .DefaultPageSettings = New PageSettings With {
                .Landscape = True '默认横向
            }
        }
            _currentPrintImages = selectedImages
            AddHandler pd.PrintPage, AddressOf Pd_Printpage
            AddHandler pd.EndPrint, AddressOf Pd_EndPrint
            Dim printDialog As New PrintDialog With {
                .Document = pd,
                .AllowPrintToFile = True,
                .AllowSomePages = False,
                .AllowSelection = False,
                .UseEXDialog = True,
                .PrinterSettings = New PrinterSettings With {
                .PrintFileName = docName
            }
        } '设置打印对话框
            Dim pageSetupDialog As New PageSetupDialog With {
                .Document = pd,
                .EnableMetric = True
            } '设置页面设置对话框
            '先显示页面设置
            If pageSetupDialog.ShowDialog() = DialogResult.OK Then
                '再显示打印对话框
                If printDialog.ShowDialog() = DialogResult.OK Then
                    StatusLabel.Text = My.Resources.Stat_Printing
                    '开始打印
                    pd.Print()
                End If
            End If
        Else
            '使用新的自定义打印对话框
            Using printForm As New PrintForm(selectedImages, nowArtwork)
                If printForm.ShowDialog() = DialogResult.OK AndAlso printForm.PrintDocumentInstance IsNot Nothing Then
                    Try
                        '开始打印
                        StatusLabel.Text = My.Resources.Stat_Printing
                        printForm.PrintDocumentInstance.Print()
                    Catch ex As Exception
                        ShowErrorDialog(ex, My.Resources.Msg_PrintFailed)
                    End Try
                ElseIf printForm.UserCancelled Then
                End If
            End Using
        End If
        StatusLabel.Text = My.Resources.Stat_Ready
    End Sub
    Private Sub Pd_Printpage(sender As Object, e As PrintPageEventArgs)
        If _currentPrintImages Is Nothing OrElse _currentPrintIndex >= _currentPrintImages.Count Then
            e.HasMorePages = False
            Return
        End If
        Try
            Dim currentImagePath = _currentPrintImages(_currentPrintIndex) '加载当前图片
            Using original As Image = Image.FromFile(currentImagePath) '使用Using确保释放资源

                Dim img As New Bitmap(original.Width, original.Height, PixelFormat.Format64bppArgb)
                '通过创建副本的方式, 保证图片尺寸不会出现问题
                Using g = Graphics.FromImage(img)
                    g.DrawImage(original, 0, 0, original.Width, original.Height)
                End Using

                Dim marginBounds = e.MarginBounds '获取页面边距区域
                Dim destRect As Rectangle '保持图片比例
                If img.Width / img.Height > marginBounds.Width / marginBounds.Height Then
                    '图片比页面区域宽
                    Dim height = CInt(img.Height * marginBounds.Width / img.Width)
                    destRect = New Rectangle(marginBounds.X,
                                         marginBounds.Y + (marginBounds.Height - height) \ 2,
                                         marginBounds.Width,
                                         height)
                Else
                    '图片比页面区域高
                    Dim width = CInt(img.Width * marginBounds.Height / img.Height)
                    destRect = New Rectangle(marginBounds.X + (marginBounds.Width - width) \ 2,
                                         marginBounds.Y,
                                         width,
                                         marginBounds.Height)
                End If
                '绘制图片
                e.Graphics.DrawImage(img, destRect)
            End Using
            '移动到下一张图片
            _currentPrintIndex += 1
            '检查是否还有更多页面
            e.HasMorePages = (_currentPrintIndex < _currentPrintImages.Count)
        Catch ex As Exception
            '处理图片加载失败的情况
            ShowErrorDialog(ex, My.Resources.Msg_ImageLoadFailed)
            '跳过这张图片，继续下一张
            _currentPrintIndex += 1
            e.HasMorePages = (_currentPrintIndex < _currentPrintImages.Count)
        End Try
    End Sub
    Private Sub Pd_EndPrint(sender As Object, e As PrintEventArgs)
        If _currentPrintImages IsNot Nothing Then '打印完成后清理资源
            _currentPrintImages.Clear()
            _currentPrintImages = Nothing
        End If
        _currentPrintIndex = 0
    End Sub
    ''' <summary>
    ''' 获得已选择的稿件UUID目录
    ''' </summary>
    ''' <returns>一个包含已选择稿件UUID的列表</returns>
    Private Function GetSelectedArtworkList() As List(Of String)
        Dim artworkPaths As New List(Of String)()
        For Each selectedItem As GalleryImage In ImageGalleryMain.SelectedImages
            Dim artworkPath = Path.Combine(_libraryManager.GetCurrentLibrary.LibraryPath, selectedItem.UUID)
            artworkPaths.Add(artworkPath)
        Next
        Return artworkPaths
    End Function
    Private Sub MnuMsOpenFolder_Click(sender As Object, e As EventArgs) Handles MnuMsOpenFolder.Click
        Dim artworkPaths = GetSelectedArtworkList()
        If artworkPaths.Count > 5 Then '当用户打开超过5个稿件时, 进行确认
            Dim buttonYes As New TaskDialogButton(ButtonType.Yes)
            Using dlg As New TaskDialog With {
                    .WindowTitle = My.Resources.FurryArtStudio,
                    .MainInstruction = String.Format(My.Resources.Msg_MultiFolderOpen, artworkPaths.Count),
                    .MainIcon = TaskDialogIcon.Information
                    }
                dlg.Buttons.Add(buttonYes)
                dlg.Buttons.Add(New TaskDialogButton(ButtonType.No))
                If dlg.ShowDialog() Is buttonYes Then
                    For Each artworkPath In artworkPaths
                        Shell($"explorer {artworkPath}", 1)
                    Next
                End If
            End Using
        Else
            For Each artworkPath In artworkPaths
                Shell($"explorer {artworkPath}", 1)
            Next
        End If
    End Sub
    Private Sub MnuMsCopy_Click(sender As Object, e As EventArgs) Handles MnuMsCopy.Click
        Dim artworkPaths = GetSelectedArtworkList() '获取所有选中的项目的目录路径
        If My.Computer.Keyboard.ShiftKeyDown Then
            Dim artworkObject As New DataObject()
            Dim artworkInfo As New StringBuilder
            artworkInfo.Append(My.Resources.Msg_StrMsInfo & vbCrLf)
            artworkInfo.Append($"{SeparatorDash}{vbCrLf}")
            Dim count As Integer = 0
            For Each selectedItem As GalleryImage In ImageGalleryMain.SelectedImages
                count += 1
                Dim artwork As Artwork = _libraryManager.GetCurrentLibrary.GetArtworkByUUID(Guid.Parse(selectedItem.UUID))
                If count > 1 Then artworkInfo.Append($"{SeparatorDash}{vbCrLf}") '当多个稿件时添加分割线
                artworkInfo.Append(String.Format(My.Resources.Msg_StrMsTitle, artwork.Title, artwork.Author) & vbCrLf)
                artworkInfo.Append(String.Format(My.Resources.Msg_StrMsUUID, selectedItem.UUID) & vbCrLf)
                artworkInfo.Append(String.Format(My.Resources.Msg_StrMsUpdate, artwork.UpdateTime) & vbCrLf)
                artworkInfo.Append(String.Format(My.Resources.Msg_StrMsImport, artwork.ImportTime) & vbCrLf)
                artworkInfo.Append(String.Format(My.Resources.Msg_StrMsCreate, artwork.CreateTime) & vbCrLf)
                artworkInfo.Append(String.Format(My.Resources.Msg_StrMsTags, FormatArrayWithEllipsis(artwork.Tags)) & vbCrLf)
                artworkInfo.Append(String.Format(My.Resources.Msg_StrMsCharacters, FormatArrayWithEllipsis(artwork.Characters)) & vbCrLf)
                artworkInfo.Append(String.Format(My.Resources.Msg_StrMsNotes, artwork.Notes) & vbCrLf)
            Next
            artworkInfo.Append($"{SeparatorDash}{vbCrLf}")
            artworkInfo.Append(String.Format(My.Resources.Msg_StrMsTotal, count) & vbCrLf) '总计
            artworkInfo.Append(String.Format(My.Resources.Msg_StrMsNow, Now) & vbCrLf) '当前时间
            artworkInfo.Append(My.Resources.Msg_StrMsSupportByFAS)
            artworkObject.SetData(DataFormats.Text, artworkInfo) '复制信息文本
            CopyDirectoryToClipboard(artworkPaths.ToArray(), artworkObject) '同时复制稿件
        Else
            CopyDirectoryToClipboard(artworkPaths.ToArray())
        End If
    End Sub
    Private Sub MnuMsCopyPath_Click(sender As Object, e As EventArgs) Handles MnuMsCopyPath.Click
        Dim artworkPaths = GetSelectedArtworkList()
        Dim sb As New StringBuilder()
        For i As Integer = 0 To artworkPaths.Count - 1
            sb.Append(artworkPaths(i))
            If i < artworkPaths.Count - 1 Then
                sb.AppendLine()
            End If
        Next
        Clipboard.SetDataObject(sb.ToString())
    End Sub
#End Region

#Region "视图菜单项"
    Private Sub MnuOnTop_Click(sender As Object, e As EventArgs) Handles MnuOnTop.Click
        SetOnTop()
    End Sub
    Private Sub MnuPrivacyProtect_Click(sender As Object, e As EventArgs) Handles MnuPrivacyProtect.Click
        If MnuPrivacyProtect.Checked Then
            MnuPrivacyProtect.Checked = False
            SetWindowDisplayAffinity(Handle, WDA_NONE)
        Else
            MnuPrivacyProtect.Checked = True
            SetWindowDisplayAffinity(Handle, WDA_MONITOR)
        End If
    End Sub
    Private Sub MnuShowStatusBar_Click(sender As Object, e As EventArgs) Handles MnuShowStatusBar.Click
        If MnuShowStatusBar.Checked Then
            MnuShowStatusBar.Checked = False
            StaStrip.Visible = False
        Else
            MnuShowStatusBar.Checked = True
            StaStrip.Visible = True
        End If
        Dim settings = AppSettings.Load()
        settings.Appearance.ShowStatusBar = MnuShowStatusBar.Checked
        settings.Save()
    End Sub
    Private Sub MnuShowToolBar_Click(sender As Object, e As EventArgs) Handles MnuShowToolBar.Click
        If MnuShowToolBar.Checked Then
            TlStrip.Visible = False
            MnuShowToolBar.Checked = False
        Else
            TlStrip.Visible = True
            MnuShowToolBar.Checked = True
        End If
        Dim settings = AppSettings.Load()
        settings.Appearance.ShowToolBar = MnuShowToolBar.Checked
        settings.Save()
    End Sub
    Private Sub MnuThemeSystem_Click(sender As Object, e As EventArgs) Handles MnuThemeSystem.Click
        Dim settings = AppSettings.Load()
        settings.Appearance.Theme = AppSettings.ThemeMode.FollowSystem
        settings.Save()
        UpdateFormTheme()
        MnuThemeSystem.Checked = True
        MnuThemeDark.Checked = False
        MnuThemeLight.Checked = False
    End Sub
    Private Sub MnuThemeLight_Click(sender As Object, e As EventArgs) Handles MnuThemeLight.Click
        Dim settings = AppSettings.Load()
        settings.Appearance.Theme = AppSettings.ThemeMode.Light
        settings.Save()
        UpdateFormTheme()
        MnuThemeSystem.Checked = False
        MnuThemeDark.Checked = False
        MnuThemeLight.Checked = True
    End Sub
    Private Sub MnuThemeDark_Click(sender As Object, e As EventArgs) Handles MnuThemeDark.Click
        Dim settings = AppSettings.Load()
        settings.Appearance.Theme = AppSettings.ThemeMode.Dark
        settings.Save()
        UpdateFormTheme()
        MnuThemeSystem.Checked = False
        MnuThemeDark.Checked = True
        MnuThemeLight.Checked = False
    End Sub
    Private Sub MnuViewPlay_Click(sender As Object, e As EventArgs) Handles MnuViewPlay.Click
        '待开发
    End Sub
    Private Sub MnuAdvancedSearch_Click(sender As Object, e As EventArgs) Handles MnuAdvancedSearch.Click
        '待开发
    End Sub
    Private Sub MnuSearch_Click(sender As Object, e As EventArgs) Handles MnuSearch.Click
        MnuSearchTxtbox.Focus()
    End Sub
    Private Sub MnuSelectAll_Click(sender As Object, e As EventArgs) Handles MnuSelectAll.Click
        ImageGalleryMain.SelectAll()
    End Sub
    Private Sub MnuSelectReverse_Click(sender As Object, e As EventArgs) Handles MnuSelectReverse.Click
        ImageGalleryMain.SelectReverse()
    End Sub
    Private Sub MnuPageUp_Click(sender As Object, e As EventArgs) Handles MnuPageUp.Click
        ImageGalleryMain.SetPage(ImageGalleryMain.Page - 1)
        UpdatePageMenu()
    End Sub
    Private Sub MnuPageDown_Click(sender As Object, e As EventArgs) Handles MnuPageDown.Click
        ImageGalleryMain.SetPage(ImageGalleryMain.Page + 1)
        UpdatePageMenu()
    End Sub
    Private Sub UpdatePageMenu()
        If ImageGalleryMain.Page = 1 Then
            MnuPageUp.Enabled = False
            TSBtnPrevPage.Enabled = False
        Else
            MnuPageUp.Enabled = True
            TSBtnPrevPage.Enabled = True
        End If
        If ImageGalleryMain.Page = ImageGalleryMain.TotalPages Then
            MnuPageDown.Enabled = False
            TSBtnNextPage.Enabled = False
        Else
            MnuPageDown.Enabled = True
            TSBtnNextPage.Enabled = True
        End If
    End Sub
#End Region

#Region "关于菜单项"
    Private Sub MnuHelpAbout_Click(sender As Object, e As EventArgs) Handles MnuHelpAbout.Click
        AboutForm.ShowDialog()
    End Sub
    Private Sub MnuHelpGithub_Click(sender As Object, e As EventArgs) Handles MnuHelpGithub.Click
        Process.Start("https://github.com/PawLaboratory/FurryArtStudio")
    End Sub
    Private Sub MnuBugReport_Click(sender As Object, e As EventArgs) Handles MnuBugReport.Click
        Process.Start("https://github.com/PawLaboratory/FurryArtStudio/issues/new?template=bug_report.yml")
    End Sub
    Private Sub MnuSuggestions_Click(sender As Object, e As EventArgs) Handles MnuSuggestions.Click
        Process.Start("https://github.com/PawLaboratory/FurryArtStudio/issues/new?template=feature_request.yml")
    End Sub
    Private Sub MnuMyFeedback_Click(sender As Object, e As EventArgs) Handles MnuMyFeedback.Click
        Process.Start("https://github.com/PawLaboratory/FurryArtStudio/issues?q=is%3Aissue%20author%3A%40me")
    End Sub
    Private Sub MnuHelpWebsite_Click(sender As Object, e As EventArgs) Handles MnuHelpWebsite.Click

    End Sub
    Private Async Sub MnuLoadSponsors_Click(sender As Object, e As EventArgs) Handles MnuLoadSponsors.Click
        Try
            Await CheckSponsors()
        Catch ex As Exception
            ShowErrorDialog(ex, "")
        End Try
    End Sub
    Private Async Function CheckSponsors() As Task
        StatusLabel.Text = My.Resources.Msg_CheckingSponsor
        Dim sponsorInfo = Await AfdianHandler.CheckSponsorsAsync()
        MnuHelpDonate.DropDownItems.Clear()
        MnuHelpDonate.DropDownItems.Add(MnuOpenAfdian) '打开爱发电
        MnuHelpDonate.DropDownItems.Add(MnuLoadSponsors) '加载赞助者信息
        MnuHelpDonate.DropDownItems.Add(New ToolStripSeparator) '分割线
        If sponsorInfo.Count = 0 Then
            Dim sponsorMenuItem As New ToolStripMenuItem() With {
                .Text = My.Resources.Msg_NoSponsor,
                .Enabled = False
            }
            MnuHelpDonate.DropDownItems.Add(sponsorMenuItem)
            Return
        End If
        For Each sponsor In sponsorInfo
            Dim sponsorMenuItem As New ToolStripMenuItem With {
                .Tag = sponsor.UserID,
                .Image = sponsor.UserAvatar,
                .Text = sponsor.UserName
            }
            MnuHelpDonate.DropDownItems.Add(sponsorMenuItem)
            AddHandler sponsorMenuItem.Click, AddressOf OnSponsorClick
        Next
        StatusLabel.Text = My.Resources.Stat_Ready
    End Function
    Private Sub OnSponsorClick(sender As Object, e As EventArgs)
        Dim menuItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
        If menuItem IsNot Nothing Then
            Dim userID As String = TryCast(menuItem.Tag, String)
            If Not String.IsNullOrEmpty(userID) Then
                Process.Start($"https://ifdian.net/u/{userID}")
            End If
        End If
    End Sub
    Private Sub MnuOpenAfdian_Click(sender As Object, e As EventArgs) Handles MnuOpenAfdian.Click
        Process.Start("https://ifdian.net/a/xionglongztz")
    End Sub
    Private Sub MnuHelpLicense_Click(sender As Object, e As EventArgs) Handles MnuHelpLicense.Click
        Dim txt As New TextBoxForm(My.Resources.Licenses.LicenseText, My.Resources.Main_StrLicense)
        txt.Show()
    End Sub
    Private Sub MnuHelpPrivacy_Click(sender As Object, e As EventArgs) Handles MnuHelpPrivacy.Click
        Dim txt As New TextBoxForm(My.Resources.Licenses.PrivacyText, My.Resources.Main_StrPrivacy)
        txt.Show()
    End Sub
    Private Sub MnuHelpTutorial_Click(sender As Object, e As EventArgs) Handles MnuHelpTutorial.Click
        Process.Start("https://github.com/PawLaboratory/FurryArtStudio/wiki")
    End Sub
    Private Async Sub MnuCheckUpdate_Click(sender As Object, e As EventArgs) Handles MnuCheckUpdate.Click
        Await CheckForUpdate()
    End Sub
    Private Async Function CheckForUpdate() As Task
        StatusLabel.Text = My.Resources.Msg_CheckingUpdate
        Dim updateInfo = Await UpdateChecker.CheckForUpdateAsync()
        If updateInfo.HasError Then '检查更新失败
            Using dlg As New TaskDialog With {
                .WindowTitle = My.Resources.FurryArtStudio,
                .MainInstruction = My.Resources.Msg_CheckUpdateFailed,
                .Content = updateInfo.ErrorMessage,
                .MainIcon = TaskDialogIcon.Error
                }
                dlg.Buttons.Add(New TaskDialogButton(ButtonType.Ok))
                dlg.ShowDialog()
            End Using
        ElseIf updateInfo.IsUpdateAvailable Then '有新版本可用
            Dim buttonDownload As New TaskDialogButton(My.Resources.Msg_DownloadNewVer)
            Using dlg As New TaskDialog With {
                .WindowTitle = My.Resources.FurryArtStudio,
                .MainInstruction = String.Format(My.Resources.Msg_NewVerFound, updateInfo.LatestVersion),
                .Content = updateInfo.ReleaseNotes,
                .MainIcon = TaskDialogIcon.Information
                }
                dlg.Buttons.Add(New TaskDialogButton(ButtonType.Cancel))
                dlg.Buttons.Add(buttonDownload)
                If dlg.ShowDialog() Is buttonDownload Then
                    Process.Start(updateInfo.DownloadUrl) '打开下载链接
                End If
            End Using
        Else '已是最新版本
            Using dlg As New TaskDialog With {
                .WindowTitle = My.Resources.FurryArtStudio,
                .MainInstruction = My.Resources.Msg_UptoDate,
                .Content = updateInfo.LatestVersion,
                .MainIcon = TaskDialogIcon.Information
                }
                dlg.Buttons.Add(New TaskDialogButton(ButtonType.Ok))
                dlg.ShowDialog()
            End Using
        End If
        StatusLabel.Text = My.Resources.Stat_Ready
    End Function
    Private Sub MnuHelpWhatsNew_Click(sender As Object, e As EventArgs) Handles MnuHelpWhatsNew.Click
        Dim txt As New TextBoxForm(My.Resources.Licenses.WhatsNewText, My.Resources.Main_StrWhatsNew)
        txt.Show()
    End Sub
    Private Sub MnuTerms_Click(sender As Object, e As EventArgs) Handles MnuTerms.Click
        Dim txt As New TextBoxForm(My.Resources.Licenses.TermsText, My.Resources.Main_StrTerms)
        txt.Show()
    End Sub
#End Region

#Region "弹出菜单"
    Private Sub ConMnuMsView_Click(sender As Object, e As EventArgs) Handles ConMnuMsView.Click
        MnuMsView.PerformClick()
    End Sub
    Private Sub ConMnuMsEdit_Click(sender As Object, e As EventArgs) Handles ConMnuMsEdit.Click
        MnuMsEdit.PerformClick()
    End Sub
    Private Sub ConMnuMsExport_Click(sender As Object, e As EventArgs) Handles ConMnuMsExport.Click
        MnuMsExport.PerformClick()
    End Sub
    Private Sub ConMnuMsPrint_Click(sender As Object, e As EventArgs) Handles ConMnuMsPrint.Click
        MnuMsPrint.PerformClick()
    End Sub
    Private Sub ConMnuMsDelete_Click(sender As Object, e As EventArgs) Handles ConMnuMsDelete.Click
        MnuMsDelete.PerformClick()
    End Sub
    Private Sub ConMnuMsOpenFolder_Click(sender As Object, e As EventArgs) Handles ConMnuMsOpenFolder.Click
        MnuMsOpenFolder.PerformClick()
    End Sub
    Private Sub ConMnuMsCopy_Click(sender As Object, e As EventArgs) Handles ConMnuMsCopy.Click
        MnuMsCopy.PerformClick()
    End Sub
    Private Sub ConMnuMsCopyPath_Click(sender As Object, e As EventArgs) Handles ConMnuMsCopyPath.Click
        MnuMsCopyPath.PerformClick()
    End Sub
#End Region

#Region "工具栏按钮"
    Private Sub TSBtnOnTop_Click(sender As Object, e As EventArgs) Handles TSBtnOnTop.Click
        MnuOnTop.PerformClick()
    End Sub
    Private Sub TSBtnProperties_Click(sender As Object, e As EventArgs) Handles TSBtnProperties.Click
        MnuProperties.PerformClick()
    End Sub
    Private Sub TSBtnRefresh_Click(sender As Object, e As EventArgs) Handles TSBtnRefresh.Click
        MnuLibRefresh.PerformClick()
    End Sub
    Private Sub TSBtnLibNew_Click(sender As Object, e As EventArgs) Handles TSBtnLibNew.Click
        MnuLibNew.PerformClick()
    End Sub
    Private Sub TSBtnLibImport_Click(sender As Object, e As EventArgs) Handles TSBtnLibImport.Click
        MnuLibImport.PerformClick()
    End Sub
    Private Sub TSBtnLibExport_Click(sender As Object, e As EventArgs) Handles TSBtnLibExport.Click
        MnuLibExport.PerformClick()
    End Sub
    Private Sub TSBtnLibRename_Click(sender As Object, e As EventArgs) Handles TSBtnLibRename.Click
        MnuLibRename.PerformClick()
    End Sub
    Private Sub TSBtnLibClose_Click(sender As Object, e As EventArgs) Handles TSBtnLibClose.Click
        MnuLibClose.PerformClick()
    End Sub
    Private Sub TSBtnMsNew_Click(sender As Object, e As EventArgs) Handles TSBtnMsNew.Click
        MnuMsNew.PerformClick()
    End Sub
    Private Sub TSBtnMsImport_Click(sender As Object, e As EventArgs) Handles TSBtnMsImport.Click
        MnuMsImport.PerformClick()
    End Sub
    Private Sub TSBtnMsExport_Click(sender As Object, e As EventArgs) Handles TSBtnMsExport.Click
        MnuMsExport.PerformClick()
    End Sub
    Private Sub TSBtnMsEdit_Click(sender As Object, e As EventArgs) Handles TSBtnMsEdit.Click
        MnuMsEdit.PerformClick()
    End Sub
    Private Sub TSBtnMsPrint_Click(sender As Object, e As EventArgs) Handles TSBtnMsPrint.Click
        MnuMsPrint.PerformClick()
    End Sub
    Private Sub TSBtnMsOpenFolder_Click(sender As Object, e As EventArgs) Handles TSBtnMsOpenFolder.Click
        MnuMsOpenFolder.PerformClick()
    End Sub
    Private Sub TSBtnPlay_Click(sender As Object, e As EventArgs) Handles TSBtnPlay.Click
        MnuViewPlay.PerformClick()
    End Sub
    Private Sub TSBtnSearch_Click(sender As Object, e As EventArgs) Handles TSBtnSearch.Click
        MnuAdvancedSearch.PerformClick()
    End Sub
    Private Sub TSBtnPrevPage_Click(sender As Object, e As EventArgs) Handles TSBtnPrevPage.Click
        MnuPageUp.PerformClick()
    End Sub
    Private Sub TSBtnNextPage_Click(sender As Object, e As EventArgs) Handles TSBtnNextPage.Click
        MnuPageDown.PerformClick()
    End Sub
#End Region

#End Region

#Region "图片墙功能"
    Private Sub ClearSelect()
        MnuMsView.Enabled = False
        MnuMsEdit.Enabled = False
        MnuMsDelete.Enabled = False
        MnuMsExport.Enabled = False
        MnuMsPrint.Enabled = False
        MnuMsOpenFolder.Enabled = False
        MnuMsCopy.Enabled = False
        MnuMsCopyPath.Enabled = False
        ConMnuMsView.Enabled = False
        ConMnuMsEdit.Enabled = False
        ConMnuMsDelete.Enabled = False
        ConMnuMsExport.Enabled = False
        ConMnuMsPrint.Enabled = False
        ConMnuMsOpenFolder.Enabled = False
        ConMnuMsCopy.Enabled = False
        ConMnuMsCopyPath.Enabled = False
        TSBtnMsExport.Enabled = False
        TSBtnMsEdit.Enabled = False
        TSBtnMsPrint.Enabled = False
        TSBtnMsOpenFolder.Enabled = False
        SelectStatusLabel.Text = String.Format(My.Resources.Main_LblMs, _artworkCount)
        ClearMetadata()
        LblTitle.Text = My.Resources.Main_LblNoSelect
        LblDeltas.Visible = False
    End Sub
    Private Sub ImageGalleryMain_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ImageGalleryMain.SelectionChanged
        Dim selectedImages = e.SelectedImages
        Dim selectedCount As Integer = selectedImages.Count
        If selectedImages.Count = 0 Then
            ClearSelect()
        ElseIf selectedImages.Count = 1 Then
            Dim selectedImage = selectedImages(0)
            Dim selectedArtwork = _libraryManager.GetCurrentLibrary.GetArtworkByUUID(Guid.Parse(selectedImage.UUID))
            MnuMsView.Enabled = True
            MnuMsEdit.Enabled = True
            MnuMsExport.Enabled = True
            MnuMsPrint.Enabled = True
            MnuMsOpenFolder.Enabled = True
            MnuMsCopy.Enabled = True
            MnuMsCopyPath.Enabled = True
            MnuMsDelete.Enabled = True
            ConMnuMsView.Enabled = True
            ConMnuMsEdit.Enabled = True
            ConMnuMsDelete.Enabled = True
            ConMnuMsExport.Enabled = True
            ConMnuMsPrint.Enabled = True
            ConMnuMsOpenFolder.Enabled = True
            ConMnuMsCopy.Enabled = True
            ConMnuMsCopyPath.Enabled = True
            TSBtnMsExport.Enabled = True
            TSBtnMsEdit.Enabled = True
            TSBtnMsPrint.Enabled = True
            TSBtnMsOpenFolder.Enabled = True
            PiChkThumb.Visible = True
            SelectStatusLabel.Text = String.Format(My.Resources.Main_LblSelectMs1, _artworkCount)
            PiChkThumb.Image = selectedImage.Thumbnail
            Dim artCount As Integer = selectedImages(0).Count
            If artCount > 1 Then '当数量大于1时, 显示实际数量
                LblDeltas.Text = String.Format(My.Resources.Main_LblDifference, artCount)
                LblDeltas.Visible = True
            Else
                LblDeltas.Visible = False
            End If
            LblTitle.Text = $"{selectedArtwork.Title}"
            LblAuthor.Text = $"{selectedArtwork.Author}"
            Dim roleCount As Integer = selectedArtwork.Characters.Count
            If roleCount = 0 Then
                LblCharacters.Visible = False
            Else
                LblCharacters.Text = String.Format(My.Resources.Main_LblCharacter, FormatArrayWithEllipsis(selectedArtwork.Characters))
                LblCharacters.Visible = True
            End If
            Dim tagCount As Integer = selectedArtwork.Tags.Count
            If tagCount = 0 Then
                LblTags.Visible = False
            Else
                LblTags.Text = String.Format(My.Resources.Main_LblTags, FormatArrayWithEllipsis(selectedArtwork.Tags))
                LblTags.Visible = True
            End If
            Dim artNote As String = selectedArtwork.Notes
            If artNote = "" Then
                LblNotes.Visible = False
            Else
                LblNotes.Text = String.Format(My.Resources.Main_LblNotes, artNote)
                LblNotes.Visible = True
            End If
        ElseIf selectedCount > 1 Then '选择多个时
            MnuMsView.Enabled = False
            MnuMsEdit.Enabled = False
            MnuMsDelete.Enabled = True
            MnuMsExport.Enabled = True
            MnuMsPrint.Enabled = False
            MnuMsOpenFolder.Enabled = True
            MnuMsCopy.Enabled = True
            MnuMsCopyPath.Enabled = True
            ConMnuMsView.Enabled = False
            ConMnuMsEdit.Enabled = False
            ConMnuMsDelete.Enabled = True
            ConMnuMsExport.Enabled = True
            ConMnuMsPrint.Enabled = False
            ConMnuMsOpenFolder.Enabled = True
            ConMnuMsCopy.Enabled = True
            ConMnuMsCopyPath.Enabled = True
            TSBtnMsExport.Enabled = True
            TSBtnMsEdit.Enabled = False
            TSBtnMsPrint.Enabled = False
            TSBtnMsOpenFolder.Enabled = True
            SelectStatusLabel.Text = String.Format(My.Resources.Main_LblSelectMs, selectedCount, _artworkCount)
            ClearMetadata()
            LblTitle.Text = String.Format(My.Resources.Main_LblTitleSelect, selectedCount)
            LblDeltas.Visible = False
        End If
    End Sub
    Private Sub ImageGalleryMain_PageChanged(page As Integer) Handles ImageGalleryMain.PageChanged
        PageStatusLabel.Text = String.Format(My.Resources.Main_LblPage, page, ImageGalleryMain.TotalPages)
        UpdatePageMenu()
    End Sub
    Private Sub ImageGalleryMain_ImageDoubleClicked(image As GalleryImage) Handles ImageGalleryMain.ImageDoubleClicked
        ViewImage(Guid.Parse(image.UUID))
    End Sub
    Private Sub ImageGalleryMain_ImageRightClicked(image As GalleryImage) Handles ImageGalleryMain.ImageRightClicked
        ConMenu.Show(Control.MousePosition)
    End Sub
#End Region

#Region "图片浏览器"
    Private Sub ViewImage(uuid As Guid)
        Try
            Dim currentArtwork As Artwork = _libraryManager.GetCurrentLibrary.GetArtworkByUUID(uuid) '获取当前选中的稿件
            Dim allArtworks As List(Of Artwork) = _imageList '获取所有稿件列表
            If currentArtwork.FilePaths Is Nothing OrElse'检查当前稿件是否有图片
            currentArtwork.FilePaths.Length = 0 OrElse
            Not currentArtwork.FilePaths.Any(Function(p) IsImageFile(p)) Then
                Using dlg As New TaskDialog With {
                    .WindowTitle = My.Resources.FurryArtStudio,
                    .MainInstruction = My.Resources.Msg_NoImg,
                    .MainIcon = TaskDialogIcon.Information
                    }
                    dlg.Buttons.Add(New TaskDialogButton(ButtonType.Ok))
                    dlg.ShowDialog()
                End Using
                Return
            End If
            Dim viewForm As New ViewForm(currentArtwork, allArtworks)
            _openViewForms.Add(viewForm)
            AddHandler viewForm.FormClosed, Sub(sender, e)
                                                _openViewForms.Remove(viewForm)
                                            End Sub '订阅窗口关闭事件
            viewForm.Show() '创建并显示图片窗口
        Catch ex As Exception
            ShowErrorDialog(ex, My.Resources.Msg_ImageLoadFailed)
        End Try
    End Sub
    Public Sub CloseLibrary()
        RaiseEvent LibraryClosed(Me, EventArgs.Empty) '触发库关闭事件, 通知所有图片窗口
        _openViewForms.Clear()
    End Sub
#End Region

End Class
