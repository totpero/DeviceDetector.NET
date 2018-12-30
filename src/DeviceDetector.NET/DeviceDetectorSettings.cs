namespace DeviceDetectorNET
{
    /// <summary>
    /// Global DeviceDetector settings
    /// </summary>
    public class DeviceDetectorSettings
    {
        static DeviceDetectorSettings()
        {
            RegexesDirectory = string.Empty;
        }

        /// <summary>
        /// Default yaml regexes path
        /// Default is <see cref="string.Empty"/>
        /// Exemple: C:\YamlRegexsFiles\
        /// </summary>
        public static string RegexesDirectory { get; set; }
    }
}
