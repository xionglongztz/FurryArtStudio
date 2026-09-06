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
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.Security.Principal
Imports System.Text
Imports System.Threading
Imports Microsoft.Win32
Imports Ookii.Dialogs.WinForms
Imports PawLab.Logger

''' <summary>
''' 基本函数
''' </summary>
Module BasicFcn

#Region "常量字段"
    '分割线
    Public ReadOnly SeparatorEqual As New String("="c, 30)
    Public ReadOnly SeparatorStar As New String("*"c, 30)
    Public ReadOnly SeparatorDash As New String("-"c, 30)
    '主题
    Public ReadOnly FrColorLight As Color = Color.Black
    Public ReadOnly BgColorLight As Color = Color.White
    Public ReadOnly FrColorDark As Color = Color.FromArgb(220, 220, 220)
    Public ReadOnly BgColorDark As Color = Color.FromArgb(32, 32, 32)
    Public ReadOnly IconColorLight As Color = Color.FromArgb(58, 162, 143)
    Public ReadOnly IconColorDark As Color = Color.FromArgb(87, 226, 180)
    Public ReadOnly IconRed As Color = Color.FromArgb(232, 65, 65)
    '全局互斥体
    Public GlobalMutex As Mutex = Nothing

#End Region

#Region "日志记录器"
    ''' <summary>
    ''' 初始化日志记录器实例
    ''' </summary>
    Public Sub LoggerInit()
        Dim appPath As String = AppContext.BaseDirectory '程序路径
        Dim logPath As String = Path.Combine(appPath, "Logs") '日志路径
        Directory.CreateDirectory(logPath)
        Dim logFilePath As String = Path.Combine(logPath, "Latest.log") '日志文件路径
        If File.Exists(logFilePath) Then
            Dim lastLogFileDate As Date = File.GetLastWriteTime(logFilePath)
            File.Move(logFilePath, Path.Combine(logPath, $"{lastLogFileDate:yyyy-MM-dd_HH-mm-ss}.log"))
        End If '当先前的日志文件存在时, 更名
        Dim logConfig = New LoggerConfig() With {
            .LogPath = logPath,
            .MinLogLevel = LogLevel.DEBUG,
            .AutoFlush = False,
            .DateFormat = "HH:mm:ss.fff",
            .LogFormat = "{timestamp} {level} {message}",
            .Encoding = Text.Encoding.UTF8
        }
        Logger.Initialize(logConfig) '初始化日志记录器
    End Sub
#End Region

#Region "时间转换"
    ''' <summary>
    ''' 将DateTime对象转换成64位时间戳
    ''' </summary>
    Public Function DateTimeToUnixTimestamp(time As DateTime) As Long
        Dim epoch As New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        Dim utcDt As DateTime = time.ToUniversalTime()
        Dim timeSpan As TimeSpan = utcDt - epoch
        Return CLng(timeSpan.TotalSeconds)
    End Function

    ''' <summary>
    ''' 将64位时间戳转换成DateTime对象
    ''' </summary>
    Public Function UnixTimestampToDateTime(unixTimestamp As Long) As DateTime
        Dim epoch As New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        Return epoch.AddSeconds(unixTimestamp).ToLocalTime()
    End Function
#End Region

#Region "文件夹信息"
    ''' <summary>
    ''' 获得文件夹信息
    ''' </summary>
    ''' <param name="folderPath">文件夹路径</param>
    ''' <returns>文件数量与文件夹大小</returns>
    Public Function GetFolderInfo(ByVal folderPath As String) As (fileCount As Long, totalSize As Long, sizeString As String)
        If Not Directory.Exists(folderPath) Then
            Throw New DirectoryNotFoundException("文件夹不存在: " & folderPath)
        End If
        Dim fileCount As Long, totalSize As Long = 0
        Dim files As String() = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories) '获取所有文件及子文件夹
        fileCount = files.Length
        For Each file In files '计算总大小
            Try
                Dim fileInfo As New FileInfo(file)
                totalSize += fileInfo.Length
            Catch ex As Exception
                '忽略无法访问的文件
            End Try
        Next
        Dim sizeString As String = FormatFileSize(totalSize) '格式化大小
        Return (fileCount, totalSize, sizeString)
    End Function

    ''' <summary>
    ''' 将存储空间转换为人类可读的格式
    ''' </summary>
    ''' <param name="bytes">字节数</param>
    ''' <returns>人类可读的存储空间</returns>
    Public Function FormatFileSize(ByVal bytes As Long) As String
        Dim size As Double = bytes
        Dim units As String() = {"B", "KB", "MB", "GB", "TB"}
        Dim unitIndex As Integer = 0

        While size >= 1024 AndAlso unitIndex < units.Length - 1
            size /= 1024
            unitIndex += 1
        End While

        Return $"{size:N2}{units(unitIndex)}"
    End Function
#End Region

#Region "文本处理"
    ''' <summary>
    ''' 将数组转换成逗号分隔的形式
    ''' </summary>
    ''' <param name="arr">要处理的数组</param>
    ''' <param name="omitAfter">(可选)要显示的元素数量</param>
    Public Function FormatArrayWithEllipsis(arr As String(), Optional omitAfter As Integer = -1) As String
        If arr Is Nothing OrElse arr.Length = 0 Then
            Return String.Empty
        End If

        If omitAfter <= 0 OrElse omitAfter >= arr.Length Then Return String.Join(", ", arr) '返回所有元素

        '获取要显示的部分
        Dim visiblePart = arr.Take(omitAfter).ToArray()
        '获取省略的部分, 用于计数
        Dim omittedCount = arr.Length - omitAfter
        '创建结果字符串
        Dim result = String.Join(", ", visiblePart)
        '添加省略号
        result &= $", ... ({omittedCount} more)"
        Return result
    End Function

    ''' <summary>
    ''' 将目录复制到剪贴板
    ''' </summary>
    ''' <param name="directoryPath">要复制的目录路径字符串</param>
    Public Sub CopyDirectoryToClipboard(directoryPath As String)
        If Not Directory.Exists(directoryPath) Then
            Throw New DirectoryNotFoundException($"目录不存在: {directoryPath}")
        End If
        Dim files As String() = Directory.GetFiles(directoryPath, "*.*",
                                                   SearchOption.AllDirectories) '创建 FileDrop 格式的数据
        Dim data As New System.Collections.Specialized.StringCollection From {
            directoryPath '添加目录本身
            } '将目录添加到列表
        Clipboard.SetFileDropList(data) '设置到剪贴板
    End Sub

    ''' <summary>
    ''' 将多个目录复制到剪贴板
    ''' </summary>
    ''' <param name="directoryPaths">要复制的目录路径字符串数组</param>
    ''' <param name="dataObject">(可选)若提供<seealso cref="DataObject"/>,则在此基础上添加数据</param>
    Public Sub CopyDirectoryToClipboard(directoryPaths As String(), Optional dataObject As DataObject = Nothing)
        '验证所有目录是否存在
        For Each dirPath As String In directoryPaths
            If Not Directory.Exists(dirPath) Then
                Throw New DirectoryNotFoundException($"目录不存在: {dirPath}")
            End If
        Next
        '创建 StringCollection 并添加所有目录
        Dim data As New System.Collections.Specialized.StringCollection()
        '添加所有目录路径
        For Each dirPath As String In directoryPaths
            data.Add(dirPath)
        Next
        If dataObject Is Nothing Then '设置到剪贴板
            Clipboard.SetFileDropList(data)
        Else
            Clipboard.SetDataObject(dataObject, True)
        End If
    End Sub
#End Region

#Region "图像处理"
    ''' <summary>
    ''' 从文件载入图片, 并裁剪为正方形的缩略图
    ''' </summary>
    ''' <param name="filePath">文件路径</param>
    ''' <returns>裁剪好的<seealso cref="Image"/>对象</returns>
    Public Function LoadImageFromFile(filePath As String) As Image
        If String.IsNullOrEmpty(filePath) Then Return Nothing
        If Not File.Exists(filePath) Then Return Nothing
        Try
            '验证文件扩展名是否为支持的图像格式
            Dim extension As String = Path.GetExtension(filePath).ToLower()
            Dim supportedFormats As String() = {".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".ico", ".wmf", ".emf"}
            If Not ImageChecker.IsImageByMIMEType(filePath) Then Return Nothing
            '使用 FromFile 方法加载图像, 但先复制到内存以避免文件锁定
            Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)
                Using memoryStream As New MemoryStream()
                    fs.CopyTo(memoryStream)
                    memoryStream.Position = 0
                    '从内存流加载图像
                    Using img As Image = Image.FromStream(memoryStream)
                        '验证图像是否有效
                        If img Is Nothing OrElse img.Width = 0 Or img.Height = 0 Then Return Nothing
                        Dim size As Integer = Math.Min(img.Width, img.Height)
                        '计算裁剪区域
                        Dim cropRect As New Rectangle(
                            (img.Width - size) \ 2,
                            (img.Height - size) \ 2,
                            size,
                            size)
                        Dim croppedImage As Bitmap = Nothing
                        Dim outputSize As Integer = 256
                        croppedImage = New Bitmap(outputSize, outputSize, img.PixelFormat)
                        croppedImage.SetResolution(img.HorizontalResolution, img.VerticalResolution) '设置图像分辨率
                        '使用Graphics对象进行裁剪
                        Using g As Graphics = Graphics.FromImage(croppedImage)
                            '设置高质量绘制选项
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic
                            g.SmoothingMode = SmoothingMode.HighQuality
                            g.PixelOffsetMode = PixelOffsetMode.HighQuality
                            g.CompositingQuality = CompositingQuality.HighQuality
                            '绘制裁剪部分
                            g.DrawImage(img,
                                    New Rectangle(0, 0, outputSize, outputSize),
                                    cropRect,
                                    GraphicsUnit.Pixel)
                        End Using
                        '返回裁剪后图像的副本
                        Return CType(croppedImage.Clone(), Image)
                    End Using 'img 在这里释放
                End Using 'memoryStream 在这里释放
            End Using 'fs 在这里释放
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' 创建圆角矩形图标
    ''' </summary>
    ''' <param name="isDarkMode">是否为深色模式</param>
    ''' <param name="bitmap">要绘制在图标上的位图</param>
    ''' <returns>32x32的Icon对象</returns>
    Public Function CreateRoundedRectangleIcon(isDarkMode As Boolean, bitmap As Bitmap) As Icon
        Dim bmp As New Bitmap(32, 32)
        Using g As Graphics = Graphics.FromImage(bmp)
            g.SmoothingMode = SmoothingMode.AntiAlias
            g.InterpolationMode = InterpolationMode.HighQualityBicubic
            Dim backColor As Color = If(isDarkMode, Color.Black, Color.White)
            Dim rect As New Rectangle(0, 0, 32, 32) ' 31确保在32x32内
            Dim radius As Integer = 8 ' 圆角半径
            Dim path As GraphicsPath = GetRoundedRectanglePath(rect, radius)
            '填充圆角矩形
            Using brush As New SolidBrush(backColor)
                g.FillPath(brush, path)
            End Using
            '绘制传入的位图（调整大小以适应圆角矩形）
            If bitmap IsNot Nothing Then
                '在圆角矩形内绘制位图，留出2像素边距
                Dim imgRect As New Rectangle(2, 2, 28, 28)
                g.DrawImage(bitmap, imgRect)
            End If
        End Using
        '从位图创建图标
        Return Icon.FromHandle(bmp.GetHicon())
    End Function

    ''' <summary>
    ''' 创建圆形图标
    ''' </summary>
    ''' <param name="isDarkMode">是否为深色模式</param>
    ''' <param name="bitmap">要绘制在图标上的位图</param>
    ''' <returns>32x32的Icon对象</returns>
    Public Function CreateCircleIcon(isDarkMode As Boolean, bitmap As Bitmap) As Icon
        Dim bmp As New Bitmap(32, 32)
        Using g As Graphics = Graphics.FromImage(bmp)
            g.SmoothingMode = SmoothingMode.AntiAlias
            g.InterpolationMode = InterpolationMode.HighQualityBicubic
            '设置背景颜色
            Dim backColor As Color = If(isDarkMode, Color.Black, Color.White)
            '创建圆形路径
            Dim rect As New Rectangle(0, 0, 32, 32) ' 31确保在32x32内
            Dim path As GraphicsPath = GetCirclePath(rect)
            Using brush As New SolidBrush(backColor) '填充圆形
                g.FillPath(brush, path)
            End Using
            '绘制传入的位图
            If bitmap IsNot Nothing Then
                '在圆形内绘制位图
                Dim imgRect As New Rectangle(2, 2, 28, 28)
                '创建圆形裁剪区域
                Dim circleClip As New GraphicsPath()
                circleClip.AddEllipse(New Rectangle(2, 2, 28, 28))
                g.SetClip(circleClip)
                g.DrawImage(bitmap, imgRect)
                g.ResetClip() '重置裁剪区域
            End If
        End Using
        Return Icon.FromHandle(bmp.GetHicon())
    End Function

    ''' <summary>
    ''' 获取圆形路径
    ''' </summary>
    Private Function GetCirclePath(rect As Rectangle) As GraphicsPath
        Dim path As New GraphicsPath()
        path.AddEllipse(rect)
        Return path
    End Function

    ''' <summary>
    ''' 获取圆角矩形路径
    ''' </summary>
    Private Function GetRoundedRectanglePath(rect As Rectangle, radius As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        '确保半径不超过矩形尺寸的一半
        radius = Math.Min(radius, Math.Min(rect.Width, rect.Height) \ 2)
        '创建四个圆弧和四条直线组成的圆角矩形
        path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90) ' 左上角
        path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90) ' 右上角
        path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90) ' 右下角
        path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90) ' 左下角
        path.CloseFigure()
        Return path
    End Function

    ''' <summary>
    ''' 从图像采样像素点
    ''' </summary>
    ''' <param name="image">图像类</param>
    ''' <param name="stepCount">步长</param>
    ''' <returns>一组包含采样点的像素列表</returns>
    Public Function GetPixelsFromImage(image As Image, Optional stepCount As Integer = 5) As List(Of Color)
        Dim pixels As New List(Of Color)()
        Using bmp = New Bitmap(image)
            For x = 0 To bmp.Width - 1 Step stepCount
                For y = 0 To bmp.Height - 1 Step stepCount
                    pixels.Add(bmp.GetPixel(x, y))
                Next
            Next
        End Using
        Return pixels
    End Function

    ''' <summary>
    ''' 从图像采样像素点(更高效率版本)
    ''' </summary>
    ''' <param name="image">图像类</param>
    ''' <param name="stepCount">步长</param>
    ''' <returns>一组包含采样点的像素列表</returns>
    Public Function GetPixelsFromImageFast(image As Image, Optional stepCount As Integer = 5) As List(Of Color)
        If stepCount < 1 Then
            Throw New ArgumentOutOfRangeException(NameOf(stepCount))
        End If

        Dim pixels As New List(Of Color)()

        Using bmp As New Bitmap(image)
            Dim rect As New Rectangle(0, 0, bmp.Width, bmp.Height)
            Dim data As BitmapData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb)

            Dim bytesPerPixel As Integer = 3
            Dim stride As Integer = data.Stride
            Dim rgbValues(bytesPerPixel * bmp.Width * bmp.Height - 1) As Byte

            Marshal.Copy(data.Scan0, rgbValues, 0, rgbValues.Length)
            bmp.UnlockBits(data)

            For y As Integer = 0 To bmp.Height - 1 Step stepCount
                Dim rowOffset As Integer = y * stride
                For x As Integer = 0 To bmp.Width - 1 Step stepCount
                    Dim pos As Integer = rowOffset + x * bytesPerPixel
                    'LockBits 顺序为 BGR
                    Dim b As Byte = rgbValues(pos)
                    Dim g As Byte = rgbValues(pos + 1)
                    Dim r As Byte = rgbValues(pos + 2)
                    pixels.Add(Color.FromArgb(r, g, b))
                Next
            Next
        End Using
        Return pixels
    End Function

#End Region

#Region "主题相关"
    ''' <summary>
    ''' 获得特定控件的全部子控件
    ''' </summary>
    ''' <param name="container">父控件</param>
    ''' <returns>子控件集合</returns>
    Public Function GetAllControls(container As Control) As List(Of Control)
        Dim controls As New List(Of Control)
        GetAllControlsRecursive(container, controls)
        Return controls
    End Function
    Private Sub GetAllControlsRecursive(container As Control, ByRef controlList As List(Of Control))
        For Each control As Control In container.Controls
            controlList.Add(control)
            ' 递归获取子控件
            If control.HasChildren Then
                GetAllControlsRecursive(control, controlList)
            End If
        Next
    End Sub
    ''' <summary>
    ''' 将 RGB 转换成 COLORREF 格式
    ''' </summary>
    Public Function RGBToCOLORREF(ByVal r As Byte, ByVal g As Byte, ByVal b As Byte) As Integer
        '0x00BBGGRR
        Return CInt(b) << 16 Or CInt(g) << 8 Or CInt(r)
    End Function
    Public Function RGBToCOLORREF(color As Color) As Integer
        Return RGBToCOLORREF(color.R, color.G, color.B)
    End Function
    Public Sub SetTitleBarColor(ByVal hwnd As IntPtr, ByVal r As Byte, ByVal g As Byte, ByVal b As Byte)
        Try
            '设置标题栏与边框背景色
            Dim colorRef As Integer = RGBToCOLORREF(r, g, b)
            DwmSetWindowAttribute(hwnd, DwmWindowAttribute.CaptionColor, colorRef, Marshal.SizeOf(Of Integer)())
            DwmSetWindowAttribute(hwnd, DwmWindowAttribute.BorderColor, colorRef, Marshal.SizeOf(Of Integer)())
            '根据背景亮度决定文字颜色
            Dim textColor As Integer = RGBToCOLORREF(GetForeColor(Color.FromArgb(r, g, b)))
            DwmSetWindowAttribute(hwnd, DwmWindowAttribute.TextColor, textColor, Marshal.SizeOf(Of Integer)())
        Catch ex As Exception
            '忽略错误
        End Try
    End Sub
    Public Sub SetTitleBarColor(ByVal hwnd As IntPtr, ByVal color As Color)
        SetTitleBarColor(hwnd, color.R, color.G, color.B)
    End Sub
    ''' <summary>
    ''' 定义一个修改系统主题变更的接口
    ''' </summary>
    Public Interface IThemeChangeable
        Sub SystemThemeChange()
    End Interface
    Public Sub UpdateFormTheme()
        For Each frm As Form In Application.OpenForms
            Dim themeable = TryCast(frm, IThemeChangeable)
            themeable?.SystemThemeChange() '当不为空时更新主题
        Next
    End Sub
    ''' <summary>
    ''' 返回特定尺寸的logo图标
    ''' </summary>
    Public Function GetIconBySize(ByVal size As Integer) As Icon
        '从资源文件获取原始图标
        Dim originalIcon As Icon = Icon.FromHandle(My.Resources.Icons.FurryArtStudio.GetHicon)
        '创建指定大小的图标
        Dim sizedIcon As Icon = New Icon(originalIcon, New Size(size, size))
        Return sizedIcon
    End Function
#End Region

#Region "菜单处理"
    ''' <summary>
    ''' 为指定的菜单项设置图标, 并处理透明度背景模拟
    ''' </summary>
    ''' <param name="hMenu">菜单句柄(hMenu)</param>
    ''' <param name="wParam">菜单项标识符(wParam)</param>
    ''' <param name="icon">原始图标资源</param>
    ''' <param name="isDarkMode">是否为深色模式</param>
    Public Sub ApplyMenuIcon(hMenu As IntPtr, wParam As Integer, icon As Bitmap, Optional isDarkMode As Boolean = False)
        '释放旧的位图句柄
        Dim mii As New MENUITEMINFO With {
            .cbSize = Marshal.SizeOf(Of MENUITEMINFO)(),
            .fMask = MIIM_BITMAP
        }
        If GetMenuItemInfo(hMenu, wParam, False, mii) Then
            If mii.hbmpItem <> IntPtr.Zero Then
                DeleteObject(mii.hbmpItem)
            End If
        End If
        '创建新位图并设置
        Dim size As Integer = 18 '图标尺寸
        Using resizedBmp As New Bitmap(size, size, PixelFormat.Format24bppRgb)
            Using g As Graphics = Graphics.FromImage(resizedBmp) '设置高质量缩放参数
                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                g.SmoothingMode = SmoothingMode.HighQuality
                g.PixelOffsetMode = PixelOffsetMode.HighQuality
                g.CompositingQuality = CompositingQuality.HighQuality
                '清空背景, 并按照主题填充
                If isDarkMode Then
                    g.Clear(Color.FromArgb(43, 43, 43))
                Else
                    g.Clear(SystemColors.Menu)
                End If
                '计算保持宽高比的绘制区域
                Dim srcWidth As Integer = icon.Width
                Dim srcHeight As Integer = icon.Height
                '计算缩放比例
                Dim ratio As Double = Math.Min(size / srcWidth, size / srcHeight)
                Dim newWidth As Integer = CInt(srcWidth * ratio)
                Dim newHeight As Integer = CInt(srcHeight * ratio)
                Dim x As Integer = (size - newWidth) \ 2
                Dim y As Integer = (size - newHeight) \ 2
                g.DrawImage(icon, New Rectangle(x, y, newWidth, newHeight), 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel)
            End Using '绘制缩放后的图像
            Dim hBitmap = resizedBmp.GetHbitmap()
            SetMenuItemBitmaps(hMenu, wParam, MF_BYCOMMAND, hBitmap, Nothing)
        End Using
    End Sub
    ''' <summary>
    ''' 设置菜单项的快捷键文本
    ''' </summary>
    ''' <param name="menuHandle">菜单句柄</param>
    ''' <param name="position">菜单位置</param>
    ''' <param name="id">菜单ID</param>
    ''' <param name="text">菜单内容</param>
    ''' <param name="shortcut">快捷键文本</param>
    Public Sub SetMenuItemWithShortcut(menuHandle As IntPtr, position As Integer, id As Integer, text As String, shortcut As String)
        Dim mii As New MENUITEMINFO()
        mii.cbSize = Marshal.SizeOf(mii)
        mii.fMask = MIIM_FTYPE Or MIIM_STRING Or MIIM_ID
        mii.fType = MFT_STRING
        mii.wID = id
        mii.dwTypeData = text & vbTab & shortcut
        mii.cch = mii.dwTypeData.Length
        SetMenuItemInfo(menuHandle, position, True, mii)
    End Sub
    ''' <summary>
    ''' 修改菜单文本
    ''' </summary>
    ''' <param name="hMenu">菜单句柄</param>
    ''' <param name="nPos">菜单位置</param>
    ''' <param name="newText">新的菜单项文本</param>
    Public Sub UpdateMenuItemText(ByVal hMenu As IntPtr, ByVal nPos As Integer, ByVal newText As String)
        Dim mii As New MENUITEMINFO With {
            .cbSize = Marshal.SizeOf(GetType(MENUITEMINFO)),
            .fMask = MIIM_STRING Or MIIM_ID Or MIIM_FTYPE,
            .fType = MFT_STRING,
            .dwTypeData = newText,
            .cch = newText.Length
        }
        SetMenuItemInfo(hMenu, nPos, True, mii)
    End Sub
#End Region

#Region "环境判断"
    ''' <summary>
    ''' 判断当前是否以管理员权限运行
    ''' </summary>
    Public Function IsAdmin() As Boolean
        Dim identity As WindowsIdentity = WindowsIdentity.GetCurrent()
        Dim principal As New WindowsPrincipal(identity)
        Return principal.IsInRole(WindowsBuiltInRole.Administrator)
    End Function

    ''' <summary>
    ''' 判断一个文件是否为图片
    ''' </summary>
    ''' <param name="filePath">文件路径</param>
    Public Function IsImageFile(filePath As String) As Boolean
        Dim imageExtensions As String() = {".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".ico", ".webp"}
        Dim ext As String = Path.GetExtension(filePath).ToLower()
        Return imageExtensions.Contains(ext)
    End Function
    ''' <summary>
    ''' 判断当前系统主题是否为深色主题
    ''' </summary>
    Public Function IsDarkMode() As Boolean
        Select Case AppSettings.Load().Appearance.Theme
            Case AppSettings.ThemeMode.Light
                Return False
            Case AppSettings.ThemeMode.Dark
                Return True
            Case AppSettings.ThemeMode.FollowSystem
                Using regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", True)
                    Return regKey.GetValue("AppsUseLightTheme", "1") = 0
                End Using
            Case Else
                Return False
        End Select
    End Function
    ''' <summary>
    ''' 获得版本号
    ''' </summary>
    Public Function GetCurrentVersion() As String
        Dim version = Assembly.GetExecutingAssembly().GetName().Version
        Return $"v{version.Major}.{version.Minor}.{version.Build}"
    End Function
    ''' <summary>
    ''' 获得当前颜色的前景色
    ''' </summary>
    Public Function GetForeColor(backcolor As Color)
        Dim brightness As Double = (0.299 * backcolor.R + 0.587 * backcolor.G + 0.114 * backcolor.B)
        Return If(brightness > 128, Color.Black, Color.White)
    End Function
    ''' <summary>
    ''' 判断程序是否首次运行
    ''' </summary>
    Public Function IsFirstRun() As Boolean
        Return Not File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppSettings.json"))
    End Function
    ''' <summary>
    ''' 判断程序是否设置开机自启动
    ''' </summary>
    Public Function IsAutoStart() As Boolean
        Return Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run").
            GetValue("FurryArtStudio") IsNot Nothing
    End Function
#End Region

#Region "本地化"
    ''' <summary>
    ''' 定义一个语言变更接口
    ''' </summary>
    Public Interface ILocalizable
        Sub LanguageChange()
    End Interface
    Public Sub UpdateFormLang()
        For Each frm As Form In Application.OpenForms
            Dim localizable = TryCast(frm, ILocalizable)
            localizable?.LanguageChange() '当不为空时更新语言
        Next
    End Sub
#End Region

#Region "对话框"
    ''' <summary>
    ''' 显示一个包含堆栈跟踪的错误对话框
    ''' </summary>
    Public Sub ShowErrorDialog(exception As Exception, mainInstruction As String)
        Dim dialog As New TaskDialog()
        dialog.WindowTitle = My.Resources.FurryArtStudio
        dialog.MainInstruction = mainInstruction
        dialog.Content = exception.Message
        If Not String.IsNullOrEmpty(exception.StackTrace) Then
            dialog.ExpandedInformation = exception.StackTrace
        End If
        dialog.ExpandedByDefault = False
        dialog.MainIcon = TaskDialogIcon.Error
        Dim copyButton As New TaskDialogButton(My.Resources.Msg_CopyDetails)
        '处理复制按钮点击事件

        Dim okButton As New TaskDialogButton(ButtonType.Ok)
        dialog.Buttons.Add(copyButton)
        dialog.Buttons.Add(okButton)
        AddHandler dialog.ButtonClicked, Sub(sender, e)
                                             '构建要复制的完整信息
                                             If e.Item Is copyButton Then
                                                 Dim info As New StringBuilder()
                                                 info.AppendLine(mainInstruction)
                                                 info.AppendLine(exception.Message)
                                                 If Not String.IsNullOrEmpty(exception.StackTrace) Then
                                                     info.AppendLine(vbCrLf)
                                                     info.AppendLine(exception.StackTrace)
                                                 End If
                                                 '复制到剪贴板
                                                 Clipboard.SetText(info.ToString())
                                                 e.Cancel = True
                                             End If
                                         End Sub
        dialog.ShowDialog()
    End Sub
    Public Sub ShowInfoDialog(content As String, Optional mainInstruction As String = "")
        Using dlg As New TaskDialog With {
            .WindowTitle = My.Resources.FurryArtStudio,
            .Content = content,
            .MainIcon = TaskDialogIcon.Information
            }
            If mainInstruction <> "" Then dlg.MainInstruction = mainInstruction
            dlg.Buttons.Add(New TaskDialogButton(ButtonType.Ok))
            dlg.ShowDialog()
        End Using
    End Sub
#End Region

#Region "窗口特权相关"
    ''' <summary>
    ''' 底层注册逻辑: 放行跨特权等级的拖拽消息(#10)
    ''' </summary>
    Public Sub RegisterUIPIDragDropFilter(hWnd As IntPtr)
        If hWnd = IntPtr.Zero Then Return
        
        Try
            Dim cfs As New CHANGEFILTERSTRUCT()
            cfs.cbSize = Marshal.SizeOf(cfs)
            '定义需要穿越UIPI墙的消息集
            Dim targetMessages As Integer() = {
                WM_DROPFILES,
                WM_COPYDATA,
                WM_COPYGLOBALDATA
            }
            For Each msg In targetMessages
                ChangeWindowMessageFilterEx(hWnd, msg, MSGFLT_ALLOW, cfs)
            Next
            '显式告知Shell接受文件流
            DragAcceptFiles(hWnd, True)
        Catch ex As Exception
            Debug.WriteLine($"Critical: Failed to set UIPI filter for {hWnd:X}. Message: {ex.Message}")
        End Try
    End Sub
#End Region

#Region "桌面快捷方式"
    ''' <summary>
    ''' 通过 PowerShell 在桌面创建快捷方式
    ''' </summary>
    ''' <param name="targetPath">目标程序的完整路径</param>
    ''' <param name="shortcutName">快捷方式名称</param>
    ''' <param name="destinationFolder">存放快捷方式的文件夹路径</param>
    ''' <param name="workingDirectory">(可选)工作目录</param>
    ''' <param name="description">(可选)快捷方式描述</param>
    ''' <param name="iconLocation">(可选)图标位置(格式:"路径,索引")</param>
    ''' <returns>成功返回 True，失败返回 False</returns>
    Public Function CreateShortcut(targetPath As String,
                                          shortcutName As String,
                                          destinationFolder As String,
                                          Optional workingDirectory As String = Nothing,
                                          Optional description As String = Nothing,
                                          Optional iconLocation As String = Nothing) As Boolean
        '获取桌面路径
        Dim shortcutPath As String = Path.Combine(destinationFolder, shortcutName & ".lnk")
        '构建 PowerShell 命令
        Dim psCommand As New StringBuilder()
        psCommand.AppendLine("$shell = New-Object -ComObject WScript.Shell")
        psCommand.AppendLine("$shortcut = $shell.CreateShortcut('" + EscapePathForPowerShell(shortcutPath) + "')")
        psCommand.AppendLine("$shortcut.TargetPath = '" + EscapePathForPowerShell(targetPath) + "'")
        If Not String.IsNullOrEmpty(workingDirectory) Then
            psCommand.AppendLine("$shortcut.WorkingDirectory = '" + EscapePathForPowerShell(workingDirectory) + "'")
        End If
        If Not String.IsNullOrEmpty(description) Then
            psCommand.AppendLine("$shortcut.Description = '" + EscapeForPowerShell(description) + "'")
        End If
        If Not String.IsNullOrEmpty(iconLocation) Then
            'iconLocation 格式如 "C:\file.exe,0"
            '需要拆分为路径和索引
            Dim parts = iconLocation.Split(","c)
            Dim iconPath = parts(0)
            Dim iconIndex = 0
            If parts.Length > 1 Then Integer.TryParse(parts(1), iconIndex)
            psCommand.AppendLine("$shortcut.IconLocation = '" + EscapePathForPowerShell(iconPath) + "', " + iconIndex.ToString())
        End If
        psCommand.AppendLine("$shortcut.Save()")
        ' 准备 PowerShell
        Dim startInfo As New ProcessStartInfo()
        startInfo.FileName = "powershell.exe"
        startInfo.Arguments = "-NoProfile -ExecutionPolicy Bypass -Command """ & psCommand.ToString().Replace("""", "\""") & """"
        startInfo.UseShellExecute = False
        startInfo.CreateNoWindow = True
        startInfo.RedirectStandardError = True
        startInfo.RedirectStandardOutput = True
        Try
            Using proc As Process = Process.Start(startInfo)
                proc.WaitForExit()
                Dim errorMsg = proc.StandardError.ReadToEnd()
                If proc.ExitCode <> 0 OrElse Not String.IsNullOrEmpty(errorMsg) Then
                    Return False
                End If
                Return True
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function
    ''' <summary>
    ''' 转义路径中的单引号并确保路径合法
    ''' </summary>
    Private Function EscapePathForPowerShell(path As String) As String
        If String.IsNullOrEmpty(path) Then Return path
        '将单引号替换为两个单引号
        Return path.Replace("'", "''")
    End Function
    ''' <summary>
    ''' 转义普通字符串中的单引号
    ''' </summary>
    Private Function EscapeForPowerShell(text As String) As String
        If String.IsNullOrEmpty(text) Then Return text
        Return text.Replace("'", "''")
    End Function
#End Region

#Region "注册表"
    ''' <summary>
    ''' 通过注册表获取指定扩展名关联的默认打开程序
    ''' </summary>
    Public Function GetFileTypeDescription(extension As String) As String
        Try
            '标准化扩展名
            If String.IsNullOrWhiteSpace(extension) Then Return ""
            If Not extension.StartsWith(".") Then extension = "." & extension
            '扩展名 -> ProgID
            Using extKey As RegistryKey = Registry.ClassesRoot.OpenSubKey(extension)
                If extKey Is Nothing Then Return extension & " Files"
                Dim progId As String = TryCast(extKey.GetValue(Nothing), String)
                If String.IsNullOrEmpty(progId) Then Return extension & " Files"
                '用 ProgID 查描述
                Using progKey As RegistryKey = Registry.ClassesRoot.OpenSubKey(progId)
                    If progKey Is Nothing Then Return extension & " Files"
                    Dim description As String = TryCast(progKey.GetValue(Nothing), String)
                    If Not String.IsNullOrEmpty(description) Then
                        Return $"{description} ({extension})"
                    End If
                End Using
            End Using
        Catch ex As Exception
        End Try
        Return extension & " Files"
    End Function
    ''' <summary>
    ''' 设置开机自启动
    ''' </summary>
    Public Sub SetAutoStart()
        Dim regResult As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True)
        If Not IsAutoStart() Then
            regResult.SetValue("FurryArtStudio", """" & Application.ExecutablePath & """")
        Else
            regResult.DeleteValue("FurryArtStudio")
        End If
    End Sub
#End Region

#Region "系统"
    ''' <summary>
    ''' 以管理员权限启动程序
    ''' </summary>
    ''' <param name="param">(可选)启动参数</param>
    ''' <returns>是否启动成功</returns>
    Public Function RunAsElevated(Optional param As String = "") As Boolean
        Dim startInfo As New ProcessStartInfo With {
        .UseShellExecute = True, '必须设置为True才能使用Verb
        .Verb = "runas", '请求管理员权限
        .FileName = Application.ExecutablePath,
        .Arguments = param
        }
        Try
            Process.Start(startInfo)
            Return True
        Catch ex As Win32Exception
            ShowErrorDialog(ex, My.Resources.Msg_ElevatedFailed)
            Return False
        End Try
    End Function
#End Region

#Region "加密"
    ''' <summary>
    ''' 生成 HmacSHA256 签名，结果经过 Base64 和 URL 编码
    ''' </summary>
    ''' <param name="secret">密钥字符串</param>
    ''' <returns>URL 编码后的 Base64 签名结果</returns>
    Public Function GenerateSignature(secret As String) As String
        '获取当前 Unix 时间戳
        Dim timestamp As Long = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        '构建待签名字符串：timestamp + "\n" + secret
        Dim stringToSign As String = timestamp.ToString() & vbLf & secret
        '转换为 UTF-8 字节数组
        Dim secretBytes As Byte() = Encoding.UTF8.GetBytes(secret)
        Dim stringToSignBytes As Byte() = Encoding.UTF8.GetBytes(stringToSign)
        '使用 HMACSHA256 计算签名
        Using hmac As New HMACSHA256(secretBytes)
            Dim signBytes As Byte() = hmac.ComputeHash(stringToSignBytes)
            'Base64 编码
            Dim base64 As String = Convert.ToBase64String(signBytes)
            'URL 编码
            Dim sign As String = Uri.EscapeDataString(base64)
            Return sign
        End Using
    End Function
#End Region

#Region "单例处理"
    ''' <summary>
    ''' 读取当前程序集GUID
    ''' </summary>
    ''' <returns>程序集Guid</returns>
    Public Function GetAssemblyGuid() As String
        Dim assembly As Assembly = Assembly.GetExecutingAssembly()
        Dim guidAttr As GuidAttribute = assembly.GetCustomAttribute(Of GuidAttribute)()
        If guidAttr IsNot Nothing Then
            Return guidAttr.Value
        Else
            Return Guid.Empty.ToString
        End If
    End Function
    ''' <summary>
    ''' 创建全局互斥体
    ''' </summary>
    Public Sub CreateGlobalMutex()
        Dim mutexName As String = "FAS_" & GetAssemblyGuid()
        GlobalMutex = New Mutex(True, mutexName, False)
    End Sub
    ''' <summary>
    ''' 销毁全局互斥体
    ''' </summary>
    Public Sub DestroyGlobalMutex()
        If GlobalMutex IsNot Nothing Then
            GlobalMutex.ReleaseMutex()
            GlobalMutex.Dispose()
        End If
    End Sub
    ''' <summary>
    ''' 判断当前是否为单例
    ''' </summary>
    ''' <returns>若是, 则返回True, 否则返回False</returns>
    Public Function IsSingleInstance()
        Return GlobalMutex.WaitOne(0, False)
    End Function
    ''' <summary>
    ''' 激活当前存在的实例
    ''' </summary>
    Public Sub ActivateExistingInstance()
        Dim currentProcess = Process.GetCurrentProcess()
        Dim processName = currentProcess.ProcessName
        '遍历所有同名进程
        For Each proc As Process In Process.GetProcessesByName(processName)
            If proc.Id <> currentProcess.Id Then '跳过自身
                Dim hWnd = proc.MainWindowHandle '注意: 进程隐藏后就获得不到了, 可能是这个函数特性
                If hWnd <> IntPtr.Zero Then
                    If MainForm.NotifyIco.Visible = True Then
                        MainForm.Show()
                        MainForm.NotifyIco.Visible = False
                    End If
                    SendMessage(hWnd, WM_SHOWME, IntPtr.Zero, IntPtr.Zero)
                    Exit For
                End If
            End If
        Next
    End Sub
#End Region

End Module
