<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PropertiesForm
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ColorDlg = New System.Windows.Forms.ColorDialog()
        Me.TabGrp = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TxtUserName = New System.Windows.Forms.TextBox()
        Me.LblMyname = New System.Windows.Forms.Label()
        Me.ChkShowStatus = New System.Windows.Forms.CheckBox()
        Me.ChkShowTool = New System.Windows.Forms.CheckBox()
        Me.ChkMenuUpper = New System.Windows.Forms.CheckBox()
        Me.ChkShowThemeColor = New System.Windows.Forms.CheckBox()
        Me.LblLanguage = New System.Windows.Forms.Label()
        Me.CboLang = New System.Windows.Forms.ComboBox()
        Me.LblCorColor = New System.Windows.Forms.Label()
        Me.LblSelColor = New System.Windows.Forms.Label()
        Me.BtnCornerColor = New System.Windows.Forms.Button()
        Me.BtnSelectedColor = New System.Windows.Forms.Button()
        Me.LblCornerColor = New System.Windows.Forms.Label()
        Me.LblSelectedColor = New System.Windows.Forms.Label()
        Me.LblThemeColor = New System.Windows.Forms.Label()
        Me.BtnThemeColor = New System.Windows.Forms.Button()
        Me.ChkShowPicThemeColor = New System.Windows.Forms.CheckBox()
        Me.RadSystem = New System.Windows.Forms.RadioButton()
        Me.RadDark = New System.Windows.Forms.RadioButton()
        Me.RadLight = New System.Windows.Forms.RadioButton()
        Me.LblTheme = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.ChkShowHito = New System.Windows.Forms.CheckBox()
        Me.ChkRestore = New System.Windows.Forms.CheckBox()
        Me.CboCheckUpdate = New System.Windows.Forms.ComboBox()
        Me.ChkAutoCheckUpdate = New System.Windows.Forms.CheckBox()
        Me.ChkAutoPlay = New System.Windows.Forms.CheckBox()
        Me.ChkAutoStart = New System.Windows.Forms.CheckBox()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.ChkLoop = New System.Windows.Forms.CheckBox()
        Me.ChkDevTools = New System.Windows.Forms.CheckBox()
        Me.ChkPlayNext = New System.Windows.Forms.CheckBox()
        Me.TxtAutoNext = New System.Windows.Forms.TextBox()
        Me.LblAutoNext = New System.Windows.Forms.Label()
        Me.ChkFileAssociation = New System.Windows.Forms.CheckBox()
        Me.ChkKeepScale = New System.Windows.Forms.CheckBox()
        Me.TxtMaxSize = New System.Windows.Forms.TextBox()
        Me.LblThumbMax = New System.Windows.Forms.Label()
        Me.TxtMinSize = New System.Windows.Forms.TextBox()
        Me.LblThumbMin = New System.Windows.Forms.Label()
        Me.TabGrp.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabGrp
        '
        Me.TabGrp.Controls.Add(Me.TabPage1)
        Me.TabGrp.Controls.Add(Me.TabPage2)
        Me.TabGrp.Controls.Add(Me.TabPage3)
        Me.TabGrp.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabGrp.Location = New System.Drawing.Point(0, 0)
        Me.TabGrp.Name = "TabGrp"
        Me.TabGrp.SelectedIndex = 0
        Me.TabGrp.Size = New System.Drawing.Size(432, 453)
        Me.TabGrp.TabIndex = 59
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.TxtUserName)
        Me.TabPage1.Controls.Add(Me.LblMyname)
        Me.TabPage1.Controls.Add(Me.ChkShowStatus)
        Me.TabPage1.Controls.Add(Me.ChkShowTool)
        Me.TabPage1.Controls.Add(Me.ChkMenuUpper)
        Me.TabPage1.Controls.Add(Me.ChkShowThemeColor)
        Me.TabPage1.Controls.Add(Me.LblLanguage)
        Me.TabPage1.Controls.Add(Me.CboLang)
        Me.TabPage1.Controls.Add(Me.LblCorColor)
        Me.TabPage1.Controls.Add(Me.LblSelColor)
        Me.TabPage1.Controls.Add(Me.BtnCornerColor)
        Me.TabPage1.Controls.Add(Me.BtnSelectedColor)
        Me.TabPage1.Controls.Add(Me.LblCornerColor)
        Me.TabPage1.Controls.Add(Me.LblSelectedColor)
        Me.TabPage1.Controls.Add(Me.LblThemeColor)
        Me.TabPage1.Controls.Add(Me.BtnThemeColor)
        Me.TabPage1.Controls.Add(Me.ChkShowPicThemeColor)
        Me.TabPage1.Controls.Add(Me.RadSystem)
        Me.TabPage1.Controls.Add(Me.RadDark)
        Me.TabPage1.Controls.Add(Me.RadLight)
        Me.TabPage1.Controls.Add(Me.LblTheme)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(424, 424)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "外观"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TxtUserName
        '
        Me.TxtUserName.Location = New System.Drawing.Point(265, 267)
        Me.TxtUserName.Name = "TxtUserName"
        Me.TxtUserName.Size = New System.Drawing.Size(143, 25)
        Me.TxtUserName.TabIndex = 96
        '
        'LblMyname
        '
        Me.LblMyname.AutoSize = True
        Me.LblMyname.Location = New System.Drawing.Point(9, 270)
        Me.LblMyname.Name = "LblMyname"
        Me.LblMyname.Size = New System.Drawing.Size(82, 15)
        Me.LblMyname.TabIndex = 95
        Me.LblMyname.Text = "我的名字："
        '
        'ChkShowStatus
        '
        Me.ChkShowStatus.AutoSize = True
        Me.ChkShowStatus.Location = New System.Drawing.Point(206, 236)
        Me.ChkShowStatus.Name = "ChkShowStatus"
        Me.ChkShowStatus.Size = New System.Drawing.Size(104, 19)
        Me.ChkShowStatus.TabIndex = 94
        Me.ChkShowStatus.Text = "显示状态栏"
        Me.ChkShowStatus.UseVisualStyleBackColor = True
        '
        'ChkShowTool
        '
        Me.ChkShowTool.AutoSize = True
        Me.ChkShowTool.Location = New System.Drawing.Point(12, 236)
        Me.ChkShowTool.Name = "ChkShowTool"
        Me.ChkShowTool.Size = New System.Drawing.Size(104, 19)
        Me.ChkShowTool.TabIndex = 93
        Me.ChkShowTool.Text = "显示工具栏"
        Me.ChkShowTool.UseVisualStyleBackColor = True
        '
        'ChkMenuUpper
        '
        Me.ChkMenuUpper.AutoSize = True
        Me.ChkMenuUpper.Location = New System.Drawing.Point(12, 127)
        Me.ChkMenuUpper.Name = "ChkMenuUpper"
        Me.ChkMenuUpper.Size = New System.Drawing.Size(119, 19)
        Me.ChkMenuUpper.TabIndex = 92
        Me.ChkMenuUpper.Text = "菜单栏全大写"
        Me.ChkMenuUpper.UseVisualStyleBackColor = True
        '
        'ChkShowThemeColor
        '
        Me.ChkShowThemeColor.AutoSize = True
        Me.ChkShowThemeColor.Location = New System.Drawing.Point(12, 49)
        Me.ChkShowThemeColor.Name = "ChkShowThemeColor"
        Me.ChkShowThemeColor.Size = New System.Drawing.Size(251, 19)
        Me.ChkShowThemeColor.TabIndex = 91
        Me.ChkShowThemeColor.Text = "显示主题色（仅Windows11有效）"
        Me.ChkShowThemeColor.UseVisualStyleBackColor = True
        '
        'LblLanguage
        '
        Me.LblLanguage.AutoSize = True
        Me.LblLanguage.Location = New System.Drawing.Point(9, 100)
        Me.LblLanguage.Name = "LblLanguage"
        Me.LblLanguage.Size = New System.Drawing.Size(52, 15)
        Me.LblLanguage.TabIndex = 90
        Me.LblLanguage.Text = "语言："
        '
        'CboLang
        '
        Me.CboLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboLang.FormattingEnabled = True
        Me.CboLang.Location = New System.Drawing.Point(191, 97)
        Me.CboLang.Name = "CboLang"
        Me.CboLang.Size = New System.Drawing.Size(217, 23)
        Me.CboLang.TabIndex = 89
        '
        'LblCorColor
        '
        Me.LblCorColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblCorColor.Location = New System.Drawing.Point(265, 187)
        Me.LblCorColor.Name = "LblCorColor"
        Me.LblCorColor.Size = New System.Drawing.Size(35, 35)
        Me.LblCorColor.TabIndex = 88
        '
        'LblSelColor
        '
        Me.LblSelColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblSelColor.Location = New System.Drawing.Point(265, 146)
        Me.LblSelColor.Name = "LblSelColor"
        Me.LblSelColor.Size = New System.Drawing.Size(35, 35)
        Me.LblSelColor.TabIndex = 87
        '
        'BtnCornerColor
        '
        Me.BtnCornerColor.Location = New System.Drawing.Point(300, 185)
        Me.BtnCornerColor.Name = "BtnCornerColor"
        Me.BtnCornerColor.Size = New System.Drawing.Size(108, 41)
        Me.BtnCornerColor.TabIndex = 86
        Me.BtnCornerColor.Text = "颜色..."
        Me.BtnCornerColor.UseVisualStyleBackColor = True
        '
        'BtnSelectedColor
        '
        Me.BtnSelectedColor.Location = New System.Drawing.Point(300, 143)
        Me.BtnSelectedColor.Name = "BtnSelectedColor"
        Me.BtnSelectedColor.Size = New System.Drawing.Size(108, 41)
        Me.BtnSelectedColor.TabIndex = 85
        Me.BtnSelectedColor.Text = "颜色..."
        Me.BtnSelectedColor.UseVisualStyleBackColor = True
        '
        'LblCornerColor
        '
        Me.LblCornerColor.AutoSize = True
        Me.LblCornerColor.Location = New System.Drawing.Point(9, 198)
        Me.LblCornerColor.Name = "LblCornerColor"
        Me.LblCornerColor.Size = New System.Drawing.Size(97, 15)
        Me.LblCornerColor.TabIndex = 84
        Me.LblCornerColor.Text = "角标背景色："
        '
        'LblSelectedColor
        '
        Me.LblSelectedColor.AutoSize = True
        Me.LblSelectedColor.Location = New System.Drawing.Point(9, 156)
        Me.LblSelectedColor.Name = "LblSelectedColor"
        Me.LblSelectedColor.Size = New System.Drawing.Size(112, 15)
        Me.LblSelectedColor.TabIndex = 83
        Me.LblSelectedColor.Text = "选中项强调色："
        '
        'LblThemeColor
        '
        Me.LblThemeColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblThemeColor.Location = New System.Drawing.Point(265, 40)
        Me.LblThemeColor.Name = "LblThemeColor"
        Me.LblThemeColor.Size = New System.Drawing.Size(35, 35)
        Me.LblThemeColor.TabIndex = 82
        '
        'BtnThemeColor
        '
        Me.BtnThemeColor.Location = New System.Drawing.Point(300, 37)
        Me.BtnThemeColor.Name = "BtnThemeColor"
        Me.BtnThemeColor.Size = New System.Drawing.Size(108, 41)
        Me.BtnThemeColor.TabIndex = 81
        Me.BtnThemeColor.Text = "颜色..."
        Me.BtnThemeColor.UseVisualStyleBackColor = True
        '
        'ChkShowPicThemeColor
        '
        Me.ChkShowPicThemeColor.AutoSize = True
        Me.ChkShowPicThemeColor.Location = New System.Drawing.Point(12, 74)
        Me.ChkShowPicThemeColor.Name = "ChkShowPicThemeColor"
        Me.ChkShowPicThemeColor.Size = New System.Drawing.Size(194, 19)
        Me.ChkShowPicThemeColor.TabIndex = 80
        Me.ChkShowPicThemeColor.Text = "预览窗口显示图片主题色"
        Me.ChkShowPicThemeColor.UseVisualStyleBackColor = True
        '
        'RadSystem
        '
        Me.RadSystem.AutoSize = True
        Me.RadSystem.Location = New System.Drawing.Point(312, 17)
        Me.RadSystem.Name = "RadSystem"
        Me.RadSystem.Size = New System.Drawing.Size(88, 19)
        Me.RadSystem.TabIndex = 30
        Me.RadSystem.Text = "跟随系统"
        Me.RadSystem.UseVisualStyleBackColor = True
        '
        'RadDark
        '
        Me.RadDark.AutoSize = True
        Me.RadDark.Location = New System.Drawing.Point(191, 17)
        Me.RadDark.Name = "RadDark"
        Me.RadDark.Size = New System.Drawing.Size(88, 19)
        Me.RadDark.TabIndex = 29
        Me.RadDark.Text = "深色模式"
        Me.RadDark.UseVisualStyleBackColor = True
        '
        'RadLight
        '
        Me.RadLight.AutoSize = True
        Me.RadLight.Location = New System.Drawing.Point(70, 17)
        Me.RadLight.Name = "RadLight"
        Me.RadLight.Size = New System.Drawing.Size(88, 19)
        Me.RadLight.TabIndex = 28
        Me.RadLight.Text = "浅色模式"
        Me.RadLight.UseVisualStyleBackColor = True
        '
        'LblTheme
        '
        Me.LblTheme.AutoSize = True
        Me.LblTheme.Location = New System.Drawing.Point(8, 17)
        Me.LblTheme.Name = "LblTheme"
        Me.LblTheme.Size = New System.Drawing.Size(52, 15)
        Me.LblTheme.TabIndex = 27
        Me.LblTheme.Text = "主题："
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.ChkShowHito)
        Me.TabPage2.Controls.Add(Me.ChkRestore)
        Me.TabPage2.Controls.Add(Me.CboCheckUpdate)
        Me.TabPage2.Controls.Add(Me.ChkAutoCheckUpdate)
        Me.TabPage2.Controls.Add(Me.ChkAutoPlay)
        Me.TabPage2.Controls.Add(Me.ChkAutoStart)
        Me.TabPage2.Location = New System.Drawing.Point(4, 25)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(424, 424)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "启动"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'ChkShowHito
        '
        Me.ChkShowHito.AutoSize = True
        Me.ChkShowHito.Location = New System.Drawing.Point(8, 117)
        Me.ChkShowHito.Name = "ChkShowHito"
        Me.ChkShowHito.Size = New System.Drawing.Size(134, 19)
        Me.ChkShowHito.TabIndex = 66
        Me.ChkShowHito.Text = "启动时显示一言"
        Me.ChkShowHito.UseVisualStyleBackColor = True
        '
        'ChkRestore
        '
        Me.ChkRestore.AutoSize = True
        Me.ChkRestore.Location = New System.Drawing.Point(8, 40)
        Me.ChkRestore.Name = "ChkRestore"
        Me.ChkRestore.Size = New System.Drawing.Size(194, 19)
        Me.ChkRestore.TabIndex = 65
        Me.ChkRestore.Text = "启动时恢复上次关闭的库"
        Me.ChkRestore.UseVisualStyleBackColor = True
        '
        'CboCheckUpdate
        '
        Me.CboCheckUpdate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboCheckUpdate.FormattingEnabled = True
        Me.CboCheckUpdate.Location = New System.Drawing.Point(185, 90)
        Me.CboCheckUpdate.Name = "CboCheckUpdate"
        Me.CboCheckUpdate.Size = New System.Drawing.Size(181, 23)
        Me.CboCheckUpdate.TabIndex = 64
        '
        'ChkAutoCheckUpdate
        '
        Me.ChkAutoCheckUpdate.AutoSize = True
        Me.ChkAutoCheckUpdate.Location = New System.Drawing.Point(8, 92)
        Me.ChkAutoCheckUpdate.Name = "ChkAutoCheckUpdate"
        Me.ChkAutoCheckUpdate.Size = New System.Drawing.Size(119, 19)
        Me.ChkAutoCheckUpdate.TabIndex = 63
        Me.ChkAutoCheckUpdate.Text = "自动检查更新"
        Me.ChkAutoCheckUpdate.UseVisualStyleBackColor = True
        '
        'ChkAutoPlay
        '
        Me.ChkAutoPlay.AutoSize = True
        Me.ChkAutoPlay.Location = New System.Drawing.Point(23, 65)
        Me.ChkAutoPlay.Name = "ChkAutoPlay"
        Me.ChkAutoPlay.Size = New System.Drawing.Size(179, 19)
        Me.ChkAutoPlay.TabIndex = 62
        Me.ChkAutoPlay.Text = "启动时自动播放幻灯片"
        Me.ChkAutoPlay.UseVisualStyleBackColor = True
        '
        'ChkAutoStart
        '
        Me.ChkAutoStart.AutoSize = True
        Me.ChkAutoStart.Location = New System.Drawing.Point(8, 15)
        Me.ChkAutoStart.Name = "ChkAutoStart"
        Me.ChkAutoStart.Size = New System.Drawing.Size(254, 19)
        Me.ChkAutoStart.TabIndex = 61
        Me.ChkAutoStart.Text = "开机时自动启动 FurryArtStudio"
        Me.ChkAutoStart.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.ChkLoop)
        Me.TabPage3.Controls.Add(Me.ChkDevTools)
        Me.TabPage3.Controls.Add(Me.ChkPlayNext)
        Me.TabPage3.Controls.Add(Me.TxtAutoNext)
        Me.TabPage3.Controls.Add(Me.LblAutoNext)
        Me.TabPage3.Controls.Add(Me.ChkFileAssociation)
        Me.TabPage3.Controls.Add(Me.ChkKeepScale)
        Me.TabPage3.Controls.Add(Me.TxtMaxSize)
        Me.TabPage3.Controls.Add(Me.LblThumbMax)
        Me.TabPage3.Controls.Add(Me.TxtMinSize)
        Me.TabPage3.Controls.Add(Me.LblThumbMin)
        Me.TabPage3.Location = New System.Drawing.Point(4, 25)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(424, 424)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "高级"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'ChkLoop
        '
        Me.ChkLoop.AutoSize = True
        Me.ChkLoop.Location = New System.Drawing.Point(10, 129)
        Me.ChkLoop.Name = "ChkLoop"
        Me.ChkLoop.Size = New System.Drawing.Size(89, 19)
        Me.ChkLoop.TabIndex = 99
        Me.ChkLoop.Text = "循环播放"
        Me.ChkLoop.UseVisualStyleBackColor = True
        '
        'ChkDevTools
        '
        Me.ChkDevTools.AutoSize = True
        Me.ChkDevTools.Location = New System.Drawing.Point(10, 179)
        Me.ChkDevTools.Name = "ChkDevTools"
        Me.ChkDevTools.Size = New System.Drawing.Size(134, 19)
        Me.ChkDevTools.TabIndex = 98
        Me.ChkDevTools.Text = "显示开发者选项"
        Me.ChkDevTools.UseVisualStyleBackColor = True
        '
        'ChkPlayNext
        '
        Me.ChkPlayNext.AutoSize = True
        Me.ChkPlayNext.Location = New System.Drawing.Point(25, 154)
        Me.ChkPlayNext.Name = "ChkPlayNext"
        Me.ChkPlayNext.Size = New System.Drawing.Size(179, 19)
        Me.ChkPlayNext.TabIndex = 97
        Me.ChkPlayNext.Text = "播放后切换到下一个库"
        Me.ChkPlayNext.UseVisualStyleBackColor = True
        '
        'TxtAutoNext
        '
        Me.TxtAutoNext.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtAutoNext.Location = New System.Drawing.Point(212, 94)
        Me.TxtAutoNext.Name = "TxtAutoNext"
        Me.TxtAutoNext.Size = New System.Drawing.Size(179, 25)
        Me.TxtAutoNext.TabIndex = 96
        '
        'LblAutoNext
        '
        Me.LblAutoNext.AutoSize = True
        Me.LblAutoNext.Location = New System.Drawing.Point(8, 100)
        Me.LblAutoNext.Name = "LblAutoNext"
        Me.LblAutoNext.Size = New System.Drawing.Size(181, 15)
        Me.LblAutoNext.TabIndex = 95
        Me.LblAutoNext.Text = "自动切换时间(单位:秒)："
        '
        'ChkFileAssociation
        '
        Me.ChkFileAssociation.AutoSize = True
        Me.ChkFileAssociation.Location = New System.Drawing.Point(10, 71)
        Me.ChkFileAssociation.Name = "ChkFileAssociation"
        Me.ChkFileAssociation.Size = New System.Drawing.Size(220, 19)
        Me.ChkFileAssociation.TabIndex = 94
        Me.ChkFileAssociation.Text = "关联稿件库备份文件(*.paw)"
        Me.ChkFileAssociation.UseVisualStyleBackColor = True
        '
        'ChkKeepScale
        '
        Me.ChkKeepScale.AutoSize = True
        Me.ChkKeepScale.Location = New System.Drawing.Point(10, 46)
        Me.ChkKeepScale.Name = "ChkKeepScale"
        Me.ChkKeepScale.Size = New System.Drawing.Size(194, 19)
        Me.ChkKeepScale.TabIndex = 93
        Me.ChkKeepScale.Text = "图片查看器保持比例放大"
        Me.ChkKeepScale.UseVisualStyleBackColor = True
        '
        'TxtMaxSize
        '
        Me.TxtMaxSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtMaxSize.Location = New System.Drawing.Point(338, 10)
        Me.TxtMaxSize.Name = "TxtMaxSize"
        Me.TxtMaxSize.Size = New System.Drawing.Size(63, 25)
        Me.TxtMaxSize.TabIndex = 92
        Me.TxtMaxSize.Text = "240"
        '
        'LblThumbMax
        '
        Me.LblThumbMax.AutoSize = True
        Me.LblThumbMax.Location = New System.Drawing.Point(209, 15)
        Me.LblThumbMax.Name = "LblThumbMax"
        Me.LblThumbMax.Size = New System.Drawing.Size(127, 15)
        Me.LblThumbMax.TabIndex = 91
        Me.LblThumbMax.Text = "缩略图最大尺寸："
        '
        'TxtMinSize
        '
        Me.TxtMinSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtMinSize.Location = New System.Drawing.Point(136, 10)
        Me.TxtMinSize.Name = "TxtMinSize"
        Me.TxtMinSize.Size = New System.Drawing.Size(63, 25)
        Me.TxtMinSize.TabIndex = 90
        Me.TxtMinSize.Text = "120"
        '
        'LblThumbMin
        '
        Me.LblThumbMin.AutoSize = True
        Me.LblThumbMin.Location = New System.Drawing.Point(7, 15)
        Me.LblThumbMin.Name = "LblThumbMin"
        Me.LblThumbMin.Size = New System.Drawing.Size(127, 15)
        Me.LblThumbMin.TabIndex = 89
        Me.LblThumbMin.Text = "缩略图最小尺寸："
        '
        'PropertiesForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(432, 453)
        Me.Controls.Add(Me.TabGrp)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PropertiesForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "选项"
        Me.TabGrp.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ColorDlg As ColorDialog
    Friend WithEvents TabGrp As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents ChkShowStatus As CheckBox
    Friend WithEvents ChkShowTool As CheckBox
    Friend WithEvents ChkMenuUpper As CheckBox
    Friend WithEvents ChkShowThemeColor As CheckBox
    Friend WithEvents LblLanguage As Label
    Friend WithEvents CboLang As ComboBox
    Friend WithEvents LblCorColor As Label
    Friend WithEvents LblSelColor As Label
    Friend WithEvents BtnCornerColor As Button
    Friend WithEvents BtnSelectedColor As Button
    Friend WithEvents LblCornerColor As Label
    Friend WithEvents LblSelectedColor As Label
    Friend WithEvents LblThemeColor As Label
    Friend WithEvents BtnThemeColor As Button
    Friend WithEvents ChkShowPicThemeColor As CheckBox
    Friend WithEvents RadSystem As RadioButton
    Friend WithEvents RadDark As RadioButton
    Friend WithEvents RadLight As RadioButton
    Friend WithEvents LblTheme As Label
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents ChkShowHito As CheckBox
    Friend WithEvents ChkRestore As CheckBox
    Friend WithEvents CboCheckUpdate As ComboBox
    Friend WithEvents ChkAutoCheckUpdate As CheckBox
    Friend WithEvents ChkAutoPlay As CheckBox
    Friend WithEvents ChkAutoStart As CheckBox
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents ChkLoop As CheckBox
    Friend WithEvents ChkDevTools As CheckBox
    Friend WithEvents ChkPlayNext As CheckBox
    Friend WithEvents TxtAutoNext As TextBox
    Friend WithEvents LblAutoNext As Label
    Friend WithEvents ChkFileAssociation As CheckBox
    Friend WithEvents ChkKeepScale As CheckBox
    Friend WithEvents TxtMaxSize As TextBox
    Friend WithEvents LblThumbMax As Label
    Friend WithEvents TxtMinSize As TextBox
    Friend WithEvents LblThumbMin As Label
    Friend WithEvents TxtUserName As TextBox
    Friend WithEvents LblMyname As Label
End Class
