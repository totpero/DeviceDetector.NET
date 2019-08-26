using System;
using System.Collections.Generic;
using System.Linq;
using DeviceDetectorNET.Cache;
using DeviceDetectorNET.Class.Device;
using DeviceDetectorNET.Parser;
using DeviceDetectorNET.Parser.Client;
using DeviceDetectorNET.Parser.Device;
using DeviceDetectorNET.RegexEngine;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Client;
using DeviceDetectorNET.Results.Device;
using YamlDotNet.Core;

namespace DeviceDetectorNET
{
    public class DeviceDetector
    {
        /// <summary>
        /// Current version number of DeviceDetector
        /// </summary>
        public const string VERSION = "3.11.6";

        /// <summary>
        /// Operating system families that are known as desktop only
        /// </summary>
        protected string[] desktopOsArray =
        {
            "AmigaOS",
            "IBM",
            "GNU/Linux",
            "Mac",
            "Unix",
            "Windows",
            "BeOS",
            "Chrome OS"
        };

        /// <summary>
        /// Constant used as value for unknown browser / os
        /// </summary>
        public const string UNKNOWN = "UNK";

        /// <summary>
        /// Holds the useragent that should be parsed
        /// </summary>
        protected string userAgent;

        /// <summary>
        /// Holds the operating system data after parsing the UA
        /// </summary>
        protected ParseResult<OsMatchResult> os = new ParseResult<OsMatchResult>();

        /// <summary>
        /// Holds the client data after parsing the UA
        /// </summary>
        protected ParseResult<ClientMatchResult> client = new ParseResult<ClientMatchResult>();

        /// <summary>
        /// Holds the device type after parsing the UA
        /// </summary>
        protected int? device;

        /// <summary>
        /// Holds the device brand data after parsing the UA
        /// </summary>
        protected string brand = string.Empty;

        /// <summary>
        /// Holds the device model data after parsing the UA
        /// </summary>
        protected string model = string.Empty;

        /// <summary>
        /// Holds bot information if parsing the UA results in a bot
        /// (All other information attributes will stay empty in that case)
        ///
        /// If $discardBotInformation is set to true, this property will be set to
        /// true if parsed UA is identified as bot, additional information will be not available
        ///
        /// If $skipBotDetection is set to true, bot detection will not be performed and isBot will
        /// always be false
        /// </summary>
        protected ParseResult<BotMatchResult> bot = new ParseResult<BotMatchResult>();

        protected bool discardBotInformation;

        protected bool skipBotDetection;

        /// <summary>
        /// Holds the cache class used for caching the parsed yml-Files
        /// </summary>
        protected ICache cache;
        protected IRegexEngine regexEngine;

        protected IParser yamlParser;

        protected List<IClientParserAbstract> clientParsers = new List<IClientParserAbstract>();

        protected List<IDeviceParserAbstract> deviceParsers = new List<IDeviceParserAbstract>();

        protected List<IBotParserAbstract> botParsers = new List<IBotParserAbstract>();

        protected bool parsed;

        /// <summary>
        ///
        /// </summary>
        /// <param name="userAgent">UA to parse</param>
        public DeviceDetector(string userAgent = "")
        {
            if (!string.IsNullOrEmpty(userAgent))
            {
                SetUserAgent(userAgent);
            }

            AddStandardClientsParser();
            AddStandardDevicesParser();

            botParsers.Add(new BotParser());
        }

        //@todo:need implemented
        public bool Is(ClientType type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the useragent to be parsed
        /// </summary>
        /// <param name="userAgent"></param>
        public void SetUserAgent(string userAgent)
        {
            if (this.userAgent != userAgent) {
                Reset();
            }
            this.userAgent = userAgent;
        }

        private void Reset()
        {
           bot = new ParseResult<BotMatchResult>();
           client = new ParseResult<ClientMatchResult>();
           device =  null;
           os = new ParseResult<OsMatchResult>();
           brand = string.Empty;
           model = string.Empty;
           parsed = false;
        }


        public void AddStandardClientsParser()
        {
            clientParsers.Add(ClientType.FeedReader.Client);
            clientParsers.Add(ClientType.MobileApp.Client);
            clientParsers.Add(ClientType.MediaPlayer.Client);
            clientParsers.Add(ClientType.PIM.Client);
            clientParsers.Add(ClientType.Browser.Client);
            clientParsers.Add(ClientType.Library.Client);
        }

        public void AddClientParser(IClientParserAbstract parser)
        {
            clientParsers.Add(parser);
        }

        public IEnumerable<IClientParserAbstract> GetClientsParsers()
        {
            return clientParsers.AsEnumerable();
        }


        public void AddStandardDevicesParser()
        {
            deviceParsers.Add(new HbbTvParser());
            deviceParsers.Add(new ConsoleParser());
            deviceParsers.Add(new CarBrowserParser());
            deviceParsers.Add(new CameraParser());
            deviceParsers.Add(new PortableMediaPlayerParser());
            deviceParsers.Add(new MobileParser());
        }

        public void AddDeviceParser(IDeviceParserAbstract parser)
        {
            deviceParsers.Add(parser);
        }

        public IEnumerable<IDeviceParserAbstract> GetDeviceParsers()
        {
           return deviceParsers.AsEnumerable();
        }

       
        /// <summary>
        /// Sets whether to discard additional bot information
        /// If information is discarded it's only possible check whether UA was detected as bot or not.
        /// (Discarding information speeds up the detection a bit)
        /// </summary>
        /// <param name="discard"></param>
        public void DiscardBotInformation(bool discard = true)
        {
            discardBotInformation = discard;
        }

        /// <summary>
        /// Sets whether to skip bot detection.
        /// It is needed if we want bots to be processed as a simple clients. So we can detect if it is mobile client,
        /// or desktop, or enything else. By default all this information is not retrieved for the bots.
        /// </summary>
        /// <param name="skip"></param>
        public void SkipBotDetection(bool skip = true)
        {
            skipBotDetection = skip;
        }

        /// <summary>
        /// Returns if the parsed UA was identified as a Bot
        /// @see bots.yml for a list of detected bots
        /// </summary>
        /// <returns></returns>
        public bool IsBot()
        {
            return bot.Success;
        }

        /// <summary>
        /// Returns if the parsed UA was identified as a touch enabled device
        /// Note: That only applies to windows 8 tablets
        /// </summary>
        /// <returns></returns>
        public bool IsTouchEnabled()
        {
            const string regex = "Touch";
            return IsMatchUserAgent(regex);
        }

        /// <summary>
        /// Returns if the parsed UA contains the 'Android; Tablet;' fragment
        /// </summary>
        /// <returns></returns>
        public bool HasAndroidTableFragment()
        {
            const string regex = @"Android( [\.0-9]+)?; Tablet;";
            return IsMatchUserAgent(regex);
        }

        /// <summary>
        /// Returns if the parsed UA contains the 'Android; Mobile;' fragment
        /// </summary>
        /// <returns></returns>
        public bool HasAndroidMobileFragment()
        {
            const string regex = @"Android( [\.0-9]+)?; Mobile;";
            return IsMatchUserAgent(regex);
        }

        private bool UsesMobileBrowser()
        {
            if (!client.Success) return false;
            var match = client.Match;
            return match.Type == ClientType.Browser.Name &&
                   BrowserParser.IsMobileOnlyBrowser(((BrowserMatchResult) match).ShortName);
        }

        public bool IsTablet()
        {
            return device.HasValue && device.Value == DeviceType.DEVICE_TYPE_TABLET;
        }

        public bool IsMobile()
        {
            var mobileDeviceTypes = new List<int>
            {
                DeviceType.DEVICE_TYPE_FEATURE_PHONE,
                DeviceType.DEVICE_TYPE_SMARTPHONE,
                DeviceType.DEVICE_TYPE_TABLET,
                DeviceType.DEVICE_TYPE_PHABLET,
                DeviceType.DEVICE_TYPE_CAMERA,
                DeviceType.DEVICE_TYPE_PORTABLE_MEDIA_PAYER,
            };
            // Mobile device types
            if (device.HasValue && mobileDeviceTypes.Contains(device.Value))
            {
                return true;
            }
            var nonMobileDeviceTypes = new List<int>
            {
                DeviceType.DEVICE_TYPE_TV,
                DeviceType.DEVICE_TYPE_SMART_DISPLAY,
                DeviceType.DEVICE_TYPE_CONSOLE,
            };
            // non mobile device types
            if (device.HasValue && nonMobileDeviceTypes.Contains(device.Value))
            {
                return false;
            }

            // Check for browsers available for mobile devices only
            if (UsesMobileBrowser()) {
                return true;
            }

            var osShort = os.Success ? os.Match.ShortName : string.Empty;
            if (string.IsNullOrEmpty(osShort) || UNKNOWN == osShort) {
                return false;
            }

            return !IsBot() && !IsDesktop();
        }

        /// <summary>
        /// Returns if the parsed UA was identified as desktop device
        /// Desktop devices are all devices with an unknown type that are running a desktop os
        /// </summary>
        /// <returns></returns>
        public bool IsDesktop()
        {
            var osShort = os.Success ? os.Match.ShortName : string.Empty;
            if (string.IsNullOrEmpty(osShort) || UNKNOWN == osShort)
            {
                return false;
            }

            // Check for browsers available for mobile devices only
            if (UsesMobileBrowser())
            {
                return false;
            }

            OperatingSystemParser.GetOsFamily(osShort,out string decodedFamily);

            return Array.IndexOf(desktopOsArray,decodedFamily) > -1;
        }

        /// <summary>
        /// Returns the operating system data extracted from the parsed UA
        /// If $attr is given only that property will be returned
        /// </summary>
        /// <returns></returns>
        public ParseResult<OsMatchResult> GetOs()
        {
            if (!os.Success)
            {
                return new ParseResult<OsMatchResult>(new UnknownOsMatchResult(), false);
            }
            return os;
        }

        public ParseResult<ClientMatchResult> GetClient()
        {
            if (!client.Success)
            {
                return new ParseResult<ClientMatchResult>(new UnknownClientMatchResult(), false);
            }
            return client;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ParseResult<BrowserMatchResult> GetBrowserClient()
        {
            if (client.Success)
            {
                if (client.Match is BrowserMatchResult browser)
                {
                    return new ParseResult<BrowserMatchResult>(browser);
                }
            }
            return new ParseResult<BrowserMatchResult>();
        }

        /// <summary>
        /// Returns the device type extracted from the parsed UA
        ///  <see cref="DeviceParserAbstract{T,TResult}.DeviceTypes"/> for available device types
        /// </summary>
        /// <returns></returns>
        public string GetDeviceName()
        {
            return device.HasValue
                ? DeviceParserAbstract<Dictionary<string, DeviceModel>, DeviceMatchResult>.GetDeviceName(device.Value).Key
                : null;
        }

        public string GetBrand()
        {
            return brand;
        }

        /// <summary>
        /// Returns the full device brand name extracted from the parsed UA
        /// @see self::$deviceBrand for available device brands
        /// </summary>
        /// <returns></returns>
        public string GetBrandName()
        {
            return DeviceParserAbstract<Dictionary<string, DeviceModel>, DeviceMatchResult>
                .GetFullName(brand);
        }

        public string GetModel()
        {
            return model;
        }

        /// <summary>
        /// Returns the bot extracted from the parsed UA
        /// </summary>
        /// <returns></returns>
        public ParseResult<BotMatchResult> GetBot()
        {
            return bot;
        }

        /// <summary>
        /// Returns true, if userAgent was already parsed with <see cref="Parse"/>
        /// </summary>
        /// <returns></returns>
        public bool IsParsed()
        {
            return parsed;
        }

        /// <summary>
        /// Triggers the parsing of the current user agent
        /// </summary>
        public void Parse()
        {
            if (IsParsed())
            {
                return;
            }

            parsed = true;

            // skip parsing for empty useragents or those not containing any letter
            if (string.IsNullOrEmpty(userAgent) || !GetRegexEngine().Match(userAgent, "([a-z])"))
            {
                return;
            }

            ParseBot();
            if (IsBot()) {
                return;
            }

            ParseOs();

            /**
             * Parse Clients
             * Clients might be browsers, Feed Readers, Mobile Apps, Media Players or
             * any other application accessing with an parseable UA
             */
            ParseClient();

            ParseDevice();
        }

        /// <summary>
        /// Parses the UA for bot information using the Bot parser
        /// </summary>
        private void ParseBot()
        {
            if (skipBotDetection)
            {
                bot = new ParseResult<BotMatchResult>();
                return;
            }

            foreach (var botParser in botParsers)
            {
                //@todo: need to be changed
                var parser = (BotParser) botParser;
                    
                parser.SetUserAgent(userAgent);
                parser.SetCache(cache);
                parser.DiscardDetails = discardBotInformation;
                var botParseResult = parser.Parse();
                if (!botParseResult.Success) continue;
                bot = botParseResult;
                return;
            }
        }

        /// <summary>
        /// @todo: refactory
        /// </summary>
        protected void ParseClient()
        {
            foreach (var clientParser in clientParsers)
            {
                clientParser.SetCache(cache);
                clientParser.SetUserAgent(userAgent);

                if (clientParser.ParserName == ClientType.FeedReader.Name)
                {
                    var parser = (FeedReaderParser)clientParser;
                    var result = parser.Parse();
                    if (result.Success)
                    {
                        client = new ParseResult<ClientMatchResult>();
                        client.AddRange(result.Matches);
                        return;
                    }
                }

                if (clientParser.ParserName == ClientType.MobileApp.Name)
                {
                    var parser = (MobileAppParser)clientParser;
                    var result = parser.Parse();
                    if (result.Success)
                    {
                        client = new ParseResult<ClientMatchResult>();
                        client.AddRange(result.Matches);
                        return;
                    }
                }

                if (clientParser.ParserName == ClientType.MediaPlayer.Name)
                {
                    var parser = (MediaPlayerParser)clientParser;
                    var result = parser.Parse();
                    if (result.Success)
                    {
                        client = new ParseResult<ClientMatchResult>();
                        client.AddRange(result.Matches);
                        return;
                    }
                }

                if (clientParser.ParserName == ClientType.PIM.Name)
                {
                    var parser = (PimParser)clientParser;
                    var result = parser.Parse();
                    if (result.Success)
                    {
                        client = new ParseResult<ClientMatchResult>();
                        client.AddRange(result.Matches);
                        return;
                    }
                }

                if (clientParser.ParserName == ClientType.Library.Name)
                {
                    var parser = (LibraryParser)clientParser;
                    var result = parser.Parse();
                    if (result.Success)
                    {
                        client = new ParseResult<ClientMatchResult>();
                        client.AddRange(result.Matches);
                        return;
                    }
                }

                if (clientParser.ParserName == ClientType.Browser.Name)
                {
                   var parser = (BrowserParser)clientParser;
                    var result = parser.Parse();
                    if (result.Success)
                    {
                        client = new ParseResult<ClientMatchResult>();
                        client.AddRange(result.Matches);
                        return;
                    }
                }


            }
        }

        /// <summary>
        /// @todo: refactory
        /// </summary>
        protected void ParseDevice()
        {
            foreach (var deviceParser in deviceParsers)
            {
                deviceParser.SetCache(cache);
                deviceParser.SetUserAgent(userAgent);

                if (deviceParser.ParserName == "tv")
                {
                    var parser = (HbbTvParser)deviceParser;

                    var result = parser.Parse();
                    if (result.Success)
                    {
                        device = result.Match.Type;
                        model = result.Match.Name;
                        brand = result.Match.Brand;
                        break;
                    }
                }
                if (deviceParser.ParserName == "consoles")
                {
                    var parser = (ConsoleParser)deviceParser;
                    var result = parser.Parse();
                    if (result.Success)
                    {
                        device = result.Match.Type;
                        model = result.Match.Name;
                        brand = result.Match.Brand;
                        break;
                    }
                }
                if (deviceParser.ParserName == "car browser")
                {
                    var parser = (CarBrowserParser)deviceParser;
                    var result = parser.Parse();
                    if (result.Success)
                    {
                        device = result.Match.Type;
                        model = result.Match.Name;
                        brand = result.Match.Brand;
                        break;
                    }
                }
                if (deviceParser.ParserName == "camera")
                {
                    var parser = (CameraParser)deviceParser;
                    var result = parser.Parse();
                    if (result.Success)
                    {
                        device = result.Match.Type;
                        model = result.Match.Name;
                        brand = result.Match.Brand;
                        break;
                    }
                }
                if (deviceParser.ParserName == "portablemediaplayer")
                {
                    var parser = (PortableMediaPlayerParser)deviceParser;
                    var result = parser.Parse();
                    if (result.Success)
                    {
                        device = result.Match.Type;
                        model = result.Match.Name;
                        brand = result.Match.Brand;
                        break;
                    }
                }
                if (deviceParser.ParserName == "mobiles")
                {
                    var parser = (MobileParser)deviceParser;
                    var result = parser.Parse();
                    if (result.Success)
                    {
                        device = result.Match.Type;
                        model = result.Match.Name;
                        brand = result.Match.Brand;
                        break;
                    }
                }
            }

            //If no brand has been assigned try to match by known vendor fragments
            if (string.IsNullOrEmpty(brand))
            {
                var vendorParser = new VendorFragmentParser();
                vendorParser.SetUserAgent(userAgent);
                //vendorParser.SetYamlParser();
                vendorParser.SetCache(cache);
                var result = vendorParser.Parse();
                if (result.Success)
                {
                    brand = result.Match.Brand;
                }

            }
            os = GetOs();

            var osShortName = string.Empty;
            var osFamily = string.Empty;
            var osVersion = string.Empty;
            if (os.Success)
            {
                osShortName = os.Match.ShortName;
                OperatingSystemParser.GetOsFamily(osShortName, out osFamily);
                osVersion = os.Match.Version;
                if (!string.IsNullOrEmpty(osVersion))
                {
                    osVersion = !osVersion.Contains(".") ? osVersion + ".0" : osVersion;
                }
            }

            client = GetClient();
            var clientName = client.Success ? client.Match.Name : string.Empty;


            //Assume all devices running iOS / Mac OS are from Apple
            if (string.IsNullOrEmpty(brand) && new [] { "ATV", "IOS", "MAC" }.Contains(osShortName))
            {
                brand = "AP";
            }

            //Chrome on Android passes the device type based on the keyword 'Mobile'
            //If it is present the device should be a smartphone, otherwise it's a tablet
            //See https://developer.chrome.com/multidevice/user-agent#chrome_for_android_user_agent
            if (!device.HasValue && osFamily == "Android" && (clientName == "Chrome" || clientName == "Chrome Mobile"))
            {
                if (IsMatchUserAgent(@"Chrome/[\.0-9]* Mobile"))
                {
                    device = DeviceType.DEVICE_TYPE_SMARTPHONE;
                }
                else if (IsMatchUserAgent(@"Chrome/[\.0-9]* (?!Mobile)"))
                {
                    device = DeviceType.DEVICE_TYPE_TABLET;
                }
            }

            //Some user agents simply contain the fragment 'Android; Tablet;' or 'Opera Tablet', so we assume those devices as tablets
            if (!device.HasValue && (HasAndroidTableFragment() || IsMatchUserAgent("Opera Tablet")))
            {
                device = DeviceType.DEVICE_TYPE_TABLET;
            }

            //Some user agents simply contain the fragment 'Android; Mobile;', so we assume those devices as smartphones
            if (!device.HasValue && HasAndroidMobileFragment())
            {
                device = DeviceType.DEVICE_TYPE_SMARTPHONE;
            }

            //Android up to 3.0 was designed for smartphones only. But as 3.0, which was tablet only, was published
            // too late, there were a bunch of tablets running with 2.x
            // With 4.0 the two trees were merged and it is for smartphones and tablets
            //
            //So were are expecting that all devices running Android < 2 are smartphones
            // Devices running Android 3.X are tablets.Device type of Android 2.X and 4.X + are unknown
            if (!device.HasValue && osShortName == "AND" && osVersion != string.Empty)
            {
                if (System.Version.TryParse(osVersion, out _) && new System.Version(osVersion).CompareTo(new System.Version("2.0")) == -1)
                {
                    device = DeviceType.DEVICE_TYPE_SMARTPHONE;
                }
                else if (System.Version.TryParse(osVersion, out _) && new System.Version(osVersion).CompareTo(new System.Version("3.0")) >=0 && new System.Version(osVersion).CompareTo(new System.Version("4.0")) == -1)
                {
                    device = DeviceType.DEVICE_TYPE_TABLET;
                }
            }

            //All detected feature phones running android are more likely a smartphone
            if (device == DeviceType.DEVICE_TYPE_FEATURE_PHONE && osFamily == "Android")
            {
                device = DeviceType.DEVICE_TYPE_SMARTPHONE;
            }

            //According to http://msdn.microsoft.com/en-us/library/ie/hh920767(v=vs.85).aspx
            //Internet Explorer 10 introduces the "Touch" UA string token. If this token is present at the end of the
            //UA string, the computer has touch capability, and is running Windows 8(or later).
            //
            // This UA string will be transmitted on a touch - enabled system running Windows 8(RT)
            //
            //As most touch enabled devices are tablets and only a smaller part are desktops / notebooks we assume that
            //all Windows 8 touch devices are tablets.
            if (!device.HasValue && (osShortName == "WRT" || (osShortName == "WIN" && System.Version.TryParse(osVersion, out _) && new System.Version(osVersion).CompareTo(new System.Version("8.0")) >= 0)) && IsTouchEnabled())
            {
                device = DeviceType.DEVICE_TYPE_TABLET;
            }

            //All devices running Opera TV Store are assumed to be a tv
            if (IsMatchUserAgent("Opera TV Store"))
            {
                device = DeviceType.DEVICE_TYPE_TV;
            }

            //Devices running Kylo or Espital TV Browsers are assumed to be a TV
            if (!device.HasValue && (clientName == "Kylo" || clientName == "Espial TV Browser"))
            {
                device = DeviceType.DEVICE_TYPE_TV;
            }

            //set device type to desktop for all devices running a desktop os that were not detected as an other device type
            if (!device.HasValue && IsDesktop())
            {
                device = DeviceType.DEVICE_TYPE_DESKTOP;
            }
        }

        private void ParseOs()
        {
            var osParser = new OperatingSystemParser();
            osParser.SetUserAgent(userAgent);
            osParser.SetCache(cache);

            os = osParser.Parse();
        }

        /// <summary>
        /// Parses a useragent and returns the detected data
        ///
        /// ATTENTION: Use that method only for testing or very small applications
        /// To get fast results from DeviceDetector you need to make your own implementation,
        /// that should use one of the caching mechanisms. See README.md for more information.
        ///
        /// </summary>
        /// <param name="ua">UserAgent to parse</param>
        /// <returns></returns>
        public static ParseResult<DeviceDetectorResult> GetInfoFromUserAgent(string ua)
        {
            var result = new ParseResult<DeviceDetectorResult>();
            var deviceDetector = new DeviceDetector(ua);

            deviceDetector.Parse();

            var match = new DeviceDetectorResult { UserAgent = deviceDetector.userAgent };

            if (deviceDetector.IsBot())
            {
                match.Bot = deviceDetector.bot.Match;
            }

            match.Os = deviceDetector.os.Match;
            match.Client = deviceDetector.client.Match;
            match.DeviceType = deviceDetector.GetDeviceName();
            match.DeviceBrand = deviceDetector.brand;
            match.DeviceModel = deviceDetector.model;

            if (deviceDetector.os.Success)
            {
                OperatingSystemParser.GetOsFamily(deviceDetector.os.Match.ShortName, out var osFamily);
                match.OsFamily = osFamily;
            }

            if (!(deviceDetector.client.Match is BrowserMatchResult browserMatch)) return result.Add(match);

            BrowserParser.GetBrowserFamily(browserMatch.ShortName, out var browserFamily);
            match.BrowserFamily = browserFamily;
            return result.Add(match);
        }

        /// <summary>
        /// Sets the Cache class
        /// </summary>
        /// <param name="cache"></param>
        public void SetCache(ICache cache)
        {
            this.cache = cache;
        }

        public ICache GetCache()
        {
            return cache ?? new DictionaryCache();
        }

        public void SetRegexEngine(IRegexEngine regexEng)
        {
            regexEngine = regexEng ?? throw new ArgumentNullException(nameof(regexEng));
        }

        public IRegexEngine GetRegexEngine()
        {
            return regexEngine ?? new MsRegexEngine();
        }

        //@todo: duplicate in parserabstract
        private bool IsMatchUserAgent(string regex)
        {
            // only match if useragent begins with given regex or there is no letter before it
            var match = GetRegexEngine().Match(userAgent, FixUserAgentRegEx(regex));
            return match;
        }

        protected string[] MatchUserAgent(string regex)
        {
            // only match if useragent begins with given regex or there is no letter before it
            var match = regexEngine.Matches(userAgent, FixUserAgentRegEx(regex));
            return match.ToArray();
        }

        private static string FixUserAgentRegEx(string regex)
        {
            return @"(?:^|[^A-Z_-])(?:" + regex.Replace("/", @"\/").Replace("++", "+") + ")";
        }

        public static void SetVersionTruncation(int versionTruncation)
        {
            ParserAbstract<List<Class.Bot>, BotMatchResult>.SetVersionTruncation(versionTruncation);
            ParserAbstract<List<Class.Os>, OsMatchResult>.SetVersionTruncation(versionTruncation);
            ParserAbstract<Dictionary<string, string[]>, VendorFragmentResult>.SetVersionTruncation(versionTruncation);

            ParserAbstract<List<Class.Client.FeedReader>, ClientMatchResult>.SetVersionTruncation(versionTruncation);
            ParserAbstract<List<Class.Client.MobileApp>, ClientMatchResult>.SetVersionTruncation(versionTruncation);
            ParserAbstract<List<Class.Client.MediaPlayer>, ClientMatchResult>.SetVersionTruncation(versionTruncation);
            ParserAbstract<List<Class.Client.Pim>, ClientMatchResult>.SetVersionTruncation(versionTruncation);
            ParserAbstract<List<Class.Client.Browser>, BrowserMatchResult>.SetVersionTruncation(versionTruncation);
            ParserAbstract<List<Class.Client.Library>, ClientMatchResult>.SetVersionTruncation(versionTruncation);

            ParserAbstract<IDictionary<string, DeviceModel>, DeviceMatchResult>.SetVersionTruncation(versionTruncation);
        }
    }
}
