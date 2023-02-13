using System;
using System.Runtime.InteropServices;
using GlobalStructures;

namespace GDI
{
    internal class GDITools
    {
        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateSolidBrush(uint crColor);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool DeleteObject(IntPtr ho);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint GetBkColor(IntPtr hdc);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint SetBkColor(IntPtr hdc, uint color);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint SetTextColor(IntPtr hdc, uint color);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int SetBkMode(IntPtr hdc, int mode);

        public const int TRANSPARENT = 1;
        public const int OPAQUE = 2;
        public const int BKMODE_LAST = 2;

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool ExtTextOut(IntPtr hdc, int x, int y, uint options, ref RECT lprect, string lpString, uint c, IntPtr lpDx);

        public const int ETO_OPAQUE = 0x0002;
        public const int ETO_CLIPPED = 0x0004;
        public const int ETO_GLYPH_INDEX = 0x0010;
        public const int ETO_RTLREADING = 0x0080;
        public const int ETO_NUMERICSLOCAL = 0x0400;
        public const int ETO_NUMERICSLATIN = 0x0800;
        public const int ETO_IGNORELANGUAGE = 0x1000;
        public const int ETO_PDY = 0x2000;
        public const int ETO_REVERSE_INDEX_MAP = 0x10000;

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool TextOut(IntPtr hdc, int x, int y, string lpString, int c);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool MoveToEx(IntPtr hdc, int x, int y, out POINT lppt);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LineTo(IntPtr hdc, int x, int y);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr CreatePen(int iStyle, int cWidth, int color);

        public const int PS_SOLID = 0;
        public const int PS_DASH = 1;       /* -------  */
        public const int PS_DOT = 2;       /* .......  */
        public const int PS_DASHDOT = 3;       /* _._._._  */
        public const int PS_DASHDOTDOT = 4;       /* _.._.._  */
        public const int PS_NULL = 5;
        public const int PS_INSIDEFRAME = 6;
        public const int PS_USERSTYLE = 7;
        public const int PS_ALTERNATE = 8;
        public const int PS_STYLE_MASK = 0x0000000F;

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool DeleteDC(IntPtr hDC);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int SetStretchBltMode(IntPtr hdc, int mode);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool StretchBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSrc, int xSrc, int ySrc, int wSrc, int hSrc, int rop);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool PatBlt(IntPtr hdc, int x, int y, int w, int h, uint rop);

        public const int BLACKONWHITE = 1;
        public const int WHITEONBLACK = 2;
        public const int COLORONCOLOR = 3;
        public const int HALFTONE = 4;
        public const int MAXSTRETCHBLTMODE = 4;

        public const int SRCCOPY = 0x00CC0020; /* dest = source                   */
        public const int SRCPAINT = 0x00EE0086; /* dest = source OR dest           */
        public const int SRCAND = 0x008800C6; /* dest = source AND dest          */
        public const int SRCINVERT = 0x00660046; /* dest = source XOR dest          */
        public const int SRCERASE = 0x00440328; /* dest = source AND (NOT dest )   */
        public const int NOTSRCCOPY = 0x00330008; /* dest = (NOT source)             */
        public const int NOTSRCERASE = 0x001100A6; /* dest = (NOT src) AND (NOT dest) */
        public const int MERGECOPY = 0x00C000CA; /* dest = (source AND pattern)     */
        public const int MERGEPAINT = 0x00BB0226; /* dest = (NOT source) OR dest     */
        public const int PATCOPY = 0x00F00021; /* dest = pattern                  */
        public const int PATPAINT = 0x00FB0A09; /* dest = DPSnoo                   */
        public const int PATINVERT = 0x005A0049; /* dest = pattern XOR dest         */
        public const int DSTINVERT = 0x00550009; /* dest = (NOT dest)               */
        public const int BLACKNESS = 0x00000042; /* dest = BLACK                    */
        public const int WHITENESS = 0x00FF0062; /* dest = WHITE                    */
        public const uint NOMIRRORBITMAP = 0x80000000; /* Do not Mirror the bitmap in this call */
        public const uint CAPTUREBLT = 0x40000000; /* Include layered windows */

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool Ellipse(IntPtr hdc, int left, int top, int right, int bottom);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int GetObject(IntPtr hFont, int nSize, out BITMAP bm);

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct BITMAP
        {
            public int bmType;
            public int bmWidth;
            public int bmHeight;
            public int bmWidthBytes;
            public short bmPlanes;
            public short bmBitsPixel;
            public IntPtr bmBits;
        }

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateFont(int cHeight, int cWidth, int cEscapement, int cOrientation, int cWeight, uint bItalic, uint bUnderline,
            uint bStrikeOut, uint iCharSet, uint iOutPrecision, uint iClipPrecision, uint iQuality, uint iPitchAndFamily, string pszFaceName);

        /* Font Weights */
        public const int FW_DONTCARE = 0;
        public const int FW_THIN = 100;
        public const int FW_EXTRALIGHT = 200;
        public const int FW_LIGHT = 300;
        public const int FW_NORMAL = 400;
        public const int FW_MEDIUM = 500;
        public const int FW_SEMIBOLD = 600;
        public const int FW_BOLD = 700;
        public const int FW_EXTRABOLD = 800;
        public const int FW_HEAVY = 900;

        public const int DEFAULT_QUALITY = 0;
        public const int DRAFT_QUALITY = 1;
        public const int PROOF_QUALITY = 2;
        public const int NONANTIALIASED_QUALITY = 3;
        public const int ANTIALIASED_QUALITY = 4;
        public const int CLEARTYPE_QUALITY = 5;
        public const int CLEARTYPE_NATURAL_QUALITY = 6;

        public const int DEFAULT_PITCH = 0;
        public const int FIXED_PITCH = 1;
        public const int VARIABLE_PITCH = 2;
        public const int MONO_FONT = 8;

        public const int OUT_DEFAULT_PRECIS = 0;
        public const int OUT_STRING_PRECIS = 1;
        public const int OUT_CHARACTER_PRECIS = 2;
        public const int OUT_STROKE_PRECIS = 3;
        public const int OUT_TT_PRECIS = 4;
        public const int OUT_DEVICE_PRECIS = 5;
        public const int OUT_RASTER_PRECIS = 6;
        public const int OUT_TT_ONLY_PRECIS = 7;
        public const int OUT_OUTLINE_PRECIS = 8;
        public const int OUT_SCREEN_OUTLINE_PRECIS = 9;
        public const int OUT_PS_ONLY_PRECIS = 10;

        public const int CLIP_DEFAULT_PRECIS = 0;
        public const int CLIP_CHARACTER_PRECIS = 1;
        public const int CLIP_STROKE_PRECIS = 2;
        public const int CLIP_MASK = 0xf;
        public const int CLIP_LH_ANGLES = (1 << 4);
        public const int CLIP_TT_ALWAYS = (2 << 4);
        public const int CLIP_DFA_DISABLE = (4 << 4);
        public const int CLIP_EMBEDDED = (8 << 4);

        public const int ANSI_CHARSET = 0;
        public const int DEFAULT_CHARSET = 1;
        public const int SYMBOL_CHARSET = 2;

        /* Font Families */
        public const int FF_DONTCARE = (0 << 4);  /* Don't care or don't know. */
        public const int FF_ROMAN = (1 << 4);  /* Variable stroke width, serifed. */
        /* Times Roman, Century Schoolbook, etc. */
        public const int FF_SWISS = (2 << 4);  /* Variable stroke width, sans-serifed. */
        /* Helvetica, Swiss, etc. */
        public const int FF_MODERN = (3 << 4);  /* Constant stroke width, serifed or sans-serifed. */
        /* Pica, Elite, Courier, etc. */
        public const int FF_SCRIPT = (4 << 4);  /* Cursive, etc. */
        public const int FF_DECORATIVE = (5 << 4);  /* Old English, etc. */

        // GDI Gradient (from MSDN)
        public struct TRIVERTEX
        {
            public int x;
            public int y;
            public ushort Red;
            public ushort Green;
            public ushort Blue;
            public ushort Alpha;
            public TRIVERTEX(int x, int y, System.Drawing.Color color)
                : this(x, y, color.R, color.G, color.B, color.A)
            {
            }
            public TRIVERTEX(
                int x, int y,
                ushort red, ushort green, ushort blue,
                ushort alpha)
            {
                this.x = x;
                this.y = y;
                this.Red = (ushort)(red << 8);
                this.Green = (ushort)(green << 8);
                this.Blue = (ushort)(blue << 8);
                this.Alpha = (ushort)(alpha << 8);
            }
        }
        public struct GRADIENT_RECT
        {
            public uint UpperLeft;
            public uint LowerRight;
            public GRADIENT_RECT(uint ul, uint lr)
            {
                this.UpperLeft = ul;
                this.LowerRight = lr;
            }
        }
        public struct GRADIENT_TRIANGLE
        {
            public uint Vertex1;
            public uint Vertex2;
            public uint Vertex3;
            public GRADIENT_TRIANGLE(uint v1, uint v2, uint v3)
            {
                this.Vertex1 = v1;
                this.Vertex2 = v2;
                this.Vertex3 = v3;
            }
        }

        [DllImport("Msimg32.dll", SetLastError = true, EntryPoint = "GradientFill")]
        public extern static bool GradientFill(IntPtr hdc, TRIVERTEX[] pVertex, uint dwNumVertex, GRADIENT_RECT[] pMesh, uint dwNumMesh, uint dwMode);

        public const int GRADIENT_FILL_RECT_H = 0x00000000;
        public const int GRADIENT_FILL_RECT_V = 0x00000001;

        // Not supported on Windows CE:
        public const int GRADIENT_FILL_TRIANGLE = 0x00000002;

        public enum FillDirection
        {
            LeftToRight = GRADIENT_FILL_RECT_H,
            TopToBottom = GRADIENT_FILL_RECT_V
        }

        public static bool Fill(IntPtr hDC, RECT rc, System.Drawing.Color startColor, System.Drawing.Color endColor, FillDirection fillDir)
        {
            TRIVERTEX[] tva = new TRIVERTEX[2];
            tva[0] = new TRIVERTEX(rc.left, rc.top, startColor);
            tva[1] = new TRIVERTEX(rc.right, rc.bottom, endColor);
            GRADIENT_RECT[] gra = new GRADIENT_RECT[] { new GRADIENT_RECT(0, 1) };
            bool b = GradientFill(hDC, tva, (uint)tva.Length, gra, (uint)gra.Length, (uint)fillDir);         
            return b;
        }

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int GetTextExtentPoint32(IntPtr hdc, string lpString, int c, out System.Drawing.Size psizl);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool SetViewportOrgEx(IntPtr hdc, int x, int y, IntPtr lppt);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool SetWindowExtEx(IntPtr hdc, int x, int y, IntPtr lpsz);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool SetViewportExtEx(IntPtr hdc, int x, int y, IntPtr lpsz);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint SetTextAlign(IntPtr hdc, uint align);

        public const int TA_NOUPDATECP = 0;
        public const int TA_UPDATECP = 1;

        public const int TA_LEFT = 0;
        public const int TA_RIGHT = 2;
        public const int TA_CENTER = 6;

        public const int TA_TOP = 0;
        public const int TA_BOTTOM = 8;
        public const int TA_BASELINE = 24;
        public const int TA_RTLREADING = 256;
        public const int TA_MASK = (TA_BASELINE + TA_CENTER + TA_UPDATECP + TA_RTLREADING);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool SetWorldTransform(IntPtr hdc, ref XFORM lpxf);

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool ModifyWorldTransform(IntPtr hdc, ref XFORM lpxf, uint mode);

        [StructLayout(LayoutKind.Sequential)]
        public struct XFORM
        {
            public float eM11;
            public float eM12;
            public float eM21;
            public float eM22;
            public float eDx;
            public float eDy;
        }

        public enum MODIFYWORLDTRANSFORM
        {
            MWT_IDENTITY = 0x01,
            MWT_LEFTMULTIPLY = 0x02,
            MWT_RIGHTMULTIPLY = 0x03,
            MWT_SET = 0x04
        }

        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int SetGraphicsMode(IntPtr hdc, int iMode);

        public const int GM_COMPATIBLE = 1;
        public const int GM_ADVANCED = 2;
    } 
}
