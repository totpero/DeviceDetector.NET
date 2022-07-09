using System;
using System.Collections.Generic;
using System.Linq;
using DeviceDetectorNET.Class;
using DeviceDetectorNET.Results;

namespace DeviceDetectorNET.Parser
{
    public class OperatingSystemParser : ParserAbstract<List<Os>, OsMatchResult>
    {

        private const string Unknown = "Unknown";
        private const string UnknownShort = "UNK";

        /// <summary>
        /// Known operating systems mapped to their internal short codes
        /// </summary>
        protected static readonly Dictionary<string, string> OperatingSystems = new Dictionary<string, string>()
        {
            {"AIX", "AIX"},
            {"AND", "Android"},
            {"ADR", "Android TV"},
            {"AMZ", "Amazon Linux"},
            {"AMG", "AmigaOS"},
            {"ATV", "tvOS"},
            {"ARL", "Arch Linux"},
            {"BTR", "BackTrack"},
            {"SBA", "Bada"},
            {"BEO", "BeOS"},
            {"BLB", "BlackBerry OS"},
            {"QNX", "BlackBerry Tablet OS"},
            {"BOS", "Bliss OS"},
            {"BMP", "Brew"},
            {"CAI", "Caixa Mágica"},
            {"CES", "CentOS"},
            {"CST", "CentOS Stream"},
            {"CLR", "ClearOS Mobile"},
            {"COS", "Chrome OS"},
            {"CRS", "Chromium OS"},
            {"CHN", "China OS"},
            {"CYN", "CyanogenMod"},
            {"DEB", "Debian"},
            {"DEE", "Deepin"},
            {"DFB", "DragonFly"},
            {"DVK", "DVKBuntu"},
            {"FED", "Fedora"},
            {"FEN", "Fenix"},
            {"FOS", "Firefox OS"},
            {"FIR", "Fire OS"},
            {"FOR", "Foresight Linux"},
            {"FRE", "Freebox"},
            {"BSD", "FreeBSD"},
            {"FYD", "FydeOS"},
            {"FUC", "Fuchsia"},
            {"GNT", "Gentoo"},
            {"GRI", "GridOS"},
            {"GTV", "Google TV"},
            {"HPX", "HP-UX"},
            {"HAI", "Haiku OS"},
            {"IPA", "iPadOS"},
            {"HAR", "HarmonyOS"},
            {"HAS", "HasCodingOS"},
            {"IRI", "IRIX"},
            {"INF", "Inferno"},
            {"JME", "Java ME"},
            {"KOS", "KaiOS"},
            {"KAN", "Kanotix"},
            {"KNO", "Knoppix"},
            {"KTV", "KreaTV"},
            {"KBT", "Kubuntu"},
            {"LIN", "GNU/Linux"},
            {"LND", "LindowsOS"},
            {"LNS", "Linspire"},
            {"LEN", "Lineage OS"},
            {"LBT", "Lubuntu"},
            {"LOS", "Lumin OS"},
            {"VLN", "VectorLinux"},
            {"MAC", "Mac"},
            {"MAE", "Maemo"},
            {"MAG", "Mageia"},
            {"MDR", "Mandriva"},
            {"SMG", "MeeGo"},
            {"MCD", "MocorDroid"},
            {"MON", "moonOS"},
            {"MIN", "Mint"},
            {"MLD", "MildWild"},
            {"MOR", "MorphOS"},
            {"NBS", "NetBSD"},
            {"MTK", "MTK / Nucleus"},
            {"MRE", "MRE"},
            {"WII", "Nintendo"},
            {"NDS", "Nintendo Mobile"},
            {"NOV", "Nova"},
            {"OS2", "OS/2"},
            {"T64", "OSF1"},
            {"OBS", "OpenBSD"},
            {"OWR", "OpenWrt"},
            {"OTV", "Opera TV"},
            {"ORD", "Ordissimo"},
            {"PAR", "Pardus"},
            {"PCL", "PCLinuxOS"},
            {"PLA", "Plasma Mobile"},
            {"PSP", "PlayStation Portable"},
            {"PS3", "PlayStation"},
            {"PUR", "PureOS"},
            {"RHT", "Red Hat"},
            {"REV", "Revenge OS"},
            {"ROS", "RISC OS"},
            {"ROK", "Roku OS"},
            {"RSO", "Rosa"},
            {"REM", "Remix OS"},
            {"REX", "REX"},
            {"RZD", "RazoDroiD"},
            {"SAB", "Sabayon"},
            {"SSE", "SUSE"},
            {"SAF", "Sailfish OS"},
            {"SEE", "SeewoOS"},
            {"SLW", "Slackware"},
            {"SOS", "Solaris"},
            {"SYL", "Syllable"},
            {"SYM", "Symbian"},
            {"SYS", "Symbian OS"},
            {"S40", "Symbian OS Series 40"},
            {"S60", "Symbian OS Series 60"},
            {"SY3", "Symbian^3"},
            {"TEN", "TencentOS"},
            {"TDX", "ThreadX"},
            {"TIZ", "Tizen"},
            {"TOS", "TmaxOS"},
            {"UBT", "Ubuntu"},
            {"WAS", "watchOS"},
            {"WTV", "WebTV"},
            {"WHS", "Whale OS"},
            {"WIN", "Windows"},
            {"WCE", "Windows CE"},
            {"WIO", "Windows IoT"},
            {"WMO", "Windows Mobile"},
            {"WPH", "Windows Phone"},
            {"WRT", "Windows RT"},
            {"XBX", "Xbox"},
            {"XBT", "Xubuntu"},
            {"YNS", "YunOS"},
            {"ZEN", "Zenwalk"},
            {"IOS", "iOS"},
            {"POS", "palmOS"},
            {"WOS", "webOS"}
        };

        /// <summary>
        /// Operating system families mapped to the short codes of the associated operating systems
        /// </summary>
        protected static readonly Dictionary<string, string[]> OsFamilies = new Dictionary<string, string[]>
        {
            {"Android"              , new [] {"AND", "CYN", "FIR", "REM", "RZD", "MLD", "MCD", "YNS", "GRI", "HAR",
                                                "ADR", "CLR", "BOS", "REV", "LEN"}},
            {"AmigaOS"              , new [] {"AMG", "MOR"}},
            //{"Apple TV"             , new [] {"ATV"}},
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
                                                "NOV"}},
            {"Mac"                  , new [] {"MAC"}},
            {"Mobile Gaming Console", new [] {"PSP", "NDS", "XBX"}},
            {"Real-time OS"         , new [] {"MTK", "TDX", "MRE", "JME", "REX"}},
            {"Other Mobile"         , new [] {"WOS", "POS", "SBA", "TIZ", "SMG", "MAE"}},
            {"Symbian"              , new [] {"SYM", "SYS", "SY3", "S60", "S40"}},
            {"Unix"                 , new [] {"SOS", "AIX", "HPX", "BSD", "NBS", "OBS", "DFB", "SYL", "IRI", "T64", "INF"}},
            {"WebTV"                , new [] {"WTV"}},
            {"Windows"              , new [] {"WIN"}},
            {"Windows Mobile"       , new [] {"WPH", "WMO", "WCE", "WRT", "WIO"}},
            {"Other Smart TV"       , new [] {"WHS"}}
        };

        /// <summary>
        /// Contains a list of mappings from OS names we use to known client hint values
        /// </summary>
        protected static readonly Dictionary<string, string[]> ClientHintMapping = new Dictionary<string, string[]>
        {
             {"GNU/Linux", new [] {"Linux"}},
             {"Mac"      , new [] {"MacOS"}},
        };

        /// <summary>
        /// Operating system families that are known as desktop only
        /// </summary>
        protected static readonly string[] DesktopOs = new string[]
        {
            "AmigaOS", "IBM", "GNU/Linux", "Mac", "Unix", "Windows", "BeOS", "Chrome OS", "Chromium OS"
        };

        /// <summary>
        /// Returns all available operating systems
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetAvailableOperatingSystems()
        {
            return OperatingSystems;
        }

        /// <summary>
        /// Returns all available operating system families
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string[]> GetAvailableOperatingSystemFamilies()
        {
            return OsFamilies;
        }

        /// <summary>
        /// Returns the os name and shot name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string[] getShortOsData(string name)
        {
            throw new NotImplementedException();

            //name = name.ToLower();
            //foreach (var operatingSystem in OperatingSystems)
            //{
            //    if (name != operatingSystem.Value)
            //    {
            //        continue;
            //    }
            //}
            //return UnknownShort;
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
            Os localOs = null;

            string[] localMatches = null;

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
                var name = BuildByMatch(localOs.Name, localMatches);
                var @short = UnknownShort;
                foreach (var operatingSystem in OperatingSystems)
                {
                    if (operatingSystem.Value.ToLower().Equals(name.ToLower()))
                    {
                        name = operatingSystem.Value;
                        @short = operatingSystem.Key;
                    }
                }
                var os = new OsMatchResult
                {
                    Name = name,
                    ShortName = @short,
                    Version = BuildVersion(localOs.Version, localMatches),
                    Platform = ParsePlatform()
                };

                if (OperatingSystems.ContainsKey(name))
                {
                    os.ShortName = OperatingSystems.Keys.FirstOrDefault(o=>o.Equals(name));
                }
                if (OperatingSystems.ContainsValue(name))
                {
                    os.Name = OperatingSystems.Values.FirstOrDefault(o => o.Equals(name));
                }

                result.Add(os);

            }
            return result;
        }

        /// <summary>
        /// Parse current UserAgent string for the operating system platform
        /// </summary>
        /// <returns></returns>
        protected string ParsePlatform()
        {
            //@todo:clientHints
            // Use architecture from client hints if available

            if (IsMatchUserAgent("arm|aarch64|Apple ?TV|Watch ?OS|Watch1,[12]")) {
                return PlatformType.ARM;
            }
            if (IsMatchUserAgent("mips"))
            {
                return PlatformType.MIPS;
            }
            if (IsMatchUserAgent("sh4"))
            {
                return PlatformType.SuperH;
            }
            if (IsMatchUserAgent("64-?bit|WOW64|(?:Intel)?x64|WINDOWS_64|win64|amd64|x86_?64")) {
                return PlatformType.X64;
            }
            return IsMatchUserAgent(".+32bit|.+win32|(?:i[0-9]|x)86|i86pc") ? PlatformType.X86 : PlatformType.NONE;
        }

        /// <summary>
        /// Returns the operating system family for the given operating system
        /// </summary>
        /// <param name="osLabel">name or short name</param>
        /// <param name="name"></param>
        /// <returns>bool|string If false, <see cref="Unknown"/></returns>
        public static bool GetOsFamily(string osLabel, out string name) //TryGetBrowserFamily
        {
            if (OperatingSystems.ContainsKey(osLabel))
            {
                //@todo..
            }
            foreach (var family in OsFamilies)
            {
                if (!family.Value.Contains(osLabel)) continue;
                name = family.Key;
                return true;
            }
            name = Unknown;
            return false;
        }

        /// <summary>
        /// Returns true if OS is desktop
        /// </summary>
        /// <param name="osName">OS short name</param>
        /// <returns></returns>
        public static bool IsDesktopOs(string osName)
        {
            throw new NotImplementedException();
            //@todo:...
            //var osFamily = GetOsFamily(osName);
            //return \in_array($osFamily, self::$desktopOsArray);
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
    }
}