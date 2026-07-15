using System;
using DeviceDetectorNET.Parser.Device;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Client;
using DeviceDetectorNET.Results.Device;

namespace DeviceDetectorNET.Icons.Matomo
{
    public partial class MatomoIconResolver
    {
        public string GetBrowser(BrowserMatchResult browser)
        {
            return browser == null ? GetBrowser(string.Empty) : GetBrowser(browser.ShortName);
        }

        public string GetOs(OsMatchResult os)
        {
            return os == null ? GetOs(string.Empty) : GetOs(os.ShortName);
        }

        public string GetBrand(DeviceMatchResult device)
        {
            return device == null ? GetBrand(string.Empty) : GetBrand(device.Brand);
        }

        public string GetDeviceType(int? deviceType)
        {
            var deviceTypeName = deviceType.HasValue ? Devices.GetDeviceName(deviceType.Value) : string.Empty;
            return GetDeviceType(deviceTypeName);
        }

        /// <summary>matomo-icons has no bot category — always returns the fallback icon regardless of the match result.</summary>
        public string GetBot(BotMatchResult bot)
        {
            return GetBot((string)null, (string)null);
        }

        /// <summary>matomo-icons only has a browsers/ folder among client kinds; non-browser clients (feed readers, media players, etc.) always resolve to the fallback icon.</summary>
        private string GetClientIcon(ClientMatchResult client)
        {
            return client is BrowserMatchResult browserMatch ? GetBrowser(browserMatch) : _prober.WithUrlBasePath(null);
        }

        public DeviceIconPaths GetIcons(DeviceDetectorResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            return new DeviceIconPaths(
                botIcon: result.IsBoot ? GetBot(result.Bot) : null,
                osIcon: GetOs(result.Os),
                clientIcon: GetClientIcon(result.Client),
                brandIcon: GetBrand(result.Device),
                deviceTypeIcon: GetDeviceType(result.Device != null ? result.Device.Type : null));
        }
    }
}
