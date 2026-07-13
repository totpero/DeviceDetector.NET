using System;
using System.IO;

namespace DeviceDetectorNET.Icons
{
    /// <summary>
    /// Resolves icon file paths for DeviceDetector.NET detection results. Ported from the
    /// resolution logic of Simbiat/DeviceDetectorIcons (MIT licensed, © 2021 Dmitrii Kustov).
    /// </summary>
    public partial class IconResolver
    {
        private readonly IconResolverOptions _options;
        private readonly Func<string, bool> _fileExists;

        public IconResolver(IconResolverOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.FileExists == null && string.IsNullOrEmpty(options.PhysicalRootPath))
            {
                throw new ArgumentException("Either PhysicalRootPath or FileExists must be set.", nameof(options));
            }

            if (options.ExtensionPriority == null || options.ExtensionPriority.Count == 0)
            {
                throw new ArgumentException("ExtensionPriority must contain at least one extension.", nameof(options));
            }

            _options = options;
            _fileExists = options.FileExists ?? DefaultFileExists;
        }

        private bool DefaultFileExists(string relativePath)
        {
            var normalized = relativePath.TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar);
            return File.Exists(Path.Combine(_options.PhysicalRootPath, normalized));
        }
    }
}
