using System;
using System.IO;

namespace DeviceDetectorNET.Icons.Tests
{
    /// <summary>
    /// Creates an isolated temp directory that mirrors the icon pack's folder layout for a test,
    /// and deletes it on dispose.
    /// </summary>
    internal sealed class TempIconDirectory : IDisposable
    {
        public TempIconDirectory()
        {
            RootPath = Path.Combine(Path.GetTempPath(), "DeviceDetectorIconsTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(RootPath);
        }

        public string RootPath { get; }

        public void CreateFile(string relativePath)
        {
            var fullPath = Path.Combine(RootPath, relativePath.TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            File.WriteAllText(fullPath, string.Empty);
        }

        public void Dispose()
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, recursive: true);
            }
        }
    }
}
