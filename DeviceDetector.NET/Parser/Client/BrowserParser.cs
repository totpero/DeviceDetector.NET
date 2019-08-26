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
    public class BrowserParser : ClientParserAbstract<List<Class.Client.Browser>, BrowserMatchResult>
    {
        /// <summary>
        /// Known browsers mapped to their internal short codes
        /// </summary>
        protected static Dictionary<string, string> AvailableBrowsers = new Dictionary<string, string>
        {
            {"36","360 Phone Browser"},
            {"3B","360 Browser"},
            {"AA","Avant Browser"},
            {"AB","ABrowse"},
            {"AF","ANT Fresco"},
            {"AG","ANTGalio"},
            {"AL","Aloha Browser"},
            {"AM","Amaya"},
            {"AO","Amigo"},
            {"AN","Android Browser"},
            {"AD","AOL Shield"},
            {"AR","Arora"},
            {"AV","Amiga Voyager"},
            {"AW","Amiga Aweb"},
            {"AT","Atomic Web Browser"},
            {"AS","Avast Secure Browser"},
            {"BA","Beaker Browser"},
            {"BB","BlackBerry Browser"},
            {"BD","Baidu Browser"},
            {"BS","Baidu Spark"},
            {"BE","Beonex"},
            {"BJ","Bunjalloo"},
            {"BL","B-Line"},
            {"BR","Brave"},
            {"BK","BriskBard"},
            {"BX","BrowseX"},
            {"CA","Camino"},
            {"CC","Coc Coc"},
            {"CD","Comodo Dragon"},
            {"C1","Coast"},
            {"CX","Charon"},
            {"CF","Chrome Frame"},
            {"HC","Headless Chrome"},
            {"CH","Chrome"},
            {"CI","Chrome Mobile iOS"},
            {"CK","Conkeror"},
            {"CM","Chrome Mobile"},
            {"CN","CoolNovo"},
            {"CO","CometBird"},
            {"CP","ChromePlus"},
            {"CR","Chromium"},
            {"CY","Cyberfox"},
            {"CS","Cheshire"},
            {"CU","Cunaguaro"},
            {"DB","dbrowser"},
            {"DE","Deepnet Explorer"},
            {"DF","Dolphin"},
            {"DO","Dorado"},
            {"DL","Dooble"},
            {"DI","Dillo"},
            {"EI","Epic"},
            {"EL","Elinks"},
            {"EB","Element Browser"},
            {"EP","GNOME Web"},
            {"ES","Espial TV Browser"},
            {"FB","Firebird"},
            {"FD","Fluid"},
            {"FE","Fennec"},
            {"FF","Firefox"},
            {"FK","Firefox Focus"},
            {"FL","Flock"},
            {"FM","Firefox Mobile"},
            {"FW","Fireweb"},
            {"FN","Fireweb Navigator"},
            {"GA","Galeon"},
            {"GE","Google Earth"},
            {"HJ","HotJava"},
            {"IA","Iceape"},
            {"IB","IBrowse"},
            {"IC","iCab"},
            {"I2","iCab Mobile"},
            {"I1","Iridium"},
            {"ID","IceDragon"},
            {"IV","Isivioo"},
            {"IW","Iceweasel"},
            {"IE","Internet Explorer"},
            {"IM","IE Mobile"},
            {"IR","Iron"},
            {"JS","Jasmine"},
            {"JI","Jig Browser"},
            {"KI","Kindle Browser"},
            {"KM","K-meleon"},
            {"KO","Konqueror"},
            {"KP","Kapiko"},
            {"KY","Kylo"},
            {"KZ","Kazehakase"},
            {"LB","Liebao"},
            {"LG","LG Browser"},
            {"LI","Links"},
            {"LU","LuaKit"},
            {"LS","Lunascape"},
            {"LX","Lynx"},
            {"MB","MicroB"},
            {"MC","NCSA Mosaic"},
            {"ME","Mercury"},
            {"MF","Mobile Safari"},
            {"MI","Midori"},
            {"MU","MIUI Browser"},
            {"MS","Mobile Silk"},
            {"MX","Maxthon"},
            {"NB","Nokia Browser"},
            {"NO","Nokia OSS Browser"},
            {"NV","Nokia Ovi Browser"},
            {"NE","NetSurf"},
            {"NF","NetFront"},
            {"NL","NetFront Life"},
            {"NP","NetPositive"},
            {"NS","Netscape"},
            {"NT","NTENT Browser"},
            {"OB","Obigo"},
            {"OD","Odyssey Web Browser"},
            {"OF","Off By One"},
            {"OE","ONE Browser"},
            {"OI","Opera Mini"},
            {"OM","Opera Mobile"},
            {"OP","Opera"},
            {"ON","Opera Next"},
            {"OO","Opera Touch"},
            {"OR","Oregano"},
            {"OV","Openwave Mobile Browser"},
            {"OW","OmniWeb"},
            {"OT","Otter Browser"},
            {"PL","Palm Blazer"},
            {"PM","Pale Moon"},
            {"PP","Oppo Browser"},
            {"PR","Palm Pre"},
            {"PU","Puffin"},
            {"PW","Palm WebPro"},
            {"PA","Palmscape"},
            {"PX","Phoenix"},
            {"PO","Polaris"},
            {"PT","Polarity"},
            {"PS","Microsoft Edge"},
            {"QQ","QQ Browser"},
            {"QT","Qutebrowser"},
            {"QZ","QupZilla"},
            {"QM","Qwant Mobile"},
            {"RK","Rekonq"},
            {"RM","RockMelt"},
            {"SB","Samsung Browser"},
            {"SA","Sailfish Browser"},
            {"SC","SEMC-Browser"},
            {"SE","Sogou Explorer"},
            {"SF","Safari"},
            {"SH","Shiira"},
            {"SK","Skyfire"},
            {"SS","Seraphic Sraf"},
            {"SL","Sleipnir"},
            {"SM","SeaMonkey"},
            {"SN","Snowshoe"},
            {"SR","Sunrise"},
            {"SP","SuperBird"},
            {"ST","Streamy"},
            {"SX","Swiftfox"},
            {"TF","TenFourFox"},
            {"TB","Tenta Browser"},
            {"TZ","Tizen Browser"},
            {"TS","TweakStyle"},
            {"UC","UC Browser"},
            {"VI","Vivaldi"},
            {"VB","Vision Mobile Browser"},
            {"WE","WebPositive"},
            {"WF","Waterfox"},
            {"WO","wOSBrowser"},
            {"WT","WeTab Browser"},
            {"YA","Yandex Browser"},
            {"XI","Xiino" }
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
            {"Chrome"             , new []{"CH", "BA", "BR", "CC", "CD", "CM", "CI", "CF", "CN", "CR", "CP", "IR", "RM", "AO", "TS", "VI", "PT", "AS", "TB", "AD"}},
            {"Firefox"            , new []{"FF", "FE", "FM", "SX", "FB", "PX", "MB", "EI", "WF", "CU", "TF", "QM"}},
            {"Internet Explorer"  , new []{"IE", "IM", "PS"}},
            {"Konqueror"          , new []{"KO"}},
            {"NetFront"           , new []{"NF"}},
            {"Nokia Browser"      , new []{"NB", "NO", "NV", "DO"}},
            {"Opera"              , new []{"OP", "OM", "OI", "ON", "OO"}},
            {"Safari"             , new []{"SF", "MF"}},
            {"Sailfish Browser"   , new []{"SA"}}
        };

        /// <summary>
        /// Browsers that are available for mobile devices only
        /// </summary>
        protected static string[] MobileOnlyBrowsers = { "36", "PU", "SK", "MF", "OI", "OM", "DB", "ST", "BL", "IV", "FM", "C1", "AL" };

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

        public override ParseResult<BrowserMatchResult> Parse()
        {
            var result = new ParseResult<BrowserMatchResult>();
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
                        var version = BuildVersion(localBrowser.Version, localMatches);
                        var engine = BuildEngine(localBrowser.Engine ?? new Engine(), version);
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
            throw new Exception("Detected browser name was not found in AvailableBrowsers");
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