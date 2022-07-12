﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using YamlDotNet.Core.Tokens;

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
        /// Represents `SSec-CH-UA-Platform-Version` header field: the platform's major version
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

        public string GetModel()
        {
            return this.Model;
        }
        
        public string GetBitness()
        {
            return this.Bitness;
        }

        /// <summary>
        /// Returns the Operating System
        /// </summary>
        /// <returns></returns>
        public string GetOperatingSystem()
        {
            return this.Platform;
        }

        public string GetOperatingSystemVersion()
        {
            return this.PlatformVersion;
        }

        /// <summary>
        ///  Returns the Architecture
        /// </summary>
        /// <returns></returns>
        public string GetArchitecture()
        {
            return this.Architecture;
        }

        public Dictionary<string, string> GetBrandList()
        {
                if (this.FullVersionList.Count > 0)
                {
                    return this.FullVersionList;
                    //return \array_combine(
                    //\array_column($this->fullVersionList, 'brand'),
                    //        \array_column($this->fullVersionList, 'version')
                    //   ) ?: [];

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

            return string.Empty;
        }

        public string GetApp()
        {
            return this.App;
        }


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
                        architecture = header.Value.ToString().Trim('"');
                        break;
                    case "http-sec-ch-ua-bitness":
                    case "sec-ch-ua-bitness":
                    case "bitness":
                        bitness = header.Value.ToString().Trim('"');
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
                            mobile = header.Value.ToString().Equals("1") || header.Value.ToString().Equals("?1");
                        }
                        break;
                    case "http-sec-ch-ua-model":
                    case "sec-ch-ua-model":
                    case "model":
                        model = header.Value.ToString().Trim('"');
                        break;
                    case "http-sec-ch-ua-full-version":
                    case "sec-ch-ua-full-version":
                    case "uafullversion":
                        uaFullVersion = header.Value.ToString().Trim('"');
                        break;
                    case "http-sec-ch-ua-platform":
                    case "sec-ch-ua-platform":
                    case "platform":
                        platform = header.Value.ToString().Trim('"');
                        break;
                    case "http-sec-ch-ua-platform-version":
                    case "sec-ch-ua-platform-version":
                    case "platformversion":
                        platformVersion = header.Value.ToString().Trim('"');
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

                        if (headerNormalized.Equals("http-sec-ch-ua") ||
                            headerNormalized.Equals("sec-ch-ua") && 
                            fullVersionList.Count > 0)
                        {
                            break;
                        }
                        const string reg = "^\"([^\"]+)\"; ?v=\"([^\"]+)\"(?:, )?";
                        var list = new Dictionary<string,string>();

                        var value = header.Value.ToString();
                        
                        while (!string.IsNullOrEmpty(value))
                        {
                            var r = new Regex(reg, RegexOptions.IgnoreCase);
                            var match = r.Match(value);

                            while (match.Success)
                            {
                                var substr = match.Groups[0].Value;
                                var brand = match.Groups[1].Value;
                                var version = match.Groups[2].Value;
                                list.Add(brand, version);
                                value = value.Substring(substr.Length);
                                match = match.NextMatch();
                            }
                        }
                        
                        if (list.Count > 0)
                        {
                            fullVersionList = list;
                        }
                        //while (\preg_match($reg, $value, $matches)) {
                        //$list[] = ['brand' => $matches[1], 'version' => $matches[2]];
                        //$value = \substr($value, \strlen($matches[0]));
                        //}

                        //if (\count($list)) {
                        //$fullVersionList = $list;
                        //}
                        break;
                    case "http-x-requested-with":
                    case "x-requested-with":
                        if ("xmlhttprequest" != header.Value.ToString().ToLower()) {
                            app = header.Value.ToString();
                        }
                        break;
                    default:
                        break;
                }
            }
            ClientHints clientHints = new ClientHints(model,platform,platformVersion,uaFullVersion, fullVersionList, mobile,architecture,bitness,app);
            return clientHints;
        }
    }
}
