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
Imports System.Runtime.InteropServices

Public Module WinAPI

#Region "菜单"
    'CreatePopupMenu 函数 - 新建弹出菜单
    <DllImport("user32.dll")>
    Public Function CreatePopupMenu() As IntPtr
    End Function
    'GetSystemMenu 函数 - 获得系统菜单
    <DllImport("user32.dll")>
    Public Function GetSystemMenu(
        ByVal hwnd As IntPtr,
        ByVal bRevert As Boolean
    ) As IntPtr
    End Function
    'AppendMenu 函数 - 添加菜单项
    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Public Function AppendMenu(
        ByVal hMenu As IntPtr,
        ByVal wFlags As Integer,
        ByVal wIDNewItem As Integer,
        ByVal lpNewItem As String
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
    'RemoveMenu 函数 - 删除菜单项
    <DllImport("user32.dll")>
    Public Function RemoveMenu(
        ByVal hMenu As IntPtr,
        ByVal uPosition As Integer,
        ByVal uFlags As Integer
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
    'CheckMenuItem 函数 - 选中/清除选中菜单项
    <DllImport("user32.dll")>
    Public Function CheckMenuItem(
        ByVal hMenu As IntPtr,
        ByVal uIDCheckItem As Integer,
        ByVal uCheck As Integer
    ) As Integer
    End Function
    'SetMenuItemBitmaps 函数 - 设置菜单位图
    <DllImport("user32.dll")>
    Public Function SetMenuItemBitmaps(
        ByVal hMenu As IntPtr,
        ByVal uPosition As Integer,
        ByVal uFlags As Integer,
        ByVal hBitmapUnchecked As IntPtr,
        ByVal hBitmapChecked As IntPtr
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
    'EnableMenuItem 函数 - 使菜单在有效与无效之间切换
    <DllImport("user32.dll")>
    Public Function EnableMenuItem(
        ByVal hMenu As IntPtr,
        ByVal uIDEnableItem As Integer,
        ByVal uEnable As Integer
    ) As Integer
    End Function
    'GetMenuItemCount 函数 - 获取菜单项数量
    <DllImport("user32.dll")>
    Public Function GetMenuItemCount(
        ByVal hMenu As IntPtr
    ) As Integer
    End Function
    'InsertMenu 函数 - 插入菜单项
    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Public Function InsertMenu(
        ByVal hMenu As IntPtr,
        ByVal nPosition As Integer,
        ByVal wFlags As Integer,
        ByVal wIDNewItem As Integer,
        <MarshalAs(UnmanagedType.LPTStr)> ByVal lpNewItem As String
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
    'TrackPopupMenu 函数 - 弹出菜单
    <DllImport("user32.dll")>
    Public Function TrackPopupMenu(hMenu As IntPtr, uFlags As Integer, x As Integer, y As Integer, nReserved As Integer, hWnd As IntPtr, prcRect As IntPtr) As Integer
    End Function
    'GetMenuItemID 函数 - 获取菜单项ID
    <DllImport("user32.dll")>
    Public Function GetMenuItemID(
        ByVal hMenu As IntPtr,
        ByVal nPos As Integer
    ) As Integer
    End Function
    'SetMenuItemInfo 函数 - 设置菜单项信息
    <DllImport("user32.dll")>
    Public Function SetMenuItemInfo(hMenu As IntPtr, un As Integer, fByPosition As Boolean, <MarshalAs(UnmanagedType.Struct, SizeConst:=80)> ByRef lpmii As MENUITEMINFO) As Boolean
    End Function
    'GetMenuItemInfo 函数 - 获取菜单项信息
    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Public Function GetMenuItemInfo(hMenu As IntPtr, uItem As Integer, fByPosition As Boolean, ByRef lpmii As MENUITEMINFO) As Boolean
    End Function
    'DeleteObject 函数 - 释放资源
    <DllImport("gdi32.dll")>
    Public Function DeleteObject(hObject As IntPtr) As Boolean
    End Function
    'MENUITEMINFO 结构体
    <StructLayout(LayoutKind.Sequential)>
    Public Structure MENUITEMINFO
        Public cbSize As Integer
        Public fMask As Integer
        Public fType As Integer
        Public fState As Integer
        Public wID As Integer
        Public hSubMenu As IntPtr
        Public hbmpChecked As IntPtr
        Public hbmpUnchecked As IntPtr
        Public dwItemData As IntPtr
        Public dwTypeData As String
        Public cch As Integer
        Public hbmpItem As IntPtr
    End Structure
    Public Const MIIM_BITMAP As Integer = &H80
    Public Const MIIM_TYPE As Integer = &H10
    Public Const MIIM_FTYPE As Integer = &H100
    Public Const MIIM_STRING As Integer = &H40
    Public Const MIIM_ID As Integer = &H2
    Public Const MFT_STRING As Integer = &H0
    '菜单常量
    Public Const MF_SEPARATOR = &H800 '分隔符
    Public Const MF_STRING = &H0 '字符串
    Public Const MF_BITMAP = &H4 '位图
    Public Const MF_GRAYED = &H1 '灰色菜单
    Public Const MF_ENABLED = &H0 '菜单可用
    Public Const MF_CHECKED = &H8 '勾选
    Public Const MF_UNCHECKED = &H0 '取消勾选
    Public Const MF_HILITE = &H80 '高亮
    Public Const MF_BYCOMMAND = &H0 '标识符
    Public Const MF_BYPOSITION = &H400 '位置
    Public Const MF_POPUP = &H10 '弹出菜单
    '菜单项常量
    Public Const SC_RESTORE = &HF120 '还原
    Public Const SC_MOVE = &HF010 '移动
    Public Const SC_SIZE = &HF000 '大小
    Public Const SC_MINIMIZE = &HF020 '最小化
    Public Const SC_MAXIMIZE = &HF030 '最大化
    Public Const SC_CLOSE = &HF060 '关闭
    '菜单显示标识
    Public Const TPM_LEFTALIGN As Integer = &H0
    Public Const TPM_RETURNCMD As Integer = &H100

#End Region

#Region "窗口"
    <StructLayout(LayoutKind.Sequential)>
    Public Structure CHANGEFILTERSTRUCT
        Public cbSize As Integer
        Public ExtStatus As Integer
    End Structure
    'SetWindowDisplayAffinity 函数 - 改变窗口亲和性从而阻止截屏录屏操作
    <DllImport("user32.dll", SetLastError:=True)>
    Public Function SetWindowDisplayAffinity(
        ByVal hWnd As IntPtr,
        ByVal dwAffinity As UInteger
    ) As Integer
    End Function
    <DllImport("shell32.dll")>
    Public Sub DragAcceptFiles(ByVal hWnd As IntPtr, ByVal fAccept As Boolean)
    End Sub
    'ChangeWindowMessageFilter 函数 - 修改指定窗口(UIPI)消息筛选器的用户界面特权隔离, 解除管理员模式下无法拖拽的问题
    <DllImport("user32.dll")>
    Public Function ChangeWindowMessageFilter(
        ByVal message As Integer,
        ByVal dwFlag As Integer
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
    'ChangeWindowMessageFilterEx 函数 - 窗口级别
    <DllImport("user32.dll", SetLastError:=True)>
    Public Function ChangeWindowMessageFilterEx(
        ByVal hwnd As IntPtr,
        ByVal message As Integer,
        ByVal action As Integer,
        ByRef pChangeFilterStruct As CHANGEFILTERSTRUCT) As Boolean
    End Function
    'SendMessage 函数 - 发送特定消息
    <DllImport("user32.dll")>
    Public Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function
    'ReleaseCapture 函数 - 处理拖动事件
    <DllImport("user32.dll")>
    Public Function ReleaseCapture() As Boolean
    End Function
    'SetForegroundWindow 函数 - 将窗口置于前台
    <DllImport("user32.dll")>
    Public Function SetForegroundWindow(ByVal hWnd As IntPtr) As Boolean
    End Function
    '消息常量
    Public Const WM_DROPFILES As Integer = &H233 '拖拽文件
    Public Const WM_COPYGLOBALDATA As Integer = &H49
    Public Const WM_COPYDATA As Integer = &H4A
    Public Const MSGFLT_ALLOW As Integer = 1
    Public Const MSGFLT_ADD As Integer = 1
    Public Const MSGFLT_REMOVE As Integer = 2
    '窗口常量
    Public Const WM_SYSCOLORCHANGE = &H15S '当系统颜色改变时, 发送此消息给所有顶级窗口
    Public Const WM_SETFOCUS = &H7S '窗体获得焦点
    Public Const WM_KILLFOCUS = &H8S '窗体失去焦点
    Public Const WM_COMMAND = &H111 '窗体选择菜单项
    Public Const WM_SYSCOMMAND = &H112 '窗体选择系统菜单项
    Public Const WM_DWMCOLORIZATIONCOLORCHANGED = &H320 '窗体主题色被更改(深色同样有效)
    Public Const WM_SYSMENU As Integer = &H313 '系统菜单常量 
    Public Const WM_NCLBUTTONDOWN As Integer = &HA1
    Public Const WM_THEMECHANGED As Integer = &H31A
    Public Const WM_SETREDRAW As Integer = &HB
    Public Const WM_SETTINGCHANGE As Integer = &H1A '设置变更
    Public Const WM_SHOWME As Integer = &H8000 + 9876 '恢复托盘图标状态消息
    '其他常量
    Public Const HTCAPTION As Integer = 2
    '阻止截屏常量
    Public Const WDA_NONE = &H0
    Public Const WDA_MONITOR = &H1
    Public Const WDA_EXCLUDEFROMCAPTURE = &H11 'Windows 10 20H1+
#End Region

#Region "文本"
    'StrCmpLogicalW 函数 - 进行人类/自然排序
    <DllImport("shlwapi.dll", CharSet:=CharSet.Unicode, ExactSpelling:=True)>
    Public Function StrCmpLogicalW(x As String, y As String) As Integer
    End Function
#End Region

#Region "进程"
    'ShellExecuteEx 函数 - 实现 Process 无法实现的功能
    <DllImport("shell32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Public Function ShellExecuteEx(ByRef lpExecInfo As SHELLEXECUTEINFO) As Boolean
    End Function
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
    Public Structure SHELLEXECUTEINFO
        Public cbSize As Integer
        Public fMask As Integer
        Public hwnd As IntPtr
        <MarshalAs(UnmanagedType.LPTStr)>
        Public lpVerb As String
        <MarshalAs(UnmanagedType.LPTStr)>
        Public lpFile As String
        <MarshalAs(UnmanagedType.LPTStr)>
        Public lpParameters As String
        <MarshalAs(UnmanagedType.LPTStr)>
        Public lpDirectory As String
        Public nShow As Integer
        Public hInstApp As IntPtr
        Public lpIDList As IntPtr
        <MarshalAs(UnmanagedType.LPTStr)>
        Public lpClass As String
        Public hkeyClass As IntPtr
        Public dwHotKey As Integer
        Public hIcon As IntPtr
        Public hProcess As IntPtr
    End Structure
    Public Const SW_SHOW As Integer = 5
    Public Const SEE_MASK_INVOKEIDLIST As Integer = &HC
#End Region

#Region "文件处理"
    <DllImport("kernel32.dll", CharSet:=CharSet.Auto)>
    Public Function CreateHardLink(
        ByVal lpFileName As String,
        ByVal lpExistingFileName As String,
        ByVal lpSecurityAttributes As IntPtr
        ) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
#End Region
End Module
