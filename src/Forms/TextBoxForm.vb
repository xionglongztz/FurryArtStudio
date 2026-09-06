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

Public Class TextBoxForm
    Implements IThemeChangeable
    Public Sub New(text As String, title As String)
        InitializeComponent()
        TxtBox.Text = text
        Me.Text = title
    End Sub
    Private Sub TextBoxForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TxtBox.ReadOnly = True
        TxtBox.Dock = DockStyle.Fill
        SystemThemeChange()
    End Sub
    Private Sub SystemThemeChange() Implements IThemeChangeable.SystemThemeChange
        '判断颜色
        If IsDarkMode() Then
            TxtBox.BackColor = BgColorDark
            TxtBox.ForeColor = FrColorDark
            Icon = CreateRoundedRectangleIcon(True, My.Resources.Icons.FormFileDark)
        Else
            TxtBox.BackColor = BgColorLight
            TxtBox.ForeColor = FrColorLight
            Icon = CreateRoundedRectangleIcon(False, My.Resources.Icons.FormFileLight)
        End If
        PawTheme.SetWindowTheme(Handle, IsDarkMode) 'PawLab.WindowsTheme
    End Sub
End Class