using System;
using DeviceDetectorNET.Parser.Device;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Client;
using DeviceDetectorNET.Results.Device;

namespace DeviceDetectorNET.Icons
{
    public partial class IconResolver
    {
        public string GetBot(BotMatchResult bot)
        {
            return bot == null
                ? GetBot(string.Empty, string.Empty)
                : GetBot(bot.Name, bot.Category);
        }

        public string GetBrowser(BrowserMatchResult browser)
        {
            return browser == null
                ? GetBrowser(string.Empty, string.Empty, string.Empty)
                : GetBrowser(browser.Name, browser.Family, browser.Engine);
        }

        public string GetClient(ClientMatchResult client)
        {
            if (client == null)
            {
                return GetClient(string.Empty, string.Empty);
            }

            return client is BrowserMatchResult browserMatch
                ? GetBrowser(browserMatch)
                : GetClient(client.Name, client.Type);
        }

        public string GetOs(OsMatchResult os)
        {
            return os == null
                ? GetOs(string.Empty, string.Empty)
                : GetOs(os.Name, os.Family);
        }

        public string GetBrand(DeviceMatchResult device)
        {
            if (device == null)
            {
                return GetBrand(string.Empty, string.Empty);
            }

            var deviceTypeName = device.Type.HasValue ? Devices.GetDeviceName(device.Type.Value) : string.Empty;
            return GetBrand(device.Brand, deviceTypeName);
        }

        public string GetDeviceType(int? deviceType)
        {
            var deviceTypeName = deviceType.HasValue ? Devices.GetDeviceName(deviceType.Value) : string.Empty;
            return GetDeviceType(deviceTypeName);
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
                clientIcon: GetClient(result.Client),
                brandIcon: GetBrand(result.Device),
                deviceTypeIcon: GetDeviceType(result.Device != null ? result.Device.Type : null));
        }
    }
}
