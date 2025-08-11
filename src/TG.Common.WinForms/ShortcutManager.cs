using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace TG.Common
{
    /// <summary>
    /// Provides helper methods for creating Windows shortcuts (.lnk) using Shell COM interop.
    /// Works for .NET Framework (4.7.2 / 4.8) and modern .NET (net6.0-windows+, net8.0-windows).
    /// </summary>
    public static class ShortcutManager
    {
        /// <summary>
        /// Creates a Windows shortcut (.lnk).
        /// </summary>
        /// <param name="path">Full path (or filename) to the shortcut to create. “.lnk” is appended if missing.</param>
        /// <param name="target">Executable or file the shortcut launches.</param>
        /// <param name="arguments">Optional command line arguments.</param>
        /// <param name="description">Optional description (tooltip).</param>
        /// <returns>true if created successfully; false otherwise.</returns>
        public static bool CreateShortcut(string path, string target, string arguments, string description)
            => CreateShortcutInternal(path, target, arguments, description, null, null, 0);

        /// <summary>
        /// Creates a Windows shortcut (.lnk) with an icon.
        /// </summary>
        public static bool CreateShortcut(string path, string target, string arguments, string description, string iconPath)
            => CreateShortcutInternal(path, target, arguments, description, null, iconPath, 0);

        /// <summary>
        /// Creates a Windows shortcut (.lnk) with a working directory and an icon.
        /// </summary>
        public static bool CreateShortcut(string path, string target, string arguments, string description, string workingDirectory, string iconPath)
            => CreateShortcutInternal(path, target, arguments, description, workingDirectory, iconPath, 0);

        /// <summary>
        /// Core implementation using ShellLink COM interop.
        /// </summary>
    private static bool CreateShortcutInternal(string path, string target, string arguments, string description, string workingDirectory, string iconPath, int iconIndex)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Shortcut path is required", nameof(path));
                if (string.IsNullOrWhiteSpace(target)) throw new ArgumentException("Target path is required", nameof(target));

                // Ensure .lnk extension and absolute path
                if (!path.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase))
                    path += ".lnk";
                path = Path.GetFullPath(path);

                // Ensure directory exists
                var dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

                var link = (IShellLinkW)new ShellLink();

                Marshal.ThrowExceptionForHR(link.SetPath(target));
                if (!string.IsNullOrWhiteSpace(arguments)) Marshal.ThrowExceptionForHR(link.SetArguments(arguments));
                if (!string.IsNullOrWhiteSpace(description)) Marshal.ThrowExceptionForHR(link.SetDescription(description));
                if (!string.IsNullOrWhiteSpace(workingDirectory)) Marshal.ThrowExceptionForHR(link.SetWorkingDirectory(workingDirectory));
                if (!string.IsNullOrWhiteSpace(iconPath)) Marshal.ThrowExceptionForHR(link.SetIconLocation(iconPath, iconIndex));

                var persist = (IPersistFile)link;
                // Save (false = do not remember if SaveCompleted will be used later)
                int hr = persist.Save(path, false);
                Marshal.ThrowExceptionForHR(hr);
                hr = persist.SaveCompleted(path);
                Marshal.ThrowExceptionForHR(hr);
                return true;
            }
            catch
            {
                return false; // Intentionally swallow; caller can inspect false.
            }
        }

        #region COM Interop
        [ComImport]
        [Guid("00021401-0000-0000-C000-000000000046")] // CLSID_ShellLink
        private class ShellLink { }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214F9-0000-0000-C000-000000000046")] // IID_IShellLinkW
        private interface IShellLinkW
        {
            // We only define the subset we need; method order must match vtable.
            int GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, IntPtr pfd, uint fFlags);
            int GetIDList(out IntPtr ppidl);
            int SetIDList(IntPtr pidl);
            int GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
            int SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            int GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
            int SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
            int GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
            int SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
            int GetHotkey(out short pwHotkey);
            int SetHotkey(short wHotkey);
            int GetShowCmd(out int piShowCmd);
            int SetShowCmd(int iShowCmd);
            int GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
            int SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
            int SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
            int Resolve(IntPtr hwnd, uint fFlags);
            int SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("0000010b-0000-0000-C000-000000000046")] // IID_IPersistFile
        private interface IPersistFile
        {
            int GetClassID(out Guid pClassID);
            int IsDirty();
            int Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, int dwMode);
            int Save([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, bool fRemember);
            int SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);
            int GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
        }
        #endregion
    }
}
