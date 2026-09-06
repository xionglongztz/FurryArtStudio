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
Imports System.Text
Imports System.Text.RegularExpressions
Public Class ColorDialogForm
    Implements IThemeChangeable
    Dim colorLabels() As Label
    Private Sub ColorDialogForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        colorLabels = {L1, L2, L3, L4, L5, L6, L7, L8, L9, L10}
        For Each label In colorLabels '为每个标签添加一个点击复制的事件
            AddHandler label.Click, AddressOf Label_Click
        Next
        Dim MnuHandle = GetSystemMenu(Handle, False) '获取菜单句柄
        RemoveMenu(MnuHandle, SC_RESTORE, MF_BYCOMMAND) '去除还原菜单
        RemoveMenu(MnuHandle, SC_MAXIMIZE, MF_BYCOMMAND) '去除最大化菜单
        RemoveMenu(MnuHandle, SC_SIZE, MF_BYCOMMAND) '去除大小菜单
        RemoveMenu(MnuHandle, SC_MINIMIZE, MF_BYCOMMAND) '去除最小化菜单
        SystemThemeChange()
    End Sub
    Private Sub Label_Click(sender As Object, e As EventArgs)
        Dim lbl As Label = DirectCast(sender, Label)
        If Not String.IsNullOrEmpty(lbl.Text) Then
            Clipboard.SetText(Regex.Match(lbl.Text, "#([A-Fa-f0-9]{6})").Value) '提取出颜色十六进制
        End If
    End Sub
    Private Sub ColorDialogForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.Control And e.KeyCode = Keys.C Then '当按下 Ctrl+C 时, 复制到剪贴板
            Dim sb As New StringBuilder
            For Each label In colorLabels
                sb.Append(label.Text & vbCrLf)
            Next
            Clipboard.SetText(sb.ToString())
        End If
        If e.KeyCode = Keys.Escape Then
            Close()
        End If
    End Sub
    Private Sub SystemThemeChange() Implements IThemeChangeable.SystemThemeChange
        If IsDarkMode() Then BackColor = BgColorDark Else BackColor = BgColorLight
        PawTheme.SetWindowTheme(Handle, IsDarkMode) 'PawLab.WindowsTheme
    End Sub
End Class