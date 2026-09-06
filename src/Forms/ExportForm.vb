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
Imports System.IO.Compression
Imports PawTheme = PawLab.WindowsTheme.ThemeService
Imports Ookii.Dialogs.WinForms
Public Class ExportForm
    Implements IThemeChangeable, ILocalizable
    Private _artworkList As List(Of Artwork)
    Private _targetPath As String
    Private _workingPath As String
    Private Enum ExportMode '导出模式枚举
        Auto
        ForceFolder
        KeepFlat
    End Enum

#Region "初始化"
    ''' <summary>
    ''' 构造函数
    ''' </summary>
    ''' <param name="artworks">要被导出的稿件</param>
    Public Sub New(artworks As List(Of Artwork))
        InitializeComponent()
        _artworkList = artworks
        PreviewPicturebox.SizeMode = PictureBoxSizeMode.Zoom
        PreviewPicturebox.Image = _artworkList(0).Thumbnail
        _targetPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) '初始化目录
        TxtPath.Text = _targetPath
        If _artworkList.Count = 1 Then
            Text = "导出稿件"
        Else
            Text = String.Format("批量导出稿件 - 总计{0}个", _artworkList.Count)
        End If
        CboCompressLevel.Items.Clear()
        CboCompressLevel.Items.Add("最佳压缩")
        CboCompressLevel.Items.Add("最快压缩")
        CboCompressLevel.Items.Add("仅存储")
        CboCompressLevel.SelectedIndex() = 0
        CboCompressLevel.Enabled = False
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
            Icon = CreateRoundedRectangleIcon(True, My.Resources.Icons.MenuFileOutputDark)
        Else
            bgColor = BgColorLight
            frColor = FrColorLight
            Icon = CreateRoundedRectangleIcon(False, My.Resources.Icons.MenuFileOutputLight)
        End If
        For Each control In controlList
            control.ForeColor = frColor
            control.BackColor = bgColor
        Next
        ForeColor = frColor
        BackColor = bgColor
        PawTheme.SetWindowTheme(Handle, IsDarkMode) 'PawLab.WindowsTheme
    End Sub
    Private Sub LanguageChange() Implements ILocalizable.LanguageChange

    End Sub
    Private Sub ExportForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SystemThemeChange()
        Dim MnuHandle = GetSystemMenu(Handle, False) '获取菜单句柄
        RemoveMenu(MnuHandle, SC_RESTORE, MF_BYCOMMAND) '去除还原菜单
        RemoveMenu(MnuHandle, SC_MAXIMIZE, MF_BYCOMMAND) '去除最大化菜单
        RemoveMenu(MnuHandle, SC_SIZE, MF_BYCOMMAND) '去除大小菜单
        RemoveMenu(MnuHandle, SC_MINIMIZE, MF_BYCOMMAND) '去除最小化菜单
        LanguageChange()
    End Sub
#End Region

#Region "UI"
    Private Sub BtnSelect_Click(sender As Object, e As EventArgs) Handles BtnSelect.Click
        SelectFolder()
    End Sub
    Private Sub TxtPath_Click(sender As Object, e As EventArgs) Handles TxtPath.Click
        SelectFolder()
    End Sub
    Private Sub SelectFolder()
        Dim folderSelect As New FolderBrowserDialog With {
        .Description = "请选择希望导出稿件的目标文件夹",
        .RootFolder = Environment.SpecialFolder.Desktop,
        .ShowNewFolderButton = True
        }
        If folderSelect.ShowDialog = DialogResult.OK Then
            _targetPath = folderSelect.SelectedPath
            TxtPath.Text = _targetPath
        End If
    End Sub
#End Region

#Region "导出逻辑"
    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExport.Click
        '读取导出分类模式
        Dim mode As ExportMode
        If RadAuto.Checked Then
            mode = ExportMode.Auto
        ElseIf RadCreate.Checked Then
            mode = ExportMode.ForceFolder
        ElseIf RadKeep.Checked Then
            mode = ExportMode.KeepFlat
        End If
        '按照分类进行导出
        Dim tempPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $".EXP_{Guid.NewGuid():N}")
        Try
            Dim folderName As String = New DirectoryInfo(_targetPath).Name '目标文件夹名称
            _workingPath = Path.Combine(tempPath, folderName)
            For Each artwork In _artworkList
                Select Case mode
                    Case ExportMode.Auto
                        ExportAuto(artwork, _workingPath)
                    Case ExportMode.ForceFolder
                        ExportToFolder(artwork, _workingPath)
                    Case ExportMode.KeepFlat
                        ExportFlat(artwork, _workingPath)
                End Select
            Next
            If ChkExportZip.Checked Then
                Dim parentDir As String = AppDomain.CurrentDomain.BaseDirectory '工作区目录
                Dim zipFilePath As String = Path.Combine(_targetPath, folderName & ".zip") '以目标文件夹命名的zip路径
                If File.Exists(zipFilePath) Then '当出现重名文件时删除
                    File.Delete(zipFilePath)
                End If
                ZipFile.CreateFromDirectory(_workingPath, zipFilePath, CboCompressLevel.SelectedIndex, True)
            Else
                My.Computer.FileSystem.CopyDirectory(_workingPath, _targetPath, True)
            End If
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
        Finally
            Directory.Delete(tempPath, True) '清理工作区
        End Try
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
    ''' <summary>
    ''' 智能复制模式
    ''' </summary>
    Private Sub ExportAuto(artwork As Artwork, rootPath As String)
        If artwork.FilePaths.Count = 1 Then
            CopyFileWithConflictHandling(artwork.FilePaths(0), rootPath)
        Else
            ExportToFolder(artwork, rootPath)
        End If
    End Sub
    ''' <summary>
    ''' 复制到文件夹
    ''' </summary>
    Private Sub ExportToFolder(artwork As Artwork, rootPath As String)
        Dim folderName = SanitizeFileName($"{artwork.Title}_{artwork.Author}")
        Dim targetDir = Path.Combine(rootPath, folderName)
        If Not Directory.Exists(targetDir) Then Directory.CreateDirectory(targetDir)
        For Each filePath In artwork.FilePaths
            CopyFileWithConflictHandling(filePath, targetDir)
        Next
    End Sub
    ''' <summary>
    ''' 直接复制文件
    ''' </summary>
    Private Sub ExportFlat(artwork As Artwork, rootPath As String)
        For Each filePath In artwork.FilePaths
            CopyFileWithConflictHandling(filePath, rootPath)
        Next
    End Sub
    Private Function SanitizeFileName(name As String) As String
        '移除 Windows 不允许的字符
        Dim invalidChars = Path.GetInvalidFileNameChars()
        Return New String(name.Where(Function(c) Not invalidChars.Contains(c)).ToArray())
    End Function
    Private Sub CopyFileWithConflictHandling(sourcePath As String, destDir As String)
        If Not File.Exists(sourcePath) Then Throw New FileNotFoundException
        Dim fileName = Path.GetFileName(sourcePath)
        Dim destPath = Path.Combine(destDir, fileName)
        '若目标已存在则添加数字后缀
        Dim counter = 1
        While File.Exists(destPath)
            Dim nameWithoutExt = Path.GetFileNameWithoutExtension(fileName)
            Dim ext = Path.GetExtension(fileName)
            destPath = Path.Combine(destDir, $"{nameWithoutExt} ({counter}){ext}")
            counter += 1
        End While
        File.Copy(sourcePath, destPath, True)
    End Sub
    Private Sub ChkExportZip_CheckedChanged(sender As Object, e As EventArgs) Handles ChkExportZip.CheckedChanged
        If ChkExportZip.Checked Then
            CboCompressLevel.Enabled = True
        Else
            CboCompressLevel.Enabled = False
        End If
    End Sub
#End Region

End Class