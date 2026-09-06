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
Imports System.IO
Imports PawTheme = PawLab.WindowsTheme.ThemeService
Imports System.Text.RegularExpressions

Public Class InputDialogForm
    Implements IThemeChangeable, ILocalizable
    Public Property InputValue As String = ""
    Public Property IsCancelled As Boolean = True

    ''' <summary>
    ''' 按下取消按钮时
    ''' </summary>
    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        IsCancelled = True
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    ''' <summary>
    ''' 用户输入时
    ''' </summary>
    Private Sub InputTxtbox_TextChanged(sender As Object, e As EventArgs) Handles InputTxtbox.TextChanged
        ErrorLabel.Text = ShowHint(InputTxtbox.Text)
        BtnOK.Enabled = (ShowHint(InputTxtbox.Text) = "")
    End Sub

    ''' <summary>
    ''' 报告输入文件夹名称的问题
    ''' </summary>
    ''' <param name="inputStr">待检测的文件夹名称</param>
    Private Function ShowHint(inputStr As String) As String
        If inputStr = "" Then Return "文件夹名不能为空"
        If inputStr.StartsWith(" ", StringComparison.Ordinal) Then Return "文件夹名不能以空格开头"
        If inputStr.EndsWith(" ", StringComparison.Ordinal) Then Return "文件夹名不能以空格结尾"
        If inputStr.EndsWith(".", StringComparison.Ordinal) Then Return "文件夹名不能以'.'结尾"
        If inputStr.Length >= 100 Then Return "文件夹名长度过长"
        Dim invalidNames As String() = {"CON", "PRN", "AUX", "CLOCK$", "NUL",
            "COM0", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
            "LPT0", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"}
        For Each invalidName As String In invalidNames '使用正则表达式匹配整个文件夹名称
            Dim pattern As String = String.Format("(^|[\\/]){0}(?:[\\/]|$)", Regex.Escape(invalidName))
            If Regex.IsMatch(inputStr, pattern, RegexOptions.IgnoreCase) Then Return "文件夹名不能为这个"
        Next
        Dim invalidChars As Char() = Path.GetInvalidFileNameChars()
        If Regex.IsMatch(inputStr, "[<>:""/\\|?*]") Then Return $"文件夹名不能包含这些字符: 
            {String.Join("", invalidChars.Where(Function(c) inputStr.Contains(c)).Distinct())}"
        If Regex.IsMatch(inputStr, "^.+~\d+$") Then Return "文件夹名包含特殊格式"
        Dim newPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Artworks", inputStr)
        If Directory.Exists(newPath) Then Return "不能与现有文件夹重名"
        Return ""
    End Function

    ''' <summary>
    ''' 确定按钮
    ''' </summary>
    Private Sub BtnOK_Click(sender As Object, e As EventArgs) Handles BtnOK.Click
        Dim hint As String = ShowHint(InputTxtbox.Text)
        If hint = "" Then
            InputValue = InputTxtbox.Text
            IsCancelled = False
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Else
            MessageBox.Show(hint, "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    ''' <summary>
    ''' 窗体加载时
    ''' </summary>
    Private Sub InputDialogForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SystemThemeChange()
        Dim MnuHandle = GetSystemMenu(Handle, False) '获取菜单句柄
        RemoveMenu(MnuHandle, SC_RESTORE, MF_BYCOMMAND) '去除还原菜单
        RemoveMenu(MnuHandle, SC_MAXIMIZE, MF_BYCOMMAND) '去除最大化菜单
        RemoveMenu(MnuHandle, SC_SIZE, MF_BYCOMMAND) '去除大小菜单
        RemoveMenu(MnuHandle, SC_MINIMIZE, MF_BYCOMMAND) '去除最小化菜单
        InputTxtbox.Focus()
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
            Icon = CreateRoundedRectangleIcon(True, My.Resources.Icons.MenuEditLineDark)
        Else
            bgColor = BgColorLight
            frColor = FrColorLight
            Icon = CreateRoundedRectangleIcon(False, My.Resources.Icons.MenuEditLineLight)
        End If
        For Each control In controlList
            control.ForeColor = frColor
            control.BackColor = bgColor
        Next
        ForeColor = frColor
        BackColor = bgColor
        'WinAPI
        PawTheme.SetWindowTheme(Handle, IsDarkMode) 'PawLab.WindowsTheme
    End Sub
    Private Sub LanguageChange() Implements ILocalizable.LanguageChange

    End Sub
End Class