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
Imports PawTheme = PawLab.WindowsTheme.ThemeService

Public Class AboutForm
    Implements IThemeChangeable, ILocalizable
    Private Sub AboutForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TxtBox.ReadOnly = True
        Dim MnuHandle = GetSystemMenu(Handle, False) '获取菜单句柄
        RemoveMenu(MnuHandle, SC_RESTORE, MF_BYCOMMAND) '去除还原菜单
        RemoveMenu(MnuHandle, SC_MAXIMIZE, MF_BYCOMMAND) '去除最大化菜单
        RemoveMenu(MnuHandle, SC_SIZE, MF_BYCOMMAND) '去除大小菜单
        RemoveMenu(MnuHandle, SC_MINIMIZE, MF_BYCOMMAND) '去除最小化菜单
        SystemThemeChange()
        TxtBox.Text = My.Resources.Licenses.AboutText
        PicBoxLogo.SizeMode = PictureBoxSizeMode.Zoom
        LblVersion.Text = GetCurrentVersion()
        LanguageChange()
    End Sub
    Private Sub SystemThemeChange() Implements IThemeChangeable.SystemThemeChange
        '获取控件集合
        Dim controlList As List(Of Control) = GetAllControls(Me)
        '判断颜色
        If IsDarkMode() Then
            LlblGitHub.LinkColor = IconColorDark
            LlblGitHub.VisitedLinkColor = IconColorLight
            LlblLicense.LinkColor = IconColorDark
            LlblLicense.VisitedLinkColor = IconColorLight
            LlblPrivacy.LinkColor = IconColorDark
            LlblPrivacy.VisitedLinkColor = IconColorLight
            LlblWebSite.LinkColor = IconColorDark
            LlblWebSite.VisitedLinkColor = IconColorLight
            LlblUserAgreement.LinkColor = IconColorDark
            LlblUserAgreement.VisitedLinkColor = IconColorLight
            Icon = CreateRoundedRectangleIcon(True, My.Resources.Icons.MenuInfoDark)
            TxtBox.BackColor = Color.FromArgb(50, 50, 50) '增加一个好看的底色
            BtnOK.ForeColor = FrColorDark
            BtnOK.BackColor = BgColorDark
            ForeColor = FrColorDark
            BackColor = BgColorDark
            TxtBox.ForeColor = FrColorDark
        Else
            LlblGitHub.LinkColor = Color.Blue
            LlblGitHub.VisitedLinkColor = Color.Purple
            LlblLicense.LinkColor = Color.Blue
            LlblLicense.VisitedLinkColor = Color.Purple
            LlblPrivacy.LinkColor = Color.Blue
            LlblPrivacy.VisitedLinkColor = Color.Purple
            LlblWebSite.LinkColor = Color.Blue
            LlblWebSite.VisitedLinkColor = Color.Purple
            LlblUserAgreement.LinkColor = Color.Blue
            LlblUserAgreement.VisitedLinkColor = Color.Purple
            Icon = CreateRoundedRectangleIcon(False, My.Resources.Icons.MenuInfoLight)
            TxtBox.BackColor = Color.FromArgb(180, 180, 180)
            BtnOK.ForeColor = FrColorLight
            BtnOK.BackColor = BgColorLight
            ForeColor = FrColorLight
            BackColor = BgColorLight
            TxtBox.ForeColor = FrColorLight
        End If
        PawTheme.SetWindowTheme(Handle, IsDarkMode) 'PawLab.WindowsTheme
    End Sub
    Private Sub LanguageChange() Implements ILocalizable.LanguageChange
        Text = My.Resources.About_Title
        LlblWebSite.Text = My.Resources.About_LinkWebSite
        LlblGitHub.Text = My.Resources.About_LinkGitHub
        LlblLicense.Text = My.Resources.About_LinkLicense
        LlblPrivacy.Text = My.Resources.About_LinkPrivacy
        LlblUserAgreement.Text = My.Resources.About_LinkUserAgreement
        BtnOK.Text = My.Resources.About_BtnOK
    End Sub
    'GitHub
    Private Sub LlblGitHub_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LlblGitHub.LinkClicked
        Process.Start("https://github.com/PawLaboratory/FurryArtStudio")
    End Sub
    '许可证
    Private Sub LlblLicense_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LlblLicense.LinkClicked
        Dim txt As New TextBoxForm(My.Resources.Licenses.LicenseText, "Apache License 2.0")
        txt.Show()
    End Sub
    '隐私政策
    Private Sub LlblPrivacy_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LlblPrivacy.LinkClicked
        Dim txt As New TextBoxForm(My.Resources.Licenses.PrivacyText, My.Resources.About_LinkPrivacy)
        txt.Show()
    End Sub
    '用户协议
    Private Sub LlblUserAgreement_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LlblUserAgreement.LinkClicked
        Dim txt As New TextBoxForm(My.Resources.Licenses.TermsText, My.Resources.About_LinkUserAgreement)
        txt.Show()
    End Sub
    '官网
    Private Sub LlblWebSite_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LlblWebSite.LinkClicked

    End Sub
    Private Sub TxtBox_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles TxtBox.LinkClicked
        Process.Start(e.LinkText)
    End Sub
End Class