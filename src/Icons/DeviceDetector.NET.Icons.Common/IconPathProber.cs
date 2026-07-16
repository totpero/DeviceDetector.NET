using System;

namespace DeviceDetectorNET.Icons.Common
{
    /// <summary>
    /// Extension-priority file probing, name sanitization, and URL-base-path prefixing shared by every
    /// icon resolver in this repo.
    /// </summary>
    public sealed class IconPathProber
    {
        private readonly IIconProbeOptions _options;
        private readonly Func<string, bool> _fileExists;

        public IconPathProber(IIconProbeOptions options, Func<string, bool> fileExists)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _fileExists = fileExists ?? throw new ArgumentNullException(nameof(fileExists));
        }

        public string SanitizeName(string name)
        {
            var sanitized = name ?? string.Empty;
            if (_options.NameReplacements == null)
            {
                return sanitized;
            }

            foreach (var replacement in _options.NameReplacements)
            {
                sanitized = sanitized.Replace(replacement.Key, replacement.Value);
            }

            return sanitized;
        }

        public string ResolveIcon(string name, string relativeFolder)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            var sanitized = SanitizeName(name);

            foreach (var extension in _options.ExtensionPriority)
            {
                var relativePath = relativeFolder + "/" + sanitized + "." + extension;
                if (_fileExists(relativePath))
                {
                    return relativePath;
                }
            }

            return null;
        }

        public string WithUrlBasePath(string relativePath)
        {
            return _options.UrlBasePath + (relativePath ?? _options.FallbackIconPath);
        }
    }
}
