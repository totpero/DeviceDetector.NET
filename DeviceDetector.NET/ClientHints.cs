using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DeviceDetectorNET
{
    /// <summary>
    /// @todo: use IRegexEngine
    /// </summary>
    public class ClientHints
    {
        /// <summary>
        /// Represents `Sec-CH-UA-Arch` header field: The underlying architecture's instruction
        /// </summary>
        public string Architecture { get; set; }

        /// <summary>
        /// Represents `Sec-CH-UA-Bitness` header field: The underlying architecture's bitness
        /// </summary>
        public string Bitness { get; set; }

        /// <summary>
        /// Represents `Sec-CH-UA-Mobile` header field: whether the user agent should receive a specifically "mobile" UX
        /// </summary>
        public bool Mobile { get; set; }

        /// <summary>
        /// Represents `Sec-CH-UA-Model` header field: the user agent's underlying device model
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Represents `Sec-CH-UA-Platform` header field: the platform's brand
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Represents `Sec-CH-UA-Platform-Version` header field: the platform's major version
        /// </summary>
        public string PlatformVersion { get; set; }

        /// <summary>
        /// Represents `Sec-CH-UA-Full-Version` header field: the platform's major version
        /// </summary>
        public string UaFullVersion { get; set; }

        /// <summary>
        /// Represents `Sec-CH-UA-Full-Version-List` header field: the full version for each brand in its brand list
        /// </summary>
        public Dictionary<string, string> FullVersionList { get; set; }

        /// <summary>
        /// Represents `x-requested-with` header field: Android app id
        /// </summary>
        public string App { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">`Sec-CH-UA-Model` header field</param>
        /// <param name="platform">`Sec-CH-UA-Platform` header field</param>
        /// <param name="platformVersion">`Sec-CH-UA-Platform-Version` header field</param>
        /// <param name="uaFullVersion">`Sec-CH-UA-Full-Version` header field</param>
        /// <param name="fullVersionList">`Sec-CH-UA-Full-Version-List` header field</param>
        /// <param name="mobile">`Sec-CH-UA-Mobile` header field</param>
        /// <param name="architecture">`Sec-CH-UA-Arch` header field</param>
        /// <param name="bitness">`Sec-CH-UA-Bitness`</param>
        /// <param name="app">`HTTP_X-REQUESTED-WITH`</param>
        public ClientHints(string model, string platform, string platformVersion, string uaFullVersion, Dictionary<string,string> fullVersionList, bool mobile, string architecture, string bitness, string app)
        {
            Model = model;
            Platform = platform;
            PlatformVersion = platformVersion;
            UaFullVersion = uaFullVersion;
            FullVersionList = fullVersionList;
            Mobile = mobile;
            Architecture = architecture;
            Bitness = bitness;
            App = app;
        }



        /// <summary>
        /// Returns if the client hints
        /// </summary>
        /// <returns></returns>
        public bool IsMobile()
        {
            return this.Mobile;
        }

        /// <summary>
        ///  Returns the Architecture
        /// </summary>
        /// <returns></returns>
        public string GetArchitecture()
        {
            return this.Architecture;
        }

        /// <summary>
        /// Returns the Bitness
        /// </summary>
        /// <returns></returns>
        public string GetBitness()
        {
            return this.Bitness;
        }

        /// <summary>
        /// Returns the device model
        /// </summary>
        /// <returns></returns>
        public string GetModel()
        {
            return this.Model;
        }
        
        /// <summary>
        /// Returns the Operating System
        /// </summary>
        /// <returns></returns>
        public string GetOperatingSystem()
        {
            return this.Platform;
        }

        /// <summary>
        /// Returns the Operating System version
        /// </summary>
        /// <returns></returns>
        public string GetOperatingSystemVersion()
        {
            return this.PlatformVersion;
        }

        /// <summary>
        /// Returns the Browser name
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetBrandList()
        {
            if (this.FullVersionList.Count > 0)
            {
                return this.FullVersionList;
                //$brands   = \array_column($this->fullVersionList, 'brand');
                //$versions = \array_column($this->fullVersionList, 'version');

                //if (\count($brands) === \count($versions)) {
                //	return \array_combine($brands, $versions);
                //}
            }

            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Returns the Browser version
        /// </summary>
        /// <returns></returns>
        public string GetBrandVersion()
        {
            if (!string.IsNullOrEmpty(UaFullVersion))
            {
                return UaFullVersion;
            }

            return null;
        }

        /// <summary>
        /// Returns the Android app id
        /// </summary>
        /// <returns></returns>
        public string GetApp()
        {
            return this.App;
        }

        /// <summary>
        /// Factory method to easily instantiate this class using an array containing all available (client hint) header
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static ClientHints Factory(Dictionary<string,string> headers)
        {
            var model = string.Empty;
            var platform = string.Empty;
            var platformVersion = string.Empty;
            var uaFullVersion = string.Empty;
            var architecture = string.Empty;
            var bitness = string.Empty;

            var app = string.Empty;
            var mobile = false;
            Dictionary<string,string> fullVersionList = new Dictionary<string, string>();

            foreach (var header in headers)
            {
                var headerNormalized = header.Key.ToLower().Replace("_", "-");
                switch (headerNormalized)
                {
                    case "http-sec-ch-ua-arch":
                    case "sec-ch-ua-arch":
                    case "arch":
                    case "architecture":
                        architecture = header.Value.Trim('"');
                        break;
                    case "http-sec-ch-ua-bitness":
                    case "sec-ch-ua-bitness":
                    case "bitness":
                        bitness = header.Value.Trim('"');
                        break;
                    case "http-sec-ch-ua-mobile":
                    case "sec-ch-ua-mobile":
                    case "mobile":
                        //if (header.Value.GetType() == typeof(bool))
                        //{
                        //    mobile = (bool)header.Value;
                        //}
                        //else
                        {
                            mobile = header.Value.Equals("1") || header.Value.Equals("?1");
                        }
                        break;
                    case "http-sec-ch-ua-model":
                    case "sec-ch-ua-model":
                    case "model":
                        model = header.Value.Trim('"');
                        break;
                    case "http-sec-ch-ua-full-version":
                    case "sec-ch-ua-full-version":
                    case "uafullversion":
                        uaFullVersion = header.Value.Trim('"');
                        break;
                    case "http-sec-ch-ua-platform":
                    case "sec-ch-ua-platform":
                    case "platform":
                        platform = header.Value.Trim('"');
                        break;
                    case "http-sec-ch-ua-platform-version":
                    case "sec-ch-ua-platform-version":
                    case "platformversion":
                        platformVersion = header.Value.Trim('"');
                        break;
                    case "brands":
                        //if (fullVersionList.Count > 0)
                        //{
                        //    break;
                        //}
                    // use this only if no other header already set the list
                    case "fullversionlist":
                        //fullVersionList = (header.Value.GetType() == typeof(List<string>))
                        //    ? (List<string>)header.Value
                        //    : fullVersionList;
                        break;
                    case "http-sec-ch-ua":
                    case "sec-ch-ua":
                        //if (fullVersionList.Count > 0)
                        //{
                        //    break;
                        //}
                    // use this only if no other header already set the list
                    case "http-sec-ch-ua-full-version-list":
                    case "sec-ch-ua-full-version-list":

                        //from up case
                        if (headerNormalized.Equals("fullversionlist"))
                        {
                            //header
                            //fullVersionList
                            //headerNormalized
                            break;
                        }

                        //from up case
                        if (headerNormalized.Equals("brands") &&
                            fullVersionList.Count > 0)
                        {
                            break;
                        }

                        //from up case
                        if (headerNormalized.Equals("http-sec-ch-ua") ||
                            headerNormalized.Equals("sec-ch-ua") && 
                            fullVersionList.Count > 0)
                        {
                            break;
                        }
                        const string reg = "^\"([^\"]+)\"; ?v=\"([^\"]+)\"(?:, )?";
                        var list = new Dictionary<string,string>();

                        var value = header.Value;
                        
                        while (!string.IsNullOrEmpty(value))
                        {
                            var r = new Regex(reg, RegexOptions.IgnoreCase);
                            var match = r.Match(value);

                            while (match.Success)
                            {
                                var substr = match.Groups[0].Value;
                                var brand = match.Groups[1].Value;
                                var version = match.Groups[2].Value;
                                if (!list.ContainsKey(brand))
                                {
                                    list.Add(brand, version);
                                }
                                value = value.Substring(substr.Length);
                                match = match.NextMatch();
                            }
                        }
                        
                        if (list.Count > 0)
                        {
                            fullVersionList = list;
                        }
                        break;
                    case "http-x-requested-with":
                    case "x-requested-with":
                        if ("xmlhttprequest" != header.Value.ToLower()) {
                            app = header.Value;
                        }
                        break;
                    default:
                        break;
                }
            }

            var clientHints = new ClientHints(model, platform, platformVersion, uaFullVersion, fullVersionList, mobile, architecture, bitness, app);
            return clientHints;
        }
    }
}

