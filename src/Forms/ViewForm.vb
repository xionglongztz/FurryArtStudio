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
Imports System.ComponentModel
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Media
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports PawLab.Chromis
Imports PawTheme = PawLab.WindowsTheme.ThemeService
Imports Ookii.Dialogs.WinForms
Public Class ViewForm
    Implements IThemeChangeable, ILocalizable

#Region "私有字段"
    '稿件
    Private _currentArtwork As Artwork '当前稿件
    Private _allArtworks As List(Of Artwork) '全部稿件列表
    '索引
    Private _currentFileIndex As Integer = 0 '当前文件索引
    Private _currentArtworkIndex As Integer = -1 '当前稿件索引
    '异步
    Private _isProcessing As Boolean = False '正在处理信号量
    Private _loadingLock As New Object() '锁对象
    Private _loadingTask As Task '异步加载任务
    Private _cancellationTokenSource As CancellationTokenSource '任务取消令牌
    '事件
    Private _mainForm As Form '保存主窗口引用
    '扩展名
    Private ReadOnly _imageExtensions As String() = {".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".ico", ".webp"}
    '菜单
    Private Const SC_PREVIMG = 1
    Private Const SC_NEXTIMG = 2
    Private Const SC_PREVART = 3
    Private Const SC_NEXTART = 4
    Private Const SC_ALWAYSONTOP = 5
    Private Const SC_COPY = 6
    Private Const SC_PROP = 7
    Private Const SC_INFO = 8
    Private Const SC_EXTRACT = 9
    Private Const SC_PLAY = 10
    Private Const SC_HELP = 11
    Private Const SC_FULLSCREEN = 12
    Private Const SC_KMEANS = 101
    Private Const SC_MEDIANCUT = 102
    Private Const SC_OCTREE = 103
    Private _hSubMenu As IntPtr '子菜单句柄
    '设置
    Private Settings As AppSettings = AppSettings.Load()
    Private useThemeColor As Boolean = False
    '全屏标志位
    Private _isFullScreen As Boolean = False
#End Region

#Region "窗体相关"
    ''' <summary>
    ''' 构造函数 - 接收当前稿件和所有稿件列表
    ''' </summary>
    Public Sub New(currentArtwork As Artwork, allArtworks As List(Of Artwork))
        InitializeComponent()
        _currentArtwork = currentArtwork
        _allArtworks = allArtworks
        _mainForm = MainForm
        If _allArtworks IsNot Nothing Then '查找当前稿件在所有稿件列表中的索引
            _currentArtworkIndex = _allArtworks.FindIndex(Function(a) a.ID = currentArtwork.ID)
        End If
        If TypeOf _mainForm Is MainForm Then
            AddHandler DirectCast(_mainForm, MainForm).LibraryClosed, AddressOf OnLibraryClosed
        End If
    End Sub
    ''' <summary>
    ''' 窗体加载事件
    ''' </summary>
    Private Sub ViewForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        Me.Text = My.Resources.View_ImageBrowser
        PictureBoxMain.SizeMode = PictureBoxSizeMode.Zoom
        PictureBoxMain.Dock = DockStyle.Fill
        SysMenuInit() '初始化菜单
        UpdateMenuStates() '更新菜单状态
        LanguageChange() '初始化语言
        SystemThemeChange() '初始化主题
        LoadCurrentArtworkFirstImage() '加载当前稿件的第一张图片
        useThemeColor = AppSettings.Load().Appearance.ViewWindowThemeColor '获取一个设置, 用来决定标题栏是否使用图片主颜色
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        Me.UpdateStyles()
    End Sub
    ''' <summary>
    ''' 窗体关闭时释放资源
    ''' </summary>
    Private Sub ViewForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If _mainForm IsNot Nothing AndAlso TypeOf _mainForm Is MainForm Then
            RemoveHandler DirectCast(_mainForm, MainForm).LibraryClosed, AddressOf OnLibraryClosed
        End If
        If PictureBoxMain.Image IsNot Nothing Then
            PictureBoxMain.Image.Dispose()
            PictureBoxMain.Image = Nothing
        End If
    End Sub
    ''' <summary>
    ''' 主题
    ''' </summary>
    Private Sub SystemThemeChange() Implements IThemeChangeable.SystemThemeChange
        If IsDarkMode() Then
            PictureBoxMain.BackColor = BgColorDark
            Icon = CreateRoundedRectangleIcon(True, My.Resources.Icons.FormImageDark)
            InitializeMenuImages(True) '设置菜单图标主题
        Else
            PictureBoxMain.BackColor = BgColorLight
            Icon = CreateRoundedRectangleIcon(False, My.Resources.Icons.FormImageLight)
            InitializeMenuImages()
        End If
        PawTheme.SetWindowTheme(Handle, IsDarkMode) 'PawLab.WindowsTheme
    End Sub
    ''' <summary>
    ''' 语言
    ''' </summary>
    Private Sub LanguageChange() Implements ILocalizable.LanguageChange
        UpdateMenuItem() '更新菜单项
    End Sub
    Private Sub UpdateMenuItem()
        Dim menuHandle = GetSystemMenu(Handle, False)
        '设置菜单项快捷键
        SetMenuItemWithShortcut(menuHandle, 0, SC_PREVIMG, My.Resources.View_PreviousImg, "PageUp")
        SetMenuItemWithShortcut(menuHandle, 1, SC_NEXTIMG, My.Resources.View_NextImg, "PageDown")
        SetMenuItemWithShortcut(menuHandle, 3, SC_PREVART, My.Resources.View_PreviousMs, "Ctrl+Left")
        SetMenuItemWithShortcut(menuHandle, 4, SC_NEXTART, My.Resources.View_NextMs, "Ctrl+Right")
        SetMenuItemWithShortcut(menuHandle, 6, SC_ALWAYSONTOP, My.Resources.Mnu_AlwaysOnTop, "Alt+T")
        SetMenuItemWithShortcut(menuHandle, 7, SC_COPY, My.Resources.View_Copy, "Ctrl+C")
        SetMenuItemWithShortcut(menuHandle, 9, SC_FULLSCREEN, My.Resources.View_Fullscreen, "F11")
        SetMenuItemWithShortcut(menuHandle, 16, SC_PROP, My.Resources.View_FileProperties, "Alt+Enter")
        SetMenuItemWithShortcut(menuHandle, 17, SC_INFO, My.Resources.View_Info, "I")
        SetMenuItemWithShortcut(menuHandle, 19, SC_PLAY, My.Resources.Mnu_Play, "Ctrl+F5")
        SetMenuItemWithShortcut(menuHandle, 20, SC_HELP, My.Resources.View_Help, "F1")
        SetMenuItemWithShortcut(_hSubMenu, 0, SC_KMEANS, My.Resources.View_KMeansCluster, "Alt+1")
        SetMenuItemWithShortcut(_hSubMenu, 1, SC_MEDIANCUT, My.Resources.View_MedianCut, "Alt+2")
        SetMenuItemWithShortcut(_hSubMenu, 2, SC_OCTREE, My.Resources.View_Octree, "Alt+3")
    End Sub
    ''' <summary>
    ''' 初始化系统菜单
    ''' </summary>
    Private Sub SysMenuInit()
        Dim hSubMenu As IntPtr = CreatePopupMenu() '新建一个子菜单
        _hSubMenu = hSubMenu
        AppendMenu(hSubMenu, MF_STRING, CType(SC_KMEANS, UIntPtr), My.Resources.View_KMeansCluster)
        AppendMenu(hSubMenu, MF_STRING, CType(SC_MEDIANCUT, UIntPtr), My.Resources.View_MedianCut)
        AppendMenu(hSubMenu, MF_STRING, CType(SC_OCTREE, UIntPtr), My.Resources.View_Octree)
        Dim menuHandle = GetSystemMenu(Handle, False) '获取菜单句柄
        InsertMenu(menuHandle, 0, MF_BYPOSITION Or MF_STRING, SC_PREVIMG, My.Resources.View_PreviousImg)
        InsertMenu(menuHandle, 1, MF_BYPOSITION Or MF_STRING, SC_NEXTIMG, My.Resources.View_NextImg)
        InsertMenu(menuHandle, 2, MF_BYPOSITION Or MF_SEPARATOR, 0, Nothing)
        InsertMenu(menuHandle, 3, MF_BYPOSITION Or MF_STRING, SC_PREVART, My.Resources.View_PreviousMs)
        InsertMenu(menuHandle, 4, MF_BYPOSITION Or MF_STRING, SC_NEXTART, My.Resources.View_NextMs)
        InsertMenu(menuHandle, 5, MF_BYPOSITION Or MF_SEPARATOR, 0, Nothing)
        InsertMenu(menuHandle, 6, MF_BYPOSITION Or MF_STRING, SC_ALWAYSONTOP, My.Resources.Mnu_AlwaysOnTop)
        InsertMenu(menuHandle, 7, MF_BYPOSITION Or MF_STRING, SC_COPY, My.Resources.View_Copy)
        InsertMenu(menuHandle, 8, MF_BYPOSITION Or MF_SEPARATOR, 0, Nothing)
        InsertMenu(menuHandle, 9, MF_BYPOSITION Or MF_STRING, SC_FULLSCREEN, My.Resources.View_Fullscreen)
        InsertMenu(menuHandle, 16, MF_BYPOSITION Or MF_STRING, SC_PROP, My.Resources.View_FileProperties)
        InsertMenu(menuHandle, 17, MF_BYPOSITION Or MF_STRING, SC_INFO, My.Resources.View_Info)
        InsertMenu(menuHandle, 18, MF_BYPOSITION Or MF_POPUP, hSubMenu, My.Resources.View_Extract)
        InsertMenu(menuHandle, 19, MF_BYPOSITION Or MF_STRING, SC_PLAY, My.Resources.Mnu_Play)
        InsertMenu(menuHandle, 20, MF_BYPOSITION Or MF_STRING, SC_HELP, My.Resources.View_Help)
        InsertMenu(menuHandle, 21, MF_BYPOSITION Or MF_SEPARATOR, 0, Nothing)
    End Sub
    Private Sub InitializeMenuImages(Optional isDarkMode As Boolean = False)
        Dim menuHandle = GetSystemMenu(Handle, False) '设置窗体菜单
        If isDarkMode Then
            ApplyMenuIcon(menuHandle, SC_PREVIMG, My.Resources.Icons.MenuPreviousDark, True)
            ApplyMenuIcon(menuHandle, SC_NEXTIMG, My.Resources.Icons.MenuNextDark, True)
            ApplyMenuIcon(menuHandle, SC_PREVART, My.Resources.Icons.MenuLeftDark, True)
            ApplyMenuIcon(menuHandle, SC_NEXTART, My.Resources.Icons.MenuRightDark, True)
            ApplyMenuIcon(menuHandle, SC_ALWAYSONTOP, My.Resources.Icons.MenuPinDark, True)
            ApplyMenuIcon(menuHandle, SC_COPY, My.Resources.Icons.MenuCopyDark, True)
            ApplyMenuIcon(menuHandle, SC_PROP, My.Resources.Icons.FormFileDark, True)
            ApplyMenuIcon(menuHandle, SC_INFO, My.Resources.Icons.MenuInfoDark, True)
            ApplyMenuIcon(menuHandle, _hSubMenu, My.Resources.Icons.MenuExtractDark, True)
            ApplyMenuIcon(menuHandle, SC_PLAY, My.Resources.Icons.MenuImagePlayDark, True)
            ApplyMenuIcon(menuHandle, SC_HELP, My.Resources.Icons.MenuTutorialDark, True)
            ApplyMenuIcon(menuHandle, SC_FULLSCREEN, My.Resources.Icons.MenuFullscreenDark, True)
        Else
            ApplyMenuIcon(menuHandle, SC_PREVIMG, My.Resources.Icons.MenuPreviousLight)
            ApplyMenuIcon(menuHandle, SC_NEXTIMG, My.Resources.Icons.MenuNextLight)
            ApplyMenuIcon(menuHandle, SC_PREVART, My.Resources.Icons.MenuLeftLight)
            ApplyMenuIcon(menuHandle, SC_NEXTART, My.Resources.Icons.MenuRightLight)
            ApplyMenuIcon(menuHandle, SC_ALWAYSONTOP, My.Resources.Icons.MenuPinLight)
            ApplyMenuIcon(menuHandle, SC_COPY, My.Resources.Icons.MenuCopyLight)
            ApplyMenuIcon(menuHandle, SC_PROP, My.Resources.Icons.FormFileLight)
            ApplyMenuIcon(menuHandle, SC_INFO, My.Resources.Icons.MenuInfoLight)
            ApplyMenuIcon(menuHandle, _hSubMenu, My.Resources.Icons.MenuExtractLight)
            ApplyMenuIcon(menuHandle, SC_PLAY, My.Resources.Icons.MenuImagePlayLight)
            ApplyMenuIcon(menuHandle, SC_HELP, My.Resources.Icons.MenuTutorialLight)
            ApplyMenuIcon(menuHandle, SC_FULLSCREEN, My.Resources.Icons.MenuFullscreenLight)
        End If
    End Sub
    Protected Overrides Sub WndProc(ByRef m As Message) '窗体消息处理函数
        If m.Msg = WM_SYSCOMMAND Then '窗体响应菜单
            Dim imageFiles As List(Of String) = GetCurrentArtworkImages()
            Dim filePath As String = imageFiles(_currentFileIndex)
            Dim hMenu = GetSystemMenu(Handle, False)
            Select Case m.WParam.ToInt32'对应菜单标号
                Case SC_PREVIMG '上一张
                    NavigatePrevious()
                Case SC_NEXTIMG '下一张
                    NavigateNext()
                Case SC_PREVART '上个稿件
                    NavigatePreviousArtwork()
                Case SC_NEXTART '下个稿件
                    NavigateNextArtwork()
                Case SC_ALWAYSONTOP '窗口置顶
                    SetWindowOnTop()
                Case SC_COPY '复制
                    Clipboard.SetImage(PictureBoxMain.Image)
                Case SC_PROP '文件属性
                    ShowProperties(filePath)
                Case SC_INFO '详情
                    ShowArtworkInfo()
                Case SC_PLAY'幻灯片放映
                    '待开发
                Case SC_HELP '帮助
                    ShowHelp()
                Case SC_FULLSCREEN '全屏
                    ToggleFullScreen()
                Case SC_KMEANS
                    UpdateExtractColor(PictureBoxMain.Image, ExtractType.KMeans)
                Case SC_MEDIANCUT
                    UpdateExtractColor(PictureBoxMain.Image, ExtractType.MedianCut)
                Case SC_OCTREE
                    UpdateExtractColor(PictureBoxMain.Image, ExtractType.Octree)
            End Select
        End If
        MyBase.WndProc(m) '循环监听消息
    End Sub
#End Region

#Region "辅助函数"
    ''' <summary>
    ''' 在新的窗口显示提取的颜色
    ''' </summary>
    ''' <param name="img">图像</param>
    ''' <param name="extractType">处理类型</param>
    Private Sub UpdateExtractColor(img As Image, extractType As ExtractType)
        Dim pixels As New List(Of RGBColor)
        For Each color In GetPixelsFromImageFast(img) '使用更快的速率提取颜色
            pixels.Add(RGBColor.FromRGB(color.R, color.G, color.B))
        Next
        Dim colors = ColorExtractor.Extract(pixels, 10, extractType)
        Dim colorLabels() = {ColorDialogForm.L1, ColorDialogForm.L2, ColorDialogForm.L3, ColorDialogForm.L4,
            ColorDialogForm.L5, ColorDialogForm.L6, ColorDialogForm.L7, ColorDialogForm.L8,
            ColorDialogForm.L9, ColorDialogForm.L10}
        For Each label In colorLabels '先强制清除所有属性
            label.Text = ""
            If IsDarkMode() Then label.BackColor = BgColorDark Else label.BackColor = BgColorLight
        Next
        For i As Integer = 0 To colors.Count - 1 '再重新赋值
            colorLabels(i).Text = $"RGB({colors(i).Color.R}, {colors(i).Color.G}, {colors(i).Color.B}) ({colors(i).Color.ToHex}) - {colors(i).Ratio:P2}"
            colorLabels(i).BackColor = Color.FromArgb(colors(i).Color.R, colors(i).Color.G, colors(i).Color.B)
            colorLabels(i).ForeColor = GetForeColor(colorLabels(i).BackColor)
        Next
        Select Case extractType
            Case ExtractType.KMeans
                ColorDialogForm.Text = My.Resources.ColorDialog_Title & " - K-Means"
            Case ExtractType.MedianCut
                ColorDialogForm.Text = My.Resources.ColorDialog_Title & " - Median Cut"
            Case ExtractType.Octree
                ColorDialogForm.Text = My.Resources.ColorDialog_Title & " - Octree"
        End Select
        ColorDialogForm.ShowDialog() '以对话框显示窗口
    End Sub
    ''' <summary>
    ''' 从文件路径数组中过滤出图片文件
    ''' </summary>
    ''' <param name="filePaths">文件夹</param>
    ''' <returns>图片文件路径</returns>
    Private Function GetImageFiles(filePaths As String()) As List(Of String)
        Dim result As New List(Of String)
        If filePaths Is Nothing Then Return result '没有文件
        For Each p In filePaths
            Dim ext As String = Path.GetExtension(p).ToLower()
            If _imageExtensions.Contains(ext) Then
                result.Add(p)
            End If
        Next
        '按文件名排序
        'result = result.OrderBy(Function(p) p, New NaturalStringComparer()).ToList()
        result.Sort(Function(a, b) StrCmpLogicalW(Path.GetFileName(a), Path.GetFileName(b)))
        Return result
    End Function

#Region "自然字符串排序比较器"
    Public Class NaturalStringComparer
        Implements IComparer(Of String)
        Private Shared ReadOnly _regex As New Regex("\d+", RegexOptions.Compiled)
        Public Function Compare(x As String, y As String) As Integer Implements IComparer(Of String).Compare
            Return CompareNatural(x, y)
        End Function
        Private Shared Function CompareNatural(x As String, y As String) As Integer
            '提取文件名
            Dim xFile = Path.GetFileNameWithoutExtension(x)
            Dim yFile = Path.GetFileNameWithoutExtension(y)
            Dim xMatches = _regex.Matches(xFile)
            Dim yMatches = _regex.Matches(yFile)
            '如果没有数字, 使用普通字符串比较
            If xMatches.Count = 0 OrElse yMatches.Count = 0 Then
                Return String.Compare(xFile, yFile, StringComparison.OrdinalIgnoreCase)
            End If
            '逐个比较数字部分
            Dim i As Integer = 0
            While i < Math.Min(xMatches.Count, yMatches.Count)
                Dim xNum As Integer
                Dim yNum As Integer
                Dim xOk = Integer.TryParse(xMatches(i).Value, xNum)
                Dim yOk = Integer.TryParse(yMatches(i).Value, yNum)

                '如果解析失败, 回退到字符串比较
                If Not xOk OrElse Not yOk Then
                    Return String.Compare(xFile, yFile, StringComparison.OrdinalIgnoreCase)
                End If

                If xNum <> yNum Then
                    Return xNum.CompareTo(yNum)
                End If
                i += 1
            End While
            '如果数字部分都相同, 比较数字段的数量
            Return xMatches.Count.CompareTo(yMatches.Count)
        End Function
    End Class
#End Region

    ''' <summary>
    ''' 加载当前稿件的第一个有效图片
    ''' </summary>
    Private Sub LoadCurrentArtworkFirstImage()
        If _currentArtwork Is Nothing OrElse _currentArtwork.FilePaths Is Nothing Then
            MessageBox.Show("当前稿件目录为空", My.Resources.FurryArtStudio, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
            Return
        End If
        Dim imageFiles As List(Of String) = GetImageFiles(_currentArtwork.FilePaths)
        If imageFiles.Count = 0 Then
            MessageBox.Show("当前稿件没有支持的图片格式文件", My.Resources.FurryArtStudio, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
            Return
        End If
        _currentFileIndex = 0
        LoadImageAsync(imageFiles(_currentFileIndex))
    End Sub
    ''' <summary>
    ''' 获取当前稿件的所有图片文件
    ''' </summary>
    ''' <returns>图片文件路径</returns>
    Private Function GetCurrentArtworkImages() As List(Of String)
        If _currentArtwork Is Nothing OrElse _currentArtwork.FilePaths Is Nothing Then
            Return New List(Of String)
        End If
        Return GetImageFiles(_currentArtwork.FilePaths)
    End Function
    ''' <summary>
    ''' 更新窗口标题
    ''' </summary>
    ''' <param name="currentFilePath">当前文件路径</param>
    Private Sub UpdateWindowTitle(Optional currentFilePath As String = Nothing)
        If _currentArtwork Is Nothing Then '没有文件
            Me.Text = My.Resources.View_ImageBrowser
            Return
        End If
        Dim title As String = _currentArtwork.Title
        If String.IsNullOrWhiteSpace(title) Then '没有标题
            title = My.Resources.View_NoTitle
        End If
        Dim imageFiles As List(Of String) = GetCurrentArtworkImages()
        Dim totalImages As Integer = imageFiles.Count
        If totalImages = 0 Then
            Me.Text = $"{title} - 图片浏览器 [0/0]"
        Else
            Dim fileName As String = ""
            If currentFilePath IsNot Nothing Then
                fileName = Path.GetFileName(currentFilePath)
            End If
            '显示格式: 标题 - [当前文件索引/总文件数] 文件名 - 图片浏览器 (当前稿件索引/总稿件数)
            If _currentArtworkIndex >= 0 AndAlso _allArtworks IsNot Nothing Then
                Me.Text = $"{title} - [{_currentFileIndex + 1}/{totalImages}] {fileName} - 图片浏览器 ({_currentArtworkIndex + 1}/{_allArtworks.Count})"
            Else
                Me.Text = $"{title} - [{_currentFileIndex + 1}/{totalImages}] {fileName} - 图片浏览器"
            End If
            UpdateMenuStates() '同时更新菜单
            'Dim a As String = Settings.Appearance.ImageWindowTitleFormat
        End If
    End Sub
    ''' <summary>
    ''' 显示稿件信息
    ''' </summary>
    Private Sub ShowArtworkInfo()
        If _currentArtwork Is Nothing Then Return
        Dim imageFiles As List(Of String) = GetCurrentArtworkImages()
        Dim filePath As String = imageFiles(_currentFileIndex)
        Dim sb As New StringBuilder
        sb.Append(String.Format(My.Resources.View_Title, _currentArtwork.Title) & vbCrLf)
        sb.Append(String.Format(My.Resources.View_Author, _currentArtwork.Author) & vbCrLf)
        sb.Append(String.Format(My.Resources.View_UUID, _currentArtwork.UUID) & vbCrLf)
        If _currentArtwork.Characters.Length > 0 Then sb.Append(String.Format(My.Resources.View_Roles, FormatArrayWithEllipsis(_currentArtwork.Characters)) & vbCrLf)
        If _currentArtwork.Tags.Length > 0 Then sb.Append(String.Format(My.Resources.View_Tags, FormatArrayWithEllipsis(_currentArtwork.Tags)) & vbCrLf)
        sb.Append(String.Format(My.Resources.View_Create, _currentArtwork.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")) & vbCrLf)
        sb.Append(String.Format(My.Resources.View_Import, _currentArtwork.ImportTime.ToString("yyyy-MM-dd HH:mm:ss")) & vbCrLf)
        sb.Append(String.Format(My.Resources.View_Update, _currentArtwork.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")) & vbCrLf)
        If _currentArtwork.Notes <> "" Then sb.Append(String.Format(My.Resources.View_Notes, _currentArtwork.Notes) & vbCrLf)
        sb.Append(vbCrLf)
        Dim fi As New FileInfo(filePath)
        Dim imgWidth As Integer = 0
        Dim imgHeight As Integer = 0
        Dim bitDepth As Integer = 0
        Dim imgDpiX As Single = 0
        Dim imgDpiY As Single = 0
        Using img As Image = Image.FromFile(filePath) '这个方法相当耗时, 导致对话框延迟加载, 且占用大量内存
            imgWidth = img.Width
            imgHeight = img.Height
            bitDepth = GetBitDepth(img.PixelFormat)
            imgDpiX = img.HorizontalResolution
            imgDpiY = img.VerticalResolution
        End Using
        sb.Append(String.Format(My.Resources.View_Resolution, $"{imgWidth}×{imgHeight}") & vbCrLf)
        sb.Append(String.Format(My.Resources.View_Depth, bitDepth) & vbCrLf)
        sb.Append($"DPI: {imgDpiX}×{imgDpiY}" & vbCrLf)
        sb.Append(vbCrLf)
        sb.Append(String.Format(My.Resources.View_FilePath, filePath) & vbCrLf)
        Dim extension = Path.GetExtension(filePath).ToLowerInvariant() '获得扩展名
        sb.Append(String.Format(My.Resources.View_FileType, GetFileTypeDescription(extension)) & vbCrLf)
        sb.Append(String.Format(My.Resources.View_FileSize, FormatFileSize(fi.Length)) & vbCrLf)
        sb.Append(String.Format(My.Resources.View_FileCreateTime, fi.CreationTime.ToString) & vbCrLf)
        sb.Append(String.Format(My.Resources.View_FileModifyTime, fi.LastWriteTime) & vbCrLf)
        Dim buttonOpenFolder As New TaskDialogButton(My.Resources.View_OpenFolder) '打开路径
        Dim buttonCopyPath As New TaskDialogButton(My.Resources.View_CopyPath) '复制路径
        Dim buttonOpen As New TaskDialogButton(My.Resources.View_Open) '打开
        Using dlg As New TaskDialog With {
            .WindowTitle = My.Resources.FurryArtStudio,
            .Content = sb.ToString,
            .MainIcon = TaskDialogIcon.Information,
            .MainInstruction = My.Resources.View_ArtInfo
            }
            dlg.Buttons.Add(New TaskDialogButton(ButtonType.Cancel))
            dlg.Buttons.Add(buttonCopyPath)
            dlg.Buttons.Add(buttonOpenFolder)
            dlg.Buttons.Add(buttonOpen)
            Dim result As TaskDialogButton = dlg.ShowDialog()
            If result Is buttonOpenFolder Then
                Shell($"explorer /select,{filePath}", 1)
            ElseIf result Is buttonCopyPath Then
                Clipboard.SetDataObject(filePath)
            ElseIf result Is buttonOpen And My.Computer.Keyboard.ShiftKeyDown Then
                Process.Start("rundll32.exe", $"shell32.dll,OpenAs_RunDLL {filePath}")
            ElseIf result Is buttonOpen Then
                Process.Start(filePath)
            End If
        End Using
    End Sub
    ''' <summary>
    ''' 根据图像格式计算位深
    ''' </summary>
    Private Function GetBitDepth(pf As PixelFormat) As Integer
        Select Case pf
            Case PixelFormat.Format1bppIndexed
                Return 1
            Case PixelFormat.Format4bppIndexed
                Return 4
            Case PixelFormat.Format8bppIndexed
                Return 8
            Case PixelFormat.Format16bppGrayScale, PixelFormat.Format16bppRgb555, PixelFormat.Format16bppRgb565, PixelFormat.Format16bppArgb1555
                Return 16
            Case PixelFormat.Format24bppRgb
                Return 24
            Case PixelFormat.Format32bppRgb, PixelFormat.Format32bppArgb, PixelFormat.Format32bppPArgb
                Return 32
            Case PixelFormat.Format48bppRgb
                Return 48
            Case PixelFormat.Format64bppArgb, PixelFormat.Format64bppPArgb
                Return 64
            Case Else
                Return 0 '未知
        End Select
    End Function
    ''' <summary>
    ''' 显示帮助信息
    ''' </summary>
    Private Sub ShowHelp()
        Dim sb As New StringBuilder
        sb.Append(My.Resources.View_HelpPrevImg & vbCrLf)
        sb.Append(My.Resources.View_HelpNextImg & vbCrLf)
        sb.Append(My.Resources.View_HelpPrevArt & vbCrLf)
        sb.Append(My.Resources.View_HelpNextArt & vbCrLf)
        sb.Append(My.Resources.View_HelpFirstArt & vbCrLf)
        sb.Append(My.Resources.View_HelpLastArt & vbCrLf)
        sb.Append(My.Resources.View_HelpClose & vbCrLf)
        sb.Append(My.Resources.View_HelpInfo & vbCrLf)
        sb.Append(My.Resources.View_HelpShowProp & vbCrLf)
        sb.Append(My.Resources.View_HelpExtract & vbCrLf)
        sb.Append(My.Resources.View_HelpPlay & vbCrLf)
        sb.Append(My.Resources.View_HelpFullScreen & vbCrLf)
        sb.Append(My.Resources.View_HelpShowHelp & vbCrLf)
        ShowInfoDialog(sb.ToString, My.Resources.View_HelpShowInfo)
    End Sub
    ''' <summary>
    ''' 置顶
    ''' </summary>
    Private Sub SetWindowOnTop()
        Dim hMenu = GetSystemMenu(Handle, False)
        If TopMost = False Then
            TopMost = True
            CheckMenuItem(hMenu, SC_ALWAYSONTOP, MF_CHECKED) '窗口置顶
        Else
            TopMost = False
            CheckMenuItem(hMenu, SC_ALWAYSONTOP, MF_UNCHECKED) '取消置顶
        End If
    End Sub
    ''' <summary>
    ''' 当库关闭时, 本窗口也将关闭
    ''' </summary>
    Private Sub OnLibraryClosed(sender As Object, e As EventArgs)
        If Me.InvokeRequired Then
            Me.Invoke(Sub() Me.Close())
        Else
            Me.Close()
        End If
    End Sub
    ''' <summary>
    ''' 异步加载图片
    ''' </summary>
    ''' <param name="filePath">图片路径</param>
    Private Async Sub LoadImageAsync(filePath As String)
        Try
            Dim hMenu = GetSystemMenu(Me.Handle, False)
            SyncLock _loadingLock
                If _isProcessing Then Return  '防止重复进入
                _isProcessing = True
            End SyncLock
            For Each i In {SC_PREVIMG, SC_NEXTIMG, SC_PREVART, SC_NEXTART}
                EnableMenuItem(hMenu, i, MF_BYCOMMAND Or MF_GRAYED)
            Next
            For Each i In {SC_KMEANS, SC_MEDIANCUT, SC_OCTREE}
                EnableMenuItem(_hSubMenu, i, MF_BYCOMMAND Or MF_GRAYED)
            Next
            EnableMenuItem(hMenu, SC_COPY, MF_BYCOMMAND Or MF_GRAYED)
            '取消之前的加载任务
            _cancellationTokenSource?.Cancel()
            _cancellationTokenSource = New CancellationTokenSource()
            '显示加载提示
            PictureBoxMain.Image = Nothing
            Me.Text = My.Resources.View_Loading & Path.GetFileName(filePath)
            Me.Cursor = Cursors.WaitCursor
            '异步加载图片
            Dim image = Await Task.Run(Function() LoadImageWithResize(filePath, 1920, 1080, _cancellationTokenSource.Token),
                                       _cancellationTokenSource.Token)
            '检查是否被取消
            If _cancellationTokenSource.Token.IsCancellationRequested Then
                image?.Dispose()
                Return
            End If
            '更新UI
            If image IsNot Nothing Then
                '保存对旧图片的引用
                Dim oldImage = PictureBoxMain.Image
                '显示新图片
                PictureBoxMain.Image = image
                '释放旧图片
                If oldImage IsNot Nothing Then
                    oldImage.Dispose()
                End If
                UpdateWindowTitle(filePath)
                If useThemeColor Then '使用更快的方法读取粗略的颜色, 减少加载时间
                    Dim pixels As New List(Of RGBColor)
                    For Each color In GetPixelsFromImageFast(image, 50)
                        pixels.Add(RGBColor.FromRGB(color.R, color.G, color.B))
                    Next
                    Dim extractColor = ColorExtractor.Extract(pixels, 8, ExtractType.Octree)(0)
                    PawTheme.SetTitleBarColor(Handle, extractColor.Color.R, extractColor.Color.G, extractColor.Color.B)
                End If
            End If
        Catch ex As OperationCanceledException
            '忽略取消事件
        Catch ex As Exception
            If useThemeColor Then '当出现错误时回退到默认配色
                If IsDarkMode() Then
                    PawTheme.SetTitleBarColor(Handle, 0, 0, 0)
                Else
                    PawTheme.SetTitleBarColor(Handle, 255, 255, 255)
                End If
            End If
            ShowErrorDialog(ex, My.Resources.Msg_ImageLoadFailed)
        Finally
            '无论如何都要释放加载状态
            SyncLock _loadingLock
                _isProcessing = False
                Dim hMenu = GetSystemMenu(Me.Handle, False)
                EnableMenuItem(hMenu, SC_COPY, MF_BYCOMMAND Or MF_ENABLED)
                For Each i In {SC_KMEANS, SC_MEDIANCUT, SC_OCTREE} '子菜单可用
                    EnableMenuItem(_hSubMenu, i, MF_BYCOMMAND Or MF_ENABLED)
                Next
            End SyncLock
            Me.Cursor = Cursors.Default
            UpdateMenuStates()
        End Try
    End Sub
    ''' <summary>
    ''' 在后台线程中加载并调整图片大小
    ''' </summary>
    ''' <param name="filePath">文件路径</param>
    ''' <param name="maxWidth">最大宽度</param>
    ''' <param name="maxHeight">最大高度</param>
    ''' <param name="cancellationToken">取消令牌</param>
    ''' <returns>新的图片</returns>
    Private Function LoadImageWithResize(filePath As String, maxWidth As Integer,
                                         maxHeight As Integer, cancellationToken As CancellationToken) As Image
        If Not File.Exists(filePath) Then Return Nothing
        Using fs As New FileStream(filePath, FileMode.Open,
                                             FileAccess.Read,
                                             FileShare.Read, 4096, True)
            cancellationToken.ThrowIfCancellationRequested()
            '使用Image.FromStream避免文件锁定
            Using original As Image = Image.FromStream(fs)
                cancellationToken.ThrowIfCancellationRequested()
                '如果图片小于最大尺寸，直接返回副本
                If original.Width <= maxWidth AndAlso original.Height <= maxHeight Then
                    Return New Bitmap(original)
                End If
                '计算缩放尺寸
                Dim ratio As Double = Math.Min(maxWidth / original.Width, maxHeight / original.Height)
                Dim newWidth As Integer = CInt(original.Width * ratio)
                Dim newHeight As Integer = CInt(original.Height * ratio)
                '创建缩放的图片
                Dim resized As New Bitmap(newWidth, newHeight)
                Using g As Graphics = Graphics.FromImage(resized)
                    g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    g.DrawImage(original, 0, 0, newWidth, newHeight)
                End Using
                cancellationToken.ThrowIfCancellationRequested()
                Return resized
            End Using
        End Using
    End Function
#End Region

#Region "图片导航"
    ''' <summary>
    ''' 导航到下一张/下一个稿件
    ''' </summary>
    Private Sub NavigateNext()
        Dim currentImageFiles As List(Of String) = GetCurrentArtworkImages()
        '如果当前稿件还有下一张图片
        If _currentFileIndex < currentImageFiles.Count - 1 Then
            _currentFileIndex += 1
            LoadImageAsync(currentImageFiles(_currentFileIndex))
            Return
        End If
        '当前稿件没有下一张图片, 尝试切换到下一个稿件
        If _allArtworks IsNot Nothing AndAlso _currentArtworkIndex < _allArtworks.Count - 1 Then
            '找到下一个有图片的稿件
            For i As Integer = _currentArtworkIndex + 1 To _allArtworks.Count - 1
                Dim nextArtwork As Artwork = _allArtworks(i)
                Dim nextImageFiles As List(Of String) = GetImageFiles(nextArtwork.FilePaths)
                If nextImageFiles.Count > 0 Then
                    '切换到下一个稿件的第一个图片
                    _currentArtworkIndex = i
                    _currentArtwork = nextArtwork
                    _currentFileIndex = 0
                    LoadImageAsync(nextImageFiles(0))
                    Return
                End If
            Next
        End If
        SystemSounds.Asterisk.Play() '播放提示音
    End Sub
    ''' <summary>
    ''' 导航到上一张/上一个稿件
    ''' </summary>
    Private Sub NavigatePrevious()
        Dim currentImageFiles As List(Of String) = GetCurrentArtworkImages()
        '如果当前稿件还有上一张图片
        If _currentFileIndex > 0 Then
            _currentFileIndex -= 1
            LoadImageAsync(currentImageFiles(_currentFileIndex))
            Return
        End If
        '当前稿件没有上一张图片, 尝试切换到上一个稿件
        If _allArtworks IsNot Nothing AndAlso _currentArtworkIndex > 0 Then
            '找到上一个有图片的稿件
            For i As Integer = _currentArtworkIndex - 1 To 0 Step -1
                Dim prevArtwork As Artwork = _allArtworks(i)
                Dim prevImageFiles As List(Of String) = GetImageFiles(prevArtwork.FilePaths)
                If prevImageFiles.Count > 0 Then
                    '切换到上一个稿件的最后一张图片
                    _currentArtworkIndex = i
                    _currentArtwork = prevArtwork
                    _currentFileIndex = prevImageFiles.Count - 1
                    LoadImageAsync(prevImageFiles(_currentFileIndex))
                    Return
                End If
            Next
        End If
        SystemSounds.Asterisk.Play() '播放提示音
    End Sub
    ''' <summary>
    ''' 导航到第一张稿件
    ''' </summary>
    Private Sub NavigateToFirstArtwork()
        If _allArtworks Is Nothing Then Return
        For i As Integer = 0 To _allArtworks.Count - 1
            Dim imageFiles As List(Of String) = GetImageFiles(_allArtworks(i).FilePaths)
            If imageFiles.Count > 0 Then
                _currentArtworkIndex = i
                _currentArtwork = _allArtworks(i)
                _currentFileIndex = 0
                LoadImageAsync(imageFiles(0))
                Exit For
            End If
        Next
    End Sub
    ''' <summary>
    ''' 导航到最后一张稿件
    ''' </summary>
    Private Sub NavigateToLastArtwork()
        If _allArtworks Is Nothing Then Return
        For i As Integer = _allArtworks.Count - 1 To 0 Step -1
            Dim imageFiles As List(Of String) = GetImageFiles(_allArtworks(i).FilePaths)
            If imageFiles.Count > 0 Then
                _currentArtworkIndex = i
                _currentArtwork = _allArtworks(i)
                _currentFileIndex = 0
                LoadImageAsync(imageFiles(0))
                Exit For
            End If
        Next
    End Sub
    ''' <summary>
    ''' 导航到上一个稿件
    ''' </summary>
    Private Sub NavigatePreviousArtwork()
        If _allArtworks Is Nothing OrElse _allArtworks.Count = 0 Then Return
        '找到上一个有图片的稿件
        For i As Integer = _currentArtworkIndex - 1 To 0 Step -1
            Dim prevArtwork As Artwork = _allArtworks(i)
            Dim prevImageFiles As List(Of String) = GetImageFiles(prevArtwork.FilePaths)

            If prevImageFiles.Count > 0 Then
                _currentArtworkIndex = i
                _currentArtwork = prevArtwork
                _currentFileIndex = 0
                LoadImageAsync(prevImageFiles(0))
                Return
            End If
        Next
    End Sub
    ''' <summary>
    ''' 导航到下一个稿件
    ''' </summary>
    Private Sub NavigateNextArtwork()
        If _allArtworks Is Nothing OrElse _allArtworks.Count = 0 Then Return
        '找到下一个有图片的稿件
        For i As Integer = _currentArtworkIndex + 1 To _allArtworks.Count - 1
            Dim nextArtwork As Artwork = _allArtworks(i)
            Dim nextImageFiles As List(Of String) = GetImageFiles(nextArtwork.FilePaths)

            If nextImageFiles.Count > 0 Then
                _currentArtworkIndex = i
                _currentArtwork = nextArtwork
                _currentFileIndex = 0
                LoadImageAsync(nextImageFiles(0))
                Return
            End If
        Next
    End Sub
#End Region

#Region "其他"
    ''' <summary>
    ''' 窗体键盘事件处理 - 使用窗体事件确保响应
    ''' </summary>
    Private Sub ViewForm_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Dim isAltPressed As Boolean = e.Alt
        If _isProcessing Then '当正在加载图片时, 取消处理按键响应
            e.Handled = True
            Return
        End If
        '处理组合键
        If e.Control Then
            Select Case e.KeyCode
                Case Keys.PageUp, Keys.Up, Keys.Left '上一个稿件
                    NavigatePreviousArtwork()
                    e.Handled = True
                    Return
                Case Keys.PageDown, Keys.Right, Keys.Down '下一个稿件
                    NavigateNextArtwork()
                    e.Handled = True
                    Return
                Case Keys.C
                    Clipboard.SetImage(PictureBoxMain.Image)
            End Select
        End If
        If e.Alt Then
            Select Case e.KeyCode
                Case Keys.Enter
                    Dim imageFiles As List(Of String) = GetCurrentArtworkImages()
                    ShowProperties(imageFiles(_currentFileIndex))
                    e.Handled = True
                    e.SuppressKeyPress = True '防止发出声音
                Case Keys.T
                    SetWindowOnTop()
                    e.Handled = True
                    e.SuppressKeyPress = True '防止发出声音
                Case Keys.D1
                    UpdateExtractColor(PictureBoxMain.Image, ExtractType.KMeans)
                Case Keys.D2
                    UpdateExtractColor(PictureBoxMain.Image, ExtractType.MedianCut)
                Case Keys.D3
                    UpdateExtractColor(PictureBoxMain.Image, ExtractType.Octree)
                Case Keys.Space
                    PopupSysMenu()
                    e.Handled = True
                    e.SuppressKeyPress = True '防止发出声音, 但是有点问题，需要修
            End Select
        End If
        '处理单键
        Select Case e.KeyCode
            Case Keys.Left, Keys.P, Keys.PageUp, Keys.Up, Keys.Oemcomma, Keys.A, Keys.W '上一张
                NavigatePrevious()
                e.Handled = True
            Case Keys.Right, Keys.N, Keys.PageDown, Keys.Down, Keys.OemPeriod, Keys.S, Keys.D,
                Keys.Space, Keys.Enter '下一张
                If Not isAltPressed Then '防止打开窗体菜单时不小心切换到下一张
                    NavigateNext()
                End If
                e.Handled = True
            Case Keys.Home '第一张
                NavigateToFirstArtwork()
                e.Handled = True
            Case Keys.End '最后一张
                NavigateToLastArtwork()
                e.Handled = True
            Case Keys.Escape '退出
                Me.Close()
                e.Handled = True
            Case Keys.F11 '全屏切换
                ToggleFullScreen()
                e.Handled = True
            Case Keys.I '显示信息
                ShowArtworkInfo()
                e.Handled = True
            Case Keys.Insert '老板键
            Case Keys.F1
                ShowHelp()
            Case Keys.Apps '菜单键
                PopupSysMenu()
        End Select
    End Sub
    ''' <summary>
    ''' 弹出系统菜单
    ''' </summary>
    Private Sub PopupSysMenu(screenPos As Point)
        '右键按下时显示系统菜单
        Dim hMenu As IntPtr = GetSystemMenu(Me.Handle, False) '获取系统菜单句柄
        If hMenu = IntPtr.Zero Then Return
        '获取要执行的菜单命令
        Dim cmd As Integer = TrackPopupMenu(hMenu, TPM_LEFTALIGN Or TPM_RETURNCMD,
                                            screenPos.X, screenPos.Y, 0, Me.Handle, IntPtr.Zero)
        '如果有命令, 发送给窗口
        If cmd <> 0 Then
            SendMessage(Me.Handle, WM_SYSCOMMAND, cmd, 0)
        End If
    End Sub
    ''' <summary>
    ''' 弹出系统菜单
    ''' </summary>
    Private Sub PopupSysMenu()
        Dim screenPos As New Point(Left, Top + SystemInformation.CaptionHeight)
        PopupSysMenu(screenPos)
    End Sub
    ''' <summary>
    ''' 切换全屏模式
    ''' </summary>
    Private Sub ToggleFullScreen()
        Dim hMenu = GetSystemMenu(Handle, False)
        If _isFullScreen Then
            _isFullScreen = False
            FormBorderStyle = FormBorderStyle.Sizable
            WindowState = FormWindowState.Normal
            CheckMenuItem(hMenu, SC_FULLSCREEN, MF_UNCHECKED)
        Else
            _isFullScreen = True
            FormBorderStyle = FormBorderStyle.None
            WindowState = FormWindowState.Maximized
            CheckMenuItem(hMenu, SC_FULLSCREEN, MF_CHECKED)
        End If
    End Sub
    ''' <summary>
    ''' 打开属性
    ''' </summary>
    ''' <param name="filePath">文件路径</param>
    Private Sub ShowProperties(filePath As String)
        Try
            If String.IsNullOrEmpty(filePath) OrElse Not File.Exists(filePath) Then
                Throw New FileNotFoundException(My.Resources.View_NoFile)
            End If
            Dim info As New SHELLEXECUTEINFO()
            info.cbSize = Marshal.SizeOf(info)
            info.lpVerb = "properties"
            info.lpFile = filePath
            info.nShow = SW_SHOW
            info.fMask = SEE_MASK_INVOKEIDLIST
            If Not ShellExecuteEx(info) Then
                Throw New Win32Exception(Marshal.GetLastWin32Error())
            End If
        Catch ex As Exception
            ShowErrorDialog(ex, My.Resources.View_FailedtoOpenProp)
        End Try
    End Sub
    Private Sub PictureBoxMain_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBoxMain.MouseDown
        If e.Button = MouseButtons.Left Then
            '左键按下时模拟标题栏拖动
            ReleaseCapture()
            SendMessage(Me.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0)
        ElseIf e.Button = MouseButtons.Right Then
            PopupSysMenu(Me.PointToScreen(New Point(e.X, e.Y))) '将客户区坐标转换为屏幕坐标
        End If
    End Sub
    Private Sub PictureBoxMain_MouseWheel(sender As Object, e As MouseEventArgs) Handles PictureBoxMain.MouseWheel
        If _isProcessing Then '当正在加载图片时, 取消处理按键响应
            Return
        End If
        If e.Delta > 0 Then
            NavigatePrevious()
        Else
            NavigateNext()
        End If
    End Sub
    ''' <summary>
    ''' 根据当前状态更新菜单项的启用/禁用
    ''' </summary>
    Private Sub UpdateMenuStates()
        Dim hMenu As IntPtr = GetSystemMenu(Me.Handle, False)
        If hMenu = IntPtr.Zero Then Return
        Dim currentImages As List(Of String) = GetCurrentArtworkImages()
        Dim isFirstImage As Boolean = (_currentArtworkIndex = 0 AndAlso _currentFileIndex = 0) '第一个稿件第一个文件
        Dim isLastImage As Boolean = (_currentArtworkIndex = _allArtworks.Count - 1 AndAlso
                                  _currentFileIndex = currentImages.Count - 1) '最后一个稿件最后一个文件
        If Not isFirstImage Then '判断是否为第一个文件
            EnableMenuItem(hMenu, SC_PREVIMG, MF_BYCOMMAND Or MF_ENABLED)
        Else
            EnableMenuItem(hMenu, SC_PREVIMG, MF_BYCOMMAND Or MF_GRAYED)
        End If
        If Not isLastImage Then '判断是否为最后一个文件
            EnableMenuItem(hMenu, SC_NEXTIMG, MF_BYCOMMAND Or MF_ENABLED)
        Else
            EnableMenuItem(hMenu, SC_NEXTIMG, MF_BYCOMMAND Or MF_GRAYED)
        End If
        If HasPreviousArtwork() Then '判断是否为第一个稿件
            EnableMenuItem(hMenu, SC_PREVART, MF_BYCOMMAND Or MF_ENABLED)
        Else
            EnableMenuItem(hMenu, SC_PREVART, MF_BYCOMMAND Or MF_GRAYED)
        End If
        If HasNextArtwork() Then '判断是否为最后一个稿件
            EnableMenuItem(hMenu, SC_NEXTART, MF_BYCOMMAND Or MF_ENABLED)
        Else
            EnableMenuItem(hMenu, SC_NEXTART, MF_BYCOMMAND Or MF_GRAYED)
        End If
    End Sub
    ''' <summary>
    ''' 检查是否存在上一个有图片的稿件
    ''' </summary>
    Private Function HasPreviousArtwork() As Boolean
        If _allArtworks Is Nothing OrElse _currentArtworkIndex <= 0 Then Return False

        For i As Integer = _currentArtworkIndex - 1 To 0 Step -1
            Dim imageFiles As List(Of String) = GetImageFiles(_allArtworks(i).FilePaths)
            If imageFiles.Count > 0 Then Return True
        Next

        Return False
    End Function
    ''' <summary>
    ''' 检查是否存在下一个有图片的稿件
    ''' </summary>
    Private Function HasNextArtwork() As Boolean
        If _allArtworks Is Nothing OrElse _currentArtworkIndex >= _allArtworks.Count - 1 Then Return False

        For i As Integer = _currentArtworkIndex + 1 To _allArtworks.Count - 1
            Dim imageFiles As List(Of String) = GetImageFiles(_allArtworks(i).FilePaths)
            If imageFiles.Count > 0 Then Return True
        Next

        Return False
    End Function
#End Region

End Class