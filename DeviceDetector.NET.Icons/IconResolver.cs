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
        private const string BotPath = "/bot";
        private const string BotCategoryPath = "/bot/category";
        private const string ClientRootPath = "/client";
        private const string ClientTypePath = "/client/type";
        private const string BrowserPath = "/client/browser";
        private const string BrowserFamilyPath = "/client/browser/family";
        private const string BrowserEnginePath = "/client/browser/engine";
        private const string OsPath = "/client/os";
        private const string OsFamilyPath = "/client/os/family";
        private const string BrandPath = "/device/brand";
        private const string DeviceTypePath = "/device/type";

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

        private string SanitizeName(string name)
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

        private string ResolveIcon(string name, string relativeFolder)
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

        private string WithUrlBasePath(string relativePath)
        {
            return _options.UrlBasePath + (relativePath ?? _options.FallbackIconPath);
        }

        public string GetBot(string bot, string category = null)
        {
            var icon = ResolveIcon(bot, BotPath) ?? ResolveIcon(category, BotCategoryPath);
            return WithUrlBasePath(icon);
        }

        public string GetBotCategory(string category)
        {
            return WithUrlBasePath(ResolveIcon(category, BotCategoryPath));
        }

        public string GetBrowser(string browser, string family = null, string engine = null)
        {
            var icon = ResolveIcon(browser, BrowserPath)
                ?? ResolveIcon(family, BrowserFamilyPath)
                ?? ResolveIcon(engine, BrowserEnginePath)
                ?? ResolveIcon("browser", ClientTypePath);
            return WithUrlBasePath(icon);
        }

        public string GetBrowserFamily(string family)
        {
            return WithUrlBasePath(ResolveIcon(family, BrowserFamilyPath));
        }

        public string GetBrowserEngine(string engine)
        {
            return WithUrlBasePath(ResolveIcon(engine, BrowserEnginePath));
        }

        public string GetOs(string os, string family = null)
        {
            var icon = ResolveIcon(os, OsPath)
                ?? ResolveIcon(family, OsFamilyPath)
                ?? ResolveIcon("os", ClientTypePath);
            return WithUrlBasePath(icon);
        }

        public string GetOsFamily(string family)
        {
            return WithUrlBasePath(ResolveIcon(family, OsFamilyPath));
        }

        public string GetClient(string client, string type)
        {
            var icon = ResolveIcon(client, ClientRootPath + "/" + type) ?? ResolveIcon(type, ClientTypePath);
            return WithUrlBasePath(icon);
        }

        public string GetClientType(string type)
        {
            return WithUrlBasePath(ResolveIcon(type, ClientTypePath));
        }

        public string GetBrand(string brand, string type = null)
        {
            var icon = ResolveIcon(brand, BrandPath) ?? ResolveIcon(type, DeviceTypePath);
            return WithUrlBasePath(icon);
        }

        public string GetDeviceType(string type)
        {
            return WithUrlBasePath(ResolveIcon(type, DeviceTypePath));
        }
    }
}
