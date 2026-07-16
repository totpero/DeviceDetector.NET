using System;
using System.Collections.Generic;
using System.IO;

namespace DeviceDetectorNET.Icons.Common
{
    /// <summary>
    /// Shared constructor-time validation and default file-existence check for icon resolver options.
    /// </summary>
    public static class IconResolverOptionsValidation
    {
        public static void Validate(string physicalRootPath, Func<string, bool> fileExists, IList<string> extensionPriority)
        {
            if (fileExists == null && string.IsNullOrEmpty(physicalRootPath))
            {
                throw new ArgumentException("Either PhysicalRootPath or FileExists must be set.", nameof(physicalRootPath));
            }

            if (extensionPriority == null || extensionPriority.Count == 0)
            {
                throw new ArgumentException("ExtensionPriority must contain at least one extension.", nameof(extensionPriority));
            }
        }

        public static Func<string, bool> DefaultFileExists(string physicalRootPath)
        {
            return relativePath =>
            {
                var normalized = relativePath.TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar);
                return File.Exists(Path.Combine(physicalRootPath, normalized));
            };
        }
    }
}
