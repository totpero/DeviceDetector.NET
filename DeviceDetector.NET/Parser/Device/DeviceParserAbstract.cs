using System;
using System.Collections.Generic;
using System.Linq;
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
            {"AA", "AllCall"},
            {"AC", "Acer"},
            {"AD", "Advance"},
            {"A3", "AGM"},
            {"AZ", "Ainol"},
            {"AI", "Airness"},
            {"AW", "Aiwa"},
            {"AK", "Akai"},
            {"AL", "Alcatel"},
            {"A2", "Allview"},
            {"A1", "Altech UEC"},
            {"A5", "altron"},
            {"AN", "Arnova"},
            {"KN", "Amazon"},
            {"AG", "AMGOO"},
            {"AO", "Amoi"},
            {"AP", "Apple"},
            {"AR", "Archos"},
            {"AS", "ARRIS"},
            {"AT", "Airties"},
            {"A4", "Ask"},
            {"AU", "Asus"},
            {"AH", "AVH"},
            {"AV", "Avvio"},
            {"AX", "Audiovox"},
            {"AY", "Axxion"},
            {"AM", "Azumi Mobile"},
            {"BB", "BBK"},
            {"BE", "Becker"},
            {"B5", "Beeline"},
            {"BI", "Bird"},
            {"BT", "Bitel"},
            {"BG", "BGH"},
            {"BL", "Beetel"},
            {"BP", "Blaupunkt"},
            {"B3", "Bluboo"},
            {"BM", "Bmobile"},
            {"BN", "Barnes & Noble"},
            {"BO", "BangOlufsen"},
            {"BQ", "BenQ"},
            {"BS", "BenQ-Siemens"},
            {"BU", "Blu"},
            {"BD", "Bluegood"},
            {"B2", "Blackview"},
            {"B4", "bogo"},
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
            {"C9", "CAGI"},
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
            {"C6", "Comio"},
            {"C7", "ComTrade Tesla"},
            {"C8", "Concord"},
            {"CX", "Crescent"},
            {"C4", "Cyrus"},
            {"DA", "Danew"},
            {"DT", "Datang"},
            {"D1", "Datsun"},
            {"DE", "Denver"},
            {"DX", "DEXP"},
            {"DS", "Desay"},
            {"DB", "Dbtel"},
            {"DC", "DoCoMo"},
            {"DG", "Dialog"},
            {"DI", "Dicam"},
            {"D3", "Digicel"},
            {"DD", "Digiland"},
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
            {"ED", "Energizer"},
            {"E4", "Echo Mobiles"},
            {"ES", "ECS"},
            {"E6", "EE"},
            {"EI", "Ezio"},
            {"EM", "Eks Mobility"},
            {"EL", "Elephone"},
            {"EP", "Easypix"},
            {"EK", "EKO"},
            {"E1", "Energy Sistem"},
            {"ER", "Ericy"},
            {"EE", "Essential"},
            {"EN", "Eton"},
            {"E2", "Essentielb"},
            {"ET", "eTouch"},
            {"EV", "Evertek"},
            {"E3", "Evolio"},
            {"EO", "Evolveo"},
            {"EX", "Explay"},
            {"E5", "Extrem"},
            {"EZ", "Ezze"},
            {"FA", "Fairphone"},
            {"FI", "FiGO"},
            {"FL", "Fly"},
            {"FT", "Freetel"},
            {"FR", "Forstar"},
            {"FO", "Foxconn"},
            {"FN", "FNB"},
            {"FU", "Fujitsu"},
            {"GT", "G-TiDE"},
            {"GM", "Garmin-Asus"},
            {"GA", "Gateway"},
            {"GD", "Gemini"},
            {"GE", "Geotel"},
            {"GH", "Ghia"},
            {"GI", "Gionee"},
            {"GG", "Gigabyte"},
            {"GS", "Gigaset"},
            {"GC", "GOCLEVER"},
            {"GL", "Goly"},
            {"GO", "Google"},
            {"G1", "GoMobile"},
            {"GR", "Gradiente"},
            {"GP", "Grape"},
            {"GU", "Grundig"},
            {"HF", "Hafury"},
            {"HA", "Haier"},
            {"HS", "Hasee"},
            {"HE", "HannSpree"},
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
            {"IH", "iHunt"},
            {"IK", "iKoMo"},
            {"IE", "iView"},
            {"IM", "i-mate"},
            {"I1", "iOcean"},
            {"I2", "IconBIT"},
            {"IL", "IMO Mobile"},
            {"IW", "iNew"},
            {"IP", "iPro"},
            {"IF", "Infinix"},
            {"I5", "InnJoo"},
            {"IN", "Innostream"},
            {"I4", "Inoi"},
            {"II", "Inkti"},
            {"IX", "Intex"},
            {"IO", "i-mobile"},
            {"IQ", "INQ"},
            {"IT", "Intek"},
            {"IV", "Inverto"},
            {"I3", "Impression"},
            {"IZ", "iTel"},
            {"JA", "JAY-Tech"},
            {"JI", "Jiayu"},
            {"JO", "Jolla"},
            {"KL", "Kalley"},
            {"KA", "Karbonn"},
            {"KD", "KDDI"},
            {"K1", "Kiano"},
            {"KI", "Kingsun"},
            {"KG", "Kogan"},
            {"KO", "Konka"},
            {"KM", "Komu"},
            {"KB", "Koobee"},
            {"KT", "K-Touch"},
            {"KH", "KT-Tech"},
            {"KK", "Kodak"},
            {"KP", "KOPO"},
            {"KW", "Konrow"},
            {"KR", "Koridy"},
            {"K2", "KRONO"},
            {"KS", "Kempler & Strauss"},
            {"KU", "Kumai"},
            {"KY", "Kyocera"},
            {"KZ", "Kazam"},
            {"KE", "Krüger&Matz"},
            {"LQ", "LAIQ"},
            {"L2", "Landvo"},
            {"LV", "Lava"},
            {"LA", "Lanix"},
            {"LC", "LCT"},
            {"L5", "Leagoo"},
            {"LD", "Ledstar"},
            {"L1", "LeEco"},
            {"L4", "Lemhoov"},
            {"LE", "Lenovo"},
            {"LN", "Lenco"},
            {"LT", "Leotec"},
            {"LP", "Le Pan"},
            {"LG", "LG"},
            {"LI", "Lingwin"},
            {"LO", "Loewe"},
            {"LM", "Logicom"},
            {"L3", "Lexand"},
            {"LX", "Lexibook"},
            {"LY", "LYF"},
            {"MN", "M4tel"},
            {"MJ", "Majestic"},
            {"MA", "Manta Multimedia"},
            {"MW", "Maxwest"},
            {"MB", "Mobistel"},
            {"M3", "Mecer"},
            {"MD", "Medion"},
            {"M2", "MEEG"},
            {"M1", "Meizu"},
            {"ME", "Metz"},
            {"MX", "MEU"},
            {"MI", "MicroMax"},
            {"M5", "MIXC"},
            {"M6", "Mobiistar"},
            {"MC", "Mediacom"},
            {"MK", "MediaTek"},
            {"MO", "Mio"},
            {"M7", "Miray"},
            {"MM", "Mpman"},
            {"M4", "Modecom"},
            {"MF", "Mofut"},
            {"MR", "Motorola"},
            {"MV", "Movic"},
            {"MS", "Microsoft"},
            {"M9", "MTC"},
            {"MP", "MegaFon"},
            {"MZ", "MSI"},
            {"MU", "Memup"},
            {"MT", "Mitsubishi"},
            {"ML", "MLLED"},
            {"MQ", "M.T.T."},
            {"N4", "MTN"},
            {"MY", "MyPhone"},
            {"MG", "MyWigo"},
            {"M8", "Myria"},
            {"N3", "Navon"},
            {"NE", "NEC"},
            {"NF", "Neffos"},
            {"NA", "Netgear"},
            {"NU", "NeuImage"},
            {"NG", "NGM"},
            {"NO", "Nous"},
            {"NI", "Nintendo"},
            {"N1", "Noain"},
            {"N2", "Nextbit"},
            {"NK", "Nokia"},
            {"NV", "Nvidia"},
            {"NB", "Noblex"},
            {"NM", "Nomi"},
            {"NL", "NUU Mobile"},
            {"NY", "NYX Mobile"},
            {"NN", "Nikon"},
            {"NW", "Newgen"},
            {"NX", "Nexian"},
            {"NT", "NextBook"},
            {"O3", "O+"},
            {"OB", "Obi"},
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
            {"OW", "öwn"},
            {"PN", "Panacom"},
            {"PA", "Panasonic"},
            {"PB", "PCBOX"},
            {"PC", "PCD"},
            {"PD", "PCD Argentina"},
            {"PE", "PEAQ"},
            {"PG", "Pentagram"},
            {"PH", "Philips"},
            {"PI", "Pioneer"},
            {"PL", "Polaroid"},
            {"P9", "Primepad"},
            {"PM", "Palm"},
            {"PO", "phoneOne"},
            {"PT", "Pantech"},
            {"PY", "Ployer"},
            {"P4", "Plum"},
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
            {"QA", "Quantum"},
            {"QU", "Quechua"},
            {"RA", "Ramos"},
            {"RC", "RCA Tablets"},
            {"RB", "Readboy"},
            {"RI", "Rikomagic"},
            {"RV", "Riviera"},
            {"RM", "RIM"},
            {"RK", "Roku"},
            {"RO", "Rover"},
            {"RT", "RT Project"},
            {"SQ", "Santin BiTBiZ"},
            {"SA", "Samsung"},
            {"S0", "Sanei"},
            {"SD", "Sega"},
            {"SL", "Selfix"},
            {"SE", "Sony Ericsson"},
            {"S1", "Sencor"},
            {"SF", "Softbank"},
            {"SX", "SFR"},
            {"SG", "Sagem"},
            {"SH", "Sharp"},
            {"SI", "Siemens"},
            {"SJ", "Silent Circle"},
            {"SN", "Sendo"},
            {"S6", "Senseit"},
            {"SW", "Sky"},
            {"SK", "Skyworth"},
            {"SC", "Smartfren"},
            {"SO", "Sony"},
            {"OI", "Sonim"},
            {"SP", "Spice"},
            {"SU", "SuperSonic"},
            {"S5", "Supra"},
            {"SV", "Selevision"},
            {"SY", "Sanyo"},
            {"SM", "Symphony"},
            {"SR", "Smart"},
            {"S7", "Smartisan"},
            {"S4", "Star"},
            {"SB", "STF Mobile"},
            {"S8", "STK"},
            {"S9", "Savio"},
            {"ST", "Storex"},
            {"S2", "Stonex"},
            {"S3", "SunVan"},
            {"SZ", "Sumvision"},
            {"SS", "SWISSMOBILITY"},
            {"TA", "Tesla"},
            {"T5", "TB Touch"},
            {"TC", "TCL"},
            {"T7", "Teclast"},
            {"TE", "Telit"},
            {"T4", "ThL"},
            {"TH", "TiPhone"},
            {"TB", "Tecno Mobile"},
            {"TP", "TechPad"},
            {"TD", "Tesco"},
            {"TI", "TIANYU"},
            {"TG", "Telego"},
            {"TL", "Telefunken"},
            {"T2", "Telenor"},
            {"TM", "T-Mobile"},
            {"TN", "Thomson"},
            {"TQ", "Timovi"},
            {"T1", "Tolino"},
            {"T9", "Top House"},
            {"TO", "Toplux"},
            {"T8", "Touchmate"},
            {"TS", "Toshiba"},
            {"TT", "TechnoTrend"},
            {"T6", "TrekStor"},
            {"T3", "Trevi"},
            {"TU", "Tunisie Telecom"},
            {"TR", "Turbo-X"},
            {"TV", "TVC"},
            {"TX", "TechniSat"},
            {"TZ", "teXet"},
            {"UC", "U.S. Cellular"},
            {"UH", "Uhappy"},
            {"UL", "Ulefone"},
            {"UO", "Unnecto"},
            {"UN", "Unowhy"},
            {"US", "Uniscope"},
            {"UX", "Unimax"},
            {"UM", "UMIDIGI"},
            {"UU", "Unonu"},
            {"UT", "UTStarcom"},
            {"VA", "Vastking"},
            {"VD", "Videocon"},
            {"VE", "Vertu"},
            {"VI", "Vitelcom"},
            {"VK", "VK Mobile"},
            {"VS", "ViewSonic"},
            {"VT", "Vestel"},
            {"VR", "Vernee"},
            {"VL", "Verykool"},
            {"VV", "Vivo"},
            {"VX", "Vertex"},
            {"V3", "Vinsoc"},
            {"V2", "Vonino"},
            {"VG", "Vorago"},
            {"V1", "Voto"},
            {"VO", "Voxtel"},
            {"VF", "Vodafone"},
            {"VZ", "Vizio"},
            {"VW", "Videoweb"},
            {"VU", "Vulcan"},
            {"WA", "Walton"},
            {"WF", "Wileyfox"},
            {"WN", "Wink"},
            {"WM", "Weimei"},
            {"WE", "WellcoM"},
            {"WY", "Wexler"},
            {"WI", "Wiko"},
            {"WL", "Wolder"},
            {"WG", "Wolfgang"},
            {"WO", "Wonu"},
            {"W1", "Woo"},
            {"WX", "Woxter"},
            {"XV", "X-View"},
            {"XI", "Xiaomi"},
            {"XN", "Xion"},
            {"XO", "Xolo"},
            {"YA", "Yarvik"},
            {"Y2", "Yes"},
            {"YE", "Yezz"},
            {"Y1", "Yu"},
            {"YU", "Yuandao"},
            {"YS", "Yusun"},
            {"YT", "Ytone"},
            {"ZE", "Zeemi"},
            {"ZK", "Zenek"},
            {"ZO", "Zonda"},
            {"ZP", "Zopo"},
            {"ZT", "ZTE"},
            {"ZU", "Zuum"},
            {"ZN", "Zen"},
            {"ZY", "Zync"},

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
                return string.Empty;
            return DeviceBrands.ContainsKey(brandId) ? DeviceBrands[brandId] : string.Empty;
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
            model = GetRegexEngine().Replace(model, " TD$", string.Empty);

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