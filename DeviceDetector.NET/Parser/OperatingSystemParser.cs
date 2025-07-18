using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DeviceDetectorNET.Class;
using DeviceDetectorNET.Results;

namespace DeviceDetectorNET.Parser
{
    public class OperatingSystemParser : AbstractParser<List<Os>, OsMatchResult>
    {

        private const string Unknown = "Unknown";
        private const string UnknownShort = "UNK";

        /// <summary>
        /// Known operating systems mapped to their internal short codes
        /// </summary>
        protected static readonly Dictionary<string, string> OperatingSystems = new Dictionary<string, string>
        {
            { "AIX", "AIX" },
            { "AND", "Android" },
            { "ADR", "Android TV" },
            { "ALP", "Alpine Linux" },
            { "AMZ", "Amazon Linux" },
            { "AMG", "AmigaOS" },
            { "ARM", "Armadillo OS" },
            { "ARO", "AROS" },
            { "ATV", "tvOS" },
            { "ARL", "Arch Linux" },
            { "AOS", "AOSC OS" },
            { "ASP", "ASPLinux" },
            { "AZU", "Azure Linux" },
            { "BTR", "BackTrack" },
            { "SBA", "Bada" },
            { "BYI", "Baidu Yi" },
            { "BEO", "BeOS" },
            { "BLB", "BlackBerry OS" },
            { "QNX", "BlackBerry Tablet OS" },
            { "PAN", "blackPanther OS" },
            { "BOS", "Bliss OS" },
            { "BMP", "Brew" },
            { "BSN", "BrightSignOS" },
            { "CAI", "Caixa M�gica" },
            { "CES", "CentOS" },
            { "CST", "CentOS Stream" },
            { "CLO", "Clear Linux OS" },
            { "CLR", "ClearOS Mobile" },
            { "COS", "Chrome OS" },
            { "CRS", "Chromium OS" },
            { "CHN", "China OS" },
            { "COL", "Coolita OS" },
            { "CYN", "CyanogenMod" },
            { "DEB", "Debian" },
            { "DEE", "Deepin" },
            { "DFB", "DragonFly" },
            { "DVK", "DVKBuntu" },
            { "ELE", "ElectroBSD" },
            { "EUL", "EulerOS" },
            { "FED", "Fedora" },
            { "FEN", "Fenix" },
            { "FOS", "Firefox OS" },
            { "FIR", "Fire OS" },
            { "FOR", "Foresight Linux" },
            { "FRE", "Freebox" },
            { "BSD", "FreeBSD" },
            { "FRI", "FRITZ!OS" },
            { "FYD", "FydeOS" },
            { "FUC", "Fuchsia" },
            { "GNT", "Gentoo" },
            { "GNX", "GENIX" },
            { "GEO", "GEOS" },
            { "GNS", "gNewSense" },
            { "GRI", "GridOS" },
            { "GTV", "Google TV" },
            { "HPX", "HP-UX" },
            { "HAI", "Haiku OS" },
            { "IPA", "iPadOS" },
            { "HAR", "HarmonyOS" },
            { "HAS", "HasCodingOS" },
            { "HEL", "HELIX OS" },
            { "IRI", "IRIX" },
            { "INF", "Inferno" },
            { "JME", "Java ME" },
            { "JOL", "Joli OS" },
            { "KOS", "KaiOS" },
            { "KAL", "Kali" },
            { "KAN", "Kanotix" },
            { "KIN", "KIN OS" },
            { "KNO", "Knoppix" },
            { "KTV", "KreaTV" },
            { "KBT", "Kubuntu" },
            { "LIN", "GNU/Linux" },
            { "LEA", "LeafOS" },
            { "LND", "LindowsOS" },
            { "LNS", "Linspire" },
            { "LEN", "Lineage OS" },
            { "LIR", "Liri OS" },
            { "LOO", "Loongnix" },
            { "LBT", "Lubuntu" },
            { "LOS", "Lumin OS" },
            { "LUN", "LuneOS" },
            { "VLN", "VectorLinux" },
            { "MAC", "Mac" },
            { "MAE", "Maemo" },
            { "MAG", "Mageia" },
            { "MDR", "Mandriva" },
            { "SMG", "MeeGo" },
            { "MET", "Meta Horizon" },
            { "MCD", "MocorDroid" },
            { "MON", "moonOS" },
            { "EZX", "Motorola EZX" },
            { "MIN", "Mint" },
            { "MLD", "MildWild" },
            { "MOR", "MorphOS" },
            { "NBS", "NetBSD" },
            { "MTK", "MTK / Nucleus" },
            { "MRE", "MRE" },
            { "NXT", "NeXTSTEP" },
            { "NWS", "NEWS-OS" },
            { "WII", "Nintendo" },
            { "NDS", "Nintendo Mobile" },
            { "NOV", "Nova" },
            { "OS2", "OS/2" },
            { "T64", "OSF1" },
            { "OBS", "OpenBSD" },
            { "OVS", "OpenVMS" },
            { "OVZ", "OpenVZ" },
            { "OWR", "OpenWrt" },
            { "OTV", "Opera TV" },
            { "ORA", "Oracle Linux" },
            { "ORD", "Ordissimo" },
            { "PAR", "Pardus" },
            { "PCL", "PCLinuxOS" },
            { "PIC", "PICO OS" },
            { "PLA", "Plasma Mobile" },
            { "PSP", "PlayStation Portable" },
            { "PS3", "PlayStation" },
            { "PVE", "Proxmox VE" },
            { "PUF", "Puffin OS" },
            { "PUR", "PureOS" },
            { "QTP", "Qtopia" },
            { "PIO", "Raspberry Pi OS" },
            { "RAS", "Raspbian" },
            { "RHT", "Red Hat" },
            { "RST", "Red Star" },
            { "RED", "RedOS" },
            { "REV", "Revenge OS" },
            { "RIS", "risingOS" },
            { "ROS", "RISC OS" },
            { "ROC", "Rocky Linux" },
            { "ROK", "Roku OS" },
            { "RSO", "Rosa" },
            { "ROU", "RouterOS" },
            { "REM", "Remix OS" },
            { "RRS", "Resurrection Remix OS" },
            { "REX", "REX" },
            { "RZD", "RazoDroiD" },
            { "RXT", "RTOS & Next" },
            { "SAB", "Sabayon" },
            { "SSE", "SUSE" },
            { "SAF", "Sailfish OS" },
            { "SCI", "Scientific Linux" },
            { "SEE", "SeewoOS" },
            { "SER", "SerenityOS" },
            { "SIR", "Sirin OS" },
            { "SLW", "Slackware" },
            { "SOS", "Solaris" },
            { "SBL", "Star-Blade OS" },
            { "SYL", "Syllable" },
            { "SYM", "Symbian" },
            { "SYS", "Symbian OS" },
            { "S40", "Symbian OS Series 40" },
            { "S60", "Symbian OS Series 60" },
            { "SY3", "Symbian^3" },
            { "TEN", "TencentOS" },
            { "TDX", "ThreadX" },
            { "TIZ", "Tizen" },
            { "TIV", "TiVo OS" },
            { "TOS", "TmaxOS" },
            { "TUR", "Turbolinux" },
            { "UBT", "Ubuntu" },
            { "ULT", "ULTRIX" },
            { "UOS", "UOS" },
            { "VID", "VIDAA" },
            { "VIZ", "ViziOS" },
            { "WAS", "watchOS" },
            { "WER", "Wear OS" },
            { "WTV", "WebTV" },
            { "WHS", "Whale OS" },
            { "WIN", "Windows" },
            { "WCE", "Windows CE" },
            { "WIO", "Windows IoT" },
            { "WMO", "Windows Mobile" },
            { "WPH", "Windows Phone" },
            { "WRT", "Windows RT" },
            { "WPO", "WoPhone" },
            { "XBX", "Xbox" },
            { "XBT", "Xubuntu" },
            { "YNS", "YunOS" },
            { "ZEN", "Zenwalk" },
            { "ZOR", "ZorinOS" },
            { "IOS", "iOS" },
            { "POS", "palmOS" },
            { "WEB", "Webian" },
            { "WOS", "webOS" },
        };

        /// <summary>
        /// Operating system families mapped to the short codes of the associated operating systems
        /// </summary>
        protected static readonly IReadOnlyDictionary<string, string[]> OsFamilies = new Dictionary<string, string[]>
        {
            {"Android"              , new [] {"AND", "CYN", "FIR", "REM", "RZD", "MLD", "MCD", "YNS", "GRI", "HAR",
                                              "ADR", "CLR", "BOS", "REV", "LEN", "SIR", "RRS", "WER", "PIC", "ARM",
                                              "HEL", "BYI", "RIS", "PUF", "LEA", "MET" }},
            {"AmigaOS"              , new [] {"AMG", "MOR", "ARO"}},
            {"BlackBerry"           , new [] {"BLB", "QNX"}},
            {"Brew"                 , new [] {"BMP"}},
            {"BeOS"                 , new [] {"BEO", "HAI"}},
            {"Chrome OS"            , new [] {"COS", "CRS", "FYD", "SEE"}},
            {"Firefox OS"           , new [] {"FOS", "KOS"}},
            {"Gaming Console"       , new [] {"WII", "PS3"}},
            {"Google TV"            , new [] {"GTV"}},
            {"IBM"                  , new [] {"OS2"}},
            {"iOS"                  , new [] {"IOS", "ATV", "WAS", "IPA"}},
            {"RISC OS"              , new [] {"ROS"}},
            {"GNU/Linux"            , new [] {"LIN", "ARL", "DEB", "KNO", "MIN", "UBT", "KBT", "XBT", "LBT", "FED",
                                                "RHT", "VLN", "MDR", "GNT", "SAB", "SLW", "SSE", "CES", "BTR", "SAF",
                                                "ORD", "TOS", "RSO", "DEE", "FRE", "MAG", "FEN", "CAI", "PCL", "HAS",
                                                "LOS", "DVK", "ROK", "OWR", "OTV", "KTV", "PUR", "PLA", "FUC", "PAR",
                                                "FOR", "MON", "KAN", "ZEN", "LND", "LNS", "CHN", "AMZ", "TEN", "CST",
                                                "NOV", "ROU", "ZOR", "RED", "KAL", "ORA", "VID", "TIV", "BSN", "RAS",
                                                "UOS", "PIO", "FRI", "LIR", "WEB", "SER", "ASP", "AOS", "LOO", "EUL",
                                                "SCI", "ALP", "CLO", "ROC", "OVZ", "PVE", "RST", "EZX", "GNS", "JOL",
                                                "TUR", "QTP", "WPO", "PAN", "VIZ", "AZU", "COL" }},
            {"Mac"                  , new [] {"MAC" }},
            {"Mobile Gaming Console", new [] {"PSP", "NDS", "XBX" }},
            {"OpenVMS"              , new [] { "OVS" }},
            {"Real-time OS"         , new [] {"MTK", "TDX", "MRE", "JME", "REX", "RXT" }},
            {"Other Mobile"         , new [] {"WOS", "POS", "SBA", "TIZ", "SMG", "MAE", "LUN", "GEO" }},
            {"Symbian"              , new [] {"SYM", "SYS", "SY3", "S60", "S40" }},
            {"Unix"                 , new [] {"SOS", "AIX", "HPX", "BSD", "NBS", "OBS", "DFB", "SYL", "IRI", "T64", 
                                                "INF", "ELE", "GNX", "ULT", "NWS", "NXT", "SBL", }},
            {"WebTV"                , new [] {"WTV" }},
            {"Windows"              , new [] {"WIN" }},
            {"Windows Mobile"       , new [] {"WPH", "WMO", "WCE", "WRT", "WIO", "KIN" }},
            {"Other Smart TV"       , new [] {"WHS" }}
        };

        /// <summary>
        /// Contains a list of mappings from OS names we use to known client hint values
        /// </summary>
        public override IReadOnlyDictionary<string, string[]> ClientHintMapping => new Dictionary<string, string[]>
        {
             {"GNU/Linux", new [] {"Linux"}},
             {"Mac"      , new [] {"MacOS"}},
        };

        /// <summary>
        /// Operating system families that are known as desktop only
        /// </summary>
        protected internal static readonly string[] DesktopOs = new[]
        {
            "AmigaOS", "IBM", "GNU/Linux", "Mac", "Unix", "Windows", "BeOS", "Chrome OS"
        };

        /// <summary>
        /// Fire OS version mapping
        /// </summary>
        protected internal static readonly IReadOnlyDictionary<string, string> FireOsVersionMapping = new Dictionary<string,string>
        {
            {"11"    , "8" },
            {"10"    , "8" },
            {"9"     , "7" },
            {"7"     , "6" },
            {"5"     , "5" },
            {"4.4.3" , "4.5.1" },
            {"4.4.2" , "4" },
            {"4.2.2" , "3" },
            {"4.0.3" , "3" },
            {"4.0.2" , "3" },
            {"4"     , "2" },
            {"2"     , "1" },
        };

        /// <summary>
        /// Lineage OS version mapping
        /// </summary>
        protected internal static readonly Dictionary<string, string> LineageOsVersionMapping = new Dictionary<string, string>
        {
            {"16"    , "23" },
            {"15"    , "22" },
            {"14"    , "21" },
            {"13"    , "20.0" },
            {"12.1"  , "19.1" },
            {"12"    , "19.0" },
            {"11"    , "18.0" },
            {"10"    , "17.0" },
            {"9"     , "16.0" },
            {"8.1.0" , "15.1" },
            {"8.0.0" , "15.0" },
            {"7.1.2" , "14.1" },
            {"7.1.1" , "14.1" },
            {"7.0"   , "14.0" },
            {"6.0.1" , "13.0" },
            {"6.0"   , "13.0" },
            {"5.1.1" , "12.1" },
            {"5.0.2" , "12.0" },
            {"5.0"   , "12.0" },
            {"4.4.4" , "11.0" },
            {"4.3"   , "10.2" },
            {"4.2.2" , "10.1" },
            {"4.0.4" , "9.1.0" },
        };

        /// <summary>
        /// Returns all available operating systems
        /// </summary>
        /// <returns></returns>
        public static IReadOnlyDictionary<string, string> GetAvailableOperatingSystems()
        {
            return OperatingSystems;
        }

        /// <summary>
        /// Returns all available operating system families
        /// </summary>
        /// <returns></returns>
        public static IReadOnlyDictionary<string, string[]> GetAvailableOperatingSystemFamilies()
        {
            return OsFamilies;
        }

        /// <summary>
        /// Returns the os name and shot name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static OsMatchResult GetShortOsData(string name)
        {
            var shortName = UnknownShort;
            name = name.ToLower();

            foreach (var operatingSystem in OperatingSystems)
            {
                if (name != operatingSystem.Value.ToLower())
                {
                    continue;
                }
                name = operatingSystem.Value;
                shortName = operatingSystem.Key;
                break;
            }
            return new OsMatchResult { Name = name, ShortName = shortName };
        }

        public OperatingSystemParser()
        {
            FixtureFile = "regexes/oss.yml";
            ParserName = "os";
            regexList = GetRegexes();
        }

        public override ParseResult<OsMatchResult> Parse()
        {
            var result = new ParseResult<OsMatchResult>();
            //Os localOs = null;

            var osFromClientHints = this.ParseOsFromClientHints();
            var osFromUserAgent = this.ParseOsFromUserAgent();

            string name;
            string version;
            string @short;
            if (!string.IsNullOrEmpty(osFromClientHints.Name))
            {
                name = osFromClientHints.Name;
                version = osFromClientHints.Version;

                // use version from user agent if non was provided in client hints, but os family from useragent matches
                if (string.IsNullOrEmpty(version) && GetOsFamily(name) == GetOsFamily(osFromUserAgent.Name))
                {
                    version = osFromUserAgent.Version;
                }

                // On Windows, version 0.0.0 can be either 7, 8 or 8.1
                if ("Windows" == name && "0.0.0" == version)
                {
                    version = osFromUserAgent.Version.Equals("10") ? string.Empty : osFromUserAgent.Version;
                }

                // If the OS name detected from client hints matches the OS family from user agent
                // but the os name is another, we use the one from user agent, as it might be more detailed
                if (GetOsFamily(osFromUserAgent.Name) == name && osFromUserAgent.Name != name)
                {
                    name = osFromUserAgent.Name;
                    if ("LeafOS" == name || "HarmonyOS" == name)
                    {
                        version = string.Empty;
                    }
                    if ("PICO OS" == name)
                    {
                        version = osFromUserAgent.Version;
                    }

                    if ("Fire OS" == name && !string.IsNullOrEmpty(osFromClientHints.Version))
                    {
                        var majorVersion = version.Split('.').Length > 0 ? version.Split('.')[0] : "0";

                        version = FireOsVersionMapping[version] ?? FireOsVersionMapping[majorVersion] ?? string.Empty;
                    }
                }

                @short = osFromClientHints.ShortName;

                // Chrome OS is in some cases reported as Linux in client hints, we fix this only if the version matches
                if ("GNU/Linux" == name 
                    && "Chrome OS" == osFromUserAgent.Name 
                    && osFromClientHints.Version == osFromUserAgent.Version)
                {
                    name = osFromUserAgent.Name;
                    @short = osFromUserAgent.ShortName;
                }

                // Chrome OS is in some cases reported as Android in client hints
                if ("Android" == name && "Chrome OS" == osFromUserAgent.Name)
                {
                    name = osFromUserAgent.Name;
                    version = string.Empty;
                    @short = osFromUserAgent.ShortName;
                }
            }
            else if (!string.IsNullOrEmpty(osFromUserAgent.Name))
            {
                name = osFromUserAgent.Name;
                version = osFromUserAgent.Version;
                @short = osFromUserAgent.ShortName;
            }
            else
            {
                return result;
            }
           
            var platform = ParsePlatform();
            var family = GetOsFamily(@short);
            var androidApps = new[] { "com.hisense.odinbrowser", "com.seraphic.openinet.pre", "com.appssppa.idesktoppcbrowser", "every.browser.inc" };


            if (ClientHints != null)
            {
                if (androidApps.Contains(ClientHints.GetApp()) && "Android" != name)
                {
                    name = "Android";
                    family = "Android";
                    @short = "ADR";
                    version = string.Empty;
                }

                if ("org.lineageos.jelly" == ClientHints.GetApp() && "Lineage OS" != name)
                {
                    var majorVersion = version.Split('.').Length > 0 ? version.Split('.')[0] : "0";

                    name = "Lineage OS";
                    family = "Android";
                    @short = "LEN";
                    version = LineageOsVersionMapping[version] ?? LineageOsVersionMapping[majorVersion] ?? string.Empty;
                }

                if ("org.mozilla.tv.firefox" == ClientHints.GetApp() && "Fire OS" != name)
                {
                    var majorVersion = version.Split('.').Length > 0 ? version.Split('.')[0] : "0";

                    name = "Fire OS";
                    family = "Android";
                    @short = "FIR";
                    version = FireOsVersionMapping[version] ?? FireOsVersionMapping[majorVersion] ?? string.Empty;
                }
            }

            var os = new OsMatchResult
            {
                Name = name,
                ShortName = @short,
                Version = version,
                Platform = platform,
                Family = family,
            };

            if (OperatingSystems.ContainsValue(os.Name))
            {
                os.ShortName = OperatingSystems.FirstOrDefault(o => o.Value.Equals(os.Name)).Key;
            }
            result.Add(os);
            //string[] localMatches = null;

            //foreach (var os in regexList)
            //{
            //    var matches = MatchUserAgent(os.Regex);
            //    if (matches.Length > 0)
            //    {
            //        localOs = os;
            //        localMatches = matches;
            //        break;
            //    }
            //}

            //if (localMatches != null)
            //{
            //    var name = BuildByMatch(localOs.Name, localMatches);
            //    var @short = UnknownShort;
            //    foreach (var operatingSystem in OperatingSystems)
            //    {
            //        if (operatingSystem.Value.ToLower().Equals(name.ToLower()))
            //        {
            //            name = operatingSystem.Value;
            //            @short = operatingSystem.Key;
            //        }
            //    }
            //    var os = new OsMatchResult
            //    {
            //        Name = name,
            //        ShortName = @short,
            //        Version = BuildVersion(localOs.Version, localMatches),
            //        Platform = ParsePlatform()
            //    };

            //    if (OperatingSystems.ContainsKey(name))
            //    {
            //        os.ShortName = OperatingSystems.Keys.FirstOrDefault(o=>o.Equals(name));
            //    }
            //    if (OperatingSystems.ContainsValue(name))
            //    {
            //        os.Name = OperatingSystems.Values.FirstOrDefault(o => o.Equals(name));
            //    }

            //    result.Add(os);

            //}
            return result;
        }


        /// <summary>
        /// Returns the operating system family for the given operating system
        /// </summary>
        /// <param name="osLabel">osLabel name or short name</param>
        /// <returns>string|null If null, <see cref="Unknown"/></returns>
        public static string GetOsFamily(string osLabel) 
        {
            if (OperatingSystems.ContainsValue(osLabel))
            {
                osLabel = OperatingSystems.FirstOrDefault(o=>o.Value == osLabel).Key;
            }
            foreach (var family in OsFamilies)
            {
                if (!family.Value.Contains(osLabel)) continue;
                return family.Key;
            }
            return Unknown;
        }

        /// <summary>
        /// Returns true if OS is desktop
        /// </summary>
        /// <param name="osName">OS short name</param>
        /// <returns></returns>
        public static bool IsDesktopOs(string osName)
        {
            var osFamily = GetOsFamily(osName);
            return DesktopOs.Contains(osFamily);
        }

        /// <summary>
        /// Returns the full name for the given short name
        /// </summary>
        /// <param name="os"></param>
        /// <param name="ver"></param>
        /// <returns></returns>
        public static string GetNameFromId(string os, string ver = "")
        {
            if (!OperatingSystems.ContainsKey(os)) return string.Empty;
            var osFullName = OperatingSystems[os];
            return (osFullName + " " + ver).Trim();
        }

        /// <summary>
        /// Returns the OS that can be safely detected from client hints
        /// </summary>
        /// <returns></returns>
        protected OsMatchResult ParseOsFromClientHints()
        {
            var name = string.Empty;
            var version = string.Empty;
            var @short = string.Empty;
            if (ClientHints != null && !string.IsNullOrEmpty(ClientHints.GetOperatingSystem()))
            {
                var hintName = ApplyClientHintMapping(ClientHints.GetOperatingSystem());

                foreach (var operatingSystem in OperatingSystems)
                {
                    if (FuzzyCompare(hintName, operatingSystem.Value))
                    {
                        name = operatingSystem.Value;
                        @short = operatingSystem.Key;
                        break;
                    }
                }

                version = ClientHints.GetOperatingSystemVersion();

                if ("Windows" == name)
                {
                    if (!string.IsNullOrEmpty(version) && version.Split('.').Length > 0)
                    {
                        var majorVersion = version.Split('.').Length > 0 ? int.Parse(version.Split('.')[0]) : 0;
                        var minorVersion = version.Split('.').Length > 1 ? int.Parse(version.Split('.')[1]) : 0;
                        if (majorVersion == 0)
                        {
                            var minorVersionMapping = new Dictionary<int, string> { { 1, "7" }, { 2, "8" }, { 3, "8.1" } };
                            if (minorVersionMapping.TryGetValue(minorVersion, out var value))
                                version = value;

                        }else if (majorVersion > 0 && majorVersion < 11)
                        {
                            version = "10";
                        }
                        else if (majorVersion > 10)
                        {
                            version = "11";
                        }
                    }
                    
                }

                // On Windows, version 0.0.0 can be either 7, 8 or 8.1, so we return 0.0.0
                if ("Windows" != name && version != null && !version.Equals("0.0.0") && version.Equals("0"))
                {
                    version = string.Empty;
                }
            }
            return new OsMatchResult
            {
                Name = name,
                ShortName = @short,
                Version = BuildVersion(version, Array.Empty<string>())
            };
        }

        /// <summary>
        /// Returns the OS that can be detected from useragent
        /// </summary>
        /// <returns></returns>
        protected OsMatchResult ParseOsFromUserAgent()
        {
            var name = string.Empty;
            var version = string.Empty;
            var @short = string.Empty;

            string[] localMatches = null;
            Os localOs = null;

            foreach (var os in regexList)
            {
                var matches = MatchUserAgent(os.Regex);
                if (matches.Length > 0)
                {
                    localOs = os;
                    localMatches = matches;
                    break;
                }
            }

            if (localMatches != null)
            {
                name = BuildByMatch(localOs.Name, localMatches);
                var shortData = GetShortOsData(name);
                name = shortData.Name;
                @short = shortData.ShortName;

                if (!string.IsNullOrEmpty(localOs.Version))
                {
                    version = BuildVersion(localOs.Version, localMatches);
                }
                if (localOs.Versions != null && localOs.Versions.Count > 0)
                {
                    foreach (var regex in localOs.Versions)
                    {
                        var matches = MatchUserAgent(regex.Regex);
                        if (matches.Length == 0)
                        {
                            continue;
                        }

                        //I don't find this logic
                        //if (\array_key_exists('name', $regex)) {
                        //    $name = $this->buildByMatch($regex['name'], $matches);
                        //    ['name' => $name, 'short' => $short] = self::getShortOsData($name);
                        //}
                        if (regex.Version.Length > 0)
                        {
                            version = BuildVersion(regex.Version, matches);
                        }
                        break;
                    }
                }           
            }

            return new OsMatchResult
            {
                Name = name,
                ShortName = @short,
                Version = version
            };
        }


        /// <summary>
        /// Parse current UserAgent string for the operating system platform
        /// </summary>
        /// <returns></returns>
        protected string ParsePlatform()
        {
            // Use architecture from client hints if available
            if (ClientHints != null && !string.IsNullOrEmpty(ClientHints.GetArchitecture()))
            {
                var arch = ClientHints.GetArchitecture().ToLower();
                if (arch.Contains("arm"))
                {
                    return PlatformType.ARM;
                }
                if (arch.Contains("loongarch64"))
                {
                    return PlatformType.LoongArch64;
                }  
                if (arch.Contains("mips"))
                {
                    return PlatformType.MIPS;
                }
                if (arch.Contains("sh4"))
                {
                    return PlatformType.SuperH;
                } 
                if (arch.Contains("sparc64"))
                {
                    return PlatformType.Sparc64;
                }
                if (arch.Contains("x64") || (arch.Contains("x86") && ClientHints.GetBitness() == "64"))
                {
                    return PlatformType.X64;
                }
                if (arch.Contains("x86"))
                {
                    return PlatformType.X86;
                }
            }


            if (IsMatchUserAgent("arm[ _;)ev]|.*arm$|.*arm64|aarch64|Apple ?TV|Watch ?OS|Watch1,[12]"))
            {
                return PlatformType.ARM;
            }
            if (IsMatchUserAgent("loongarch64"))
            {
                return PlatformType.LoongArch64;
            } 
            if (IsMatchUserAgent("mips"))
            {
                return PlatformType.MIPS;
            }
            if (IsMatchUserAgent("sh4"))
            {
                return PlatformType.SuperH;
            } 
            if (IsMatchUserAgent("sparc64"))
            {
                return PlatformType.Sparc64;
            }
            if (IsMatchUserAgent("64-?bit|WOW64|(?:Intel)?x64|WINDOWS_64|win64|.*amd64|.*x86_?64"))
            {
                return PlatformType.X64;
            }
            return IsMatchUserAgent(".*32bit|.*win32|(?:i[0-9]|x)86|i86pc") ? PlatformType.X86 : PlatformType.NONE;
        }
    }
}