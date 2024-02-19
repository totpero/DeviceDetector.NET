using DeviceDetectorNET.Class.Device;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Device;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeviceDetectorNET.Parser.Device
{
    public abstract class AbstractDeviceParser<T> : AbstractParser<T, DeviceMatchResult>, IDeviceParserAbstract
        where T : class, IDictionary<string, DeviceModel>
    {
        protected string model;
        protected string brand;
        protected int? deviceType;

        protected const string UnknownBrand = "Unknown";

        [Obsolete("Use Devices.DeviceTypes")]
        public static Dictionary<string, int> DeviceTypes => Devices.DeviceTypes;


        [Obsolete("Use Devices.DeviceBrands")]
        public static Dictionary<string, string> DeviceBrands => Devices.DeviceBrands;

        public int? GetDeviceType()
        {
            return deviceType;
        }

        [Obsolete("Use Devices.DeviceTypes")]
        public static Dictionary<string, int> GetAvailableDeviceTypes()
        {
            return Devices.DeviceTypes;
        }

        [Obsolete("Use Devices.GetAvailableDeviceTypeNames")]
        public static List<string> GetAvailableDeviceTypeNames()
        {
            return Devices.DeviceTypes.Keys.ToList();
        }

        [Obsolete("Use Devices.GetDeviceName")]
        public static string GetDeviceName(int deviceType)
        {
            return Devices.GetDeviceName(deviceType);
        }

        /// <summary>
        /// Returns the detected device model
        /// </summary>
        /// <returns></returns>
        public string GetModel()
        {
            return model;
        }

        /// <summary>
        /// Returns the detected device brand
        /// </summary>
        /// <returns></returns>
        public string GetBrand()
        {
            return brand;
        }

        [Obsolete("Use Devices.GetFullName")]
        public static string GetFullName(string brandId) => Devices.GetFullName(brandId);

        /// <inheritdoc />
        public override void SetUserAgent(string ua)
        {
            Reset();
            base.SetUserAgent(ua);
        }

        public override ParseResult<DeviceMatchResult> Parse()
        {
            var result = new ParseResult<DeviceMatchResult>();

            var resultClientHint = ParseClientHints();
            var deviceModel = resultClientHint.Model ?? string.Empty;

            // is freeze user-agent then restoring the original UA for the device definition
            if (!string.IsNullOrEmpty(deviceModel) && 
                GetRegexEngine().MatchesUniq(UserAgent, @"Android 10[.\d]*; K(?: Build/|[;)])").ToArray().Any())
            {
                var  osVersion = ClientHints != null ? ClientHints.GetOperatingSystemVersion() : string.Empty;
                SetUserAgent(GetRegexEngine().Replace(
                    UserAgent, 
                    @"(Android 10[.\d]*; K)",
                    $"Android {(!string.IsNullOrEmpty(osVersion) ? osVersion : "10")}; {deviceModel}"));
            }

            if (string.IsNullOrEmpty(deviceModel) && HasDesktopFragment())
                return result;

            var regexes = regexList.ToList();

            if (!regexes.Any()) return result;

            KeyValuePair<string, DeviceModel> localDevice = new KeyValuePair<string, DeviceModel>(null,null);
            string[] localMatches = null;
            foreach (var regex in regexes)
            {
                var matches = MatchUserAgent(regex.Value.Regex);
                if (matches.Length > 0)
                {
                    localDevice = regex;
                    localMatches = matches;
                    break;
                }
            }

            if (localMatches == null)
            {
                return result;
            }

            if (!localDevice.Key.Equals(UnknownBrand))
            {
                var localBrand = Devices.DeviceBrands.SingleOrDefault(x => x.Value == localDevice.Key).Value;
                if (string.IsNullOrEmpty(localBrand))
                {
                    // This Exception should never be thrown. If so a defined brand name is missing in DeviceBrands
                    throw new Exception("The brand with name '"+ localDevice.Key + "' should be listed in the deviceBrands array. Tried to parse user agent: "+ UserAgent);
                }
                brand = localBrand; //localDevice.Key
            }

            if (localDevice.Value.Device != null && Devices.DeviceTypes.TryGetValue(localDevice.Value.Device, out var localDeviceType))
            {
                deviceType = localDeviceType;
            }

            model = string.Empty;

            if (!string.IsNullOrEmpty(localDevice.Value.Name))
            {
                model = BuildModel(localDevice.Value.Name, localMatches);
            }

            if (localDevice.Value.Models != null)
            {
                Model localModel = null;
                string[] localModelMatches = null;
                foreach (var localmodel in localDevice.Value.Models)
                {
                    var modelMatches = MatchUserAgent(localmodel.Regex);
                    if (modelMatches.Length > 0)
                    {
                        localModel = localmodel;
                        localModelMatches = modelMatches;
                        break;
                    }
                }

                if (localModelMatches == null) {
                    result.Add(GetResult());
                    return result;
                }

                model = BuildModel(localModel.Name, localModelMatches)?.Trim();

                if (localModel.Brand != null)
                {
                    var localBrand = Devices.DeviceBrands.SingleOrDefault(x => x.Value == localModel.Brand).Key;
                    if (!string.IsNullOrEmpty(localBrand))
                    {
                        brand = localModel.Brand;
                    }
                }
                if (localModel.Device != null && Devices.DeviceTypes.TryGetValue(localModel.Device, out localDeviceType))
                {
                    deviceType = localDeviceType;
                }
            }
            result.Add(GetResult());

            return result;
        }

        protected string BuildModel(string model, string[] matches)
        {
            model = BuildByMatch(model, matches);

            model = model.Replace('_', ' ');
            model = GetRegexEngine().Replace(model, " TD$", string.Empty);
            model = model.Replace("$1", "");

            return model == "Build" ? null : model.Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected DeviceMatchResult ParseClientHints()
        {
            if (ClientHints != null && !string.IsNullOrEmpty(ClientHints.GetModel()))
            {
                return new DeviceMatchResult
                {
                    Type = null,
                    Name = ClientHints.GetModel(),
                    Brand = string.Empty
                };
            }
            return new DeviceMatchResult();
        }

        /// <summary>
        /// Returns if the parsed UA contains the 'Windows NT;' or 'X11; Linux x86_64' fragments
        /// </summary>
        /// <returns></returns>
        protected bool HasDesktopFragment()
        {
            var regexExcludeDesktopFragment = string.Join("|", 
                "CE-HTML", 
                " Mozilla/|Andr[o0]id|Tablet|Mobile|iPhone|Windows Phone|ricoh|OculusBrowser", 
                "PicoBrowser|Lenovo|compatible; MSIE|Trident/|Tesla/|XBOX|FBMD/|ARM; ?([^)]+)");

            return IsMatchUserAgent("(?:Windows (?:NT|IoT)|X11; Linux x86_64)") && 
                !IsMatchUserAgent(regexExcludeDesktopFragment);
        }

        protected void Reset()
        {
            deviceType = null;
            model = null;
            brand = null;
        }

        protected DeviceMatchResult GetResult()
        {
            return new DeviceMatchResult
            {
                Type = deviceType,
                Name = model,
                Brand = brand,
            };
        }
    }
}