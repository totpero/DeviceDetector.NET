using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace DeviceDetectorNET.Tests.Class
{
    public class DeviceDetectorFixture
    {
        [YamlMember(Alias = "user_agent")]
        public string UserAgent { get; set; }
        
        public object os { get; set; }
        public ClientDevice client { get; set; }
        public Device device { get; set; }
        public string os_family { get; set; }
        public string browser_family { get; set; }
        public Headers headers { get; set; }
        public Bot bot { get; set; }

    }

    public class Os
    {
        public string name { get; set; }
        public string short_name { get; set; }
        public string version { get; set; }
        public string platform { get; set; }
    }

    public class ClientDevice
    {
        public string type { get; set; }
        public string name { get; set; }
        public string short_name { get; set; }
        public string version { get; set; }
        public string engine { get; set; }
        [YamlMember(Alias = "engine_version")]
        public string EngineVersion { get; set; }
    }

    public class Device
    {
        public string type { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
    }

    public class Bot
    {
        public string name { get; set; }
        public string category { get; set; }
        public string url { get; set; }

    }

    public class BrandDetails
    {
        public string brand { get; set; }
        public string version { get; set; }
    }

    public class Headers: Dictionary<string,object>
    {
        public List< string> brands { get; set; }
        public List<string> fullVersionList { get; set; }

        public const string HttpXRequestedWithConst = "http-x-requested-with";
        public const string SecChUaFormFactorsConst = "sec-ch-ua-form-factors";
        public const string SecChUaModelConst = "sec-ch-ua-model";
        public const string SecChUaConst = "Sec-CH-UA";
        public const string SecChUaPlatformConst = "Sec-CH-UA-Platform";
        public const string SecChUaMobileConst = "Sec-CH-UA-Mobile";
        public const string SecChUaFullVersionConst = "Sec-CH-UA-Full-Version";
        public const string SecChUaPlatformVersionConst = "Sec-CH-UA-Platform-Version";
        public const string SecChPrefersColorSchemeConst = "Sec-CH-Prefers-Color-Scheme";

        [YamlIgnore] public string Mobile => ContainsKey(nameof(Mobile)) ? this[nameof(Mobile)]?.ToString() : string.Empty;
        [YamlIgnore] public string Model => ContainsKey(nameof(Model)) ? this[nameof(Model)]?.ToString() : string.Empty;
        [YamlIgnore] public string Platform => ContainsKey(nameof(Platform)) ? this[nameof(Platform)]?.ToString() : string.Empty;
        [YamlIgnore] public string PlatformVersion => ContainsKey(nameof(PlatformVersion)) ? this[nameof(PlatformVersion)]?.ToString() : string.Empty;
        [YamlIgnore] public string UaFullVersion => ContainsKey(nameof(UaFullVersion)) ? this[nameof(UaFullVersion)]?.ToString() : string.Empty;
        [YamlIgnore] public string Wow64 => ContainsKey(nameof(Wow64)) ? this[nameof(Wow64)]?.ToString() : string.Empty;
        [YamlIgnore] public string Architecture => ContainsKey(nameof(Architecture)) ? this[nameof(Architecture)]?.ToString() : string.Empty;
        [YamlIgnore] public string HttpXRequestedWith => ContainsKey(nameof(HttpXRequestedWithConst)) ? this[HttpXRequestedWithConst]?.ToString() : string.Empty;
        [YamlIgnore] public string SecChUaFormFactors => ContainsKey(nameof(SecChUaFormFactorsConst)) ? this[SecChUaFormFactorsConst]?.ToString() : string.Empty;
        [YamlIgnore] public string SecChUaModel => ContainsKey(nameof(SecChUaModelConst)) ? this[SecChUaModelConst]?.ToString() : string.Empty;
        [YamlIgnore] public string SecChUa => ContainsKey(nameof(SecChUaConst)) ? this[SecChUaConst]?.ToString() : string.Empty;
        [YamlIgnore] public string SecChUaPlatform => ContainsKey(nameof(SecChUaPlatformConst)) ? this[SecChUaPlatformConst]?.ToString() : string.Empty;
        [YamlIgnore] public string SecChUaMobile => ContainsKey(nameof(SecChUaMobileConst)) ? this[SecChUaMobileConst]?.ToString() : string.Empty;
        [YamlIgnore] public string SecChUaVersion => ContainsKey(nameof(SecChUaFullVersionConst)) ? this[SecChUaFullVersionConst]?.ToString() : string.Empty;
        [YamlIgnore] public string SecChUaPlatformVersion => ContainsKey(nameof(SecChUaPlatformVersionConst)) ? this[SecChUaPlatformVersionConst]?.ToString() : string.Empty;
        [YamlIgnore] public string SecChPrefersColorScheme => ContainsKey(nameof(SecChPrefersColorSchemeConst)) ? this[SecChPrefersColorSchemeConst]?.ToString() : string.Empty;

        public Dictionary<string, string> ToDictionary()
        {
            var result = new Dictionary<string, string>
            {
                { nameof(Mobile), Mobile },
                { nameof(Model), Model },
                { nameof(Platform), Platform },
                { nameof(PlatformVersion), PlatformVersion },
                { nameof(UaFullVersion), UaFullVersion },
                { nameof(Wow64), Wow64 },
                { nameof(Architecture), Architecture },
                { nameof(HttpXRequestedWithConst), HttpXRequestedWith },
                { nameof(SecChUaFormFactorsConst), SecChUaFormFactors },
                { nameof(SecChUaModelConst), SecChUaModel },
                { nameof(SecChUaConst), SecChUa },
                { nameof(SecChUaPlatformConst), SecChUaPlatform },
                { nameof(SecChUaMobileConst), SecChUaMobile },
                { nameof(SecChUaFullVersionConst), SecChUaVersion },
                { nameof(SecChUaPlatformVersionConst), SecChUaPlatformVersion },
                { nameof(SecChPrefersColorSchemeConst), SecChPrefersColorScheme },
            };

            return result;
        }
    }
}
