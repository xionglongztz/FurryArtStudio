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
Imports System.Drawing.Imaging
Imports System.Drawing.Printing
Imports PawTheme = PawLab.WindowsTheme.ThemeService
Public Class PrintForm
    Implements IThemeChangeable, ILocalizable
    '打印设置变量
    Private _printSettings As PrinterSettings
    Private _pageSettings As PageSettings
    Private _selectedImages As List(Of String)
    Private _printCount As Integer = 1
    Private _currentPrintIndex As Integer = 0
    Private _copiesPrinted As Integer = 0
    Private _artwork As Artwork
    '用于外部调用的属性
    Public Property PrintDocumentInstance As PrintDocument
    Public Property UserCancelled As Boolean = False
    '构造函数
    Public Sub New(images As List(Of String), artwork As Artwork)
        InitializeComponent()
        _selectedImages = images
        _artwork = artwork
        _printSettings = New PrinterSettings()
        _pageSettings = New PageSettings(_printSettings)
        InitializeForm()
    End Sub
    '初始化窗体
    Private Sub InitializeForm()
        '加载打印机和纸张
        LoadPrinters()
        '设置默认值
        txtPrintCount.Text = "1"
        txtTop.Text = "25"
        txtBottom.Text = "25"
        txtLeft.Text = "25"
        txtRight.Text = "25"
        RbHorizonal.Checked = True
        '绑定事件
        AddHandler BtnPrint.Click, AddressOf BtnPrint_Click
    End Sub

#Region "打印机与纸张设置"
    Private Sub LoadPrinters() '设置打印机
        cbPrinter.Items.Clear()
        For Each printerName As String In PrinterSettings.InstalledPrinters
            cbPrinter.Items.Add(printerName)
        Next
        Dim settings As New PrinterSettings()
        cbPrinter.SelectedItem = settings.PrinterName '设置默认打印机
    End Sub
    Private Sub LoadPaperSizes(printerName As String) '设置纸张
        cbPaper.Items.Clear()
        Dim settings As New PrinterSettings() With {
            .PrinterName = printerName
        }
        Dim pd As New PrintDocument With {
            .PrinterSettings = settings
        }
        For Each paperSize As PaperSize In pd.DefaultPageSettings.PrinterSettings.PaperSizes
            cbPaper.Items.Add(paperSize.PaperName)
        Next
        cbPaper.SelectedIndex = 0
    End Sub
    Private Sub CbPrinter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbPrinter.SelectedIndexChanged
        LoadPaperSizes(cbPrinter.Text)
    End Sub
#End Region

#Region "限制文本框输入"
    Private Function ValidateInputs() As Boolean
        '验证打印份数
        If Not Integer.TryParse(txtPrintCount.Text, _printCount) OrElse _printCount < 1 Then
            MessageBox.Show("打印份数必须大于0", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPrintCount.Focus()
            txtPrintCount.SelectAll()
            Return False
        End If
        '验证边距输入
        Dim margins As New List(Of TextBox) From {txtTop, txtBottom, txtLeft, txtRight}
        For Each txtBox In margins
            If Not Integer.TryParse(txtBox.Text, Nothing) Then
                MessageBox.Show("边距必须是数字", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtBox.Focus()
                txtBox.SelectAll()
                Return False
            End If
        Next
        '检查打印机选择
        If String.IsNullOrWhiteSpace(cbPrinter.Text) Then
            MessageBox.Show("请选择打印机", "打印机错误", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        Return True
    End Function
    Private Sub TxtTop_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTop.KeyPress
        If Char.IsDigit(e.KeyChar) Or e.KeyChar = Chr(8) Then '检测0-9，退格键
            e.Handled = False '处理
        Else
            e.Handled = True '程序认为已经处理过了，于是不会处理
        End If '进行检测处理，禁止输入0-9以及退格键以外的东西
    End Sub
    Private Sub TxtBottom_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtBottom.KeyPress
        If Char.IsDigit(e.KeyChar) Or e.KeyChar = Chr(8) Then '检测0-9，退格键
            e.Handled = False '处理
        Else
            e.Handled = True '程序认为已经处理过了，于是不会处理
        End If '进行检测处理，禁止输入0-9以及退格键以外的东西
    End Sub
    Private Sub TxtLeft_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtLeft.KeyPress
        If Char.IsDigit(e.KeyChar) Or e.KeyChar = Chr(8) Then '检测0-9，退格键
            e.Handled = False '处理
        Else
            e.Handled = True '程序认为已经处理过了，于是不会处理
        End If '进行检测处理，禁止输入0-9以及退格键以外的东西
    End Sub
    Private Sub TxtRight_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRight.KeyPress
        If Char.IsDigit(e.KeyChar) Or e.KeyChar = Chr(8) Then '检测0-9，退格键
            e.Handled = False '处理
        Else
            e.Handled = True '程序认为已经处理过了，于是不会处理
        End If '进行检测处理，禁止输入0-9以及退格键以外的东西
    End Sub
    Private Sub TxtPrintCount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPrintCount.KeyPress
        If Char.IsDigit(e.KeyChar) Or e.KeyChar = Chr(8) Then '检测0-9，退格键
            e.Handled = False '处理
        Else
            e.Handled = True '程序认为已经处理过了，于是不会处理
        End If '进行检测处理，禁止输入0-9以及退格键以外的东西
    End Sub
#End Region

#Region "窗体相关"
    Private Sub PrintForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadPrinters()
        LoadPaperSizes(cbPrinter.Text)
        Dim MnuHandle = GetSystemMenu(Handle, False) '获取菜单句柄
        RemoveMenu(MnuHandle, SC_RESTORE, MF_BYCOMMAND) '去除还原菜单
        RemoveMenu(MnuHandle, SC_MAXIMIZE, MF_BYCOMMAND) '去除最大化菜单
        RemoveMenu(MnuHandle, SC_SIZE, MF_BYCOMMAND) '去除大小菜单
        RemoveMenu(MnuHandle, SC_MINIMIZE, MF_BYCOMMAND) '去除最小化菜单
        LanguageChange()
        SystemThemeChange()
    End Sub
    Private Sub LanguageChange() Implements ILocalizable.LanguageChange
        Text = My.Resources.Print_Title
        LblPrinter.Text = My.Resources.Print_LblPrinter
        LblCopies.Text = My.Resources.Print_LblCount
        LblPaper.Text = My.Resources.Print_LblPaper
        LblTop.Text = My.Resources.Print_LblTop
        LblBottom.Text = My.Resources.Print_LblBottom
        LblLeft.Text = My.Resources.Print_LblLeft
        LblRight.Text = My.Resources.Print_LblRight
        LblLayout.Text = My.Resources.Print_LblType
        BtnPrint.Text = My.Resources.Print_BtnPrint
        BtnCancel.Text = My.Resources.Print_BtnCancel
        BtnPrinterSetup.Text = My.Resources.Print_PrinterManager
        RbVertical.Text = My.Resources.Print_Vertical
        RbHorizonal.Text = My.Resources.Print_Horizontal
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
            Icon = CreateRoundedRectangleIcon(True, My.Resources.Icons.MenuPrintDark)
        Else
            bgColor = BgColorLight
            frColor = FrColorLight
            Icon = CreateRoundedRectangleIcon(False, My.Resources.Icons.MenuPrintLight)
        End If
        For Each control In controlList
            control.ForeColor = frColor
            control.BackColor = bgColor
        Next
        ForeColor = frColor
        BackColor = bgColor
        PawTheme.SetWindowTheme(Handle, IsDarkMode) 'PawLab.WindowsTheme
    End Sub
    Private Sub BtnPrinterSetup_Click(sender As Object, e As EventArgs) Handles BtnPrinterSetup.Click
        Dim isShiftPressed As Boolean = My.Computer.Keyboard.ShiftKeyDown
        If isShiftPressed Then
            Process.Start("control", "printers")
        Else
            Process.Start("ms-settings:printers")
        End If
    End Sub
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs)
        If Not ValidateInputs() Then Return '判断输入是否正确
        Dim pd As New PrintDocument() '创建打印文档
        PrintDocumentInstance = pd
        '配置打印机设置
        pd.PrinterSettings.PrinterName = cbPrinter.Text
        pd.PrinterSettings.Copies = CShort(_printCount)
        '配置页面设置
        Dim pageSettings = pd.DefaultPageSettings
        pageSettings.PrinterSettings = pd.PrinterSettings
        '设置纸张大小
        If cbPaper.SelectedItem IsNot Nothing Then
            For Each paperSize As PaperSize In pageSettings.PrinterSettings.PaperSizes
                If paperSize.PaperName = cbPaper.Text Then
                    pageSettings.PaperSize = paperSize
                    Exit For
                End If
            Next
        End If
        '设置方向
        pageSettings.Landscape = RbHorizonal.Checked
        '设置边距(转换为百分之一英寸)
        pageSettings.Margins = New Margins(
            CInt(txtLeft.Text),
            CInt(txtRight.Text),
            CInt(txtTop.Text),
            CInt(txtBottom.Text)
        )
        '设置文档名称
        pd.DocumentName = $"{_artwork.Title} - {_artwork.Author}"
        '添加打印事件处理
        AddHandler pd.BeginPrint, AddressOf Pd_BeginPrint
        AddHandler pd.PrintPage, AddressOf Pd_PrintPage
        AddHandler pd.EndPrint, AddressOf Pd_EndPrint
        '设置对话框结果并关闭
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
    Private Sub BtnCancel_Click(sender As Object, e As EventArgs)
        UserCancelled = True
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
#End Region

#Region "打印事件处理"
    Private Sub Pd_BeginPrint(sender As Object, e As PrintEventArgs)
        _currentPrintIndex = 0
        _copiesPrinted = 0
    End Sub
    Private Sub Pd_PrintPage(sender As Object, e As PrintPageEventArgs)
        If _selectedImages Is Nothing OrElse _currentPrintIndex >= _selectedImages.Count Then
            e.HasMorePages = False
            Return
        End If
        Try
            Dim currentImagePath = _selectedImages(_currentPrintIndex)
            Using original As Image = Image.FromFile(currentImagePath)

                Dim img As New Bitmap(original.Width, original.Height, PixelFormat.Format64bppArgb)
                '通过创建副本的方式, 保证图片尺寸不会出现问题
                Using g = Graphics.FromImage(img)
                    g.DrawImage(original, 0, 0, original.Width, original.Height)
                End Using

                '获取可打印区域
                Dim marginBounds = e.MarginBounds
                '计算保持比例的矩形
                Dim destRect As Rectangle
                If img.Width / img.Height > marginBounds.Width / marginBounds.Height Then
                    '图片比页面区域宽
                    Dim height = CInt(img.Height * marginBounds.Width / img.Width)
                    destRect = New Rectangle(
                        marginBounds.X,
                        marginBounds.Y + (marginBounds.Height - height) \ 2,
                        marginBounds.Width,
                        height
                    )
                Else
                    '图片比页面区域高
                    Dim width = CInt(img.Width * marginBounds.Height / img.Height)
                    destRect = New Rectangle(
                        marginBounds.X + (marginBounds.Width - width) \ 2,
                        marginBounds.Y,
                        width,
                        marginBounds.Height
                    )
                End If
                '绘制图片
                e.Graphics.DrawImage(img, destRect)
            End Using
            _currentPrintIndex += 1 '移动到下一张图片
            e.HasMorePages = (_currentPrintIndex < _selectedImages.Count) '检查是否还有更多页面

        Catch ex As Exception
            '处理图片加载失败的情况
            _currentPrintIndex += 1 '跳过这张图片
            e.HasMorePages = (_currentPrintIndex < _selectedImages.Count)
        End Try
    End Sub
    Private Sub Pd_EndPrint(sender As Object, e As PrintEventArgs)
        '清理资源
        If e.PrintAction = PrintAction.PrintToPrinter Then
        End If
    End Sub
#End Region

End Class