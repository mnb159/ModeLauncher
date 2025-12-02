using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ModeLauncher.Services
{
    public class IconService
    {
        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SHGetFileInfo(
            string pszPath,
            uint dwFileAttributes,
            ref SHFILEINFO psfi,
            uint cbFileInfo,
            uint uFlags);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x0; // large icon

        [DllImport("User32.dll", SetLastError = true)]
        private static extern bool DestroyIcon(IntPtr hIcon);

        public ImageSource? GetIcon(string? exePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(exePath))
                    return null;

                exePath = exePath.Trim('"', ' ');

                if (!File.Exists(exePath))
                    return null;

                var info = new SHFILEINFO();
                IntPtr result = SHGetFileInfo(
                    exePath,
                    0,
                    ref info,
                    (uint)Marshal.SizeOf(info),
                    SHGFI_ICON | SHGFI_LARGEICON);

                if (result == IntPtr.Zero || info.hIcon == IntPtr.Zero)
                    return null;

                var src = Imaging.CreateBitmapSourceFromHIcon(
                    info.hIcon,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(64, 64));

                src.Freeze();
                DestroyIcon(info.hIcon);

                return src;
            }
            catch
            {
                return null;
            }
        }
    }
}
