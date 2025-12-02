using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ModeLauncher.Services
{
    public static class IconService
    {
        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SHGetFileInfo(string pszPath,
            uint dwFileAttributes,
            ref SHFILEINFO psfi,
            uint cbFileInfo,
            uint uFlags);

        private const uint SHGFI_ICON = 0x000000100;
        private const uint SHGFI_LARGEICON = 0x000000000;  // Large icon
        private const uint SHGFI_SMALLICON = 0x000000001;  // Small icon

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        public static ImageSource? FromExe(string exePath)
        {
            try
            {
                if (!File.Exists(exePath))
                    return null;

                SHFILEINFO shinfo = new SHFILEINFO();

                IntPtr hImg = SHGetFileInfo(
                    exePath,
                    0,
                    ref shinfo,
                    (uint)Marshal.SizeOf(shinfo),
                    SHGFI_ICON | SHGFI_LARGEICON);

                if (shinfo.hIcon == IntPtr.Zero)
                    return null;

                // Convert HICON to WPF ImageSource
                var img = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                    shinfo.hIcon,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

                // free icon handle
                DestroyIcon(shinfo.hIcon);

                return img;
            }
            catch
            {
                return null;
            }
        }

        [DllImport("User32.dll")]
        private static extern bool DestroyIcon(IntPtr hIcon);
    }
}
