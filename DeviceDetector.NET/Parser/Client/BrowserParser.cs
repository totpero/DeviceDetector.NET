using System;
using System.Collections.Generic;
using System.Linq;
using DeviceDetectorNET.Class.Client;
using DeviceDetectorNET.Parser.Client.Browser;
using DeviceDetectorNET.Parser.Client.Browser.Engine;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Client;

namespace DeviceDetectorNET.Parser.Client
{
    public class BrowserParser : ClientParserAbstract<List<Class.Client.Browser>>
    {
        /// <summary>
        /// Known browsers mapped to their internal short codes
        /// </summary>
        protected static Dictionary<string, string> AvailableBrowsers = new Dictionary<string, string>
        {
            {"1B", "115 Browser"},
            {"2B", "2345 Browser"},
            {"36", "360 Phone Browser"},
            {"3B", "360 Browser"},
            {"AA", "Avant Browser"},
            {"AB", "ABrowse"},
            {"AF", "ANT Fresco"},
            {"AG", "ANTGalio"},
            {"AL", "Aloha Browser"},
            {"AH", "Aloha Browser Lite"},
            {"AM", "Amaya"},
            {"AO", "Amigo"},
            {"AN", "Android Browser"},
            {"AE", "AOL Desktop"},
            {"AD", "AOL Shield"},
            {"AR", "Arora"},
            {"AX", "Arctic Fox"},
            {"AV", "Amiga Voyager"},
            {"AW", "Amiga Aweb"},
            {"A0", "Atom"},
            {"AT", "Atomic Web Browser"},
            {"AS", "Avast Secure Browser"},
            {"VG", "AVG Secure Browser"},
            {"BA", "Beaker Browser"},
            {"BM", "Beamrise"},
            {"BB", "BlackBerry Browser"},
            {"BD", "Baidu Browser"},
            {"BS", "Baidu Spark"},
            {"BI", "Basilisk"},
            {"BE", "Beonex"},
            {"BH", "BlackHawk"},
            {"BJ", "Bunjalloo"},
            {"BL", "B-Line"},
            {"BU", "Blue Browser"},
            {"BR", "Brave"},
            {"BK", "BriskBard"},
            {"BX", "BrowseX"},
            {"CA", "Camino"},
            {"CL", "CCleaner"},
            {"C0", "Centaury"},
            {"CC", "Coc Coc"},
            {"C2", "Colibri"},
            {"CD", "Comodo Dragon"},
            {"C1", "Coast"},
            {"CX", "Charon"},
            {"CE", "CM Browser"},
            {"CF", "Chrome Frame"},
            {"HC", "Headless Chrome"},
            {"CH", "Chrome"},
            {"CI", "Chrome Mobile iOS"},
            {"CK", "Conkeror"},
            {"CM", "Chrome Mobile"},
            {"CN", "CoolNovo"},
            {"CO", "CometBird"},
            {"CB", "COS Browser"},
            {"CP", "ChromePlus"},
            {"CR", "Chromium"},
            {"CY", "Cyberfox"},
            {"CS", "Cheshire"},
            {"CT", "Crusta"},
            {"CU", "Cunaguaro"},
            {"CV", "Chrome Webview"},
            {"DB", "dbrowser"},
            {"DE", "Deepnet Explorer"},
            {"DT", "Delta Browser"},
            {"DF", "Dolphin"},
            {"DO", "Dorado"},
            {"DL", "Dooble"},
            {"DI", "Dillo"},
            {"DD", "DuckDuckGo Privacy Browser"},
            {"EC", "Ecosia"},
            {"EI", "Epic"},
            {"EL", "Elinks"},
            {"EB", "Element Browser"},
            {"EE", "Elements Browser"},
            {"EZ", "eZ Browser"},
            {"EU", "EUI Browser"},
            {"EP", "GNOME Web"},
            {"ES", "Espial TV Browser"},
            {"FA", "Falkon"},
            {"FX", "Faux Browser"},
            {"F1", "Firefox Mobile iOS"},
            {"FB", "Firebird"},
            {"FD", "Fluid"},
            {"FE", "Fennec"},
            {"FF", "Firefox"},
            {"FK", "Firefox Focus"},
            {"FY", "Firefox Reality"},
            {"FR", "Firefox Rocket"},
            {"FL", "Flock"},
            {"FM", "Firefox Mobile"},
            {"FW", "Fireweb"},
            {"FN", "Fireweb Navigator"},
            {"FU", "FreeU"},
            {"GA", "Galeon"},
            {"GB", "Glass Browser"},
            {"GE", "Google Earth"},
            {"HA", "Hawk Turbo Browser"},
            {"HO", "hola! Browser"},
            {"HJ", "HotJava"},
            {"HU", "Huawei Browser"},
            {"IB", "IBrowse"},
            {"IC", "iCab"},
            {"I2", "iCab Mobile"},
            {"I1", "Iridium"},
            {"I3", "Iron Mobile"},
            {"I4", "IceCat"},
            {"ID", "IceDragon"},
            {"IV", "Isivioo"},
            {"IW", "Iceweasel"},
            {"IE", "Internet Explorer"},
            {"IM", "IE Mobile"},
            {"IR", "Iron"},
            {"JS", "Jasmine"},
            {"JI", "Jig Browser"},
            {"JP", "Jig Browser Plus"},
            {"JO", "Jio Browser"},
            {"KB", "K.Browser"},
            {"KI", "Kindle Browser"},
            {"KM", "K-meleon"},
            {"KO", "Konqueror"},
            {"KP", "Kapiko"},
            {"KN", "Kinza"},
            {"KW", "Kiwi"},
            {"KY", "Kylo"},
            {"KZ", "Kazehakase"},
            {"LB", "Cheetah Browser"},
            {"LF", "LieBaoFast"},
            {"LG", "LG Browser"},
            {"LH", "Light"},
            {"LI", "Links"},
            {"LO", "Lovense Browser"},
            {"LU", "LuaKit"},
            {"LL", "Lulumi"},
            {"LS", "Lunascape"},
            {"LN", "Lunascape Lite"},
            {"LX", "Lynx"},
            {"M1", "mCent"},
            {"MB", "MicroB"},
            {"MC", "NCSA Mosaic"},
            {"MZ", "Meizu Browser"},
            {"ME", "Mercury"},
            {"MF", "Mobile Safari"},
            {"MI", "Midori"},
            {"MO", "Mobicip"},
            {"MU", "MIUI Browser"},
            {"MS", "Mobile Silk"},
            {"MN", "Minimo"},
            {"MT", "Mint Browser"},
            {"MX", "Maxthon"},
            {"MY", "Mypal"},
            {"NB", "Nokia Browser"},
            {"NO", "Nokia OSS Browser"},
            {"NV", "Nokia Ovi Browser"},
            {"NX", "Nox Browser"},
            {"NE", "NetSurf"},
            {"NF", "NetFront"},
            {"NL", "NetFront Life"},
            {"NP", "NetPositive"},
            {"NS", "Netscape"},
            {"NT", "NTENT Browser"},
            {"OC", "Oculus Browser"},
            {"O1", "Opera Mini iOS"},
            {"OB", "Obigo"},
            {"OD", "Odyssey Web Browser"},
            {"OF", "Off By One"},
            {"HH", "OhHai Browser"},
            {"OE", "ONE Browser"},
            {"OX", "Opera GX"},
            {"OG", "Opera Neon"},
            {"OH", "Opera Devices"},
            {"OI", "Opera Mini"},
            {"OM", "Opera Mobile"},
            {"OP", "Opera"},
            {"ON", "Opera Next"},
            {"OO", "Opera Touch"},
            {"OS", "Ordissimo"},
            {"OR", "Oregano"},
            {"O0", "Origin In-Game Overlay"},
            {"OY", "Origyn Web Browser"},
            {"OV", "Openwave Mobile Browser"},
            {"OW", "OmniWeb"},
            {"OT", "Otter Browser"},
            {"PL", "Palm Blazer"},
            {"PM", "Pale Moon"},
            {"PY", "Polypane"},
            {"PP", "Oppo Browser"},
            {"PR", "Palm Pre"},
            {"PU", "Puffin"},
            {"PW", "Palm WebPro"},
            {"PA", "Palmscape"},
            {"PX", "Phoenix"},
            {"PO", "Polaris"},
            {"PT", "Polarity"},
            {"PS", "Microsoft Edge"},
            {"Q1", "QQ Browser Mini"},
            {"QQ", "QQ Browser"},
            {"QT", "Qutebrowser"},
            {"QU", "Quark"},
            {"QZ", "QupZilla"},
            {"QM", "Qwant Mobile"},
            {"QW", "QtWebEngine"},
            {"RE", "Realme Browser"},
            {"RK", "Rekonq"},
            {"RM", "RockMelt"},
            {"SB", "Samsung Browser"},
            {"SA", "Sailfish Browser"},
            {"SC", "SEMC-Browser"},
            {"SE", "Sogou Explorer"},
            {"SF", "Safari"},
            {"S5", "Safe Exam Browser"},
            {"SW", "SalamWeb"},
            {"SH", "Shiira"},
            {"S1", "SimpleBrowser"},
            {"SY", "Sizzy"},
            {"SK", "Skyfire"},
            {"SS", "Seraphic Sraf"},
            {"SL", "Sleipnir"},
            {"SN", "Snowshoe"},
            {"SO", "Sogou Mobile Browser"},
            {"S2", "Splash"},
            {"SI", "Sputnik Browser"},
            {"SR", "Sunrise"},
            {"SP", "SuperBird"},
            {"SU", "Super Fast Browser"},
            {"S3", "surf"},
            {"S0", "START Internet Browser"},
            {"S4", "Steam In-Game Overlay"},
            {"ST", "Streamy"},
            {"SX", "Swiftfox"},
            {"SZ", "Seznam Browser"},
            {"TO", "t-online.de Browser"},
            {"TA", "Tao Browser"},
            {"TF", "TenFourFox"},
            {"TB", "Tenta Browser"},
            {"TZ", "Tizen Browser"},
            {"TU", "Tungsten"},
            {"TG", "ToGate"},
            {"TS", "TweakStyle"},
            {"TV", "TV Bro"},
            {"UB", "UBrowser"},
            {"UC", "UC Browser"},
            {"UM", "UC Browser Mini"},
            {"UT", "UC Browser Turbo"},
            {"UZ", "Uzbl"},
            {"VI", "Vivaldi"},
            {"VV", "vivo Browser"},
            {"VB", "Vision Mobile Browser"},
            {"VM", "VMware AirWatch"},
            {"WI", "Wear Internet Browser"},
            {"WP", "Web Explorer"},
            {"WE", "WebPositive"},
            {"WF", "Waterfox"},
            {"WH", "Whale Browser"},
            {"WO", "wOSBrowser"},
            {"WT", "WeTab Browser"},
            {"YJ", "Yahoo! Japan Browser"},
            {"YA", "Yandex Browser"},
            {"YL", "Yandex Browser Lite"},
            {"YN", "Yaani Browser"},
            {"XI", "Xiino"},
            {"XV", "Xvast"},
            {"ZV", "Zvu"},

            // detected browsers in older versions
            // {"IA","Iceape"},  => pim
            // {"SM","SeaMonkey"},  => pim
        };

        /// <summary>
        /// Browser families mapped to the short codes of the associated browsers
        /// </summary>
        protected static Dictionary<string, string[]> BrowserFamilies = new Dictionary<string, string[]>
        {
            {"Android Browser"    , new []{"AN", "MU"}},
            {"BlackBerry Browser" , new []{"BB"}},
            {"Baidu"              , new []{"BD", "BS"}},
            {"Amiga"              , new []{"AV", "AW"}},
            {"Chrome"             , new []{"CH", "BA", "BR", "CC", "CD", "CM", "CI", "CF", "CN", "CR", "CP", "DD", "IR", "RM", "AO", "TS", "VI", "PT", "AS", "TB", "AD", "SB", "WP", "I3", "CV", "WH", "SZ", "QW", "LF", "KW", "2B", "CE", "EC", "MT", "MS", "HA", "OC", "MZ", "BM", "KN", "SW", "M1", "FA", "TA", "AH", "CL", "SU", "EU", "UB", "LO", "VG", "TV", "A0", "1B", "S4", "EE", "AE", "VM", "O0", "TG", "GB", "SY", "HH", "YJ", "LL", "TU", "XV", "C2", "QU", "YN"}},
            {"Firefox"            , new []{"FF", "FE", "FM", "SX", "FB", "PX", "MB", "EI", "WF", "CU", "TF", "QM", "FR", "I4", "GZ", "MO", "F1", "BI", "MN", "BH", "TO", "OS", "MY", "FY", "AX", "C0", "LH", "S5", "ZV''FF", "FE", "FM", "SX", "FB", "PX", "MB", "EI", "WF", "CU", "TF", "QM", "FR", "I4", "GZ", "MO", "F1", "BI", "MN", "BH", "TO", "OS", "MY", "FY", "AX", "C0", "LH", "S5", "ZV"}},
            {"Internet Explorer"  , new []{"IE", "IM", "PS"}},
            {"Konqueror"          , new []{"KO"}},
            {"NetFront"           , new []{"NF"}},
            {"Nokia Browser"      , new []{"NB", "NO", "NV", "DO"}},
            {"Opera"              , new []{"OP", "OM", "OI", "ON", "OO","OG", "OH", "O1", "OX"}},
            {"Safari"             , new []{"SF", "MF","SO"}},
            {"Sailfish Browser"   , new []{"SA"}}
        };

        /// <summary>
        /// Browsers that are available for mobile devices only
        /// </summary>
        protected static string[] MobileOnlyBrowsers = { "36", "OC", "PU", "SK", "MF", "OI", "OM", "DD", "DB", "ST", "BL", "IV", "FM", "C1", "AL", "SA", "SB", "FR", "WP", "HA", "NX", "HU", "VV", "RE", "CB", "MZ", "UM", "FK", "FX", "WI", "MN", "M1", "AH", "SU", "EU", "EZ", "UT", "DT", "S0", "QU", "YN" };

        public BrowserParser()
        {
            FixtureFile = "regexes/client/browsers.yml";
            ParserName = "browser";
            regexList = GetRegexes();
            //regexList = regexList.Select(r =>
            //{
            //    r.Engine.Versions = r.Engine.Versions ?? new Dictionary<string, string>();
            //    return r;
            //}).ToList();
        }

        //public BrowserParser(string ua): base(ua)
        //{

        //}

        /// <summary>
        /// Returns list of all available browsers
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetAvailableBrowsers()
        {
            return AvailableBrowsers;
        }

        /// <summary>
        /// Returns list of all available browser families
        /// </summary>
        public static Dictionary<string, string[]> GetAvailableBrowserFamilies()
        {
            return BrowserFamilies;
        }

        ///  <summary>
        ///
        ///  </summary>
        ///  <param name="browserLabel"></param>
        /// <param name="name"></param>
        /// <returns>bool|string If false, "Unknown"</returns>
        public static bool GetBrowserFamily(string browserLabel , out string name) //TryGetBrowserFamily
        {
            foreach (var family in BrowserFamilies)
            {
                if (!family.Value.Contains(browserLabel)) continue;
                name = family.Key;
                return true;
            }
            name = "Unknown";
            return false;
        }

        /// <summary>
        ///  Returns if the given browser is mobile only
        /// </summary>
        /// <param name="browser">Label or name of browser</param>
        /// <returns></returns>
        public static bool IsMobileOnlyBrowser(string browser)
        {
            //@todo:unfinished
            //return in_array($browser, self::$mobileOnlyBrowsers) || (in_array($browser, self::$availableBrowsers) && in_array(array_search($browser, self::$availableBrowsers), self::$mobileOnlyBrowsers));
            return MobileOnlyBrowsers.Contains(browser) || (AvailableBrowsers.ContainsKey(browser) && MobileOnlyBrowsers.Contains(AvailableBrowsers[browser]));
        }

        public override ParseResult<ClientMatchResult> Parse()
        {
            var result = new ParseResult<ClientMatchResult>();
            Class.Client.Browser localBrowser = null;
            string[] localMatches = null;
            foreach (var browser in regexList)
            {
                var matches = MatchUserAgent(browser.Regex);
                if (matches.Length > 0)
                {
                    localBrowser = browser;
                    localMatches = matches;
                    break;
                }
            }
            if (localMatches != null)
            {
                var name = BuildByMatch(localBrowser.Name, localMatches);

                foreach (var availableBrowser in AvailableBrowsers)
                {
                    if (string.Equals(name, availableBrowser.Value, StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (localBrowser.Engine == null)
                            localBrowser.Engine = new Engine();
                        var version = BuildVersion(localBrowser.Version, localMatches);
                        var engine = BuildEngine(localBrowser.Engine, version);
                        var engineVersion = BuildEngineVersion(engine);
                        result.Add(new BrowserMatchResult
                        {
                            Type = ParserName,
                            Name = name,
                            Version = version,
                            ShortName = availableBrowser.Key,
                            Engine = engine,
                            EngineVersion = engineVersion
                        });
                    }
                }
            }

            return result;
            throw new Exception("Detected browser name was not found in AvailableBrowsers. Tried to parse user agent: "+UserAgent);
        }

        protected string BuildEngine(Engine engineData, string browserVersion)
        {
            var engine = string.Empty;
            // if an engine is set as default
            if (!string.IsNullOrEmpty(engineData.Default))
            {
                engine = engineData.Default;
            }

            // check if engine is set for browser version
            if (engineData.Versions != null && engineData.Versions.Count > 0)
            {
                foreach (var version in engineData.Versions)
                {
                    if (string.IsNullOrEmpty(browserVersion)) continue;

                    var ver = !version.Key.Contains(".") ? version.Key + ".0" : version.Key;

                    if (browserVersion.EndsWith(".", StringComparison.Ordinal))
                        browserVersion = browserVersion.TrimEnd('.');

                    browserVersion = !browserVersion.Contains(".") ? browserVersion + ".0" : browserVersion;

                    if (new Version(browserVersion).CompareTo(new Version(ver)) >= 0)
                    {
                        engine = version.Value;
                    }
                }
            }

            if (string.IsNullOrEmpty(engine))
            {
                var engineParser = new EngineParser();
                engineParser.SetUserAgent(UserAgent);
                var engineResult = engineParser.Parse();
                if (engineResult.Success)
                {
                    engine = engineResult.Match.Name;
                }
            }
           
            return engine;
        }

        protected string BuildEngineVersion(string engine)
        {
            var engineVersion = new VersionParser(UserAgent,engine);
            var result = engineVersion.Parse();
            return result.Success? result.Match.Name : string.Empty;
        }
    }
}
