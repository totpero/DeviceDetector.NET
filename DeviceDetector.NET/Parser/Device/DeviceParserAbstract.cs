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
            {"phablet", DeviceType.DEVICE_TYPE_PHABLET},
            {"smart speaker", DeviceType.DEVICE_TYPE_SMART_SPEAKER},
            {"wearable", DeviceType.DEVICE_TYPE_WEARABLE}
        };

        /// <summary>
        /// Known device brands
        /// Note: Before using a new brand in on of the regex files, it needs to be added here
        /// </summary>
        public static Dictionary<string, string> DeviceBrands = new Dictionary<string, string>
        {
            {"36", "360"},
            {"3Q", "3Q"},
            {"4G", "4Good"},
            {"AE", "Ace"},
            {"AA", "AllCall"},
            {"AC", "Acer"},
            {"A9", "Advan"},
            {"AD", "Advance"},
            {"A3", "AGM"},
            {"AZ", "Ainol"},
            {"AI", "Airness"},
            {"0A", "AIS"},
            {"AW", "Aiwa"},
            {"AK", "Akai"},
            {"1A", "Alba"},
            {"AL", "Alcatel"},
            {"4A", "Aligator"},
            {"3A", "AllDocube"},
            {"A2", "Allview"},
            {"A7", "Allwinner"},
            {"A1", "Altech UEC"},
            {"A5", "altron"},
            {"AN", "Arnova"},
            {"5A", "ArmPhone"},
            {"2A", "Atom"},
            {"KN", "Amazon"},
            {"AG", "AMGOO"},
            {"AO", "Amoi"},
            {"AP", "Apple"},
            {"AR", "Archos"},
            {"AS", "ARRIS"},
            {"AB", "Arian Space"},
            {"AT", "Airties"},
            {"A6", "Ark"},
            {"A4", "Ask"},
            {"A8", "Assistant"},
            {"A0", "ANS"},
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
            {"B7", "Bitmore"},
            {"BG", "BGH"},
            {"BL", "Beetel"},
            {"BP", "Blaupunkt"},
            {"B3", "Bluboo"},
            {"BF", "Black Fox"},
            {"B6", "BDF"},
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
            {"BZ", "Bezkam"},
            {"BX", "bq"},
            {"BV", "Bravis"},
            {"BR", "Brondi"},
            {"B1", "Bush"},
            {"CB", "CUBOT"},
            {"CF", "Carrefour"},
            {"CP", "Captiva"},
            {"CS", "Casio"},
            {"R4", "Casper"},
            {"CA", "Cat"},
            {"C9", "CAGI"},
            {"CE", "Celkon"},
            {"CC", "ConCorde"},
            {"C2", "Changhong"},
            {"2C", "Ghong"},
            {"CH", "Cherry Mobile"},
            {"1C", "Chuwi"},
            {"L8", "Clarmin"},
            {"C0", "Clout"},
            {"CK", "Cricket"},
            {"C1", "Crosscall"},
            {"CL", "Compal"},
            {"CN", "CnM"},
            {"CM", "Crius Mea"},
            {"C3", "China Mobile"},
            {"CR", "CreNova"},
            {"CT", "Capitel"},
            {"CQ", "Compaq"},
            {"3C", "Contixo"},
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
            {"CV", "CVTE"},
            {"D5", "Daewoo"},
            {"DA", "Danew"},
            {"DT", "Datang"},
            {"D7", "Datawind"},
            {"D1", "Datsun"},
            {"DE", "Denver"},
            {"DW", "DeWalt"},
            {"DX", "DEXP"},
            {"DS", "Desay"},
            {"DB", "Dbtel"},
            {"DC", "DoCoMo"},
            {"DG", "Dialog"},
            {"DI", "Dicam"},
            {"D4", "Digi"},
            {"D3", "Digicel"},
            {"DD", "Digiland"},
            {"D2", "Digma"},
            {"D6", "Divisat"},
            {"DL", "Dell"},
            {"DN", "DNS"},
            {"DM", "DMM"},
            {"DO", "Doogee"},
            {"DV", "Doov"},
            {"DP", "Dopod"},
            {"DR", "Doro"},
            {"D8", "Droxio"},
            {"DU", "Dune HD"},
            {"EB", "E-Boda"},
            {"2E", "E-Ceros"},
            {"EA", "EBEST"},
            {"EC", "Ericsson"},
            {"E7", "Ergo"},
            {"ED", "Energizer"},
            {"E4", "Echo Mobiles"},
            {"ES", "ECS"},
            {"E6", "EE"},
            {"EI", "Ezio"},
            {"EM", "Eks Mobility"},
            {"EL", "Elephone"},
            {"L0", "Element"},
            {"EG", "Elenberg"},
            {"EP", "Easypix"},
            {"EK", "EKO"},
            {"E1", "Energy Sistem"},
            {"ER", "Ericy"},
            {"EE", "Essential"},
            {"EN", "Eton"},
            {"E2", "Essentielb"},
            {"1E", "Etuline"},
            {"ET", "eTouch"},
            {"EV", "Evertek"},
            {"E3", "Evolio"},
            {"EO", "Evolveo"},
            {"EX", "Explay"},
            {"E0", "EvroMedia"},
            {"E5", "Extrem"},
            {"EZ", "Ezze"},
            {"E8", "E-tel"},
            {"E9", "Evercoss"},
            {"EU", "Eurostar"},
            {"FA", "Fairphone"},
            {"FM", "Famoco"},
            {"FE", "Fengxiang"},
            {"FI", "FiGO"},
            {"FL", "Fly"},
            {"F1", "FinePower"},
            {"FT", "Freetel"},
            {"FR", "Forstar"},
            {"FO", "Foxconn"},
            {"F2", "FORME"},
            {"FN", "FNB"},
            {"FU", "Fujitsu"},
            {"FD", "Fondi"},
            {"GT", "G-TiDE"},
            {"GM", "Garmin-Asus"},
            {"GA", "Gateway"},
            {"GD", "Gemini"},
            {"GN", "General Mobile"},
            {"GE", "Geotel"},
            {"GH", "Ghia"},
            {"GI", "Gionee"},
            {"GG", "Gigabyte"},
            {"GS", "Gigaset"},
            {"GZ", "Ginzzu"},
            {"G4", "Globex"},
            {"GC", "GOCLEVER"},
            {"GL", "Goly"},
            {"GO", "Google"},
            {"G5", "Gome"},
            {"G1", "GoMobile"},
            {"GR", "Gradiente"},
            {"GP", "Grape"},
            {"G6", "Gree"},
            {"G0", "Goophone"},
            {"GU", "Grundig"},
            {"HF", "Hafury"},
            {"HA", "Haier"},
            {"HS", "Hasee"},
            {"HE", "HannSpree"},
            {"HI", "Hisense"},
            {"HL", "Hi-Level"},
            {"H2", "Highscreen"},
            {"H1", "Hoffmann"},
            {"HM", "Homtom"},
            {"HO", "Hosin"},
            {"HW", "How"},
            {"HZ", "Hoozo"},
            {"HP", "HP"},
            {"HT", "HTC"},
            {"HD", "Huadoo"},
            {"HU", "Huawei"},
            {"HX", "Humax"},
            {"HY", "Hyrican"},
            {"HN", "Hyundai"},
            {"IG", "iGet"},
            {"IA", "Ikea"},
            {"IB", "iBall"},
            {"IJ", "i-Joy"},
            {"IY", "iBerry"},
            {"IH", "iHunt"},
            {"IK", "iKoMo"},
            {"IE", "iView"},
            {"I8", "iVA"},
            {"IM", "i-mate"},
            {"I1", "iOcean"},
            {"I2", "IconBIT"},
            {"IL", "IMO Mobile"},
            {"I7", "iLA"},
            {"IW", "iNew"},
            {"IP", "iPro"},
            {"IF", "Infinix"},
            {"I0", "InFocus"},
            {"I5", "InnJoo"},
            {"IN", "Innostream"},
            {"IS", "Insignia"},
            {"I4", "Inoi"},
            {"IR", "iRola"},
            {"IU", "iRulu"},
            {"I6", "Irbis"},
            {"II", "Inkti"},
            {"IX", "Intex"},
            {"IO", "i-mobile"},
            {"IQ", "INQ"},
            {"IT", "Intek"},
            {"IV", "Inverto"},
            {"I3", "Impression"},
            {"IZ", "iTel"},
            {"I9", "iZotron"},
            {"JA", "JAY-Tech"},
            {"JI", "Jiayu"},
            {"JO", "Jolla"},
            {"J5", "Just5"},
            {"JF", "JFone"},
            {"JK", "JKL"},
            {"KL", "Kalley"},
            {"K4", "Kaan"},
            {"K7", "Kaiomy"},
            {"K6", "Kanji"},
            {"KA", "Karbonn"},
            {"K5", "KATV1"},
            {"KD", "KDDI"},
            {"K1", "Kiano"},
            {"KV", "Kivi"},
            {"KI", "Kingsun"},
            {"KC", "Kocaso"},
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
            {"K3", "Keneksi"},
            {"K8", "Kuliao"},
            {"KU", "Kumai"},
            {"KY", "Kyocera"},
            {"KZ", "Kazam"},
            {"KE", "Krüger&Matz"},
            {"LQ", "LAIQ"},
            {"L2", "Landvo"},
            {"L6", "Land Rover"},
            {"LV", "Lava"},
            {"LA", "Lanix"},
            {"LK", "Lark"},
            {"LC", "LCT"},
            {"L5", "Leagoo"},
            {"LD", "Ledstar"},
            {"L1", "LeEco"},
            {"L4", "Lemhoov"},
            {"LE", "Lenovo"},
            {"LN", "Lenco"},
            {"LT", "Leotec"},
            {"L7", "Lephone"},
            {"LP", "Le Pan"},
            {"LG", "LG"},
            {"LI", "Lingwin"},
            {"LO", "Loewe"},
            {"LM", "Logicom"},
            {"L3", "Lexand"},
            {"LX", "Lexibook"},
            {"LY", "LYF"},
            {"LU", "Lumus"},
            {"L9", "Luna"},
            {"MN", "M4tel"},
            {"XM", "Macoox"},
            {"MJ", "Majestic"},
            {"MA", "Manta Multimedia"},
            {"6X", "Maxtron"},
            {"5M", "Mann"},
            {"2M", "Masstel"},
            {"MW", "Maxwest"},
            {"7M", "Maxcom"},
            {"M0", "Maze"},
            {"MB", "Mobistel"},
            {"9M", "Mobo"},
            {"0M", "Mecool"},
            {"M3", "Mecer"},
            {"MD", "Medion"},
            {"M2", "MEEG"},
            {"M1", "Meizu"},
            {"3M", "Meitu"},
            {"ME", "Metz"},
            {"MX", "MEU"},
            {"MI", "MicroMax"},
            {"8M", "Mito"},
            {"M5", "MIXC"},
            {"MH", "Mobiola"},
            {"4M", "Mobicel"},
            {"M6", "Mobiistar"},
            {"MC", "Mediacom"},
            {"MK", "MediaTek"},
            {"MO", "Mio"},
            {"M7", "Miray"},
            {"MM", "Mpman"},
            {"LS", "MLS"},
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
            {"1M", "MYFON"},
            {"MG", "MyWigo"},
            {"M8", "Myria"},
            {"6M", "Mystery"},
            {"1R", "Multilaser"},
            {"N3", "Navon"},
            {"N7", "National"},
            {"N5", "NOA"},
            {"NE", "NEC"},
            {"NF", "Neffos"},
            {"NA", "Netgear"},
            {"NU", "NeuImage"},
            {"NG", "NGM"},
            {"NZ", "NG Optics"},
            {"N6", "Nobby"},
            {"NO", "Nous"},
            {"NI", "Nintendo"},
            {"0N", "Newman"},
            {"N1", "Noain"},
            {"N2", "Nextbit"},
            {"NK", "Nokia"},
            {"NV", "Nvidia"},
            {"NB", "Noblex"},
            {"NM", "Nomi"},
            {"N0", "Nuvo"},
            {"NL", "NUU Mobile"},
            {"NY", "NYX Mobile"},
            {"NN", "Nikon"},
            {"N9", "Newland"},
            {"NW", "Newgen"},
            {"NS", "NewsMy"},
            {"NX", "Nexian"},
            {"N8", "NEXON"},
            {"NT", "NextBook"},
            {"O3", "O+"},
            {"OB", "Obi"},
            {"O1", "Odys"},
            {"OD", "Onda"},
            {"ON", "OnePlus"},
            {"OX", "Onix"},
            {"OP", "OPPO"},
            {"O4", "ONN"},
            {"OR", "Orange"},
            {"O5", "Orbic"},
            {"OS", "Ordissimo"},
            {"OT", "O2"},
            {"OK", "Ouki"},
            {"OE", "Oukitel"},
            {"OU", "OUYA"},
            {"OO", "Opsson"},
            {"OV", "Overmax"},
            {"OY", "Oysters"},
            {"OW", "öwn"},
            {"O2", "Owwo"},
            {"PN", "Panacom"},
            {"PA", "Panasonic"},
            {"PB", "PCBOX"},
            {"PC", "PCD"},
            {"PD", "PCD Argentina"},
            {"PE", "PEAQ"},
            {"PG", "Pentagram"},
            {"PH", "Philips"},
            {"4P", "Philco"},
            {"1P", "Phicomm"},
            {"PI", "Pioneer"},
            {"PX", "Pixus"},
            {"PL", "Polaroid"},
            {"P5", "Polytron"},
            {"P9", "Primepad"},
            {"2P", "Prixton"},
            {"P6", "Proline"},
            {"PM", "Palm"},
            {"0P", "POCO"},
            {"3P", "Positivo BGH"},
            {"PO", "phoneOne"},
            {"PT", "Pantech"},
            {"PY", "Ployer"},
            {"P4", "Plum"},
            {"P8", "PocketBook"},
            {"PV", "Point of View"},
            {"PP", "PolyPad"},
            {"P2", "Pomp"},
            {"P3", "PPTV"},
            {"PS", "Positivo"},
            {"PR", "Prestigio"},
            {"P7", "Protruly"},
            {"P1", "ProScan"},
            {"PU", "PULID"},
            {"QB", "Q.Bell"},
            {"QI", "Qilive"},
            {"QT", "Qtek"},
            {"QH", "Q-Touch"},
            {"QM", "QMobile"},
            {"QA", "Quantum"},
            {"QU", "Quechua"},
            {"QO", "Qumo"},
            {"RA", "Ramos"},
            {"RE", "Realme"},
            {"R8", "RED"},
            {"RZ", "Razer"},
            {"RC", "RCA Tablets"},
            {"RB", "Readboy"},
            {"RI", "Rikomagic"},
            {"RN", "Rinno"},
            {"RV", "Riviera"},
            {"RM", "RIM"},
            {"RK", "Roku"},
            {"RO", "Rover"},
            {"R6", "RoverPad"},
            {"RR", "Roadrover"},
            {"R1", "Rokit"},
            {"R3", "Rombica"},
            {"RT", "RT Project"},
            {"RX", "Ritmix"},
            {"R7", "Ritzviva"},
            {"R5", "Ross&Moor"},
            {"R2", "R-TV"},
            {"RG", "RugGear"},
            {"RU", "Runbo"},
            {"RY", "Ryte"},
            {"SQ", "Santin"},
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
            {"7S", "Shift Phones"},
            {"3S", "Shuttle"},
            {"SI", "Siemens"},
            {"SJ", "Silent Circle"},
            {"1S", "Sigma"},
            {"SN", "Sendo"},
            {"0S", "SEMP TCL"},
            {"S6", "Senseit"},
            {"EW", "Senwa"},
            {"SW", "Sky"},
            {"SK", "Skyworth"},
            {"SC", "Smartfren"},
            {"SO", "Sony"},
            {"OI", "Sonim"},
            {"8S", "Soyes"},
            {"SP", "Spice"},
            {"6S", "Spectrum"},
            {"9S", "Sugar"},
            {"5S", "Sunvell"},
            {"SU", "SuperSonic"},
            {"S5", "Supra"},
            {"SV", "Selevision"},
            {"SY", "Sanyo"},
            {"SM", "Symphony"},
            {"4S", "Syrox"},
            {"SR", "Smart"},
            {"S7", "Smartisan"},
            {"S4", "Star"},
            {"SB", "STF Mobile"},
            {"S8", "STK"},
            {"S9", "Savio"},
            {"2S", "Starway"},
            {"ST", "Storex"},
            {"S2", "Stonex"},
            {"S3", "SunVan"},
            {"SZ", "Sumvision"},
            {"SS", "SWISSMOBILITY"},
            {"0W", "Swipe"},
            {"10", "Simbans"},
            {"X1", "Safaricom"},
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
            {"T0", "TD Systems"},
            {"TI", "TIANYU"},
            {"TG", "Telego"},
            {"TL", "Telefunken"},
            {"T2", "Telenor"},
            {"TM", "T-Mobile"},
            {"TN", "Thomson"},
            {"TQ", "Timovi"},
            {"TY", "Tooky"},
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
            {"1T", "Turbo"},
            {"11", "True"},
            {"TV", "TVC"},
            {"TW", "TWM"},
            {"TX", "TechniSat"},
            {"TZ", "teXet"},
            {"UC", "U.S. Cellular"},
            {"UH", "Uhappy"},
            {"U1", "Uhans"},
            {"UG", "Ugoos"},
            {"UL", "Ulefone"},
            {"UO", "Unnecto"},
            {"UN", "Unowhy"},
            {"US", "Uniscope"},
            {"UX", "Unimax"},
            {"UM", "UMIDIGI"},
            {"UU", "Unonu"},
            {"UK", "UTOK"},
            {"UA", "Umax"},
            {"UT", "UTStarcom"},
            {"UZ", "Unihertz"},
            {"VA", "Vastking"},
            {"VD", "Videocon"},
            {"VE", "Vertu"},
            {"VN", "Venso"},
            {"V5", "Vivax"},
            {"VI", "Vitelcom"},
            {"V7", "Vinga"},
            {"VK", "VK Mobile"},
            {"VS", "ViewSonic"},
            {"V9", "Vsun"},
            {"V8", "Vesta"},
            {"VT", "Vestel"},
            {"VR", "Vernee"},
            {"V4", "Verizon"},
            {"VL", "Verykool"},
            {"V6", "VGO TEL"},
            {"VV", "Vivo"},
            {"VX", "Vertex"},
            {"V3", "Vinsoc"},
            {"V2", "Vonino"},
            {"1V", "Vontar"},
            {"VG", "Vorago"},
            {"2V", "Vorke"},
            {"V1", "Voto"},
            {"VO", "Voxtel"},
            {"VF", "Vodafone"},
            {"VM", "Vodacom"},
            {"V0", "VKworld"},
            {"VY", "Voyo"},
            {"VZ", "Vizio"},
            {"VW", "Videoweb"},
            {"VU", "Vulcan"},
            {"WA", "Walton"},
            {"WF", "Wileyfox"},
            {"WN", "Wink"},
            {"WM", "Weimei"},
            {"WE", "WellcoM"},
            {"WY", "Wexler"},
            {"W2", "Wigor"},
            {"WI", "Wiko"},
            {"WP", "Wieppo"},
            {"WL", "Wolder"},
            {"WG", "Wolfgang"},
            {"WO", "Wonu"},
            {"W1", "Woo"},
            {"WX", "Woxter"},
            {"XV", "X-View"},
            {"XI", "Xiaomi"},
            {"XL", "Xiaolajiao"},
            {"XN", "Xion"},
            {"XO", "Xolo"},
            {"XR", "Xoro"},
            {"YA", "Yarvik"},
            {"YD", "Yandex"},
            {"Y2", "Yes"},
            {"YE", "Yezz"},
            {"Y1", "Yu"},
            {"YU", "Yuandao"},
            {"YS", "Yusun"},
            {"YO", "Yota"},
            {"YT", "Ytone"},
            {"YX", "Yxtel"},
            {"ZE", "Zeemi"},
            {"ZK", "Zenek"},
            {"ZF", "Zfiner"},
            {"ZO", "Zonda"},
            {"ZI", "Zidoo"},
            {"ZP", "Zopo"},
            {"ZT", "ZTE"},
            {"ZU", "Zuum"},
            {"ZN", "Zen"},
            {"ZY", "Zync"},
            {"ZQ", "ZYQ"},
            {"XS", "Xshitou"},
            {"XT", "X-TIGI"},
            {"XB", "NEXBOX"},

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
                    throw new Exception("The brand with name '"+ localDevice.Key + "' should be listed in the deviceBrands array. Tried to parse user agent: "+ UserAgent);
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