using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

using System.Text;
using System.Drawing;

using NotifyIcon;
using static NotifyIcon.NotifyIconTools;
using GlobalStructures;
using GDIPlus;
using static GDIPlus.GDIPlusTools;
using GDI;
using static GDI.GDITools;
using System.Reflection.Metadata;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3_NotifyIcon
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public static int HIWORD(int n)
        {
            return (n >> 16) & 0xffff;
        }
        public static int LOWORD(int n)
        {
            return n & 0xffff;
        }   

        public delegate int SUBCLASSPROC(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, uint dwRefData);

        [DllImport("Comctl32.dll", SetLastError = true)]
        public static extern bool SetWindowSubclass(IntPtr hWnd, SUBCLASSPROC pfnSubclass, uint uIdSubclass, uint dwRefData);

        [DllImport("Comctl32.dll", SetLastError = true)]
        public static extern int DefSubclassProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        public const int WM_CONTEXTMENU = 0x007B;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_RBUTTONUP = 0x0205;
        public const int WM_ENTERMENULOOP = 0x0211;
        public const int WM_EXITMENULOOP = 0x0212;
        public const int WM_INITMENUPOPUP = 0x0117;
        public const int WM_UNINITMENUPOPUP = 0x0125;

        public const int WM_DRAWITEM = 0x002B;
        public const int WM_MEASUREITEM = 0x002C;

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool GetCursorPos(out Windows.Graphics.PointInt32 lpPoint);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr CreatePopupMenu();

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int GetMenuItemCount(IntPtr hMenu);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool DestroyMenu(IntPtr hMenu);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool AppendMenu(IntPtr hMenu, uint uFlags, IntPtr uIDNewItem, string lpNewItem);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool ModifyMenu(IntPtr hMnu, uint uPosition, uint uFlags, IntPtr uIDNewItem, IntPtr lpNewItem);

        public const int MF_STRING = 0x00000000;
        public const int MF_BITMAP = 0x00000004;
        public const int MF_OWNERDRAW = 0x00000100;

        public const int MF_POPUP = 0x00000010;
        public const int MF_MENUBARBREAK = 0x00000020;
        public const int MF_MENUBREAK = 0x00000040;
        public const int MF_SEPARATOR = 0x00000800;

        public const int MF_BYCOMMAND = 0x00000000;
        public const int MF_BYPOSITION = 0x00000400;

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr prcRect);

        public const int TPM_LEFTBUTTON = 0x0000;
        public const int TPM_RIGHTBUTTON = 0x0002;
        public const int TPM_LEFTALIGN = 0x0000;
        public const int TPM_CENTERALIGN = 0x0004;
        public const int TPM_RIGHTALIGN = 0x0008;
        public const int TPM_TOPALIGN = 0x0000;
        public const int TPM_VCENTERALIGN = 0x0010;
        public const int TPM_BOTTOMALIGN = 0x0020;
        public const int TPM_HORIZONTAL = 0x0000;     /* Horz alignment matters more */
        public const int TPM_VERTICAL = 0x0040;     /* Vert alignment matters more */
        public const int TPM_NONOTIFY = 0x0080;     /* Don't send any notification msgs */
        public const int TPM_RETURNCMD = 0x0100;
        public const int TPM_RECURSE = 0x0001;
        public const int TPM_HORPOSANIMATION = 0x0400;
        public const int TPM_HORNEGANIMATION = 0x0800;
        public const int TPM_VERPOSANIMATION = 0x1000;
        public const int TPM_VERNEGANIMATION = 0x2000;
        public const int TPM_NOANIMATION = 0x4000;
        public const int TPM_LAYOUTRTL = 0x8000;
        public const int TPM_WORKAREA = 0x10000;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct DRAWITEMSTRUCT
        {
            public uint CtlType;
            public uint CtlID;
            public uint itemID;
            public uint itemAction;
            public uint itemState;
            public IntPtr hwndItem;
            public IntPtr hDC;
            public RECT rcItem;
            public IntPtr itemData;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct MEASUREITEMSTRUCT
        {
            public uint CtlType;
            public uint CtlID;
            public uint itemID;
            public uint itemWidth;
            public uint itemHeight;
            public IntPtr itemData;
        }

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetProcAddress")]
        public static extern IntPtr GetProcAddressByOrdinal(IntPtr hModule, int lpProcName);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern IntPtr LoadLibrary(string lpLibFileName);

        public enum PreferredAppMode { Default, AllowDark, ForceDark, ForceLight, Max };
        internal delegate PreferredAppMode SetPreferredAppModeDelegate(PreferredAppMode appMode);
        internal delegate bool AllowDarkModeForAppDelegate(bool bAllow);
        internal delegate bool FlushMenuThemesDelegate();

        [DllImport("Ntdll.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint RtlGetVersion(ref RTL_OSVERSIONINFOW lpVersionInformation);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct RTL_OSVERSIONINFOW
        {
            public uint dwOSVersionInfoSize;
            public uint dwMajorVersion;
            public uint dwMinorVersion;
            public uint dwBuildNumber;
            public uint dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
        }

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr HWND, string lpText, string lpCaption, uint Type);

        public const int MB_OK = 0x00000000;
        public const int MB_OKCANCEL = 0x00000001;
        public const int MB_ABORTRETRYIGNORE = 0x00000002;
        public const int MB_YESNOCANCEL = 0x00000003;
        public const int MB_YESNO = 0x00000004;
        public const int MB_RETRYCANCEL = 0x00000005;
        public const int MB_CANCELTRYCONTINUE = 0x00000006;

        public const int MB_ICONHAND = 0x00000010;
        public const int MB_ICONQUESTION = 0x00000020;
        public const int MB_ICONEXCLAMATION = 0x00000030;
        public const int MB_ICONASTERISK = 0x00000040;
        public const int MB_USERICON = 0x00000080;
        public const int MB_ICONWARNING = MB_ICONEXCLAMATION;
        public const int MB_ICONERROR = MB_ICONHAND;
        public const int MB_ICONINFORMATION = MB_ICONASTERISK;
        public const int MB_ICONSTOP = MB_ICONHAND;

        public const int MB_DEFBUTTON1 = 0x00000000;
        public const int MB_DEFBUTTON2 = 0x00000100;
        public const int MB_DEFBUTTON3 = 0x00000200;
        public const int MB_DEFBUTTON4 = 0x00000300;

        public const int MB_APPLMODAL = 0x00000000;
        public const int MB_SYSTEMMODAL = 0x00001000;
        public const int MB_TASKMODAL = 0x00002000;
        public const int MB_HELP = 0x00004000; // Help Button
        public const int MB_NOFOCUS = 0x00008000;
        public const int MB_SETFOREGROUND = 0x00010000;
        public const int MB_DEFAULT_DESKTOP_ONLY = 0x00020000;
        public const int MB_TOPMOST = 0x00040000;
        public const int MB_RIGHT = 0x00080000;
        public const int MB_RTLREADING = 0x00100000;
        public const int MB_SERVICE_NOTIFICATION = 0x00200000;
        public const int MB_SERVICE_NOTIFICATION_NT3X = 0x00040000;

        public const int MB_TYPEMASK = 0x0000000F;
        public const int MB_ICONMASK = 0x000000F0;
        public const int MB_DEFMASK = 0x00000F00;
        public const int MB_MODEMASK = 0x00003000;
        public const int MB_MISCMASK = 0x0000C000;

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadImage(IntPtr hInst, string lpszName, UInt32 uType, int cxDesired, int cyDesired, UInt32 fuLoad);

        public const int IMAGE_BITMAP = 0;
        public const int IMAGE_ICON = 1;
        public const int IMAGE_CURSOR = 2;
        public const int IMAGE_ENHMETAFILE = 3;

        public const int LR_DEFAULTCOLOR = 0x00000000;
        public const int LR_MONOCHROME = 0x00000001;
        public const int LR_COLOR = 0x00000002;
        public const int LR_COPYRETURNORG = 0x00000004;
        public const int LR_COPYDELETEORG = 0x00000008;
        public const int LR_LOADFROMFILE = 0x00000010;

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("UxTheme.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern HRESULT DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pRect, IntPtr pClipRect);
      
        [DllImport("UxTheme.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr OpenThemeData(IntPtr hwnd, string pszClassList);

        [DllImport("UxTheme.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern HRESULT CloseThemeData(IntPtr hTheme);

        public const int ODS_SELECTED = 0x0001;
        public const int ODS_GRAYED = 0x0002;
        public const int ODS_DISABLED = 0x0004;
        public const int ODS_CHECKED = 0x0008;
        public const int ODS_FOCUS = 0x0010;
        public const int ODS_DEFAULT = 0x0020;
        public const int ODS_COMBOBOXEDIT = 0x1000;
        public const int ODS_HOTLIGHT = 0x0040;
        public const int ODS_INACTIVE = 0x0080;
        public const int ODS_NOACCEL = 0x0100;
        public const int ODS_NOFOCUSRECT = 0x0200;

        public enum MENUPARTS
        {
            MENU_MENUITEM_TMSCHEMA = 1,
            MENU_MENUDROPDOWN_TMSCHEMA = 2,
            MENU_MENUBARITEM_TMSCHEMA = 3,
            MENU_MENUBARDROPDOWN_TMSCHEMA = 4,
            MENU_CHEVRON_TMSCHEMA = 5,
            MENU_SEPARATOR_TMSCHEMA = 6,
            MENU_BARBACKGROUND = 7,
            MENU_BARITEM = 8,
            MENU_POPUPBACKGROUND = 9,
            MENU_POPUPBORDERS = 10,
            MENU_POPUPCHECK = 11,
            MENU_POPUPCHECKBACKGROUND = 12,
            MENU_POPUPGUTTER = 13,
            MENU_POPUPITEM = 14,
            MENU_POPUPSEPARATOR = 15,
            MENU_POPUPSUBMENU = 16,
            MENU_SYSTEMCLOSE = 17,
            MENU_SYSTEMMAXIMIZE = 18,
            MENU_SYSTEMMINIMIZE = 19,
            MENU_SYSTEMRESTORE = 20,
        }

        public enum POPUPITEMSTATES
        {
            MPI_NORMAL = 1,
            MPI_HOT = 2,
            MPI_DISABLED = 3,
            MPI_DISABLEDHOT = 4,
        }

        public enum POPUPSUBMENUSTATES
        {
            MSM_NORMAL = 1,
            MSM_DISABLED = 2,
        }

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool FillRect(IntPtr hdc, [In] ref RECT rect, IntPtr hbrush);    

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern int DrawText(IntPtr hdc, string lpStr, int nCount, ref RECT lpRect, int wFormat);

        public const int
          DT_TOP = 0x00000000,
          DT_LEFT = 0x00000000,
          DT_CENTER = 0x00000001,
          DT_RIGHT = 0x00000002,
          DT_VCENTER = 0x00000004,
          DT_BOTTOM = 0x00000008,
          DT_WORDBREAK = 0x00000010,
          DT_SINGLELINE = 0x00000020,
          DT_EXPANDTABS = 0x00000040,
          DT_TABSTOP = 0x00000080,
          DT_NOCLIP = 0x00000100,
          DT_EXTERNALLEADING = 0x00000200,
          DT_CALCRECT = 0x00000400,
          DT_NOPREFIX = 0x00000800,
          DT_INTERNAL = 0x00001000,
          DT_EDITCONTROL = 0x00002000,
          DT_PATH_ELLIPSIS = 0x00004000,
          DT_END_ELLIPSIS = 0x00008000,
          DT_MODIFYSTRING = 0x00010000,
          DT_RTLREADING = 0x00020000,
          DT_WORD_ELLIPSIS = 0x00040000,
          DT_NOFULLWIDTHCHARBREAK = 0x00080000,
          DT_HIDEPREFIX = 0x00100000,
          DT_PREFIXONLY = 0x00200000; 

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);    

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("Comctl32.dll", SetLastError = true)]
        public static extern IntPtr ImageList_Create(int cx, int cy, uint flags, int cInitial, int cGrow);

        [DllImport("Comctl32.dll", SetLastError = true)]
        public static extern int ImageList_AddMasked(IntPtr himl, IntPtr hbmImage, int crMask);

        [DllImport("Comctl32.dll", SetLastError = true)]
        public static extern bool ImageList_Destroy(IntPtr himl);

        [DllImport("Comctl32.dll", SetLastError = true)]
        public static extern bool ImageList_Draw(IntPtr himl, int i, IntPtr hdcDst, int x, int y, uint fStyle);

        public const int ILC_MASK = 0x00000001;
        public const int ILC_COLOR = 0x00000000;
        public const int ILC_COLORDDB = 0x000000FE;
        public const int ILC_COLOR4 = 0x00000004;
        public const int ILC_COLOR8 = 0x00000008;
        public const int ILC_COLOR16 = 0x00000010;
        public const int ILC_COLOR24 = 0x00000018;
        public const int ILC_COLOR32 = 0x00000020;
        public const int ILC_PALETTE = 0x00000800;     // (not implemented)

        public const int ILC_MIRROR = 0x00002000;      // Mirror the icons contained, if the process is mirrored
        public const int ILC_PERITEMMIRROR = 0x00008000;      // Causes the mirroring code to mirror each item when inserting a set of images, verses the whole strip

        public const int ILC_ORIGINALSIZE = 0x00010000;      // Imagelist should accept smaller than set images and apply OriginalSize based on image added
        public const int ILC_HIGHQUALITYSCALE = 0x00020000;     // Imagelist should enable use of the high quality scaler.

        public const int ILD_NORMAL = 0x00000000;
        public const int ILD_TRANSPARENT = 0x00000001;
        public const int ILD_MASK = 0x00000010;
        public const int ILD_IMAGE = 0x00000020;
        public const int ILD_ROP = 0x00000040;
        public const int ILD_BLEND25 = 0x00000002;
        public const int ILD_BLEND50 = 0x00000004;
        public const int ILD_OVERLAYMASK = 0x00000F00;
        //public const int INDEXTOOVERLAYMASK(i) = ((i) << 8);
        public const int ILD_PRESERVEALPHA = 0x00001000;  // This preserves the alpha channel in dest
        public const int ILD_SCALE = 0x00002000;  // Causes the image to be scaled to cx, cy instead of clipped
        public const int ILD_DPISCALE = 0x00004000;

        public const int ILD_ASYNC = 0x00008000;

        public const int ILD_SELECTED = ILD_BLEND50;
        public const int ILD_FOCUS = ILD_BLEND25;
        public const int ILD_BLEND = ILD_BLEND50;


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct ODM_DATA
        {
            public int nImageListIndex;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szItemText;
        }

        private IntPtr hWndMain = IntPtr.Zero;
        private Microsoft.UI.Windowing.AppWindow m_Apw;
        private IntPtr m_hIcon = IntPtr.Zero;
        private IntPtr m_hBalloonIcon = IntPtr.Zero;

        private SUBCLASSPROC SubClassDelegate;

        private void tsNotify_Toggled(object sender, RoutedEventArgs e)
        {

        }

        IntPtr m_initToken = IntPtr.Zero;
        IntPtr m_hBitmapBackground = IntPtr.Zero;
        IntPtr m_hBitmapImageList = IntPtr.Zero;
        IntPtr m_hBitmapBlueEye = IntPtr.Zero;
        IntPtr m_hBitmapClose = IntPtr.Zero;
        IntPtr m_hImageList = IntPtr.Zero;
        IntPtr m_hFontMenu = IntPtr.Zero;
        IntPtr m_hFontVertical = IntPtr.Zero;
        IntPtr m_hBitmapVertical = IntPtr.Zero;
        int m_SizeBitmap = 48;
        int m_WidthVerticalText = 60;

        public MainWindow()
        {
            this.InitializeComponent();

            hWndMain = WinRT.Interop.WindowNative.GetWindowHandle(this);
            Microsoft.UI.WindowId myWndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWndMain);
            m_Apw = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(myWndId);
            m_Apw.Resize(new Windows.Graphics.SizeInt32(440, 220));
            m_Apw.Move(new Windows.Graphics.PointInt32(700, 420));          

            StartupInput input = StartupInput.GetDefault();
            StartupOutput output;
            GpStatus nStatus = GdiplusStartup(out m_initToken, ref input, out output);

            Application.Current.Resources["ButtonBackgroundPressed"] = new SolidColorBrush(Microsoft.UI.Colors.LightSteelBlue);
            Application.Current.Resources["ButtonBackgroundPointerOver"] = new SolidColorBrush(Microsoft.UI.Colors.RoyalBlue);
            this.Closed += MainWindow_Closed;

            SubClassDelegate = new SUBCLASSPROC(WindowSubClass);
            bool bRet = SetWindowSubclass(hWndMain, SubClassDelegate, 0, 0);

            this.Title = "WinUI 3 - Test Shell_NotifyIcon";
            SetDarkMode();
            //string sDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //string sIconFile = sDirectory + @"Assets\Butterfly.ico";
            m_hIcon = LoadImage(IntPtr.Zero, @".\Assets\Butterfly.ico", IMAGE_ICON, 32, 32, LR_LOADFROMFILE);
            m_hBalloonIcon = LoadImage(IntPtr.Zero, @".\Assets\Butterfly.ico", IMAGE_ICON, 128, 128, LR_LOADFROMFILE);
            // Bad resizing if only 128 in .ico file
            // m_Apw.SetIcon(Microsoft.UI.Win32Interop.GetIconIdFromIcon(m_hIcon));
            m_Apw.SetIcon(@".\Assets\Butterfly.ico");

            IntPtr pImage = IntPtr.Zero;
            nStatus = GdipCreateBitmapFromFile(@".\Assets\pink-marble.jpg", out pImage);
            if (nStatus == GpStatus.Ok)
            {
                GdipCreateHBITMAPFromBitmap(pImage, out m_hBitmapBackground, RGB(Microsoft.UI.Colors.Black.R, Microsoft.UI.Colors.Black.G, Microsoft.UI.Colors.Black.B));
                GdipDisposeImage(pImage);
            }
            nStatus = GdipCreateBitmapFromFile(@".\Assets\Balls_ImageList.png", out pImage);
            if (nStatus == GpStatus.Ok)
            {
                GdipCreateHBITMAPFromBitmap(pImage, out m_hBitmapImageList, RGB(Microsoft.UI.Colors.White.R, Microsoft.UI.Colors.White.G, Microsoft.UI.Colors.White.B));
                GdipDisposeImage(pImage);
            }
            nStatus = GdipCreateBitmapFromFile(@".\Assets\Blue_eye_48.png", out pImage);
            if (nStatus == GpStatus.Ok)
            {
                GdipCreateHBITMAPFromBitmap(pImage, out m_hBitmapBlueEye, RGB(Microsoft.UI.Colors.White.R, Microsoft.UI.Colors.White.G, Microsoft.UI.Colors.White.B));
                GdipDisposeImage(pImage);
            }
            nStatus = GdipCreateBitmapFromFile(@".\Assets\Close_48.png", out pImage);
            if (nStatus == GpStatus.Ok)
            {
                GdipCreateHBITMAPFromBitmap(pImage, out m_hBitmapClose, RGB(Microsoft.UI.Colors.White.R, Microsoft.UI.Colors.White.G, Microsoft.UI.Colors.White.B));
                GdipDisposeImage(pImage);
            }

            m_hImageList = ImageList_Create(m_SizeBitmap, m_SizeBitmap, (uint)(ILC_COLOR32 | ILC_MASK), 1, 0);
            int nIndex = ImageList_AddMasked(m_hImageList, m_hBitmapBlueEye, ColorTranslator.ToWin32(System.Drawing.Color.White));
            nIndex = ImageList_AddMasked(m_hImageList, m_hBitmapImageList, ColorTranslator.ToWin32(System.Drawing.Color.White));
            nIndex = ImageList_AddMasked(m_hImageList, m_hBitmapClose, ColorTranslator.ToWin32(System.Drawing.Color.White));
 
            m_hFontMenu = CreateFont(26, 0, 0, 0, FW_DONTCARE, 0, 0, 0, DEFAULT_CHARSET, OUT_RASTER_PRECIS, CLIP_DEFAULT_PRECIS,
                ANTIALIASED_QUALITY, VARIABLE_PITCH | FF_DONTCARE, "Arial");
            m_hFontVertical = CreateFont(36, 0, 0, 0, FW_BOLD, 0, 0, 0, DEFAULT_CHARSET, OUT_RASTER_PRECIS, CLIP_DEFAULT_PRECIS,
                ANTIALIASED_QUALITY, DEFAULT_PITCH, "Arial");            
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            if (TrayMessage(hWndMain, "Right-Click for options", m_hIcon, IntPtr.Zero, NIM.ADD, NIIF.NONE, null, null, 0))
            {
                if (tsNotify.IsOn)
                    TrayMessage(hWndMain, null, m_hIcon, m_hBalloonIcon, NIM.MODIFY, NIIF.USER | NIIF.LARGE_ICON, "Message from WinUI 3", "Information", 0);
                m_Apw.Hide();
            }
        }

        private void SetDarkMode()
        {
            IntPtr pDll = LoadLibrary("UXTheme.dll");
            RTL_OSVERSIONINFOW ovi = new RTL_OSVERSIONINFOW { dwOSVersionInfoSize = (uint)Marshal.SizeOf(typeof(RTL_OSVERSIONINFOW)) };
            uint ntStatus = RtlGetVersion(ref ovi);
            if (ovi.dwMajorVersion >= 10 && ovi.dwBuildNumber >= 18362)
            {
                IntPtr pFunc = GetProcAddressByOrdinal(pDll, 135);
                if (pFunc != IntPtr.Zero)
                {
                    SetPreferredAppModeDelegate pSetPreferredAppMode = (SetPreferredAppModeDelegate)System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(pFunc, typeof(SetPreferredAppModeDelegate));
                    if (pSetPreferredAppMode != null)
                    {
                        try
                        {
                            PreferredAppMode nMode = pSetPreferredAppMode(PreferredAppMode.AllowDark);
                        }
                        catch
                        {
                            // error
                        }
                    }
                }
            }
            else
            {
                IntPtr pFunc = GetProcAddressByOrdinal(pDll, 135);
                if (pFunc != IntPtr.Zero)
                {
                    AllowDarkModeForAppDelegate pAllowDarkModeForApp = (AllowDarkModeForAppDelegate)System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(pFunc, typeof(AllowDarkModeForAppDelegate));
                    if (pAllowDarkModeForApp != null)
                    {
                        try
                        {
                            bool bRet = pAllowDarkModeForApp(true);
                        }
                        catch
                        {
                            // error
                        }
                    }
                }
            }
            {
                IntPtr pFunc = GetProcAddressByOrdinal(pDll, 136);
                if (pFunc != IntPtr.Zero)
                {
                    FlushMenuThemesDelegate pFlushMenuThemes = (FlushMenuThemesDelegate)System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(pFunc, typeof(FlushMenuThemesDelegate));
                    if (pFlushMenuThemes != null)
                    {
                        try
                        {
                            bool bRet = pFlushMenuThemes();
                        }
                        catch
                        {
                            // error
                        }
                    }
                }
            }
        }

        RECT rcMenu;
        IntPtr hBitmap = IntPtr.Zero;
        IntPtr hDCMem = IntPtr.Zero;
        IntPtr hBitmapOld = IntPtr.Zero;
  
        private int WindowSubClass(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam, IntPtr uIdSubclass, uint dwRefData)
        {          
            switch (uMsg)
            {
                case WM_TRAYMOUSEMESSAGE:
                    {
                        switch (LOWORD((int)lParam))
                        {
                            case WM_CONTEXTMENU:
                            case WM_RBUTTONUP:
                                {
                                    Windows.Graphics.PointInt32 ptCursor;
                                    GetCursorPos(out ptCursor);

                                    rcMenu = new RECT();

                                    IntPtr hMenu = CreatePopupMenu();
                                    int i = 1;
                                    AppendMenu(hMenu, MF_STRING, (IntPtr)i, "Show window");                                   
                                    for (i = 2; i <= 6; i++)
                                    {
                                        AppendMenu(hMenu, MF_STRING, (IntPtr)i, "Menu Item " + i.ToString());
                                    }
                                    i += 1;
                                    AppendMenu(hMenu, MF_SEPARATOR, (IntPtr)i, null);
                                    i += 1;
                                    AppendMenu(hMenu, MF_STRING, (IntPtr)i, "Exit");
                                    if ((bool)rbOD.IsChecked)
                                    {
                                        ODM_DATA odmd = new ODM_DATA();
                                        IntPtr pODMD = IntPtr.Zero;

                                        i = 1;
                                        pODMD = Marshal.AllocHGlobal(Marshal.SizeOf(odmd));
                                        odmd.szItemText = "Show window";
                                        odmd.nImageListIndex = i-1;
                                        Marshal.StructureToPtr(odmd, pODMD, false);
                                        ModifyMenu(hMenu, (uint)i, MF_BYCOMMAND | MF_OWNERDRAW, (IntPtr)i, pODMD);

                                        for (i = 2; i <= 6; i++)
                                        {                                           
                                            pODMD = Marshal.AllocHGlobal(Marshal.SizeOf(odmd));
                                            odmd.szItemText = "Menu Item " + i.ToString();
                                            odmd.nImageListIndex = i-1;
                                            Marshal.StructureToPtr(odmd, pODMD, false);
                                            ModifyMenu(hMenu, (uint)i, MF_BYCOMMAND | MF_OWNERDRAW, (IntPtr)(i), pODMD);
                                            //Marshal.FreeHGlobal(pODMD);
                                        }
                                        i += 1;
                                        pODMD = Marshal.AllocHGlobal(Marshal.SizeOf(odmd));
                                        odmd.szItemText = "";
                                        odmd.nImageListIndex = -1;
                                        Marshal.StructureToPtr(odmd, pODMD, false);
                                        ModifyMenu(hMenu, (uint)i, MF_BYCOMMAND | MF_OWNERDRAW | MF_SEPARATOR, (IntPtr)i, pODMD);
                                        //Marshal.FreeHGlobal(pODMD);
                                        i += 1;
                                        pODMD = Marshal.AllocHGlobal(Marshal.SizeOf(odmd));
                                        odmd.szItemText = "Exit";
                                        odmd.nImageListIndex = i+5 -1;
                                        Marshal.StructureToPtr(odmd, pODMD, false);
                                        ModifyMenu(hMenu, (uint)i, MF_BYCOMMAND | MF_OWNERDRAW, (IntPtr)i, pODMD);
                                        //Marshal.FreeHGlobal(pODMD);
                                    }
                                    // https://web.archive.org/web/20121015064650/https://support.microsoft.com/kb/135788
                                    SetForegroundWindow(hWnd);                                  
                                    uint nCmd = TrackPopupMenu(hMenu, TPM_LEFTALIGN | TPM_LEFTBUTTON | TPM_RIGHTBUTTON | TPM_RETURNCMD, ptCursor.X, ptCursor.Y, 0, hWnd, IntPtr.Zero);
                                    PostMessage(hWnd, 0x0000, IntPtr.Zero, IntPtr.Zero);
                                    if (nCmd != 0)
                                    {
                                        if (nCmd == i)
                                        {
                                            TrayMessage(hWndMain, null, IntPtr.Zero, IntPtr.Zero, NIM.DELETE, NIIF.NONE, null, null, 0);
                                            this.Close();
                                        }
                                        else if (nCmd == 1)
                                        {
                                            TrayMessage(hWndMain, null, IntPtr.Zero, IntPtr.Zero, NIM.DELETE, NIIF.NONE, null, null, 0);
                                            m_Apw.Show();
                                        }
                                        else
                                        {
                                            string sText = "Selected Menu Item : " + nCmd.ToString();
                                            MessageBox(IntPtr.Zero, sText, "Information", MB_OK | MB_ICONINFORMATION);
                                        }
                                    }                                  
                                }
                                break;
                        }
                    }
                    break;
                case WM_DRAWITEM:
                    {                       
                        DRAWITEMSTRUCT dis = (DRAWITEMSTRUCT)Marshal.PtrToStructure(lParam, typeof(DRAWITEMSTRUCT));
                        ODM_DATA odmd = (ODM_DATA)Marshal.PtrToStructure(dis.itemData, typeof(ODM_DATA));

                        uint nBackColor, nTextColor;

                        if (rcMenu.right == 0)
                        {
                            rcMenu.right = (int)dis.rcItem.right;
                            hDCMem = CreateCompatibleDC(IntPtr.Zero);
                            hBitmap = CreateCompatibleBitmap(dis.hDC, rcMenu.right - rcMenu.left, rcMenu.bottom - rcMenu.top);
                            hBitmapOld = SelectObject(hDCMem, hBitmap);
                            //Ellipse(hDCMem, 20, 20, rcMenu.right - 20, rcMenu.bottom - 20);
                            //BitBlt(GetDC(IntPtr.Zero), 0, 0, rcMenu.right - rcMenu.left, rcMenu.bottom - rcMenu.top, hDCMem, 0, 0, SRCCOPY);

                            IntPtr hDCMem2 = CreateCompatibleDC(IntPtr.Zero);
                            IntPtr hBitmapOld2 = SelectObject(hDCMem2, m_hBitmapBackground);
                            BITMAP bm;
                            GetObject(m_hBitmapBackground, Marshal.SizeOf(typeof(BITMAP)), out bm);
                            int n = SetStretchBltMode(hDCMem, HALFTONE);
                            StretchBlt(hDCMem, 0, 0, rcMenu.right - rcMenu.left, rcMenu.bottom - rcMenu.top, hDCMem2, 0, 0, bm.bmWidth, bm.bmHeight, SRCCOPY);                            

                            SelectObject(hDCMem2, hBitmapOld2);
                            //DeleteObject(m_hBitmap);
                            DeleteDC(hDCMem2);

                            IntPtr hDCMemHorizontal = CreateCompatibleDC(IntPtr.Zero);
                            RECT rcHorizontal = new RECT(0, 0, rcMenu.bottom - rcMenu.top, m_WidthVerticalText);
                            IntPtr hBitmapHorizontal  = CreateCompatibleBitmap(dis.hDC, rcHorizontal.right - rcHorizontal.left, rcHorizontal.bottom - rcHorizontal.top);
                            IntPtr hBitmapHorizontalOld = SelectObject(hDCMemHorizontal, hBitmapHorizontal);
                            Fill(hDCMemHorizontal, rcHorizontal, Color.DarkBlue, Color.Red, FillDirection.LeftToRight);
                            string sText = " WinUI 3 Notify Icon";
                            int nOldModeVertical = SetBkMode(hDCMemHorizontal, TRANSPARENT);
                            uint nTextColorVertical = SetTextColor(hDCMemHorizontal, (uint)System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.Yellow));
                            IntPtr hFontOldVertical = IntPtr.Zero;
                            if (m_hFontVertical != IntPtr.Zero)
                                hFontOldVertical = SelectObject(hDCMemHorizontal, m_hFontVertical);
                            DrawText(hDCMemHorizontal, sText, -1, ref rcHorizontal, DT_SINGLELINE | DT_VCENTER);
                            SetTextColor(hDCMemHorizontal, nTextColorVertical);
                            SetBkMode(hDCMemHorizontal, nOldModeVertical);
                            if (m_hFontVertical != IntPtr.Zero)
                                SelectObject(hDCMemHorizontal, hFontOldVertical);
                            SelectObject(hDCMemHorizontal, hBitmapHorizontalOld);
                            DeleteDC(hDCMemHorizontal);

                            m_hBitmapVertical = RotateBitmap(hBitmapHorizontal, 90, (uint)System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.Black));
                            DeleteObject(hBitmapHorizontal);                           
                        }
                        if (m_hBitmapVertical != IntPtr.Zero)
                        {
                            BITMAP bmVertical;
                            GetObject(m_hBitmapVertical, Marshal.SizeOf(typeof(BITMAP)), out bmVertical);
                            IntPtr hDCMemVertical = CreateCompatibleDC(IntPtr.Zero);
                            IntPtr hBitmapVerticalOld = SelectObject(hDCMemVertical, m_hBitmapVertical);
                            BitBlt(dis.hDC, 0, 0, bmVertical.bmWidth, bmVertical.bmHeight, hDCMemVertical, 0, 0, SRCCOPY);
                            SelectObject(hDCMemVertical, hBitmapVerticalOld);
                            DeleteDC(hDCMemVertical);
                        }

                        //if (dis.itemID == 20)
                        //{
                        //    IntPtr hWndMenu = FindWindow("#32768", null);
                        //    if (hWndMenu != IntPtr.Zero)
                        //    {
                        //        GetClientRect(hWndMenu, out rcMenu);
                        //    }
                        //}

                        if ((dis.itemState & ODS_SELECTED) != 0)
                        {
                            //IntPtr hMenuTheme = OpenThemeData(IntPtr.Zero, "MENU");
                            //HRESULT hr = DrawThemeBackground(hMenuTheme, dis.hDC, (int)MENUPARTS.MENU_POPUPITEM, (int)POPUPITEMSTATES.MPI_HOT, ref dis.rcItem, IntPtr.Zero);
                            //CloseThemeData(hMenuTheme);

                            nBackColor = SetBkColor(dis.hDC, (uint)System.Drawing.ColorTranslator.ToWin32(System.Drawing.SystemColors.MenuHighlight));
                            if ((dis.itemState & ODS_DISABLED) != 0)
                                nTextColor = SetTextColor(dis.hDC, (uint)System.Drawing.ColorTranslator.ToWin32(System.Drawing.SystemColors.GrayText));
                            else
                                nTextColor = SetTextColor(dis.hDC, (uint)System.Drawing.ColorTranslator.ToWin32(System.Drawing.SystemColors.HighlightText));
 
                            RECT rcBack = dis.rcItem;
                            rcBack.left +=  m_WidthVerticalText;
                            ExtTextOut(dis.hDC, rcBack.left, dis.rcItem.top, ETO_OPAQUE, ref rcBack, null, 0, IntPtr.Zero);
                        }
                        else
                        {
                            nBackColor = SetBkColor(dis.hDC, (uint)System.Drawing.ColorTranslator.ToWin32(System.Drawing.SystemColors.Menu));
                            if ((dis.itemState & ODS_DISABLED) != 0)
                                nTextColor = SetTextColor(dis.hDC, (uint)System.Drawing.ColorTranslator.ToWin32(System.Drawing.SystemColors.GrayText));
                            else
                                nTextColor = SetTextColor(dis.hDC, (uint)System.Drawing.ColorTranslator.ToWin32(System.Drawing.SystemColors.WindowText));

                            BitBlt(dis.hDC, dis.rcItem.left + m_WidthVerticalText, dis.rcItem.top, dis.rcItem.right - dis.rcItem.left, dis.rcItem.bottom - dis.rcItem.top, hDCMem, dis.rcItem.left, dis.rcItem.top, SRCCOPY);
                        }

                        ImageList_Draw(m_hImageList, odmd.nImageListIndex, dis.hDC, dis.rcItem.left + m_WidthVerticalText, dis.rcItem.top, ILD_TRANSPARENT);

                        IntPtr hFontOld = IntPtr.Zero;
                        if (m_hFontMenu != IntPtr.Zero)
                             hFontOld = SelectObject(dis.hDC, m_hFontMenu);
                        int nOldMode = SetBkMode(dis.hDC, TRANSPARENT);
                        if (odmd.szItemText != "")
                        {
                            RECT rcText = dis.rcItem;
                            rcText.left += m_SizeBitmap + 8 + m_WidthVerticalText;
                            DrawText(dis.hDC, odmd.szItemText, -1, ref rcText, DT_SINGLELINE | DT_VCENTER);
                        }
                        else
                        {
                            IntPtr hPen = CreatePen(PS_SOLID, 2, System.Drawing.ColorTranslator.ToWin32(System.Drawing.SystemColors.MenuHighlight));
                            IntPtr hPenOld = SelectObject(dis.hDC, hPen);
                            MoveToEx(dis.hDC, dis.rcItem.left + m_WidthVerticalText, dis.rcItem.top + (dis.rcItem.bottom - dis.rcItem.top) / 2, out POINT pt);
                            LineTo(dis.hDC, dis.rcItem.right, dis.rcItem.top + (dis.rcItem.bottom - dis.rcItem.top) / 2);
                            SelectObject(dis.hDC, hPenOld);
                            DeleteObject(hPen);
                        }
                        if (m_hFontMenu != IntPtr.Zero)
                            SelectObject(dis.hDC, hFontOld);

                        SetTextColor(dis.hDC, nTextColor);
                        SetBkColor(dis.hDC, nBackColor);
                        SetBkMode(dis.hDC, nOldMode);
                        return 1;
                    }
                    break;
                case WM_MEASUREITEM:
                    {                       
                        MEASUREITEMSTRUCT mis = (MEASUREITEMSTRUCT)Marshal.PtrToStructure(lParam, typeof(MEASUREITEMSTRUCT));
                        ODM_DATA odmd = (ODM_DATA)Marshal.PtrToStructure(mis.itemData, typeof(ODM_DATA));
                        mis.itemWidth = 260 + (uint)m_WidthVerticalText;
                        if (odmd.szItemText == "")
                            mis.itemHeight = 2;
                        else
                            mis.itemHeight = (uint)m_SizeBitmap;
                        Marshal.StructureToPtr(mis, lParam, false);

                        //rcMenu.right = (int)mis.itemWidth;
                        rcMenu.bottom += (int)mis.itemHeight;

                        return 1;
                    }
                    break;
                case WM_EXITMENULOOP:
                    {
                        if (hBitmap != IntPtr.Zero)
                        {
                            SelectObject(hDCMem, hBitmapOld);
                            DeleteObject(hBitmap);
                            DeleteDC(hDCMem);
                        }
                    }
                    break;
            }
            return DefSubclassProc(hWnd, uMsg, wParam, lParam);
        }

        private IntPtr RotateBitmap(IntPtr hBitmap, float nDegrees, uint nColorBackground)
        {
            float nRadians = (float)(Math.PI * nDegrees / 180.0);
            float nSinus = (float)Math.Sin(nRadians);
            float nCosinus = (float)Math.Cos(nRadians);

            IntPtr hDCSource = CreateCompatibleDC(IntPtr.Zero);
            IntPtr hDCDest = CreateCompatibleDC(IntPtr.Zero);
            BITMAP bm;
            GetObject(hBitmap, Marshal.SizeOf(typeof(BITMAP)), out bm);

            int nX1 = (int)(bm.bmHeight * nSinus);
            int nY1 = (int)(bm.bmHeight * nCosinus);
            int nX2 = (int)(bm.bmWidth * nCosinus + bm.bmHeight * nSinus);
            int nY2 = (int)(bm.bmHeight * nCosinus - bm.bmWidth * nSinus);
            int nX3 = (int)(bm.bmWidth * nCosinus);
            int nY3 = (int)(-bm.bmWidth * nSinus);
            int nMinX = Math.Min(0, Math.Min(nX1, Math.Min(nX2, nX3)));
            int nMinY = Math.Min(0, Math.Min(nY1, Math.Min(nY2, nY3)));
            int nMaxX = Math.Max(0, Math.Max(nX1, Math.Max(nX2, nX3)));
            int nMaxY = Math.Max(0, Math.Max(nY1, Math.Max(nY2, nY3)));
            int nWidth = nMaxX - nMinX;
            int hHeight = nMaxY - nMinY;

            IntPtr hDC = GetDC(IntPtr.Zero);
            IntPtr hBitmapNew = CreateCompatibleBitmap(hDC, nWidth, hHeight);
            IntPtr hBitmapSourceOld = SelectObject(hDCSource, hBitmap);
            IntPtr hBitmapDestOld = SelectObject(hDCDest, hBitmapNew);

            IntPtr hBrush = CreateSolidBrush(nColorBackground);
            IntPtr hBrushOld = SelectObject(hDCDest, hBrush);
            PatBlt(hDCDest, 0, 0, nWidth, hHeight, PATCOPY);
            DeleteObject(SelectObject(hDCDest, hBrushOld));

            SetGraphicsMode(hDCDest, GM_ADVANCED);
            XFORM xForm = new XFORM();
            xForm.eM11 = nCosinus;
            xForm.eM12 = -nSinus;
            xForm.eM21 = nSinus;
            xForm.eM22 = nCosinus;
            xForm.eDx = (float)-nMinX;
            xForm.eDy = (float)-nMinY;
            SetWorldTransform(hDCDest, ref xForm);

            BitBlt(hDCDest, 0, 0, bm.bmWidth, bm.bmHeight, hDCSource, 0, 0, SRCCOPY);

            SelectObject(hDCSource, hBitmapSourceOld);
            DeleteDC(hDCSource);
            SelectObject(hDCDest, hBitmapDestOld);
            DeleteDC(hDCDest);
            ReleaseDC(IntPtr.Zero, hDC);

            return hBitmapNew;
        }

        private void rbContextMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        void Clean()
        {
            if (m_hBitmapBackground != IntPtr.Zero)
                DeleteObject(m_hBitmapBackground);
            if (m_hBitmapImageList != IntPtr.Zero)
                DeleteObject(m_hBitmapImageList);
            if (m_hBitmapBlueEye != IntPtr.Zero)
                DeleteObject(m_hBitmapBlueEye);
            if (m_hBitmapClose != IntPtr.Zero)
                DeleteObject(m_hBitmapClose);
            if (m_hFontMenu != IntPtr.Zero)
                DeleteObject(m_hFontMenu);
            if (m_hFontVertical != IntPtr.Zero)
                DeleteObject(m_hFontVertical);
            if (m_hBitmapVertical != IntPtr.Zero)
                DeleteObject(m_hBitmapVertical);

            if (m_hIcon != IntPtr.Zero)
                DestroyIcon(m_hIcon);
            if (m_hBalloonIcon != IntPtr.Zero)
                DestroyIcon(m_hBalloonIcon);            

            if (m_hImageList != IntPtr.Zero)
                ImageList_Destroy(m_hImageList);

            GdiplusShutdown(m_initToken);
        }

        private void MainWindow_Closed(object sender, WindowEventArgs args)
        {
            Clean();
        }
    }
}
