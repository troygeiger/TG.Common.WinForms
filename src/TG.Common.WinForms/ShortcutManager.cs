using System;

namespace TG.Common
{
    /// <summary>
    /// Provides helper methods for creating Windows shortcuts. All members throw
    /// <see cref="NotSupportedException"/> in this target framework build.
    /// </summary>
    public static class ShortcutManager
    {
        /// <summary>
        /// Creates a Windows shortcut (.lnk).
        /// </summary>
        /// <returns>Always throws <see cref="NotSupportedException"/> in this build.</returns>
        public static bool CreateShortcut(string path, string target, string arguments, string description)
        {
            throw new NotSupportedException("Shortcut creation via IWshRuntimeLibrary is not supported in net6.0-windows with dotnet build. Consider referencing Windows Script Host Object Model via COM in a .NET Framework project, or use Windows API Code Pack.");
        }

    /// <summary>
    /// Creates a Windows shortcut (.lnk) with an icon.
    /// </summary>
    /// <returns>Always throws <see cref="NotSupportedException"/> in this build.</returns>
    public static bool CreateShortcut(string path, string target, string arguments, string description, string iconPath)
        {
            throw new NotSupportedException();
        }

    /// <summary>
    /// Creates a Windows shortcut (.lnk) with a working directory and an icon.
    /// </summary>
    /// <returns>Always throws <see cref="NotSupportedException"/> in this build.</returns>
    public static bool CreateShortcut(string path, string target, string arguments, string description, string workingDirectory, string iconPath)
        {
            throw new NotSupportedException();
        }
    }
}
