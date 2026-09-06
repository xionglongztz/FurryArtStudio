<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.MnuStrip = New System.Windows.Forms.MenuStrip()
        Me.MnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuDevTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuRunAsElevated = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuRunTerminal = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuOpenPath = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator20 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuCreateShortcut = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuAddStartMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator21 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuProperties = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuTray = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuLibrary = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuLibList = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuLibRefresh = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuLibNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuLibImport = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuLibExport = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuLibExportCSV = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuLibClone = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuLibOpenFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuLibCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuLibCopyPath = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuLibClose = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuLibRename = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuLibDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuLibStatistics = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuManuscript = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuMsNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuMsImport = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuMsView = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuMsEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuMsExport = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuMsPrint = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuMsDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuMsOpenFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuMsCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuMsCopyPath = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuViews = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuOnTop = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuPrivacyProtect = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuShowStatusBar = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuShowToolBar = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuTheme = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuThemeSystem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuThemeLight = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuThemeDark = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator18 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuViewPlay = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuSelectAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuSelectReverse = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuSearch = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuAdvancedSearch = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuPageUp = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuPageDown = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuExtension = New System.Windows.Forms.ToolStripMenuItem()
        Me.WebHookToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuPlugin = New System.Windows.Forms.ToolStripMenuItem()
        Me.打开插件目录PToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator19 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuHelpTutorial = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuHelpWebsite = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuHelpDonate = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuOpenAfdian = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuLoadSponsors = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuHelpGithub = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuFeedback = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuBugReport = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuSuggestions = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuMyFeedback = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuCheckUpdate = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuHelpWhatsNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.MnuHelpLicense = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuHelpPrivacy = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuTerms = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuHelpAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.MnuSearchTxtbox = New System.Windows.Forms.ToolStripTextBox()
        Me.StaStrip = New System.Windows.Forms.StatusStrip()
        Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TSSep1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.SearchStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TSspace = New System.Windows.Forms.ToolStripStatusLabel()
        Me.PageStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TSSep2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ArtworkStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TSSep3 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.SelectStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TSSep4 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.StorageStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TSSep5 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.KryptonMgrMain = New Krypton.Toolkit.KryptonManager(Me.components)
        Me.ConMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ConMnuMsView = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConMnuMsEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConMnuMsExport = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConMnuMsPrint = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConMnuMsDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator()
        Me.ConMnuMsOpenFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConMnuMsCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConMnuMsCopyPath = New System.Windows.Forms.ToolStripMenuItem()
        Me.TlStrip = New System.Windows.Forms.ToolStrip()
        Me.TSBtnOnTop = New System.Windows.Forms.ToolStripButton()
        Me.TSBtnProperties = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
        Me.TSBtnRefresh = New System.Windows.Forms.ToolStripButton()
        Me.TSBtnLibNew = New System.Windows.Forms.ToolStripButton()
        Me.TSBtnLibImport = New System.Windows.Forms.ToolStripButton()
        Me.TSBtnLibExport = New System.Windows.Forms.ToolStripButton()
        Me.TSBtnLibRename = New System.Windows.Forms.ToolStripButton()
        Me.TSBtnLibClose = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator16 = New System.Windows.Forms.ToolStripSeparator()
        Me.TSBtnMsNew = New System.Windows.Forms.ToolStripButton()
        Me.TSBtnMsImport = New System.Windows.Forms.ToolStripButton()
        Me.TSBtnMsExport = New System.Windows.Forms.ToolStripButton()
        Me.TSBtnMsEdit = New System.Windows.Forms.ToolStripButton()
        Me.TSBtnMsPrint = New System.Windows.Forms.ToolStripButton()
        Me.TSBtnMsOpenFolder = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator17 = New System.Windows.Forms.ToolStripSeparator()
        Me.TSBtnPlay = New System.Windows.Forms.ToolStripButton()
        Me.TSBtnSearch = New System.Windows.Forms.ToolStripButton()
        Me.TSBtnPrevPage = New System.Windows.Forms.ToolStripButton()
        Me.TSBtnNextPage = New System.Windows.Forms.ToolStripButton()
        Me.ArtworkListSplitContainer = New System.Windows.Forms.SplitContainer()
        Me.LblNotes = New System.Windows.Forms.Label()
        Me.LblDeltas = New System.Windows.Forms.Label()
        Me.LblTags = New System.Windows.Forms.Label()
        Me.LblCharacters = New System.Windows.Forms.Label()
        Me.LblAuthor = New System.Windows.Forms.Label()
        Me.LblTitle = New System.Windows.Forms.Label()
        Me.PiChkThumb = New System.Windows.Forms.PictureBox()
        Me.NotifyIco = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ImageGalleryMain = New FurryArtStudio.ImageGallery()
        Me.MnuStrip.SuspendLayout()
        Me.StaStrip.SuspendLayout()
        Me.ConMenu.SuspendLayout()
        Me.TlStrip.SuspendLayout()
        CType(Me.ArtworkListSplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ArtworkListSplitContainer.Panel1.SuspendLayout()
        Me.ArtworkListSplitContainer.Panel2.SuspendLayout()
        Me.ArtworkListSplitContainer.SuspendLayout()
        CType(Me.PiChkThumb, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MnuStrip
        '
        Me.MnuStrip.Font = New System.Drawing.Font("Microsoft YaHei UI", 15.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World)
        Me.MnuStrip.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MnuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MnuFile, Me.MnuLibrary, Me.MnuManuscript, Me.MnuViews, Me.MnuExtension, Me.MnuHelp, Me.MnuSearchTxtbox})
        Me.MnuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MnuStrip.Name = "MnuStrip"
        Me.MnuStrip.Size = New System.Drawing.Size(962, 31)
        Me.MnuStrip.TabIndex = 4
        Me.MnuStrip.Text = "MenuStrip1"
        '
        'MnuFile
        '
        Me.MnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MnuDevTools, Me.MnuRunAsElevated, Me.MnuRunTerminal, Me.MnuOpenPath, Me.ToolStripSeparator20, Me.MnuCreateShortcut, Me.MnuAddStartMenu, Me.ToolStripSeparator21, Me.MnuProperties, Me.ToolStripSeparator1, Me.MnuTray, Me.MnuExit})
        Me.MnuFile.Name = "MnuFile"
        Me.MnuFile.Size = New System.Drawing.Size(71, 27)
        Me.MnuFile.Text = "文件(&F)"
        '
        'MnuDevTools
        '
        Me.MnuDevTools.Image = CType(resources.GetObject("MnuDevTools.Image"), System.Drawing.Image)
        Me.MnuDevTools.Name = "MnuDevTools"
        Me.MnuDevTools.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.MnuDevTools.Size = New System.Drawing.Size(257, 26)
        Me.MnuDevTools.Text = "开发者工具(&D)..."
        Me.MnuDevTools.Visible = False
        '
        'MnuRunAsElevated
        '
        Me.MnuRunAsElevated.Image = CType(resources.GetObject("MnuRunAsElevated.Image"), System.Drawing.Image)
        Me.MnuRunAsElevated.Name = "MnuRunAsElevated"
        Me.MnuRunAsElevated.Size = New System.Drawing.Size(257, 26)
        Me.MnuRunAsElevated.Text = "以管理员权限重启(&R)"
        '
        'MnuRunTerminal
        '
        Me.MnuRunTerminal.Image = CType(resources.GetObject("MnuRunTerminal.Image"), System.Drawing.Image)
        Me.MnuRunTerminal.Name = "MnuRunTerminal"
        Me.MnuRunTerminal.Size = New System.Drawing.Size(257, 26)
        Me.MnuRunTerminal.Text = "在此处打开终端(&T)"
        '
        'MnuOpenPath
        '
        Me.MnuOpenPath.Image = CType(resources.GetObject("MnuOpenPath.Image"), System.Drawing.Image)
        Me.MnuOpenPath.Name = "MnuOpenPath"
        Me.MnuOpenPath.Size = New System.Drawing.Size(257, 26)
        Me.MnuOpenPath.Text = "打开程序文件夹(&P)"
        '
        'ToolStripSeparator20
        '
        Me.ToolStripSeparator20.Name = "ToolStripSeparator20"
        Me.ToolStripSeparator20.Size = New System.Drawing.Size(254, 6)
        '
        'MnuCreateShortcut
        '
        Me.MnuCreateShortcut.Image = CType(resources.GetObject("MnuCreateShortcut.Image"), System.Drawing.Image)
        Me.MnuCreateShortcut.Name = "MnuCreateShortcut"
        Me.MnuCreateShortcut.Size = New System.Drawing.Size(257, 26)
        Me.MnuCreateShortcut.Text = "创建桌面快捷方式(&S)"
        '
        'MnuAddStartMenu
        '
        Me.MnuAddStartMenu.Image = CType(resources.GetObject("MnuAddStartMenu.Image"), System.Drawing.Image)
        Me.MnuAddStartMenu.Name = "MnuAddStartMenu"
        Me.MnuAddStartMenu.Size = New System.Drawing.Size(257, 26)
        Me.MnuAddStartMenu.Text = "添加到""开始""菜单(&A)"
        '
        'ToolStripSeparator21
        '
        Me.ToolStripSeparator21.Name = "ToolStripSeparator21"
        Me.ToolStripSeparator21.Size = New System.Drawing.Size(254, 6)
        '
        'MnuProperties
        '
        Me.MnuProperties.Image = CType(resources.GetObject("MnuProperties.Image"), System.Drawing.Image)
        Me.MnuProperties.Name = "MnuProperties"
        Me.MnuProperties.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.K), System.Windows.Forms.Keys)
        Me.MnuProperties.Size = New System.Drawing.Size(257, 26)
        Me.MnuProperties.Text = "选项(&O)..."
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(254, 6)
        '
        'MnuTray
        '
        Me.MnuTray.Image = CType(resources.GetObject("MnuTray.Image"), System.Drawing.Image)
        Me.MnuTray.Name = "MnuTray"
        Me.MnuTray.ShortcutKeys = System.Windows.Forms.Keys.Insert
        Me.MnuTray.Size = New System.Drawing.Size(257, 26)
        Me.MnuTray.Text = "最小化到托盘(&M)"
        '
        'MnuExit
        '
        Me.MnuExit.Image = CType(resources.GetObject("MnuExit.Image"), System.Drawing.Image)
        Me.MnuExit.Name = "MnuExit"
        Me.MnuExit.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F4), System.Windows.Forms.Keys)
        Me.MnuExit.Size = New System.Drawing.Size(257, 26)
        Me.MnuExit.Text = "退出(&X)"
        '
        'MnuLibrary
        '
        Me.MnuLibrary.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MnuLibList, Me.MnuLibRefresh, Me.ToolStripSeparator11, Me.MnuLibNew, Me.MnuLibImport, Me.MnuLibExport, Me.MnuLibExportCSV, Me.ToolStripSeparator5, Me.MnuLibClone, Me.MnuLibOpenFolder, Me.MnuLibCopy, Me.MnuLibCopyPath, Me.ToolStripSeparator6, Me.MnuLibClose, Me.MnuLibRename, Me.MnuLibDelete, Me.ToolStripSeparator7, Me.MnuLibStatistics})
        Me.MnuLibrary.Name = "MnuLibrary"
        Me.MnuLibrary.Size = New System.Drawing.Size(86, 27)
        Me.MnuLibrary.Text = "稿件库(&L)"
        '
        'MnuLibList
        '
        Me.MnuLibList.Image = CType(resources.GetObject("MnuLibList.Image"), System.Drawing.Image)
        Me.MnuLibList.Name = "MnuLibList"
        Me.MnuLibList.Size = New System.Drawing.Size(258, 26)
        Me.MnuLibList.Text = "当前库(&U)"
        '
        'MnuLibRefresh
        '
        Me.MnuLibRefresh.Image = CType(resources.GetObject("MnuLibRefresh.Image"), System.Drawing.Image)
        Me.MnuLibRefresh.Name = "MnuLibRefresh"
        Me.MnuLibRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.MnuLibRefresh.Size = New System.Drawing.Size(258, 26)
        Me.MnuLibRefresh.Text = "刷新(&R)"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(255, 6)
        '
        'MnuLibNew
        '
        Me.MnuLibNew.Image = CType(resources.GetObject("MnuLibNew.Image"), System.Drawing.Image)
        Me.MnuLibNew.Name = "MnuLibNew"
        Me.MnuLibNew.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.MnuLibNew.Size = New System.Drawing.Size(258, 26)
        Me.MnuLibNew.Text = "新建(&N)..."
        '
        'MnuLibImport
        '
        Me.MnuLibImport.Image = CType(resources.GetObject("MnuLibImport.Image"), System.Drawing.Image)
        Me.MnuLibImport.Name = "MnuLibImport"
        Me.MnuLibImport.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.MnuLibImport.Size = New System.Drawing.Size(258, 26)
        Me.MnuLibImport.Text = "导入(&I)..."
        '
        'MnuLibExport
        '
        Me.MnuLibExport.Image = CType(resources.GetObject("MnuLibExport.Image"), System.Drawing.Image)
        Me.MnuLibExport.Name = "MnuLibExport"
        Me.MnuLibExport.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.MnuLibExport.Size = New System.Drawing.Size(258, 26)
        Me.MnuLibExport.Text = "导出(&E)..."
        '
        'MnuLibExportCSV
        '
        Me.MnuLibExportCSV.Image = CType(resources.GetObject("MnuLibExportCSV.Image"), System.Drawing.Image)
        Me.MnuLibExportCSV.Name = "MnuLibExportCSV"
        Me.MnuLibExportCSV.Size = New System.Drawing.Size(258, 26)
        Me.MnuLibExportCSV.Text = "导出为 CSV 文件(&V)..."
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(255, 6)
        '
        'MnuLibClone
        '
        Me.MnuLibClone.Image = CType(resources.GetObject("MnuLibClone.Image"), System.Drawing.Image)
        Me.MnuLibClone.Name = "MnuLibClone"
        Me.MnuLibClone.Size = New System.Drawing.Size(258, 26)
        Me.MnuLibClone.Text = "克隆(&L)..."
        '
        'MnuLibOpenFolder
        '
        Me.MnuLibOpenFolder.Image = CType(resources.GetObject("MnuLibOpenFolder.Image"), System.Drawing.Image)
        Me.MnuLibOpenFolder.Name = "MnuLibOpenFolder"
        Me.MnuLibOpenFolder.Size = New System.Drawing.Size(258, 26)
        Me.MnuLibOpenFolder.Text = "打开目录(&O)"
        '
        'MnuLibCopy
        '
        Me.MnuLibCopy.Image = CType(resources.GetObject("MnuLibCopy.Image"), System.Drawing.Image)
        Me.MnuLibCopy.Name = "MnuLibCopy"
        Me.MnuLibCopy.Size = New System.Drawing.Size(258, 26)
        Me.MnuLibCopy.Text = "复制文件夹(&C)"
        '
        'MnuLibCopyPath
        '
        Me.MnuLibCopyPath.Name = "MnuLibCopyPath"
        Me.MnuLibCopyPath.Size = New System.Drawing.Size(258, 26)
        Me.MnuLibCopyPath.Text = "复制目录路径(&T)"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(255, 6)
        '
        'MnuLibClose
        '
        Me.MnuLibClose.Image = CType(resources.GetObject("MnuLibClose.Image"), System.Drawing.Image)
        Me.MnuLibClose.Name = "MnuLibClose"
        Me.MnuLibClose.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.W), System.Windows.Forms.Keys)
        Me.MnuLibClose.Size = New System.Drawing.Size(258, 26)
        Me.MnuLibClose.Text = "关闭(&C)"
        '
        'MnuLibRename
        '
        Me.MnuLibRename.Image = CType(resources.GetObject("MnuLibRename.Image"), System.Drawing.Image)
        Me.MnuLibRename.Name = "MnuLibRename"
        Me.MnuLibRename.ShortcutKeys = CType((System.Windows.Forms.Keys.Shift Or System.Windows.Forms.Keys.F2), System.Windows.Forms.Keys)
        Me.MnuLibRename.Size = New System.Drawing.Size(258, 26)
        Me.MnuLibRename.Text = "重命名(&M)..."
        '
        'MnuLibDelete
        '
        Me.MnuLibDelete.Image = CType(resources.GetObject("MnuLibDelete.Image"), System.Drawing.Image)
        Me.MnuLibDelete.Name = "MnuLibDelete"
        Me.MnuLibDelete.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.Delete), System.Windows.Forms.Keys)
        Me.MnuLibDelete.Size = New System.Drawing.Size(258, 26)
        Me.MnuLibDelete.Text = "删除(&D)..."
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(255, 6)
        '
        'MnuLibStatistics
        '
        Me.MnuLibStatistics.Image = CType(resources.GetObject("MnuLibStatistics.Image"), System.Drawing.Image)
        Me.MnuLibStatistics.Name = "MnuLibStatistics"
        Me.MnuLibStatistics.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.I), System.Windows.Forms.Keys)
        Me.MnuLibStatistics.Size = New System.Drawing.Size(258, 26)
        Me.MnuLibStatistics.Text = "统计信息(&S)..."
        '
        'MnuManuscript
        '
        Me.MnuManuscript.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MnuMsNew, Me.MnuMsImport, Me.ToolStripSeparator3, Me.MnuMsView, Me.MnuMsEdit, Me.MnuMsExport, Me.MnuMsPrint, Me.MnuMsDelete, Me.ToolStripSeparator4, Me.MnuMsOpenFolder, Me.MnuMsCopy, Me.MnuMsCopyPath})
        Me.MnuManuscript.Name = "MnuManuscript"
        Me.MnuManuscript.Size = New System.Drawing.Size(78, 27)
        Me.MnuManuscript.Text = "稿件(&M)"
        '
        'MnuMsNew
        '
        Me.MnuMsNew.Image = CType(resources.GetObject("MnuMsNew.Image"), System.Drawing.Image)
        Me.MnuMsNew.Name = "MnuMsNew"
        Me.MnuMsNew.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.MnuMsNew.Size = New System.Drawing.Size(243, 26)
        Me.MnuMsNew.Text = "新建(&N)..."
        '
        'MnuMsImport
        '
        Me.MnuMsImport.Image = CType(resources.GetObject("MnuMsImport.Image"), System.Drawing.Image)
        Me.MnuMsImport.Name = "MnuMsImport"
        Me.MnuMsImport.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.MnuMsImport.Size = New System.Drawing.Size(243, 26)
        Me.MnuMsImport.Text = "导入(&I)..."
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(240, 6)
        '
        'MnuMsView
        '
        Me.MnuMsView.Image = CType(resources.GetObject("MnuMsView.Image"), System.Drawing.Image)
        Me.MnuMsView.Name = "MnuMsView"
        Me.MnuMsView.ShortcutKeyDisplayString = "Enter"
        Me.MnuMsView.Size = New System.Drawing.Size(243, 26)
        Me.MnuMsView.Text = "预览(&V)"
        '
        'MnuMsEdit
        '
        Me.MnuMsEdit.Image = CType(resources.GetObject("MnuMsEdit.Image"), System.Drawing.Image)
        Me.MnuMsEdit.Name = "MnuMsEdit"
        Me.MnuMsEdit.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.MnuMsEdit.Size = New System.Drawing.Size(243, 26)
        Me.MnuMsEdit.Text = "编辑(&E)..."
        '
        'MnuMsExport
        '
        Me.MnuMsExport.Image = CType(resources.GetObject("MnuMsExport.Image"), System.Drawing.Image)
        Me.MnuMsExport.Name = "MnuMsExport"
        Me.MnuMsExport.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.MnuMsExport.Size = New System.Drawing.Size(243, 26)
        Me.MnuMsExport.Text = "导出(&X)..."
        '
        'MnuMsPrint
        '
        Me.MnuMsPrint.Image = CType(resources.GetObject("MnuMsPrint.Image"), System.Drawing.Image)
        Me.MnuMsPrint.Name = "MnuMsPrint"
        Me.MnuMsPrint.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.MnuMsPrint.Size = New System.Drawing.Size(243, 26)
        Me.MnuMsPrint.Text = "打印(&P)..."
        '
        'MnuMsDelete
        '
        Me.MnuMsDelete.Image = CType(resources.GetObject("MnuMsDelete.Image"), System.Drawing.Image)
        Me.MnuMsDelete.Name = "MnuMsDelete"
        Me.MnuMsDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.MnuMsDelete.Size = New System.Drawing.Size(243, 26)
        Me.MnuMsDelete.Text = "删除(&D)..."
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(240, 6)
        '
        'MnuMsOpenFolder
        '
        Me.MnuMsOpenFolder.Image = CType(resources.GetObject("MnuMsOpenFolder.Image"), System.Drawing.Image)
        Me.MnuMsOpenFolder.Name = "MnuMsOpenFolder"
        Me.MnuMsOpenFolder.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.T), System.Windows.Forms.Keys)
        Me.MnuMsOpenFolder.Size = New System.Drawing.Size(243, 26)
        Me.MnuMsOpenFolder.Text = "打开目录(&O)"
        '
        'MnuMsCopy
        '
        Me.MnuMsCopy.Image = CType(resources.GetObject("MnuMsCopy.Image"), System.Drawing.Image)
        Me.MnuMsCopy.Name = "MnuMsCopy"
        Me.MnuMsCopy.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.MnuMsCopy.Size = New System.Drawing.Size(243, 26)
        Me.MnuMsCopy.Text = "复制文件夹(&C)"
        '
        'MnuMsCopyPath
        '
        Me.MnuMsCopyPath.Name = "MnuMsCopyPath"
        Me.MnuMsCopyPath.Size = New System.Drawing.Size(243, 26)
        Me.MnuMsCopyPath.Text = "复制目录路径(&T)"
        '
        'MnuViews
        '
        Me.MnuViews.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MnuOnTop, Me.MnuPrivacyProtect, Me.ToolStripSeparator9, Me.MnuShowStatusBar, Me.MnuShowToolBar, Me.MnuTheme, Me.ToolStripSeparator18, Me.MnuViewPlay, Me.ToolStripSeparator12, Me.MnuSelectAll, Me.MnuSelectReverse, Me.ToolStripSeparator13, Me.MnuSearch, Me.MnuAdvancedSearch, Me.ToolStripSeparator2, Me.MnuPageUp, Me.MnuPageDown})
        Me.MnuViews.Name = "MnuViews"
        Me.MnuViews.Size = New System.Drawing.Size(73, 27)
        Me.MnuViews.Text = "视图(&V)"
        '
        'MnuOnTop
        '
        Me.MnuOnTop.Image = CType(resources.GetObject("MnuOnTop.Image"), System.Drawing.Image)
        Me.MnuOnTop.Name = "MnuOnTop"
        Me.MnuOnTop.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.T), System.Windows.Forms.Keys)
        Me.MnuOnTop.Size = New System.Drawing.Size(283, 26)
        Me.MnuOnTop.Text = "窗口置顶(&T)"
        '
        'MnuPrivacyProtect
        '
        Me.MnuPrivacyProtect.Image = CType(resources.GetObject("MnuPrivacyProtect.Image"), System.Drawing.Image)
        Me.MnuPrivacyProtect.Name = "MnuPrivacyProtect"
        Me.MnuPrivacyProtect.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.MnuPrivacyProtect.Size = New System.Drawing.Size(283, 26)
        Me.MnuPrivacyProtect.Text = "隐私保护(&I)"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(280, 6)
        '
        'MnuShowStatusBar
        '
        Me.MnuShowStatusBar.Image = CType(resources.GetObject("MnuShowStatusBar.Image"), System.Drawing.Image)
        Me.MnuShowStatusBar.Name = "MnuShowStatusBar"
        Me.MnuShowStatusBar.Size = New System.Drawing.Size(283, 26)
        Me.MnuShowStatusBar.Text = "显示状态栏(&S)"
        '
        'MnuShowToolBar
        '
        Me.MnuShowToolBar.Image = CType(resources.GetObject("MnuShowToolBar.Image"), System.Drawing.Image)
        Me.MnuShowToolBar.Name = "MnuShowToolBar"
        Me.MnuShowToolBar.Size = New System.Drawing.Size(283, 26)
        Me.MnuShowToolBar.Text = "显示工具栏(&T)"
        '
        'MnuTheme
        '
        Me.MnuTheme.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MnuThemeSystem, Me.MnuThemeLight, Me.MnuThemeDark})
        Me.MnuTheme.Name = "MnuTheme"
        Me.MnuTheme.Size = New System.Drawing.Size(283, 26)
        Me.MnuTheme.Text = "主题(&H)"
        '
        'MnuThemeSystem
        '
        Me.MnuThemeSystem.Image = CType(resources.GetObject("MnuThemeSystem.Image"), System.Drawing.Image)
        Me.MnuThemeSystem.Name = "MnuThemeSystem"
        Me.MnuThemeSystem.Size = New System.Drawing.Size(173, 26)
        Me.MnuThemeSystem.Text = "跟随系统(&S)"
        '
        'MnuThemeLight
        '
        Me.MnuThemeLight.Image = CType(resources.GetObject("MnuThemeLight.Image"), System.Drawing.Image)
        Me.MnuThemeLight.Name = "MnuThemeLight"
        Me.MnuThemeLight.Size = New System.Drawing.Size(173, 26)
        Me.MnuThemeLight.Text = "浅色模式(&L)"
        '
        'MnuThemeDark
        '
        Me.MnuThemeDark.Image = CType(resources.GetObject("MnuThemeDark.Image"), System.Drawing.Image)
        Me.MnuThemeDark.Name = "MnuThemeDark"
        Me.MnuThemeDark.Size = New System.Drawing.Size(173, 26)
        Me.MnuThemeDark.Text = "深色模式(&D)"
        '
        'ToolStripSeparator18
        '
        Me.ToolStripSeparator18.Name = "ToolStripSeparator18"
        Me.ToolStripSeparator18.Size = New System.Drawing.Size(280, 6)
        '
        'MnuViewPlay
        '
        Me.MnuViewPlay.Image = CType(resources.GetObject("MnuViewPlay.Image"), System.Drawing.Image)
        Me.MnuViewPlay.Name = "MnuViewPlay"
        Me.MnuViewPlay.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F5), System.Windows.Forms.Keys)
        Me.MnuViewPlay.Size = New System.Drawing.Size(283, 26)
        Me.MnuViewPlay.Text = "幻灯片放映(&P)"
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(280, 6)
        '
        'MnuSelectAll
        '
        Me.MnuSelectAll.Name = "MnuSelectAll"
        Me.MnuSelectAll.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.MnuSelectAll.Size = New System.Drawing.Size(283, 26)
        Me.MnuSelectAll.Text = "全选(&A)"
        '
        'MnuSelectReverse
        '
        Me.MnuSelectReverse.Name = "MnuSelectReverse"
        Me.MnuSelectReverse.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.MnuSelectReverse.Size = New System.Drawing.Size(283, 26)
        Me.MnuSelectReverse.Text = "反选(&R)"
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(280, 6)
        '
        'MnuSearch
        '
        Me.MnuSearch.Image = CType(resources.GetObject("MnuSearch.Image"), System.Drawing.Image)
        Me.MnuSearch.Name = "MnuSearch"
        Me.MnuSearch.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.MnuSearch.Size = New System.Drawing.Size(283, 26)
        Me.MnuSearch.Text = "搜索(&F)"
        '
        'MnuAdvancedSearch
        '
        Me.MnuAdvancedSearch.Image = CType(resources.GetObject("MnuAdvancedSearch.Image"), System.Drawing.Image)
        Me.MnuAdvancedSearch.Name = "MnuAdvancedSearch"
        Me.MnuAdvancedSearch.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.MnuAdvancedSearch.Size = New System.Drawing.Size(283, 26)
        Me.MnuAdvancedSearch.Text = "高级搜索(&D)..."
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(280, 6)
        '
        'MnuPageUp
        '
        Me.MnuPageUp.Image = CType(resources.GetObject("MnuPageUp.Image"), System.Drawing.Image)
        Me.MnuPageUp.Name = "MnuPageUp"
        Me.MnuPageUp.ShortcutKeyDisplayString = "PageUp"
        Me.MnuPageUp.Size = New System.Drawing.Size(283, 26)
        Me.MnuPageUp.Text = "上一页(&E)"
        '
        'MnuPageDown
        '
        Me.MnuPageDown.Image = CType(resources.GetObject("MnuPageDown.Image"), System.Drawing.Image)
        Me.MnuPageDown.Name = "MnuPageDown"
        Me.MnuPageDown.ShortcutKeyDisplayString = "PageDown"
        Me.MnuPageDown.Size = New System.Drawing.Size(283, 26)
        Me.MnuPageDown.Text = "下一页(&N)"
        '
        'MnuExtension
        '
        Me.MnuExtension.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.WebHookToolStripMenuItem, Me.MnuPlugin})
        Me.MnuExtension.Name = "MnuExtension"
        Me.MnuExtension.Size = New System.Drawing.Size(71, 27)
        Me.MnuExtension.Text = "扩展(&E)"
        '
        'WebHookToolStripMenuItem
        '
        Me.WebHookToolStripMenuItem.Name = "WebHookToolStripMenuItem"
        Me.WebHookToolStripMenuItem.Size = New System.Drawing.Size(224, 26)
        Me.WebHookToolStripMenuItem.Text = "&WebHook..."
        '
        'MnuPlugin
        '
        Me.MnuPlugin.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.打开插件目录PToolStripMenuItem, Me.ToolStripSeparator19})
        Me.MnuPlugin.Name = "MnuPlugin"
        Me.MnuPlugin.Size = New System.Drawing.Size(224, 26)
        Me.MnuPlugin.Text = "插件(&P)"
        '
        '打开插件目录PToolStripMenuItem
        '
        Me.打开插件目录PToolStripMenuItem.Name = "打开插件目录PToolStripMenuItem"
        Me.打开插件目录PToolStripMenuItem.Size = New System.Drawing.Size(201, 26)
        Me.打开插件目录PToolStripMenuItem.Text = "打开插件目录(&P)"
        '
        'ToolStripSeparator19
        '
        Me.ToolStripSeparator19.Name = "ToolStripSeparator19"
        Me.ToolStripSeparator19.Size = New System.Drawing.Size(198, 6)
        '
        'MnuHelp
        '
        Me.MnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MnuHelpTutorial, Me.ToolStripSeparator8, Me.MnuHelpWebsite, Me.MnuHelpDonate, Me.MnuHelpGithub, Me.MnuFeedback, Me.MnuCheckUpdate, Me.MnuHelpWhatsNew, Me.ToolStripSeparator10, Me.MnuHelpLicense, Me.MnuHelpPrivacy, Me.MnuTerms, Me.MnuHelpAbout})
        Me.MnuHelp.Name = "MnuHelp"
        Me.MnuHelp.Size = New System.Drawing.Size(75, 27)
        Me.MnuHelp.Text = "帮助(&H)"
        '
        'MnuHelpTutorial
        '
        Me.MnuHelpTutorial.Image = CType(resources.GetObject("MnuHelpTutorial.Image"), System.Drawing.Image)
        Me.MnuHelpTutorial.Name = "MnuHelpTutorial"
        Me.MnuHelpTutorial.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.MnuHelpTutorial.Size = New System.Drawing.Size(218, 26)
        Me.MnuHelpTutorial.Text = "教程(&T)"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(215, 6)
        '
        'MnuHelpWebsite
        '
        Me.MnuHelpWebsite.Image = CType(resources.GetObject("MnuHelpWebsite.Image"), System.Drawing.Image)
        Me.MnuHelpWebsite.Name = "MnuHelpWebsite"
        Me.MnuHelpWebsite.Size = New System.Drawing.Size(218, 26)
        Me.MnuHelpWebsite.Text = "官网(&W)"
        '
        'MnuHelpDonate
        '
        Me.MnuHelpDonate.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MnuOpenAfdian, Me.MnuLoadSponsors})
        Me.MnuHelpDonate.Name = "MnuHelpDonate"
        Me.MnuHelpDonate.Size = New System.Drawing.Size(218, 26)
        Me.MnuHelpDonate.Text = "捐赠(&D)"
        '
        'MnuOpenAfdian
        '
        Me.MnuOpenAfdian.Image = CType(resources.GetObject("MnuOpenAfdian.Image"), System.Drawing.Image)
        Me.MnuOpenAfdian.Name = "MnuOpenAfdian"
        Me.MnuOpenAfdian.Size = New System.Drawing.Size(215, 26)
        Me.MnuOpenAfdian.Text = "打开爱发电(&A)"
        '
        'MnuLoadSponsors
        '
        Me.MnuLoadSponsors.Name = "MnuLoadSponsors"
        Me.MnuLoadSponsors.Size = New System.Drawing.Size(215, 26)
        Me.MnuLoadSponsors.Text = "加载赞助者名单(&L)"
        '
        'MnuHelpGithub
        '
        Me.MnuHelpGithub.Image = CType(resources.GetObject("MnuHelpGithub.Image"), System.Drawing.Image)
        Me.MnuHelpGithub.Name = "MnuHelpGithub"
        Me.MnuHelpGithub.Size = New System.Drawing.Size(218, 26)
        Me.MnuHelpGithub.Text = "&GitHub"
        '
        'MnuFeedback
        '
        Me.MnuFeedback.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MnuBugReport, Me.MnuSuggestions, Me.MnuMyFeedback})
        Me.MnuFeedback.Name = "MnuFeedback"
        Me.MnuFeedback.Size = New System.Drawing.Size(218, 26)
        Me.MnuFeedback.Text = "反馈(&F)"
        '
        'MnuBugReport
        '
        Me.MnuBugReport.Image = CType(resources.GetObject("MnuBugReport.Image"), System.Drawing.Image)
        Me.MnuBugReport.Name = "MnuBugReport"
        Me.MnuBugReport.Size = New System.Drawing.Size(177, 26)
        Me.MnuBugReport.Text = "报告问题(&B)"
        '
        'MnuSuggestions
        '
        Me.MnuSuggestions.Image = CType(resources.GetObject("MnuSuggestions.Image"), System.Drawing.Image)
        Me.MnuSuggestions.Name = "MnuSuggestions"
        Me.MnuSuggestions.Size = New System.Drawing.Size(177, 26)
        Me.MnuSuggestions.Text = "建议功能(&S)"
        '
        'MnuMyFeedback
        '
        Me.MnuMyFeedback.Name = "MnuMyFeedback"
        Me.MnuMyFeedback.Size = New System.Drawing.Size(177, 26)
        Me.MnuMyFeedback.Text = "我的反馈(&M)"
        '
        'MnuCheckUpdate
        '
        Me.MnuCheckUpdate.Image = CType(resources.GetObject("MnuCheckUpdate.Image"), System.Drawing.Image)
        Me.MnuCheckUpdate.Name = "MnuCheckUpdate"
        Me.MnuCheckUpdate.Size = New System.Drawing.Size(218, 26)
        Me.MnuCheckUpdate.Text = "检查更新(&U)"
        '
        'MnuHelpWhatsNew
        '
        Me.MnuHelpWhatsNew.Image = CType(resources.GetObject("MnuHelpWhatsNew.Image"), System.Drawing.Image)
        Me.MnuHelpWhatsNew.Name = "MnuHelpWhatsNew"
        Me.MnuHelpWhatsNew.Size = New System.Drawing.Size(218, 26)
        Me.MnuHelpWhatsNew.Text = "新增功能(&H)"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(215, 6)
        '
        'MnuHelpLicense
        '
        Me.MnuHelpLicense.Name = "MnuHelpLicense"
        Me.MnuHelpLicense.Size = New System.Drawing.Size(218, 26)
        Me.MnuHelpLicense.Text = "许可证(&L)..."
        '
        'MnuHelpPrivacy
        '
        Me.MnuHelpPrivacy.Name = "MnuHelpPrivacy"
        Me.MnuHelpPrivacy.Size = New System.Drawing.Size(218, 26)
        Me.MnuHelpPrivacy.Text = "隐私政策(&P)..."
        '
        'MnuTerms
        '
        Me.MnuTerms.Name = "MnuTerms"
        Me.MnuTerms.Size = New System.Drawing.Size(218, 26)
        Me.MnuTerms.Text = "用户协议(&T)..."
        '
        'MnuHelpAbout
        '
        Me.MnuHelpAbout.Image = CType(resources.GetObject("MnuHelpAbout.Image"), System.Drawing.Image)
        Me.MnuHelpAbout.Name = "MnuHelpAbout"
        Me.MnuHelpAbout.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F1), System.Windows.Forms.Keys)
        Me.MnuHelpAbout.Size = New System.Drawing.Size(218, 26)
        Me.MnuHelpAbout.Text = "关于(&A)..."
        '
        'MnuSearchTxtbox
        '
        Me.MnuSearchTxtbox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.MnuSearchTxtbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.MnuSearchTxtbox.Font = New System.Drawing.Font("Microsoft YaHei UI", 9.0!)
        Me.MnuSearchTxtbox.Name = "MnuSearchTxtbox"
        Me.MnuSearchTxtbox.Size = New System.Drawing.Size(150, 27)
        '
        'StaStrip
        '
        Me.StaStrip.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.StaStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel, Me.TSSep1, Me.SearchStatusLabel, Me.TSspace, Me.PageStatusLabel, Me.TSSep2, Me.ArtworkStatusLabel, Me.TSSep3, Me.SelectStatusLabel, Me.TSSep4, Me.StorageStatusLabel, Me.TSSep5})
        Me.StaStrip.Location = New System.Drawing.Point(0, 567)
        Me.StaStrip.Name = "StaStrip"
        Me.StaStrip.Size = New System.Drawing.Size(962, 26)
        Me.StaStrip.TabIndex = 8
        Me.StaStrip.Text = "StatusStrip1"
        '
        'StatusLabel
        '
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(39, 20)
        Me.StatusLabel.Text = "就绪"
        '
        'TSSep1
        '
        Me.TSSep1.Name = "TSSep1"
        Me.TSSep1.Size = New System.Drawing.Size(13, 20)
        Me.TSSep1.Text = "|"
        '
        'SearchStatusLabel
        '
        Me.SearchStatusLabel.Name = "SearchStatusLabel"
        Me.SearchStatusLabel.Size = New System.Drawing.Size(108, 20)
        Me.SearchStatusLabel.Text = "已搜到0个稿件"
        '
        'TSspace
        '
        Me.TSspace.Name = "TSspace"
        Me.TSspace.Size = New System.Drawing.Size(349, 20)
        Me.TSspace.Spring = True
        Me.TSspace.Text = " "
        '
        'PageStatusLabel
        '
        Me.PageStatusLabel.Name = "PageStatusLabel"
        Me.PageStatusLabel.Size = New System.Drawing.Size(71, 20)
        Me.PageStatusLabel.Text = "页码: 0/0"
        '
        'TSSep2
        '
        Me.TSSep2.Name = "TSSep2"
        Me.TSSep2.Size = New System.Drawing.Size(13, 20)
        Me.TSSep2.Text = "|"
        '
        'ArtworkStatusLabel
        '
        Me.ArtworkStatusLabel.Name = "ArtworkStatusLabel"
        Me.ArtworkStatusLabel.Size = New System.Drawing.Size(91, 20)
        Me.ArtworkStatusLabel.Text = "<无稿件库>"
        '
        'TSSep3
        '
        Me.TSSep3.Name = "TSSep3"
        Me.TSSep3.Size = New System.Drawing.Size(13, 20)
        Me.TSSep3.Text = "|"
        '
        'SelectStatusLabel
        '
        Me.SelectStatusLabel.Name = "SelectStatusLabel"
        Me.SelectStatusLabel.Size = New System.Drawing.Size(91, 20)
        Me.SelectStatusLabel.Text = "<没有稿件>"
        '
        'TSSep4
        '
        Me.TSSep4.Name = "TSSep4"
        Me.TSSep4.Size = New System.Drawing.Size(13, 20)
        Me.TSSep4.Text = "|"
        '
        'StorageStatusLabel
        '
        Me.StorageStatusLabel.Name = "StorageStatusLabel"
        Me.StorageStatusLabel.Size = New System.Drawing.Size(133, 20)
        Me.StorageStatusLabel.Text = "存储: 0B (0个文件)"
        '
        'TSSep5
        '
        Me.TSSep5.Name = "TSSep5"
        Me.TSSep5.Size = New System.Drawing.Size(13, 20)
        Me.TSSep5.Text = " "
        '
        'KryptonMgrMain
        '
        Me.KryptonMgrMain.GlobalPaletteMode = Krypton.Toolkit.PaletteMode.ProfessionalSystem
        Me.KryptonMgrMain.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details..."
        Me.KryptonMgrMain.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details..."
        '
        'ConMenu
        '
        Me.ConMenu.Font = New System.Drawing.Font("Microsoft YaHei UI", 15.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World)
        Me.ConMenu.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ConMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConMnuMsView, Me.ConMnuMsEdit, Me.ConMnuMsExport, Me.ConMnuMsPrint, Me.ConMnuMsDelete, Me.ToolStripSeparator14, Me.ConMnuMsOpenFolder, Me.ConMnuMsCopy, Me.ConMnuMsCopyPath})
        Me.ConMenu.Name = "ConMenu"
        Me.ConMenu.Size = New System.Drawing.Size(230, 202)
        '
        'ConMnuMsView
        '
        Me.ConMnuMsView.Name = "ConMnuMsView"
        Me.ConMnuMsView.ShortcutKeyDisplayString = "Enter"
        Me.ConMnuMsView.Size = New System.Drawing.Size(229, 24)
        Me.ConMnuMsView.Text = "预览(&V)"
        '
        'ConMnuMsEdit
        '
        Me.ConMnuMsEdit.Name = "ConMnuMsEdit"
        Me.ConMnuMsEdit.ShortcutKeyDisplayString = "F2"
        Me.ConMnuMsEdit.Size = New System.Drawing.Size(229, 24)
        Me.ConMnuMsEdit.Text = "编辑(&E)..."
        '
        'ConMnuMsExport
        '
        Me.ConMnuMsExport.Name = "ConMnuMsExport"
        Me.ConMnuMsExport.ShortcutKeyDisplayString = "Ctrl+E"
        Me.ConMnuMsExport.Size = New System.Drawing.Size(229, 24)
        Me.ConMnuMsExport.Text = "导出(&X)..."
        '
        'ConMnuMsPrint
        '
        Me.ConMnuMsPrint.Name = "ConMnuMsPrint"
        Me.ConMnuMsPrint.ShortcutKeyDisplayString = "Ctrl+P"
        Me.ConMnuMsPrint.Size = New System.Drawing.Size(229, 24)
        Me.ConMnuMsPrint.Text = "打印(&P)..."
        '
        'ConMnuMsDelete
        '
        Me.ConMnuMsDelete.Name = "ConMnuMsDelete"
        Me.ConMnuMsDelete.ShortcutKeyDisplayString = "Delete"
        Me.ConMnuMsDelete.Size = New System.Drawing.Size(229, 24)
        Me.ConMnuMsDelete.Text = "删除(&D)..."
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(226, 6)
        '
        'ConMnuMsOpenFolder
        '
        Me.ConMnuMsOpenFolder.Name = "ConMnuMsOpenFolder"
        Me.ConMnuMsOpenFolder.ShortcutKeyDisplayString = "Ctrl+T"
        Me.ConMnuMsOpenFolder.Size = New System.Drawing.Size(229, 24)
        Me.ConMnuMsOpenFolder.Text = "打开目录(&A)"
        '
        'ConMnuMsCopy
        '
        Me.ConMnuMsCopy.Name = "ConMnuMsCopy"
        Me.ConMnuMsCopy.ShortcutKeyDisplayString = "Ctrl+C"
        Me.ConMnuMsCopy.Size = New System.Drawing.Size(229, 24)
        Me.ConMnuMsCopy.Text = "复制文件夹(&C)"
        '
        'ConMnuMsCopyPath
        '
        Me.ConMnuMsCopyPath.Name = "ConMnuMsCopyPath"
        Me.ConMnuMsCopyPath.Size = New System.Drawing.Size(229, 24)
        Me.ConMnuMsCopyPath.Text = "复制目录路径(&T)"
        '
        'TlStrip
        '
        Me.TlStrip.Font = New System.Drawing.Font("Microsoft YaHei UI", 15.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World)
        Me.TlStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.TlStrip.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.TlStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSBtnOnTop, Me.TSBtnProperties, Me.ToolStripSeparator15, Me.TSBtnRefresh, Me.TSBtnLibNew, Me.TSBtnLibImport, Me.TSBtnLibExport, Me.TSBtnLibRename, Me.TSBtnLibClose, Me.ToolStripSeparator16, Me.TSBtnMsNew, Me.TSBtnMsImport, Me.TSBtnMsExport, Me.TSBtnMsEdit, Me.TSBtnMsPrint, Me.TSBtnMsOpenFolder, Me.ToolStripSeparator17, Me.TSBtnPlay, Me.TSBtnSearch, Me.TSBtnPrevPage, Me.TSBtnNextPage})
        Me.TlStrip.Location = New System.Drawing.Point(0, 31)
        Me.TlStrip.Name = "TlStrip"
        Me.TlStrip.Size = New System.Drawing.Size(962, 27)
        Me.TlStrip.TabIndex = 9
        Me.TlStrip.Text = "TlStrip"
        '
        'TSBtnOnTop
        '
        Me.TSBtnOnTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSBtnOnTop.Image = CType(resources.GetObject("TSBtnOnTop.Image"), System.Drawing.Image)
        Me.TSBtnOnTop.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSBtnOnTop.Name = "TSBtnOnTop"
        Me.TSBtnOnTop.Size = New System.Drawing.Size(29, 24)
        Me.TSBtnOnTop.Text = "窗口置顶"
        '
        'TSBtnProperties
        '
        Me.TSBtnProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSBtnProperties.Image = CType(resources.GetObject("TSBtnProperties.Image"), System.Drawing.Image)
        Me.TSBtnProperties.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSBtnProperties.Name = "TSBtnProperties"
        Me.TSBtnProperties.Size = New System.Drawing.Size(29, 24)
        Me.TSBtnProperties.Text = "选项"
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(6, 27)
        '
        'TSBtnRefresh
        '
        Me.TSBtnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSBtnRefresh.Image = CType(resources.GetObject("TSBtnRefresh.Image"), System.Drawing.Image)
        Me.TSBtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSBtnRefresh.Name = "TSBtnRefresh"
        Me.TSBtnRefresh.Size = New System.Drawing.Size(29, 24)
        Me.TSBtnRefresh.Text = "刷新"
        '
        'TSBtnLibNew
        '
        Me.TSBtnLibNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSBtnLibNew.Image = CType(resources.GetObject("TSBtnLibNew.Image"), System.Drawing.Image)
        Me.TSBtnLibNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSBtnLibNew.Name = "TSBtnLibNew"
        Me.TSBtnLibNew.Size = New System.Drawing.Size(29, 24)
        Me.TSBtnLibNew.Text = "新建稿件库"
        '
        'TSBtnLibImport
        '
        Me.TSBtnLibImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSBtnLibImport.Image = CType(resources.GetObject("TSBtnLibImport.Image"), System.Drawing.Image)
        Me.TSBtnLibImport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSBtnLibImport.Name = "TSBtnLibImport"
        Me.TSBtnLibImport.Size = New System.Drawing.Size(29, 24)
        Me.TSBtnLibImport.Text = "导入稿件库"
        '
        'TSBtnLibExport
        '
        Me.TSBtnLibExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSBtnLibExport.Image = CType(resources.GetObject("TSBtnLibExport.Image"), System.Drawing.Image)
        Me.TSBtnLibExport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSBtnLibExport.Name = "TSBtnLibExport"
        Me.TSBtnLibExport.Size = New System.Drawing.Size(29, 24)
        Me.TSBtnLibExport.Text = "导出稿件库"
        '
        'TSBtnLibRename
        '
        Me.TSBtnLibRename.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSBtnLibRename.Image = CType(resources.GetObject("TSBtnLibRename.Image"), System.Drawing.Image)
        Me.TSBtnLibRename.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSBtnLibRename.Name = "TSBtnLibRename"
        Me.TSBtnLibRename.Size = New System.Drawing.Size(29, 24)
        Me.TSBtnLibRename.Text = "重命名稿件库"
        '
        'TSBtnLibClose
        '
        Me.TSBtnLibClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSBtnLibClose.Image = CType(resources.GetObject("TSBtnLibClose.Image"), System.Drawing.Image)
        Me.TSBtnLibClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSBtnLibClose.Name = "TSBtnLibClose"
        Me.TSBtnLibClose.Size = New System.Drawing.Size(29, 24)
        Me.TSBtnLibClose.Text = "关闭稿件库"
        '
        'ToolStripSeparator16
        '
        Me.ToolStripSeparator16.Name = "ToolStripSeparator16"
        Me.ToolStripSeparator16.Size = New System.Drawing.Size(6, 27)
        '
        'TSBtnMsNew
        '
        Me.TSBtnMsNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSBtnMsNew.Image = CType(resources.GetObject("TSBtnMsNew.Image"), System.Drawing.Image)
        Me.TSBtnMsNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSBtnMsNew.Name = "TSBtnMsNew"
        Me.TSBtnMsNew.Size = New System.Drawing.Size(29, 24)
        Me.TSBtnMsNew.Text = "新建稿件"
        '
        'TSBtnMsImport
        '
        Me.TSBtnMsImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSBtnMsImport.Image = CType(resources.GetObject("TSBtnMsImport.Image"), System.Drawing.Image)
        Me.TSBtnMsImport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSBtnMsImport.Name = "TSBtnMsImport"
        Me.TSBtnMsImport.Size = New System.Drawing.Size(29, 24)
        Me.TSBtnMsImport.Text = "导入稿件"
        '
        'TSBtnMsExport
        '
        Me.TSBtnMsExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSBtnMsExport.Image = CType(resources.GetObject("TSBtnMsExport.Image"), System.Drawing.Image)
        Me.TSBtnMsExport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSBtnMsExport.Name = "TSBtnMsExport"
        Me.TSBtnMsExport.Size = New System.Drawing.Size(29, 24)
        Me.TSBtnMsExport.Text = "导出稿件"
        '
        'TSBtnMsEdit
        '
        Me.TSBtnMsEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSBtnMsEdit.Image = CType(resources.GetObject("TSBtnMsEdit.Image"), System.Drawing.Image)
        Me.TSBtnMsEdit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSBtnMsEdit.Name = "TSBtnMsEdit"
        Me.TSBtnMsEdit.Size = New System.Drawing.Size(29, 24)
        Me.TSBtnMsEdit.Text = "编辑稿件"
        '
        'TSBtnMsPrint
        '
        Me.TSBtnMsPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSBtnMsPrint.Image = CType(resources.GetObject("TSBtnMsPrint.Image"), System.Drawing.Image)
        Me.TSBtnMsPrint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSBtnMsPrint.Name = "TSBtnMsPrint"
        Me.TSBtnMsPrint.Size = New System.Drawing.Size(29, 24)
        Me.TSBtnMsPrint.Text = "打印稿件"
        '
        'TSBtnMsOpenFolder
        '
        Me.TSBtnMsOpenFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSBtnMsOpenFolder.Image = CType(resources.GetObject("TSBtnMsOpenFolder.Image"), System.Drawing.Image)
        Me.TSBtnMsOpenFolder.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSBtnMsOpenFolder.Name = "TSBtnMsOpenFolder"
        Me.TSBtnMsOpenFolder.Size = New System.Drawing.Size(29, 24)
        Me.TSBtnMsOpenFolder.Text = "打开稿件文件夹"
        '
        'ToolStripSeparator17
        '
        Me.ToolStripSeparator17.Name = "ToolStripSeparator17"
        Me.ToolStripSeparator17.Size = New System.Drawing.Size(6, 27)
        '
        'TSBtnPlay
        '
        Me.TSBtnPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSBtnPlay.Image = CType(resources.GetObject("TSBtnPlay.Image"), System.Drawing.Image)
        Me.TSBtnPlay.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSBtnPlay.Name = "TSBtnPlay"
        Me.TSBtnPlay.Size = New System.Drawing.Size(29, 24)
        Me.TSBtnPlay.Text = "幻灯片播放"
        '
        'TSBtnSearch
        '
        Me.TSBtnSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSBtnSearch.Image = CType(resources.GetObject("TSBtnSearch.Image"), System.Drawing.Image)
        Me.TSBtnSearch.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSBtnSearch.Name = "TSBtnSearch"
        Me.TSBtnSearch.Size = New System.Drawing.Size(29, 24)
        Me.TSBtnSearch.Text = "高级搜索"
        '
        'TSBtnPrevPage
        '
        Me.TSBtnPrevPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSBtnPrevPage.Image = CType(resources.GetObject("TSBtnPrevPage.Image"), System.Drawing.Image)
        Me.TSBtnPrevPage.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSBtnPrevPage.Name = "TSBtnPrevPage"
        Me.TSBtnPrevPage.Size = New System.Drawing.Size(29, 24)
        Me.TSBtnPrevPage.Text = "上一页"
        '
        'TSBtnNextPage
        '
        Me.TSBtnNextPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSBtnNextPage.Image = CType(resources.GetObject("TSBtnNextPage.Image"), System.Drawing.Image)
        Me.TSBtnNextPage.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSBtnNextPage.Name = "TSBtnNextPage"
        Me.TSBtnNextPage.Size = New System.Drawing.Size(29, 24)
        Me.TSBtnNextPage.Text = "下一页"
        '
        'ArtworkListSplitContainer
        '
        Me.ArtworkListSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ArtworkListSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.ArtworkListSplitContainer.Location = New System.Drawing.Point(0, 58)
        Me.ArtworkListSplitContainer.Name = "ArtworkListSplitContainer"
        '
        'ArtworkListSplitContainer.Panel1
        '
        Me.ArtworkListSplitContainer.Panel1.Controls.Add(Me.ImageGalleryMain)
        Me.ArtworkListSplitContainer.Panel1.Padding = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.ArtworkListSplitContainer.Panel1MinSize = 500
        '
        'ArtworkListSplitContainer.Panel2
        '
        Me.ArtworkListSplitContainer.Panel2.Controls.Add(Me.LblNotes)
        Me.ArtworkListSplitContainer.Panel2.Controls.Add(Me.LblDeltas)
        Me.ArtworkListSplitContainer.Panel2.Controls.Add(Me.LblTags)
        Me.ArtworkListSplitContainer.Panel2.Controls.Add(Me.LblCharacters)
        Me.ArtworkListSplitContainer.Panel2.Controls.Add(Me.LblAuthor)
        Me.ArtworkListSplitContainer.Panel2.Controls.Add(Me.LblTitle)
        Me.ArtworkListSplitContainer.Panel2.Controls.Add(Me.PiChkThumb)
        Me.ArtworkListSplitContainer.Panel2.Padding = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.ArtworkListSplitContainer.Panel2MinSize = 220
        Me.ArtworkListSplitContainer.Size = New System.Drawing.Size(962, 509)
        Me.ArtworkListSplitContainer.SplitterDistance = 730
        Me.ArtworkListSplitContainer.TabIndex = 3
        '
        'LblNotes
        '
        Me.LblNotes.Dock = System.Windows.Forms.DockStyle.Top
        Me.LblNotes.Location = New System.Drawing.Point(0, 325)
        Me.LblNotes.Name = "LblNotes"
        Me.LblNotes.Size = New System.Drawing.Size(225, 120)
        Me.LblNotes.TabIndex = 7
        Me.LblNotes.Text = "备注"
        '
        'LblDeltas
        '
        Me.LblDeltas.Dock = System.Windows.Forms.DockStyle.Top
        Me.LblDeltas.Location = New System.Drawing.Point(0, 305)
        Me.LblDeltas.Name = "LblDeltas"
        Me.LblDeltas.Size = New System.Drawing.Size(225, 20)
        Me.LblDeltas.TabIndex = 8
        Me.LblDeltas.Text = "差分"
        Me.LblDeltas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LblTags
        '
        Me.LblTags.Dock = System.Windows.Forms.DockStyle.Top
        Me.LblTags.Location = New System.Drawing.Point(0, 285)
        Me.LblTags.Name = "LblTags"
        Me.LblTags.Size = New System.Drawing.Size(225, 20)
        Me.LblTags.TabIndex = 4
        Me.LblTags.Text = "标签"
        Me.LblTags.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LblCharacters
        '
        Me.LblCharacters.Dock = System.Windows.Forms.DockStyle.Top
        Me.LblCharacters.Location = New System.Drawing.Point(0, 265)
        Me.LblCharacters.Name = "LblCharacters"
        Me.LblCharacters.Size = New System.Drawing.Size(225, 20)
        Me.LblCharacters.TabIndex = 3
        Me.LblCharacters.Text = "角色"
        Me.LblCharacters.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LblAuthor
        '
        Me.LblAuthor.Dock = System.Windows.Forms.DockStyle.Top
        Me.LblAuthor.Location = New System.Drawing.Point(0, 245)
        Me.LblAuthor.Name = "LblAuthor"
        Me.LblAuthor.Size = New System.Drawing.Size(225, 20)
        Me.LblAuthor.TabIndex = 2
        Me.LblAuthor.Text = "作者"
        Me.LblAuthor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LblTitle
        '
        Me.LblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.LblTitle.Location = New System.Drawing.Point(0, 220)
        Me.LblTitle.Name = "LblTitle"
        Me.LblTitle.Size = New System.Drawing.Size(225, 25)
        Me.LblTitle.TabIndex = 1
        Me.LblTitle.Text = "标题"
        Me.LblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PiChkThumb
        '
        Me.PiChkThumb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PiChkThumb.Dock = System.Windows.Forms.DockStyle.Top
        Me.PiChkThumb.Location = New System.Drawing.Point(0, 0)
        Me.PiChkThumb.Name = "PiChkThumb"
        Me.PiChkThumb.Size = New System.Drawing.Size(225, 220)
        Me.PiChkThumb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PiChkThumb.TabIndex = 0
        Me.PiChkThumb.TabStop = False
        '
        'NotifyIco
        '
        Me.NotifyIco.Text = "FurryArtStudio"
        Me.NotifyIco.Visible = True
        '
        'ImageGalleryMain
        '
        Me.ImageGalleryMain.AutoScroll = True
        Me.ImageGalleryMain.BackColor = System.Drawing.Color.White
        Me.ImageGalleryMain.BadgeColor = System.Drawing.Color.Red
        Me.ImageGalleryMain.DisplayMode = FurryArtStudio.GalleryDisplayMode.Normal
        Me.ImageGalleryMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ImageGalleryMain.Location = New System.Drawing.Point(3, 0)
        Me.ImageGalleryMain.MaxItemSize = 240
        Me.ImageGalleryMain.MinItemSize = 120
        Me.ImageGalleryMain.Name = "ImageGalleryMain"
        Me.ImageGalleryMain.PageSize = 100
        Me.ImageGalleryMain.SelectionAccentColor = System.Drawing.Color.Blue
        Me.ImageGalleryMain.Size = New System.Drawing.Size(727, 509)
        Me.ImageGalleryMain.TabIndex = 3
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(120.0!, 120.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(962, 593)
        Me.Controls.Add(Me.ArtworkListSplitContainer)
        Me.Controls.Add(Me.TlStrip)
        Me.Controls.Add(Me.MnuStrip)
        Me.Controls.Add(Me.StaStrip)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MnuStrip
        Me.MinimumSize = New System.Drawing.Size(500, 250)
        Me.Name = "MainForm"
        Me.Text = "FurryArtStudio"
        Me.MnuStrip.ResumeLayout(False)
        Me.MnuStrip.PerformLayout()
        Me.StaStrip.ResumeLayout(False)
        Me.StaStrip.PerformLayout()
        Me.ConMenu.ResumeLayout(False)
        Me.TlStrip.ResumeLayout(False)
        Me.TlStrip.PerformLayout()
        Me.ArtworkListSplitContainer.Panel1.ResumeLayout(False)
        Me.ArtworkListSplitContainer.Panel2.ResumeLayout(False)
        CType(Me.ArtworkListSplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ArtworkListSplitContainer.ResumeLayout(False)
        CType(Me.PiChkThumb, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MnuStrip As MenuStrip
    Friend WithEvents MnuLibrary As ToolStripMenuItem
    Friend WithEvents MnuManuscript As ToolStripMenuItem
    Friend WithEvents MnuMsView As ToolStripMenuItem
    Friend WithEvents MnuMsEdit As ToolStripMenuItem
    Friend WithEvents MnuMsDelete As ToolStripMenuItem
    Friend WithEvents MnuHelp As ToolStripMenuItem
    Friend WithEvents MnuLibNew As ToolStripMenuItem
    Friend WithEvents MnuLibImport As ToolStripMenuItem
    Friend WithEvents MnuLibExport As ToolStripMenuItem
    Friend WithEvents MnuFile As ToolStripMenuItem
    Friend WithEvents MnuProperties As ToolStripMenuItem
    Friend WithEvents MnuExit As ToolStripMenuItem
    Friend WithEvents MnuMsNew As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents MnuLibClone As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents MnuMsOpenFolder As ToolStripMenuItem
    Friend WithEvents MnuLibOpenFolder As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
    Friend WithEvents MnuLibDelete As ToolStripMenuItem
    Friend WithEvents MnuLibRename As ToolStripMenuItem
    Friend WithEvents MnuLibRefresh As ToolStripMenuItem
    Friend WithEvents MnuLibCopy As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents MnuHelpAbout As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator8 As ToolStripSeparator
    Friend WithEvents MnuHelpWebsite As ToolStripMenuItem
    Friend WithEvents MnuHelpGithub As ToolStripMenuItem
    Friend WithEvents MnuLibList As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator11 As ToolStripSeparator
    Friend WithEvents ArtworkListSplitContainer As SplitContainer
    Friend WithEvents MnuHelpTutorial As ToolStripMenuItem
    Friend WithEvents MnuMsExport As ToolStripMenuItem
    Friend WithEvents MnuMsPrint As ToolStripMenuItem
    Friend WithEvents MnuLibCopyPath As ToolStripMenuItem
    Friend WithEvents MnuMsCopy As ToolStripMenuItem
    Friend WithEvents MnuMsCopyPath As ToolStripMenuItem
    Friend WithEvents MnuMsImport As ToolStripMenuItem
    Friend WithEvents MnuLibStatistics As ToolStripMenuItem
    Friend WithEvents MnuLibClose As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents StaStrip As StatusStrip
    Friend WithEvents StatusLabel As ToolStripStatusLabel
    Friend WithEvents TSspace As ToolStripStatusLabel
    Friend WithEvents ArtworkStatusLabel As ToolStripStatusLabel
    Friend WithEvents SelectStatusLabel As ToolStripStatusLabel
    Friend WithEvents StorageStatusLabel As ToolStripStatusLabel
    Friend WithEvents TSSep2 As ToolStripStatusLabel
    Friend WithEvents TSSep3 As ToolStripStatusLabel
    Friend WithEvents TSSep4 As ToolStripStatusLabel
    Friend WithEvents ImageGalleryMain As ImageGallery
    Friend WithEvents PiChkThumb As PictureBox
    Friend WithEvents PageStatusLabel As ToolStripStatusLabel
    Friend WithEvents TSSep1 As ToolStripStatusLabel
    Friend WithEvents LblTitle As Label
    Friend WithEvents LblAuthor As Label
    Friend WithEvents LblNotes As Label
    Friend WithEvents LblTags As Label
    Friend WithEvents LblCharacters As Label
    Friend WithEvents MnuViews As ToolStripMenuItem
    Friend WithEvents MnuSelectAll As ToolStripMenuItem
    Friend WithEvents MnuSelectReverse As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator13 As ToolStripSeparator
    Friend WithEvents MnuSearch As ToolStripMenuItem
    Friend WithEvents MnuAdvancedSearch As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator12 As ToolStripSeparator
    Friend WithEvents MnuViewPlay As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents MnuPageUp As ToolStripMenuItem
    Friend WithEvents MnuPageDown As ToolStripMenuItem
    Friend WithEvents MnuHelpPrivacy As ToolStripMenuItem
    Friend WithEvents MnuHelpLicense As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator10 As ToolStripSeparator
    Friend WithEvents KryptonMgrMain As Krypton.Toolkit.KryptonManager
    Friend WithEvents TSSep5 As ToolStripStatusLabel
    Friend WithEvents SearchStatusLabel As ToolStripStatusLabel
    Friend WithEvents MnuLibExportCSV As ToolStripMenuItem
    Friend WithEvents MnuDevTools As ToolStripMenuItem
    Friend WithEvents MnuCheckUpdate As ToolStripMenuItem
    Friend WithEvents MnuHelpWhatsNew As ToolStripMenuItem
    Friend WithEvents MnuRunAsElevated As ToolStripMenuItem
    Friend WithEvents MnuRunTerminal As ToolStripMenuItem
    Friend WithEvents MnuTerms As ToolStripMenuItem
    Friend WithEvents ConMenu As ContextMenuStrip
    Friend WithEvents ConMnuMsView As ToolStripMenuItem
    Friend WithEvents ConMnuMsEdit As ToolStripMenuItem
    Friend WithEvents ConMnuMsExport As ToolStripMenuItem
    Friend WithEvents ConMnuMsPrint As ToolStripMenuItem
    Friend WithEvents ConMnuMsDelete As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator14 As ToolStripSeparator
    Friend WithEvents ConMnuMsOpenFolder As ToolStripMenuItem
    Friend WithEvents ConMnuMsCopy As ToolStripMenuItem
    Friend WithEvents ConMnuMsCopyPath As ToolStripMenuItem
    Friend WithEvents MnuHelpDonate As ToolStripMenuItem
    Friend WithEvents MnuOpenAfdian As ToolStripMenuItem
    Friend WithEvents MnuLoadSponsors As ToolStripMenuItem
    Friend WithEvents MnuCreateShortcut As ToolStripMenuItem
    Friend WithEvents MnuFeedback As ToolStripMenuItem
    Friend WithEvents MnuBugReport As ToolStripMenuItem
    Friend WithEvents MnuMyFeedback As ToolStripMenuItem
    Friend WithEvents MnuSuggestions As ToolStripMenuItem
    Friend WithEvents MnuOpenPath As ToolStripMenuItem
    Friend WithEvents TlStrip As ToolStrip
    Friend WithEvents TSBtnOnTop As ToolStripButton
    Friend WithEvents TSBtnProperties As ToolStripButton
    Friend WithEvents ToolStripSeparator15 As ToolStripSeparator
    Friend WithEvents TSBtnRefresh As ToolStripButton
    Friend WithEvents TSBtnLibNew As ToolStripButton
    Friend WithEvents TSBtnLibImport As ToolStripButton
    Friend WithEvents TSBtnLibExport As ToolStripButton
    Friend WithEvents TSBtnLibRename As ToolStripButton
    Friend WithEvents TSBtnLibClose As ToolStripButton
    Friend WithEvents ToolStripSeparator16 As ToolStripSeparator
    Friend WithEvents TSBtnMsNew As ToolStripButton
    Friend WithEvents TSBtnMsImport As ToolStripButton
    Friend WithEvents TSBtnMsExport As ToolStripButton
    Friend WithEvents TSBtnMsEdit As ToolStripButton
    Friend WithEvents TSBtnMsPrint As ToolStripButton
    Friend WithEvents TSBtnMsOpenFolder As ToolStripButton
    Friend WithEvents ToolStripSeparator17 As ToolStripSeparator
    Friend WithEvents TSBtnPlay As ToolStripButton
    Friend WithEvents TSBtnPrevPage As ToolStripButton
    Friend WithEvents TSBtnNextPage As ToolStripButton
    Friend WithEvents TSBtnSearch As ToolStripButton
    Friend WithEvents LblDeltas As Label
    Friend WithEvents MnuSearchTxtbox As ToolStripTextBox
    Friend WithEvents MnuShowStatusBar As ToolStripMenuItem
    Friend WithEvents MnuShowToolBar As ToolStripMenuItem
    Friend WithEvents MnuTheme As ToolStripMenuItem
    Friend WithEvents MnuThemeSystem As ToolStripMenuItem
    Friend WithEvents MnuThemeLight As ToolStripMenuItem
    Friend WithEvents MnuThemeDark As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator18 As ToolStripSeparator
    Friend WithEvents MnuOnTop As ToolStripMenuItem
    Friend WithEvents MnuPrivacyProtect As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator9 As ToolStripSeparator
    Friend WithEvents MnuExtension As ToolStripMenuItem
    Friend WithEvents WebHookToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MnuPlugin As ToolStripMenuItem
    Friend WithEvents 打开插件目录PToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator19 As ToolStripSeparator
    Friend WithEvents MnuTray As ToolStripMenuItem
    Friend WithEvents NotifyIco As NotifyIcon
    Friend WithEvents MnuAddStartMenu As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator20 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator21 As ToolStripSeparator
End Class