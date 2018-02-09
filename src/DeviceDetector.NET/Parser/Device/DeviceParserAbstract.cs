using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DeviceDetectorNET.Class.Device;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Device;

namespace DeviceDetectorNET.Parser.Device
{
    public abstract class DeviceParserAbstract<T, TResult> : ParserAbstract<T, TResult>, IDeviceParserAbstract
        where T : class, IDictionary<string, DeviceModel>
        where TResult : class, IDeviceMatchResult, new()
    {

        protected string model;
        protected string brand;
        protected int? deviceType;

        protected const string UnknownBrand = "Unknown";

        /// <summary>
        /// Detectable device types
        /// </summary>
        public static Dictionary<string, int> DeviceTypes = new Dictionary<string, int>
        {
            {"desktop", DeviceType.DEVICE_TYPE_DESKTOP},
            {"smartphone", DeviceType.DEVICE_TYPE_SMARTPHONE},
            {"tablet", DeviceType.DEVICE_TYPE_TABLET},
            {"feature phone", DeviceType.DEVICE_TYPE_FEATURE_PHONE},
            {"console", DeviceType.DEVICE_TYPE_CONSOLE},
            {"tv", DeviceType.DEVICE_TYPE_TV},
            {"car browser", DeviceType.DEVICE_TYPE_CAR_BROWSER},
            {"smart display", DeviceType.DEVICE_TYPE_SMART_DISPLAY},
            {"camera", DeviceType.DEVICE_TYPE_CAMERA},
            {"portable media player", DeviceType.DEVICE_TYPE_PORTABLE_MEDIA_PAYER},
            {"phablet", DeviceType.DEVICE_TYPE_PHABLET}
        };

        /// <summary>
        /// Known device brands
        /// Note: Before using a new brand in on of the regex files, it needs to be added here
        /// </summary>
        public static Dictionary<string, string> DeviceBrands = new Dictionary<string, string>
        {
            {"3Q", "3Q"},
            {"4G", "4Good"},
            {"AC", "Acer"},
            {"AZ", "Ainol"},
            {"AI", "Airness"},
            {"AL", "Alcatel"},
            {"A2", "Allview"},
            {"A1", "Altech UEC"},
            {"AN", "Arnova"},
            {"KN", "Amazon"},
            {"AO", "Amoi"},
            {"AP", "Apple"},
            {"AR", "Archos"},
            {"AS", "ARRIS"},
            {"AT", "Airties"},
            {"AU", "Asus"},
            {"AV", "Avvio"},
            {"AX", "Audiovox"},
            {"AY", "Axxion"},
            {"AM", "Azumi Mobile"},
            {"BB", "BBK"},
            {"BE", "Becker"},
            {"BI", "Bird"},
            {"BG", "BGH"},
            {"BL", "Beetel"},
            {"BP", "Blaupunkt"},
            {"BM", "Bmobile"},
            {"BN", "Barnes & Noble"},
            {"BO", "BangOlufsen"},
            {"BQ", "BenQ"},
            {"BS", "BenQ-Siemens"},
            {"BU", "Blu"},
            {"B2", "Blackview"},
            {"BW", "Boway"},
            {"BX", "bq"},
            {"BV", "Bravis"},
            {"BR", "Brondi"},
            {"B1", "Bush"},
            {"CB", "CUBOT"},
            {"CF", "Carrefour"},
            {"CP", "Captiva"},
            {"CS", "Casio"},
            {"CA", "Cat"},
            {"CE", "Celkon"},
            {"CC", "ConCorde"},
            {"C2", "Changhong"},
            {"CH", "Cherry Mobile"},
            {"CK", "Cricket"},
            {"C1", "Crosscall"},
            {"CL", "Compal"},
            {"CN", "CnM"},
            {"CM", "Crius Mea"},
            {"C3", "China Mobile"},
            {"CR", "CreNova"},
            {"CT", "Capitel"},
            {"CQ", "Compaq"},
            {"CO", "Coolpad"},
            {"C5", "Condor"},
            {"CW", "Cowon"},
            {"CU", "Cube"},
            {"CY", "Coby Kyros"},
            {"C4", "Cyrus"},
            {"DA", "Danew"},
            {"DT", "Datang"},
            {"DE", "Denver"},
            {"DX", "DEXP"},
            {"DS", "Desay"},
            {"DB", "Dbtel"},
            {"DC", "DoCoMo"},
            {"DI", "Dicam"},
            {"D2", "Digma"},
            {"DL", "Dell"},
            {"DN", "DNS"},
            {"DM", "DMM"},
            {"DO", "Doogee"},
            {"DV", "Doov"},
            {"DP", "Dopod"},
            {"DR", "Doro"},
            {"DU", "Dune HD"},
            {"EB", "E-Boda"},
            {"EA", "EBEST"},
            {"EC", "Ericsson"},
            {"ES", "ECS"},
            {"EI", "Ezio"},
            {"EL", "Elephone"},
            {"EP", "Easypix"},
            {"EK", "EKO"},
            {"E1", "Energy Sistem"},
            {"ER", "Ericy"},
            {"EN", "Eton"},
            {"ET", "eTouch"},
            {"EV", "Evertek"},
            {"EO", "Evolveo"},
            {"EX", "Explay"},
            {"EZ", "Ezze"},
            {"FA", "Fairphone"},
            {"FL", "Fly"},
            {"FT", "Freetel"},
            {"FO", "Foxconn"},
            {"FU", "Fujitsu"},
            {"GM", "Garmin-Asus"},
            {"GA", "Gateway"},
            {"GD", "Gemini"},
            {"GI", "Gionee"},
            {"GG", "Gigabyte"},
            {"GS", "Gigaset"},
            {"GC", "GOCLEVER"},
            {"GL", "Goly"},
            {"GO", "Google"},
            {"GR", "Gradiente"},
            {"GU", "Grundig"},
            {"HA", "Haier"},
            {"HS", "Hasee"},
            {"HI", "Hisense"},
            {"HL", "Hi-Level"},
            {"HM", "Homtom"},
            {"HO", "Hosin"},
            {"HP", "HP"},
            {"HT", "HTC"},
            {"HU", "Huawei"},
            {"HX", "Humax"},
            {"HY", "Hyrican"},
            {"HN", "Hyundai"},
            {"IA", "Ikea"},
            {"IB", "iBall"},
            {"IJ", "i-Joy"},
            {"IY", "iBerry"},
            {"IK", "iKoMo"},
            {"IM", "i-mate"},
            {"I1", "iOcean"},
            {"I2", "IconBIT"},
            {"IW", "iNew"},
            {"IF", "Infinix"},
            {"IN", "Innostream"},
            {"II", "Inkti"},
            {"IX", "Intex"},
            {"IO", "i-mobile"},
            {"IQ", "INQ"},
            {"IT", "Intek"},
            {"IV", "Inverto"},
            {"IZ", "iTel"},
            {"JA", "JAY-Tech"},
            {"JI", "Jiayu"},
            {"JO", "Jolla"},
            {"KA", "Karbonn"},
            {"KD", "KDDI"},
            {"K1", "Kiano"},
            {"KI", "Kingsun"},
            {"KO", "Konka"},
            {"KM", "Komu"},
            {"KB", "Koobee"},
            {"KT", "K-Touch"},
            {"KH", "KT-Tech"},
            {"KP", "KOPO"},
            {"KW", "Konrow"},
            {"KR", "Koridy"},
            {"KU", "Kumai"},
            {"KY", "Kyocera"},
            {"KZ", "Kazam"},
            {"L2", "Landvo"},
            {"LV", "Lava"},
            {"LA", "Lanix"},
            {"LC", "LCT"},
            {"L1", "LeEco"},
            {"LE", "Lenovo"},
            {"LN", "Lenco"},
            {"LP", "Le Pan"},
            {"LG", "LG"},
            {"LI", "Lingwin"},
            {"LO", "Loewe"},
            {"LM", "Logicom"},
            {"L3", "Lexand"},
            {"LX", "Lexibook"},
            {"LY", "LYF"},
            {"MJ", "Majestic"},
            {"MA", "Manta Multimedia"},
            {"MB", "Mobistel"},
            {"M3", "Mecer"},
            {"MD", "Medion"},
            {"M2", "MEEG"},
            {"M1", "Meizu"},
            {"ME", "Metz"},
            {"MX", "MEU"},
            {"MI", "MicroMax"},
            {"M5", "MIXC"},
            {"MC", "Mediacom"},
            {"MK", "MediaTek"},
            {"MO", "Mio"},
            {"MM", "Mpman"},
            {"M4", "Modecom"},
            {"MF", "Mofut"},
            {"MR", "Motorola"},
            {"MS", "Microsoft"},
            {"MZ", "MSI"},
            {"MU", "Memup"},
            {"MT", "Mitsubishi"},
            {"ML", "MLLED"},
            {"MQ", "M.T.T."},
            {"MY", "MyPhone"},
            {"NE", "NEC"},
            {"NF", "Neffos"},
            {"NA", "Netgear"},
            {"NG", "NGM"},
            {"NO", "Nous"},
            {"NI", "Nintendo"},
            {"N1", "Noain"},
            {"NK", "Nokia"},
            {"NV", "Nvidia"},
            {"NB", "Noblex"},
            {"NM", "Nomi"},
            {"NN", "Nikon"},
            {"NW", "Newgen"},
            {"NX", "Nexian"},
            {"NT", "NextBook"},
            {"O1", "Odys"},
            {"OD", "Onda"},
            {"ON", "OnePlus"},
            {"OP", "OPPO"},
            {"OR", "Orange"},
            {"OT", "O2"},
            {"OK", "Ouki"},
            {"OU", "OUYA"},
            {"OO", "Opsson"},
            {"OV", "Overmax"},
            {"OY", "Oysters"},
            {"PA", "Panasonic"},
            {"PE", "PEAQ"},
            {"PG", "Pentagram"},
            {"PH", "Philips"},
            {"PI", "Pioneer"},
            {"PL", "Polaroid"},
            {"PM", "Palm"},
            {"PO", "phoneOne"},
            {"PT", "Pantech"},
            {"PY", "Ployer"},
            {"PV", "Point of View"},
            {"PP", "PolyPad"},
            {"P2", "Pomp"},
            {"P3", "PPTV"},
            {"PS", "Positivo"},
            {"PR", "Prestigio"},
            {"P1", "ProScan"},
            {"PU", "PULID"},
            {"QI", "Qilive"},
            {"QT", "Qtek"},
            {"QM", "QMobile"},
            {"QU", "Quechua"},
            {"RA", "Ramos"},
            {"RC", "RCA Tablets"},
            {"RB", "Readboy"},
            {"RI", "Rikomagic"},
            {"RM", "RIM"},
            {"RK", "Roku"},
            {"RO", "Rover"},
            {"SA", "Samsung"},
            {"SD", "Sega"},
            {"SE", "Sony Ericsson"},
            {"S1", "Sencor"},
            {"SF", "Softbank"},
            {"SX", "SFR"},
            {"SG", "Sagem"},
            {"SH", "Sharp"},
            {"SI", "Siemens"},
            {"SN", "Sendo"},
            {"S6", "Senseit"},
            {"SK", "Skyworth"},
            {"SC", "Smartfren"},
            {"SO", "Sony"},
            {"SP", "Spice"},
            {"SU", "SuperSonic"},
            {"S5", "Supra"},
            {"SV", "Selevision"},
            {"SY", "Sanyo"},
            {"SM", "Symphony"},
            {"SR", "Smart"},
            {"S7", "Smartisan"},
            {"S4", "Star"},
            {"S8", "STK"},
            {"ST", "Storex"},
            {"S2", "Stonex"},
            {"S3", "SunVan"},
            {"SZ", "Sumvision"},
            {"TA", "Tesla"},
            {"T5", "TB Touch"},
            {"TC", "TCL"},
            {"T7", "Teclast"},
            {"TE", "Telit"},
            {"T4", "ThL"},
            {"TH", "TiPhone"},
            {"TB", "Tecno Mobile"},
            {"TD", "Tesco"},
            {"TI", "TIANYU"},
            {"TL", "Telefunken"},
            {"T2", "Telenor"},
            {"TM", "T-Mobile"},
            {"TN", "Thomson"},
            {"T1", "Tolino"},
            {"TO", "Toplux"},
            {"TS", "Toshiba"},
            {"TT", "TechnoTrend"},
            {"T6", "TrekStor"},
            {"T3", "Trevi"},
            {"TU", "Tunisie Telecom"},
            {"TR", "Turbo-X"},
            {"TV", "TVC"},
            {"TX", "TechniSat"},
            {"TZ", "teXet"},
            {"UN", "Unowhy"},
            {"US", "Uniscope"},
            {"UM", "UMIDIGI"},
            {"UT", "UTStarcom"},
            {"VA", "Vastking"},
            {"VD", "Videocon"},
            {"VE", "Vertu"},
            {"VI", "Vitelcom"},
            {"VK", "VK Mobile"},
            {"VS", "ViewSonic"},
            {"VT", "Vestel"},
            {"VL", "Verykool"},
            {"VV", "Vivo"},
            {"V1", "Voto"},
            {"VO", "Voxtel"},
            {"VF", "Vodafone"},
            {"VZ", "Vizio"},
            {"VW", "Videoweb"},
            {"WA", "Walton"},
            {"WF", "Wileyfox"},
            {"WE", "WellcoM"},
            {"WY", "Wexler"},
            {"WI", "Wiko"},
            {"WL", "Wolder"},
            {"WG", "Wolfgang"},
            {"WO", "Wonu"},
            {"WX", "Woxter"},
            {"XI", "Xiaomi"},
            {"XO", "Xolo"},
            {"YA", "Yarvik"},
            {"YU", "Yuandao"},
            {"YS", "Yusun"},
            {"YT", "Ytone"},
            {"ZE", "Zeemi"},
            {"ZO", "Zonda"},
            {"ZP", "Zopo"},
            {"ZT", "ZTE"},
            {"ZN", "Zen"},

            // legacy brands, might be removed in future versions
            {"WB", "Web TV"},
            {"XX", "Unknown"}
        };

        public int? GetDeviceType()
        {
            return deviceType;
        }

        /// <summary>
        /// Returns available device types
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> GetAvailableDeviceTypes()
        {
            return DeviceTypes;
        }

        /// <summary>
        /// Returns names of all available device types
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAvailableDeviceTypeNames()
        {
            return DeviceTypes.Keys.ToList();
        }

        public static KeyValuePair<string,int> GetDeviceName(int deviceType)
        {
            return DeviceTypes.ContainsValue(deviceType) ? DeviceTypes.FirstOrDefault(t=>t.Value.Equals(deviceType)): new KeyValuePair<string, int>();
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public static string  GetFullName(string brandId)
        {
            if (string.IsNullOrEmpty(brandId))
                return "";
            return DeviceBrands.ContainsKey(brandId) ? DeviceBrands[brandId] : "";
        }

        /// <inheritdoc />
        public override void SetUserAgent(string ua)
        {
            Reset();
            base.SetUserAgent(ua);
        }

        public override ParseResult<TResult> Parse()
        {
            var result = new ParseResult<TResult>();
            var regexes = regexList.Cast<KeyValuePair<string, DeviceModel>>();

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

            if (localMatches == null) return result;

            if (!localDevice.Key.Equals(UnknownBrand))
            {
                var localBrand = DeviceBrands.SingleOrDefault(x => x.Value == localDevice.Key).Key;
                if (string.IsNullOrEmpty(localBrand))
                {
                    // This Exception should never be thrown. If so a defined brand name is missing in DeviceBrands
                    throw new Exception("The brand with name '"+ localDevice.Key + "' should be listed in the deviceBrands array.");
                }
                brand = localBrand;
            }

            if (localDevice.Value.Device != null && DeviceTypes.ContainsKey(localDevice.Value.Device))
            {
                DeviceTypes.TryGetValue(localDevice.Value.Device, out var localDeviceType);
                deviceType = localDeviceType;
            }
            model = "";
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
                    result.Add(new TResult { Name = model, Brand = brand, Type = deviceType });
                    return result;
                 }

                model = BuildModel(localModel.Name, localModelMatches)?.Trim();

                if (localModel.Brand != null)
                {
                    var localBrand = DeviceBrands.SingleOrDefault(x => x.Value == localModel.Brand).Key;
                    if (!string.IsNullOrEmpty(brand))
                    {
                        brand = localBrand;
                    }
                }
                if (localModel.Device != null && DeviceTypes.ContainsKey(localModel.Device))
                {
                    DeviceTypes.TryGetValue(localModel.Device, out var localDeviceType);
                    deviceType = localDeviceType;
                }
            }
            result.Add(new TResult { Name = model, Brand = brand, Type = deviceType });

            return result;
        }

        protected string BuildModel(string model, string[] matches)
        {
            model = BuildByMatch(model, matches);

            model = model.Replace('_', ' ');
            model = Regex.Replace(model, " TD$", "", RegexOptions.IgnoreCase);

            return model == "Build" ? null : model;
        }

        protected void Reset()
        {
            deviceType = null;
            model = null;
            brand = null;
        }
    }
}