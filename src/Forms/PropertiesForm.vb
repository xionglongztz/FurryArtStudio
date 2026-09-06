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
Imports System.Globalization
Imports PawTheme = PawLab.WindowsTheme.ThemeService
Imports System.Threading
Imports Microsoft.Win32

Public Class PropertiesForm
    Implements IThemeChangeable, ILocalizable
    Private Setting As AppSettings
    Private _isUpdatingCheckbox As Boolean = False '用来区分用户设置与程序设置
#Region "窗体"
    Private Sub PropertiesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim MnuHandle = GetSystemMenu(Handle, False) '获取菜单句柄
        RemoveMenu(MnuHandle, SC_RESTORE, MF_BYCOMMAND) '去除还原菜单
        RemoveMenu(MnuHandle, SC_MAXIMIZE, MF_BYCOMMAND) '去除最大化菜单
        RemoveMenu(MnuHandle, SC_SIZE, MF_BYCOMMAND) '去除大小菜单
        RemoveMenu(MnuHandle, SC_MINIMIZE, MF_BYCOMMAND) '去除最小化菜单
        SystemThemeChange()
        InitSettings()
    End Sub
    Private Sub PropertiesForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Setting.Appearance.UserName = TxtUserName.Text
        Setting.Save()
    End Sub
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
            Icon = CreateRoundedRectangleIcon(True, My.Resources.Icons.MenuSettingsDark)
        Else
            bgColor = BgColorLight
            frColor = FrColorLight
            Icon = CreateRoundedRectangleIcon(False, My.Resources.Icons.MenuSettingsLight)
        End If
        Dim excludeControls As String() = {"LblThemeColor", "LblSelColor", "LblCorColor"}
        For Each control In controlList
            If excludeControls.Contains(control.Name) Then
                Continue For
            End If
            control.ForeColor = frColor
            control.BackColor = bgColor
        Next
        ForeColor = frColor
        BackColor = bgColor
        PawTheme.SetWindowTheme(Handle, IsDarkMode) 'PawLab.WindowsTheme
    End Sub
    Private Sub InitSettings()
        Setting = AppSettings.Load()
        Dim appearances = Setting.Appearance
        Select Case appearances.Theme
            Case AppSettings.ThemeMode.FollowSystem
                RadSystem.Checked = True
            Case AppSettings.ThemeMode.Light
                RadLight.Checked = True
            Case AppSettings.ThemeMode.Dark
                RadDark.Checked = True
        End Select
        ChkShowThemeColor.Checked = appearances.ShowThemeColor
        If ChkShowThemeColor.Checked = False Then
            BtnThemeColor.Enabled = False
        Else
            BtnThemeColor.Enabled = True
        End If
        ChkShowPicThemeColor.Checked = appearances.ViewWindowThemeColor
        LblThemeColor.BackColor = Color.FromArgb(appearances.ThemeColorArgb)
        LblSelColor.BackColor = Color.FromArgb(appearances.SelectionAccentColorArgb)
        LblCorColor.BackColor = Color.FromArgb(appearances.BadgeColorArgb)
        CboLang.Items.Clear() '删除所有选项
        CboLang.Items.Add("English")
        CboLang.Items.Add("简体中文")
        CboLang.Items.Add("繁體中文")
        CboLang.SelectedIndex() = appearances.Language
        ChkMenuUpper.Checked = appearances.MenuUppercase
        ChkShowTool.Checked = appearances.ShowToolBar
        ChkShowStatus.Checked = appearances.ShowStatusBar
        TxtUserName.Text = appearances.UserName
        Dim startups = Setting.Startup
        CheckAutoStartStatus()
        ChkRestore.Checked = startups.RestoreLastLibrary
        ChkAutoPlay.Checked = startups.AutoPlaySlideshow
        ChkAutoCheckUpdate.Checked = startups.AutoCheckUpdate
        ChkShowHito.Checked = startups.ShowHitokoto
    End Sub
    Private Sub LanguageChange() Implements ILocalizable.LanguageChange

    End Sub
    Private Sub TabGrp_TabIndexChanged(sender As Object, e As EventArgs) Handles TabGrp.TabIndexChanged
        Select Case TabGrp.SelectedIndex
            Case 0
                Text = "选项 - 外观"
            Case 1
                Text = "选项 - 启动"
            Case 2
                Text = "选项 - 高级"
        End Select
    End Sub
#End Region

#Region "外观"
    Private Sub RadLight_CheckedChanged(sender As Object, e As EventArgs) Handles RadLight.CheckedChanged
        Setting.Appearance.Theme = AppSettings.ThemeMode.Light
        Setting.Save()
        UpdateFormTheme()
        MainForm.MnuThemeSystem.Checked = False
        MainForm.MnuThemeDark.Checked = False
        MainForm.MnuThemeLight.Checked = True
    End Sub
    Private Sub RadDark_CheckedChanged(sender As Object, e As EventArgs) Handles RadDark.CheckedChanged
        Setting.Appearance.Theme = AppSettings.ThemeMode.Dark
        Setting.Save()
        UpdateFormTheme()
        MainForm.MnuThemeSystem.Checked = False
        MainForm.MnuThemeDark.Checked = True
        MainForm.MnuThemeLight.Checked = False
    End Sub
    Private Sub RadSystem_CheckedChanged(sender As Object, e As EventArgs) Handles RadSystem.CheckedChanged
        Setting.Appearance.Theme = AppSettings.ThemeMode.FollowSystem
        Setting.Save()
        UpdateFormTheme()
        MainForm.MnuThemeSystem.Checked = True
        MainForm.MnuThemeDark.Checked = False
        MainForm.MnuThemeLight.Checked = False
    End Sub
    Private Sub ChkShowThemeColor_CheckedChanged(sender As Object, e As EventArgs) Handles ChkShowThemeColor.CheckedChanged
        If ChkShowThemeColor.Checked Then
            BtnThemeColor.Enabled = True
            Setting.Appearance.ShowThemeColor = True
        Else
            BtnThemeColor.Enabled = False
            Setting.Appearance.ShowThemeColor = False
        End If
    End Sub
    Private Sub ChkShowPicThemeColor_CheckedChanged(sender As Object, e As EventArgs) Handles ChkShowPicThemeColor.CheckedChanged
        Setting.Appearance.ViewWindowThemeColor = ChkShowPicThemeColor.Checked
        Setting.Save()
    End Sub
    Private Sub BtnThemeColor_Click(sender As Object, e As EventArgs) Handles BtnThemeColor.Click
        Using dlg As New ColorDialog With {
            .Color = Color.FromArgb(Setting.Appearance.ThemeColorArgb)
        }
            If dlg.ShowDialog = DialogResult.OK Then
                Setting.Appearance.ThemeColorArgb = dlg.Color.ToArgb
                LblThemeColor.BackColor = dlg.Color
            End If
        End Using
    End Sub
    Private Sub CboLang_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CboLang.SelectedIndexChanged
        Select Case CboLang.SelectedIndex
            Case 0
                Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
                Setting.Appearance.Language = AppSettings.LanguageOption.English
            Case 1
                Thread.CurrentThread.CurrentUICulture = New CultureInfo("zh-Hans")
                Setting.Appearance.Language = AppSettings.LanguageOption.ChineseSimplified
            Case 2
                Thread.CurrentThread.CurrentUICulture = New CultureInfo("zh-Hant")
                Setting.Appearance.Language = AppSettings.LanguageOption.ChineseTraditional
        End Select
        UpdateFormLang() '更新语言
        Setting.Save()
    End Sub
    Private Sub BtnSelectedColor_Click(sender As Object, e As EventArgs) Handles BtnSelectedColor.Click
        Using dlg As New ColorDialog With {
            .Color = Color.FromArgb(Setting.Appearance.SelectionAccentColorArgb)
        }
            If dlg.ShowDialog = DialogResult.OK Then
                Setting.Appearance.SelectionAccentColorArgb = dlg.Color.ToArgb
                LblSelColor.BackColor = dlg.Color
            End If
            MainForm.ImageGalleryMain.SelectionAccentColor = dlg.Color
        End Using
        Setting.Save()
    End Sub
    Private Sub BtnCornerColor_Click(sender As Object, e As EventArgs) Handles BtnCornerColor.Click
        Using dlg As New ColorDialog With {
            .Color = Color.FromArgb(Setting.Appearance.BadgeColorArgb)
        }
            If dlg.ShowDialog = DialogResult.OK Then
                Setting.Appearance.BadgeColorArgb = dlg.Color.ToArgb
                LblCorColor.BackColor = dlg.Color
            End If
            MainForm.ImageGalleryMain.BadgeColor = dlg.Color
        End Using
        Setting.Save()
    End Sub
    Private Sub ChkMenuUpper_CheckedChanged(sender As Object, e As EventArgs) Handles ChkMenuUpper.CheckedChanged
        Setting.Appearance.MenuUppercase = ChkMenuUpper.Checked
        Setting.Save()
        UpdateFormLang()
    End Sub
    Private Sub ChkShowTool_CheckedChanged(sender As Object, e As EventArgs) Handles ChkShowTool.CheckedChanged
        If ChkShowTool.Checked Then
            Setting.Appearance.ShowToolBar = True
            MainForm.TlStrip.Visible = True
        Else
            Setting.Appearance.ShowToolBar = False
            MainForm.TlStrip.Visible = False
        End If
        MainForm.MnuShowToolBar.Checked = ChkShowTool.Checked
    End Sub
    Private Sub ChkShowStatus_CheckedChanged(sender As Object, e As EventArgs) Handles ChkShowStatus.CheckedChanged
        If ChkShowStatus.Checked Then
            Setting.Appearance.ShowToolBar = True
            MainForm.StaStrip.Visible = True
        Else
            Setting.Appearance.ShowToolBar = False
            MainForm.StaStrip.Visible = False
        End If
        MainForm.MnuShowStatusBar.Checked = ChkShowStatus.Checked
    End Sub
#End Region

#Region "启动"
    Private Sub ChkAutoStart_CheckedChanged(sender As Object, e As EventArgs) Handles ChkAutoStart.CheckedChanged
        If _isUpdatingCheckbox Then Return
        Dim regResult As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
        Try
            SetAutoStart()
        Catch ex As Exception
            ShowErrorDialog(ex, "无法修改注册表")
        Finally
            CheckAutoStartStatus()
        End Try
    End Sub
    Private Sub CheckAutoStartStatus()
        _isUpdatingCheckbox = True
        Try
            If IsAutoStart() Then
                ChkAutoStart.Checked = True
            Else
                ChkAutoStart.Checked = False
            End If
        Finally
            _isUpdatingCheckbox = False
        End Try
    End Sub
    Private Sub ChkRestore_CheckedChanged(sender As Object, e As EventArgs) Handles ChkRestore.CheckedChanged

    End Sub
    Private Sub ChkAutoPlay_CheckedChanged(sender As Object, e As EventArgs) Handles ChkAutoPlay.CheckedChanged

    End Sub
    Private Sub ChkAutoCheckUpdate_CheckedChanged(sender As Object, e As EventArgs) Handles ChkAutoCheckUpdate.CheckedChanged

    End Sub
    Private Sub CboCheckUpdate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CboCheckUpdate.SelectedIndexChanged

    End Sub
    Private Sub ChkShowHito_CheckedChanged(sender As Object, e As EventArgs) Handles ChkShowHito.CheckedChanged
        Setting.Startup.ShowHitokoto = ChkShowHito.Checked
    End Sub






#End Region

End Class