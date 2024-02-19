using DeviceDetectorNET.Cache;
using DeviceDetectorNET.Class.Device;
using DeviceDetectorNET.Parser;
using DeviceDetectorNET.Parser.Client;
using DeviceDetectorNET.Parser.Device;
using DeviceDetectorNET.RegexEngine;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Client;
using DeviceDetectorNET.Results.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LiteDB;
using YamlDotNet.Core;

namespace DeviceDetectorNET
{
    public class DeviceDetector
    {
        /// <summary>
        /// Current version number of DeviceDetector
        /// </summary>
        public const string VERSION = "6.3.0";

        /// <summary>
        /// Constant used as value for unknown browser / os
        /// </summary>
        public const string UNKNOWN = "UNK";
        public const string UNKNOWN_FULL = "Unknown";

        /// <summary>
        /// Holds the useragent that should be parsed
        /// </summary>
        protected string userAgent;

        /// <summary>
        /// Holds the client hints that should be parsed
        /// </summary>
        protected ClientHints clientHints = null;

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

        protected List<IAbstractClientParser> clientParsers = new List<IAbstractClientParser>
        {
            ClientType.FeedReader.Client,
            ClientType.MobileApp.Client,
            ClientType.MediaPlayer.Client,
            ClientType.PIM.Client,
            ClientType.Browser.Client,
            ClientType.Library.Client
        };

        protected List<IDeviceParserAbstract> deviceParsers = new List<IDeviceParserAbstract>
        {
            new HbbTvParser(),
            new ShellTvParser(),
            new NotebookParser(),
            new ConsoleParser(),
            new CarBrowserParser(),
            new CameraParser(),
            new PortableMediaPlayerParser(),
            new MobileParser()
        };

        protected List<IBotParserAbstract> botParsers = new List<IBotParserAbstract>
        {
            new BotParser()
        };

        protected bool parsed;

        /// <summary>
        ///
        /// </summary>
        /// <param name="userAgent">UA to parse</param>
        /// <param name="clientHints">Browser client hints to parse</param>
        public DeviceDetector(string userAgent = "", ClientHints clientHints = null)
        {
            if (!string.IsNullOrEmpty(userAgent))
            {
                SetUserAgent(userAgent);
            }
            if (clientHints != null)
            {
                SetClientHints(clientHints);
            }
        }

        public bool Is(ClientType type)
        {
            return client.ParserName == type.Name;
        }

        /// <summary>
        /// Sets the useragent to be parsed
        /// </summary>
        /// <param name="userAgent"></param>
        public void SetUserAgent(string userAgent)
        {
            if (this.userAgent != userAgent)
            {
                Reset();
            }
            this.userAgent = userAgent;
        }

        /// <summary>
        /// Sets the browser client hints to be parsed
        /// </summary>
        /// <param name="clientHints"></param>
        public void SetClientHints(ClientHints clientHints)
        {
            if (this.clientHints != null || this.clientHints != clientHints)
            {
                Reset();
            }
            this.clientHints = clientHints;
        }

        private void Reset()
        {
            bot = new ParseResult<BotMatchResult>();
            client = new ParseResult<ClientMatchResult>();
            device = null;
            os = new ParseResult<OsMatchResult>();
            brand = string.Empty;
            model = string.Empty;
            parsed = false;
        }

        public void AddClientParser(IAbstractClientParser parser)
        {
            clientParsers.Add(parser);
        }

        public IEnumerable<IAbstractClientParser> GetClientsParsers()
        {
            return clientParsers.AsEnumerable();
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
        /// or desktop, or anything else. By default all this information is not retrieved for the bots.
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
            return IsMatchUserAgent("Touch");
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

        /// <summary> 
        /// Returns if the parsed UA contains the 'Desktop;', 'Desktop x32;', 'Desktop x64;' or 'Desktop WOW64;' fragment
        /// </summary>
        /// <returns>True if contains fragment</returns>
        protected bool HasDesktopFragment()
        {
            const string regex = "Desktop(?: (x(?:32|64)|WOW64))?;";
            return IsMatchUserAgent(regex);
        }

        private bool UsesMobileBrowser()
        {
            if (!client.Success) return false;
            var match = client.Match;
            return match.Type == ClientType.Browser.Name &&
                   BrowserParser.IsMobileOnlyBrowser(((BrowserMatchResult)match).ShortName);
        }

        public bool IsTablet()
        {
            return device == DeviceType.DEVICE_TYPE_TABLET;
        }

        public bool IsTv()
        {
            return device == DeviceType.DEVICE_TYPE_TV;
        }
        
        public bool IsWearable()
        {
            return device == DeviceType.DEVICE_TYPE_WEARABLE;
        }

        public bool IsMobile()
        {
            // Client hints indicate a mobile device
            if (clientHints != null && clientHints.IsMobile()) {
                return true;
            }

            // Mobile device types
            if (device.HasValue && mobileDeviceTypes.Contains(device.Value))
            {
                return true;
            }

            // non mobile device types
            if (device.HasValue && nonMobileDeviceTypes.Contains(device.Value))
            {
                return false;
            }

            // Check for browsers available for mobile devices only
            if (UsesMobileBrowser())
            {
                return true;
            }

            var osShort = os.Success ? os.Match.ShortName : string.Empty;
            if (string.IsNullOrEmpty(osShort) || UNKNOWN == osShort)
            {
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

            var decodedFamily = OperatingSystemParser.GetOsFamily(osShort);

            return Array.IndexOf(OperatingSystemParser.DesktopOs, decodedFamily) > -1;
        }

        /// <summary>
        /// Returns the operating system data extracted from the parsed UA
        /// If $attr is given only that property will be returned
        /// </summary>
        /// <returns></returns>
        public ParseResult<OsMatchResult> GetOs()
        {
            return os.Success ? os : new ParseResult<OsMatchResult>(new UnknownOsMatchResult(), false);
        }

        public ParseResult<ClientMatchResult> GetClient()
        {
            return client.Success ? client : new ParseResult<ClientMatchResult>(new UnknownClientMatchResult(), false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ParseResult<BrowserMatchResult> GetBrowserClient()
        {
            if (client.Success && client.Match is BrowserMatchResult browser)
            {
                return new ParseResult<BrowserMatchResult>(browser);
            }
            return new ParseResult<BrowserMatchResult>();
        }

        /// <summary>
        /// Returns the device type extracted from the parsed UA
        ///  <see cref="Devices.DeviceTypes"/> for available device types
        /// </summary>
        /// <returns></returns>
        public string GetDeviceName()
        {
            return device.HasValue
                ? Devices.GetDeviceName(device.Value)
                : null; //todo: string.Empty?
        }

        /**
         * Returns the device brand extracted from the parsed UA
         *
         * @see self::$deviceBrand for available device brands
         *
         * @return string
         *
         * @deprecated since 4.0 - short codes might be removed in next major release
         */
        public string GetBrand()
        {
            return Devices.GetShortCode(brand);
        }

        /// <summary>
        /// Returns the full device brand name extracted from the parsed UA
        /// @see self::$deviceBrand for available device brands
        /// </summary>
        /// <returns></returns>
        public string GetBrandName()
        {
            return brand;
        }

        public string GetModel()
        {
            return model ?? string.Empty;
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

        private static readonly List<int> mobileDeviceTypes = new List<int>
        {
            DeviceType.DEVICE_TYPE_FEATURE_PHONE,
            DeviceType.DEVICE_TYPE_SMARTPHONE,
            DeviceType.DEVICE_TYPE_TABLET,
            DeviceType.DEVICE_TYPE_PHABLET,
            DeviceType.DEVICE_TYPE_CAMERA,
            DeviceType.DEVICE_TYPE_PORTABLE_MEDIA_PAYER,
        };

        private static readonly List<int> nonMobileDeviceTypes = new List<int>
        {
            DeviceType.DEVICE_TYPE_TV,
            DeviceType.DEVICE_TYPE_SMART_DISPLAY,
            DeviceType.DEVICE_TYPE_CONSOLE,
        };

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

            // skip parsing for empty useragents or those not containing any letter (if no client hints were provided)
            if ((string.IsNullOrEmpty(userAgent) || !GetRegexEngine().Match(userAgent, "([a-z])"))
                && clientHints == null)
            {
                return;
            }

            var useCache = DeviceDetectorSettings.ParseCacheDBExpiration != TimeSpan.Zero;

            if (useCache)
            {
                var key = $"{userAgent}_{skipBotDetection}_{discardBotInformation}_{_versionTruncation}";

                if (LoadCacheData(key)) return;

                ParseBase();

                SaveCacheData(key);
            }
            else
            {
                ParseBase();
            }
        }

        private bool LoadCacheData(string key)
        {
            var data = ParseCache.Instance.FindById(key);

            if (data == null) return false;

            device = data.Device;
            brand = data.Brand;
            parsed = data.Parsed;
            model = data.Model;
            bot = data.Bot;
            client = data.Client;
            os = data.Os;
            return true;
        }

        private void SaveCacheData(string key)
        {
            var ent = new DeviceDetectorCachedData
            {
                Device = device,
                Brand = brand,
                Parsed = parsed,
                Model = model,
                Bot = bot,
                Client = client,
                Os = os
            };

            ParseCache.Instance.Upsert(key, ent);
        }

        private void ParseBase()
        {
            ParseBot();
            if (IsBot()) return;

            ParseOs();

            /*
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
                var parser = (BotParser)botParser;

                parser.SetUserAgent(userAgent);
                parser.SetCache(cache);
                parser.SetClientHints(this.clientHints);
                parser.DiscardDetails = discardBotInformation;
                var botParseResult = parser.Parse();
                if (!botParseResult.Success) continue;

                bot = botParseResult;
                return;
            }
        }

        /// <summary>
        /// </summary>
        protected void ParseClient()
        {
            //if (!clientParsers.Any( c=> c.ParserName == MobileAppParser.AppParserName))
            //{
            //    AddClientParser(new MobileAppParser(userAgent, clientHints));
            //}

            foreach (var clientParser in clientParsers)
            {
                clientParser.SetCache(cache);
                clientParser.SetUserAgent(userAgent);
                clientParser.SetClientHints(clientHints);

                var result = clientParser.Parse();
                if (!result.Success) continue;

                client = new ParseResult<ClientMatchResult>
                {
                    ParserName = clientParser.ParserName
                };
                client.AddRange(result.Matches);
                return;
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
                deviceParser.SetClientHints(clientHints);

                var result = deviceParser.Parse();
                if (!result.Success) continue;

                //var parserName = deviceParser.ParserName;
                device = result.Match.Type;
                model = result.Match.Name;
                brand = result.Match.Brand;
                break;
            }

            //If no model could be parsed from useragent, we use the one from client hints if available
            if (clientHints != null && string.IsNullOrEmpty(model)) {
                model = clientHints.GetModel();
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
            var osName = string.Empty;
            var osFamily = string.Empty;
            var osVersion = string.Empty;
            var appleOsNames = new[] { "iPadOS", "tvOS", "watchOS", "iOS", "Mac" };

            if (os.Success)
            {
                osShortName = os.Match.ShortName;
                osName = os.Match.Name;
                osFamily = OperatingSystemParser.GetOsFamily(osShortName);
                osVersion = os.Match.Version;
                if (!string.IsNullOrEmpty(osVersion))
                {
                    osVersion = !osVersion.Contains(".") ? osVersion + ".0" : osVersion;
                }
            }

            client = GetClient();
            var clientName = client.Success ? client.Match.Name : string.Empty;

            //if it's fake UA then it's best not to identify it as Apple running Android OS or GNU/Linux
            if ("Apple" == brand && !appleOsNames.Contains(osName))
            {
                device = null;
                brand = string.Empty;
                model = string.Empty;
            }

            //Assume all devices running iOS / Mac OS are from Apple
            if (string.IsNullOrEmpty(brand) && appleOsNames.Contains(osName))
            {
                brand = "Apple";
            }

            //All devices containing VR fragment are assumed to be a wearable
            if (!device.HasValue && IsMatchUserAgent(" VR "))
            {
                device = DeviceType.DEVICE_TYPE_WEARABLE;
            }

            //Chrome on Android passes the device type based on the keyword 'Mobile'
            //If it is present the device should be a smartphone, otherwise it's a tablet
            //See https://developer.chrome.com/multidevice/user-agent#chrome_for_android_user_agent
            //Note: We do not check for browser (family) here, as there might be mobile apps using Chrome, that won't have
            //a detected browser, but can still be detected. So we check the useragent for Chrome instead.
            if (!device.HasValue && osFamily == "Android" 
                                 && IsMatchUserAgent(@"Chrome/[\.0-9]*"))
            {
                if (IsMatchUserAgent("(?:Mobile|eliboM) "))
                {
                    device = DeviceType.DEVICE_TYPE_SMARTPHONE;
                }
                else
                {
                    device = DeviceType.DEVICE_TYPE_TABLET;
                }
            }

            //Some UA contain the fragment 'Pad/APad', so we assume those devices as tablets
            if (DeviceType.DEVICE_TYPE_SMARTPHONE == device && IsMatchUserAgent("Pad/APad"))
            {
                device = DeviceType.DEVICE_TYPE_TABLET;
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
            if (!device.HasValue && osName == "Android" && osVersion != string.Empty)
            {
                if (System.Version.TryParse(osVersion, out _) 
                    && new System.Version(osVersion).CompareTo(new System.Version("2.0")) == -1)
                {
                    device = DeviceType.DEVICE_TYPE_SMARTPHONE;
                }
                else if (System.Version.TryParse(osVersion, out _) 
                    && new System.Version(osVersion).CompareTo(new System.Version("3.0")) >= 0 
                    && new System.Version(osVersion).CompareTo(new System.Version("4.0")) == -1)
                {
                    device = DeviceType.DEVICE_TYPE_TABLET;
                }
            }

            //All detected feature phones running android are more likely a smartphone
            if (device == DeviceType.DEVICE_TYPE_FEATURE_PHONE && osFamily == "Android")
            {
                device = DeviceType.DEVICE_TYPE_SMARTPHONE;
            }

            //All unknown devices under running Java ME are more likely a features phones
            if ("Java ME" == osName && !device.HasValue) {
                device = DeviceType.DEVICE_TYPE_FEATURE_PHONE;
            }


            /*According to http://msdn.microsoft.com/en-us/library/ie/hh920767(v=vs.85).aspx
             *Internet Explorer 10 introduces the "Touch" UA string token. If this token is present at the end of the
             *UA string, the computer has touch capability, and is running Windows 8(or later).
             *
             * This UA string will be transmitted on a touch - enabled system running Windows 8(RT)
             *
             *As most touch enabled devices are tablets and only a smaller part are desktops / notebooks we assume that
             *all Windows 8 touch devices are tablets.
            */
            if (!device.HasValue && (osName == "Windows RT" || (osName == "Windows" 
                && System.Version.TryParse(osVersion, out _) 
                && new System.Version(osVersion).CompareTo(new System.Version("8.0")) >= 0)) && IsTouchEnabled())
            {
                device = DeviceType.DEVICE_TYPE_TABLET;
            }

            //All devices running Opera TV Store are assumed to be a tv
            if (IsMatchUserAgent("Opera TV Store| OMI/"))
            {
                device = DeviceType.DEVICE_TYPE_TV;
            }

            //All devices that contain Andr0id in string are assumed to be a tv
            if (IsMatchUserAgent("Andr0id|(?:Android(?: UHD)?|Google) TV|\\(lite\\) TV|BRAVIA"))
            {
                device = DeviceType.DEVICE_TYPE_TV;
            }

            //All devices running Tizen TV or SmartTV are assumed to be a tv
            if (!device.HasValue && IsMatchUserAgent("SmartTV|Tizen.+ TV .+$"))
            {
                device = DeviceType.DEVICE_TYPE_TV;
            }

            //Devices running those clients are assumed to be a TV
            if (!device.HasValue && new[]
                {
                    "Kylo", "Espial TV Browser", "LUJO TV Browser", "LogicUI TV Browser", "Open TV Browser", "Seraphic Sraf",
                    "Opera Devices", "Crow Browser", "Vewd Browser", "TiviMate", "Quick Search TV"
                }.Contains(clientName))
            {
                device = DeviceType.DEVICE_TYPE_TV;
            }

            //All devices containing TV fragment are assumed to be a tv
            if (!device.HasValue && IsMatchUserAgent("\\(TV;"))
            {
                device = DeviceType.DEVICE_TYPE_TV;
            }

            // Set device type desktop if string ua contains desktop
            if (device != DeviceType.DEVICE_TYPE_DESKTOP && !string.IsNullOrEmpty(userAgent) && !userAgent.Contains("Desktop") && HasDesktopFragment())
            {
                device = DeviceType.DEVICE_TYPE_DESKTOP;
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
            osParser.SetClientHints(clientHints);
            //osParser.SetYamlParser(yamlParser);
            osParser.SetCache(cache);

            os = osParser.Parse();
        }

        public bool IsBrowser()
        {
            return client.Success && client.Match is BrowserMatchResult; // && deviceName == BrowserParser.DefaultParserName;
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
        public static ParseResult<DeviceDetectorResult> GetInfoFromUserAgent(string ua, ClientHints clientHints = null)
        {
            var result = new ParseResult<DeviceDetectorResult>();
            var deviceDetector = new DeviceDetector(ua, clientHints); //@todo:singleton

            deviceDetector.Parse();

            var match = new DeviceDetectorResult { UserAgent = deviceDetector.userAgent };

            if (deviceDetector.IsBot())
            {
                match.Bot = deviceDetector.bot.Match;
                //return;
            }

            if (deviceDetector.IsBrowser())
            {
                var browserMatch = deviceDetector.client.Match as BrowserMatchResult;
                match.BrowserFamily = browserMatch.Family;
            }

            if (deviceDetector.os.Success)
            {
                var osFamily = deviceDetector.os.Match.Family;
                match.OsFamily = osFamily ?? UNKNOWN_FULL;
            }

            match.Os = deviceDetector.os.Match;
            match.Client = deviceDetector.client.Match;
           
            match.Device = new DeviceMatchResult
            {
                //Name = deviceDetector.GetDeviceName(),
                Type = deviceDetector.device,
                Brand = deviceDetector.GetBrandName(),
                Name = deviceDetector.GetModel(),
            };
            match.DeviceType = deviceDetector.GetDeviceName();
            match.DeviceBrand = deviceDetector.GetBrandName();
            match.DeviceModel = deviceDetector.GetModel();

            return result.Add(match);

            //if (deviceDetector.client.Success &&
            //  deviceDetector.client.ParserName.Equals(BrowserParser.DefaultParserName))
            //{
            //    client.Match.Family
            //}
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
            return cache ?? (cache = new DictionaryCache());
        }

        public void SetRegexEngine(IRegexEngine regexEng)
        {
            regexEngine = regexEng ?? throw new ArgumentNullException(nameof(regexEng));
        }

        public IRegexEngine GetRegexEngine()
        {
            return regexEngine ?? (regexEngine = new MSRegexCompiledEngine());
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
            return "(?:^|[^A-Z_-])(?:" + regex.Replace("/", @"\/").Replace("++", "+") + ")";
        }

        private static int _versionTruncation = AbstractParser<List<Class.Bot>, ClientMatchResult>.VERSION_TRUNCATION_NONE;

        public static void SetVersionTruncation(int versionTruncation)
        {
            _versionTruncation = versionTruncation;
            AbstractParser<List<Class.Bot>, BotMatchResult>.SetVersionTruncation(versionTruncation);
            AbstractParser<List<Class.Os>, OsMatchResult>.SetVersionTruncation(versionTruncation);
            AbstractParser<Dictionary<string, string[]>, VendorFragmentResult>.SetVersionTruncation(versionTruncation);

            AbstractParser<List<Class.Client.FeedReader>, ClientMatchResult>.SetVersionTruncation(versionTruncation);
            AbstractParser<List<Class.Client.MobileApp>, ClientMatchResult>.SetVersionTruncation(versionTruncation);
            AbstractParser<List<Class.Client.MediaPlayer>, ClientMatchResult>.SetVersionTruncation(versionTruncation);
            AbstractParser<List<Class.Client.Pim>, ClientMatchResult>.SetVersionTruncation(versionTruncation);
            AbstractParser<List<Class.Client.Browser>, ClientMatchResult>.SetVersionTruncation(versionTruncation);
            AbstractParser<List<Class.Client.Library>, ClientMatchResult>.SetVersionTruncation(versionTruncation);

            AbstractParser<IDictionary<string, DeviceModel>, DeviceMatchResult>.SetVersionTruncation(versionTruncation);
        }
    }

    [Serializable]
    [DataContract]
    public class CachedDataHolder
    {
        [DataMember]
        [BsonId]
        public string Id { get; set; }

        [DataMember]
        public string Json { get; set; }

        [DataMember]
        public DateTime ExpirationDate { get; set; }
    }

    /// <summary>
    /// Expiration data of the object, stored in UTC
    /// </summary>
    [Serializable]
    [DataContract]
    public class DeviceDetectorCachedData
    {
        [DataMember]
        public ParseResult<BotMatchResult> Bot { get; set; }
        [DataMember]
        public ParseResult<ClientMatchResult> Client { get; set; }
        [DataMember]
        public int? Device { get; set; }
        [DataMember]
        public ParseResult<OsMatchResult> Os { get; set; }
        [DataMember]
        public string Brand { get; set; }
        [DataMember]
        public string Model { get; set; }
        [DataMember]
        public bool Parsed { get; set; }
    }
}
