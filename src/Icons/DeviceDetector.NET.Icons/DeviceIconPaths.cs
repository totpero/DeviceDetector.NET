namespace DeviceDetectorNET.Icons
{
    /// <summary>
    /// Bundles the icon paths for every part of a <see cref="DeviceDetectorNET.Results.DeviceDetectorResult"/>.
    /// </summary>
    public sealed class DeviceIconPaths
    {
        public DeviceIconPaths(string botIcon, string osIcon, string clientIcon, string brandIcon, string deviceTypeIcon)
        {
            BotIcon = botIcon;
            OsIcon = osIcon;
            ClientIcon = clientIcon;
            BrandIcon = brandIcon;
            DeviceTypeIcon = deviceTypeIcon;
        }

        /// <summary>Null when the result did not match a bot.</summary>
        public string BotIcon { get; }
        public string OsIcon { get; }
        public string ClientIcon { get; }
        public string BrandIcon { get; }
        public string DeviceTypeIcon { get; }
    }
}
