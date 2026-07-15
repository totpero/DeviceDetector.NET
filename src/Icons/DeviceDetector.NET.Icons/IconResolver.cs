using System;
using DeviceDetectorNET.Icons.Common;

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
        private readonly IconPathProber _prober;

        public IconResolver(IconResolverOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            IconResolverOptionsValidation.Validate(options.PhysicalRootPath, options.FileExists, options.ExtensionPriority);

            _options = options;
            var fileExists = options.FileExists ?? IconResolverOptionsValidation.DefaultFileExists(options.PhysicalRootPath);
            _prober = new IconPathProber(options, fileExists);
        }

        public string GetBot(string bot, string category = null)
        {
            var icon = _prober.ResolveIcon(bot, BotPath) ?? _prober.ResolveIcon(category, BotCategoryPath);
            return _prober.WithUrlBasePath(icon);
        }

        public string GetBotCategory(string category)
        {
            return _prober.WithUrlBasePath(_prober.ResolveIcon(category, BotCategoryPath));
        }

        public string GetBrowser(string browser, string family = null, string engine = null)
        {
            var icon = _prober.ResolveIcon(browser, BrowserPath)
                ?? _prober.ResolveIcon(family, BrowserFamilyPath)
                ?? _prober.ResolveIcon(engine, BrowserEnginePath)
                ?? _prober.ResolveIcon("browser", ClientTypePath);
            return _prober.WithUrlBasePath(icon);
        }

        public string GetBrowserFamily(string family)
        {
            return _prober.WithUrlBasePath(_prober.ResolveIcon(family, BrowserFamilyPath));
        }

        public string GetBrowserEngine(string engine)
        {
            return _prober.WithUrlBasePath(_prober.ResolveIcon(engine, BrowserEnginePath));
        }

        public string GetOs(string os, string family = null)
        {
            var icon = _prober.ResolveIcon(os, OsPath)
                ?? _prober.ResolveIcon(family, OsFamilyPath)
                ?? _prober.ResolveIcon("os", ClientTypePath);
            return _prober.WithUrlBasePath(icon);
        }

        public string GetOsFamily(string family)
        {
            return _prober.WithUrlBasePath(_prober.ResolveIcon(family, OsFamilyPath));
        }

        public string GetClient(string client, string type)
        {
            var icon = _prober.ResolveIcon(client, ClientRootPath + "/" + type) ?? _prober.ResolveIcon(type, ClientTypePath);
            return _prober.WithUrlBasePath(icon);
        }

        public string GetClientType(string type)
        {
            return _prober.WithUrlBasePath(_prober.ResolveIcon(type, ClientTypePath));
        }

        public string GetBrand(string brand, string type = null)
        {
            var icon = _prober.ResolveIcon(brand, BrandPath) ?? _prober.ResolveIcon(type, DeviceTypePath);
            return _prober.WithUrlBasePath(icon);
        }

        public string GetDeviceType(string type)
        {
            return _prober.WithUrlBasePath(_prober.ResolveIcon(type, DeviceTypePath));
        }
    }
}
