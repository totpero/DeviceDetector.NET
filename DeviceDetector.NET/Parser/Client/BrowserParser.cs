using System;
using System.Collections.Generic;
using System.Linq;
using DeviceDetectorNET.Class.Client;
using DeviceDetectorNET.Parser.Client.Browser;
using DeviceDetectorNET.Parser.Client.Browser.Engine;
using DeviceDetectorNET.Parser.Client.Hints;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Client;

namespace DeviceDetectorNET.Parser.Client
{
    public class BrowserParser : AbstractClientParser<List<Class.Client.Browser>>
    {

        public const string DefaultParserName = "browser";

        /// <summary>
        /// BrowserHints | null
        /// </summary>
        private readonly BrowserHints browserHints;

        /// <summary>
        /// Known browsers mapped to their internal short codes
        /// </summary>
        protected static readonly Dictionary<string, string> AvailableBrowsers = new Dictionary<string, string>
        {
            { "V1", "Via" },
            { "1P", "Pure Mini Browser" },
            { "4P", "Pure Lite Browser" },
            { "1R", "Raise Fast Browser" },
            { "R1", "Rabbit Private Browser" },
            { "FQ", "Fast Browser UC Lite" },
            { "FJ", "Fast Explorer" },
            { "1L", "Lightning Browser" },
            { "1C", "Cake Browser" },
            { "1I", "IE Browser Fast" },
            { "1V", "Vegas Browser" },
            { "1O", "OH Browser" },
            { "3O", "OH Private Browser" },
            { "1X", "XBrowser Mini" },
            { "1S", "Sharkee Browser" },
            { "2L", "Lark Browser" },
            { "3P", "Pluma" },
            { "1A", "Anka Browser" },
            { "AZ", "Azka Browser" },
            { "1D", "Dragon Browser" },
            { "1E", "Easy Browser" },
            { "DW", "Dark Web Browser" },
            { "D6", "Dark Browser" },
            { "18", "18+ Privacy Browser" },
            { "1B", "115 Browser" },
            { "DM", "1DM Browser" },
            { "1M", "1DM+ Browser" },
            { "2B", "2345 Browser" },
            { "3B", "360 Browser" },
            { "36", "360 Phone Browser" },
            { "7B", "7654 Browser" },
            { "AA", "Avant Browser" },
            { "AB", "ABrowse" },
            { "BW", "AdBlock Browser" },
            { "A7", "Adult Browser" },
            { "A9", "Airfind Secure Browser" },
            { "AF", "ANT Fresco" },
            { "AG", "ANTGalio" },
            { "AL", "Aloha Browser" },
            { "AH", "Aloha Browser Lite" },
            { "A8", "ALVA" },
            { "AM", "Amaya" },
            { "A3", "Amaze Browser" },
            { "A5", "Amerigo" },
            { "AO", "Amigo" },
            { "AN", "Android Browser" },
            { "AE", "AOL Desktop" },
            { "AD", "AOL Shield" },
            { "A4", "AOL Shield Pro" },
            { "A6", "AppBrowzer" },
            { "AP", "APUS Browser" },
            { "AR", "Arora" },
            { "AX", "Arctic Fox" },
            { "AV", "Amiga Voyager" },
            { "AW", "Amiga Aweb" },
            { "PN", "APN Browser" },
            { "RA", "Arc" },
            { "AI", "Arvin" },
            { "AK", "Ask.com" },
            { "AU", "Asus Browser" },
            { "A0", "Atom" },
            { "AT", "Atomic Web Browser" },
            { "A2", "Atlas" },
            { "AS", "Avast Secure Browser" },
            { "VG", "AVG Secure Browser" },
            { "AC", "Avira Secure Browser" },
            { "A1", "AwoX" },
            { "BA", "Beaker Browser" },
            { "BM", "Beamrise" },
            { "F7", "BF Browser" },
            { "BB", "BlackBerry Browser" },
            { "H1", "BrowseHere" },
            { "B8", "Browser Hup Pro" },
            { "BD", "Baidu Browser" },
            { "BS", "Baidu Spark" },
            { "B9", "Bangla Browser" },
            { "BI", "Basilisk" },
            { "BV", "Belva Browser" },
            { "B5", "Beyond Private Browser" },
            { "BE", "Beonex" },
            { "B2", "Berry Browser" },
            { "BT", "Bitchute Browser" },
            { "BH", "BlackHawk" },
            { "B0", "Bloket" },
            { "BJ", "Bunjalloo" },
            { "BL", "B-Line" },
            { "B6", "Black Lion Browser" },
            { "BU", "Blue Browser" },
            { "BO", "Bonsai" },
            { "BN", "Borealis Navigator" },
            { "BR", "Brave" },
            { "BK", "BriskBard" },
            { "K2", "BroKeep Browser" },
            { "B3", "Browspeed Browser" },
            { "BX", "BrowseX" },
            { "BZ", "Browzar" },
            { "B7", "Browlser" },
            { "4B", "BrowsBit" },
            { "BY", "Biyubi" },
            { "BF", "Byffox" },
            { "B4", "BXE Browser" },
            { "CA", "Camino" },
            { "0C", "Cave Browser" },
            { "CL", "CCleaner" },
            { "C8", "CG Browser" },
            { "CJ", "ChanjetCloud" },
            { "C6", "Chedot" },
            { "C9", "Cherry Browser" },
            { "C0", "Centaury" },
            { "CC", "Coc Coc" },
            { "C4", "CoolBrowser" },
            { "C2", "Colibri" },
            { "CD", "Comodo Dragon" },
            { "C1", "Coast" },
            { "CX", "Charon" },
            { "CE", "CM Browser" },
            { "C7", "CM Mini" },
            { "CF", "Chrome Frame" },
            { "HC", "Headless Chrome" },
            { "CH", "Chrome" },
            { "CI", "Chrome Mobile iOS" },
            { "CK", "Conkeror" },
            { "CM", "Chrome Mobile" },
            { "3C", "Chowbo" },
            { "CN", "CoolNovo" },
            { "CO", "CometBird" },
            { "2C", "Comfort Browser" },
            { "CB", "COS Browser" },
            { "CW", "Cornowser" },
            { "C3", "Chim Lac" },
            { "CP", "ChromePlus" },
            { "CR", "Chromium" },
            { "C5", "Chromium GOST" },
            { "CY", "Cyberfox" },
            { "CS", "Cheshire" },
            { "RC", "Crow Browser" },
            { "CT", "Crusta" },
            { "CG", "Craving Explorer" },
            { "CZ", "Crazy Browser" },
            { "CU", "Cunaguaro" },
            { "CV", "Chrome Webview" },
            { "YC", "CyBrowser" },
            { "DB", "dbrowser" },
            { "PD", "Peeps dBrowser" },
            { "D1", "Debuggable Browser" },
            { "DC", "Decentr" },
            { "DE", "Deepnet Explorer" },
            { "DG", "deg-degan" },
            { "DA", "Deledao" },
            { "DT", "Delta Browser" },
            { "D0", "Desi Browser" },
            { "DS", "DeskBrowse" },
            { "D2", "DoCoMo" },
            { "DF", "Dolphin" },
            { "DZ", "Dolphin Zero" },
            { "DO", "Dorado" },
            { "DR", "Dot Browser" },
            { "DL", "Dooble" },
            { "DI", "Dillo" },
            { "DU", "DUC Browser" },
            { "DD", "DuckDuckGo Privacy Browser" },
            { "EC", "Ecosia" },
            { "EW", "Edge WebView" },
            { "EV", "Every Browser" },
            { "EI", "Epic" },
            { "EL", "Elinks" },
            { "EN", "EinkBro" },
            { "EB", "Element Browser" },
            { "EE", "Elements Browser" },
            { "EX", "Explore Browser" },
            { "EZ", "eZ Browser" },
            { "EU", "EUI Browser" },
            { "EP", "GNOME Web" },
            { "G1", "G Browser" },
            { "ES", "Espial TV Browser" },
            { "FG", "fGet" },
            { "FA", "Falkon" },
            { "FX", "Faux Browser" },
            { "F4", "Fiery Browser" },
            { "F1", "Firefox Mobile iOS" },
            { "FB", "Firebird" },
            { "FD", "Fluid" },
            { "FE", "Fennec" },
            { "FF", "Firefox" },
            { "FK", "Firefox Focus" },
            { "FY", "Firefox Reality" },
            { "FR", "Firefox Rocket" },
            { "1F", "Firefox Klar" },
            { "F0", "Float Browser" },
            { "FL", "Flock" },
            { "FP", "Floorp" },
            { "FO", "Flow" },
            { "F2", "Flow Browser" },
            { "FM", "Firefox Mobile" },
            { "FW", "Fireweb" },
            { "FN", "Fireweb Navigator" },
            { "FH", "Flash Browser" },
            { "FS", "Flast" },
            { "F5", "Flyperlink" },
            { "FU", "FreeU" },
            { "F6", "Freedom Browser" },
            { "FT", "Frost" },
            { "F3", "Frost+" },
            { "FI", "Fulldive" },
            { "GA", "Galeon" },
            { "G8", "Gener8" },
            { "GH", "Ghostery Privacy Browser" },
            { "GI", "GinxDroid Browser" },
            { "GB", "Glass Browser" },
            { "GD", "Godzilla Browser" },
            { "GE", "Google Earth" },
            { "GP", "Google Earth Pro" },
            { "GO", "GOG Galaxy" },
            { "GR", "GoBrowser" },
            { "G2", "GO Browser" },
            { "HB", "Harman Browser" },
            { "HS", "HasBrowser" },
            { "HA", "Hawk Turbo Browser" },
            { "HQ", "Hawk Quick Browser" },
            { "HE", "Helio" },
            { "HX", "Hexa Web Browser" },
            { "HI", "Hi Browser" },
            { "HO", "hola! Browser" },
            { "H4", "Holla Web Browser" },
            { "H5", "HotBrowser" },
            { "HJ", "HotJava" },
            { "HT", "HTC Browser" },
            { "HU", "Huawei Browser Mobile" },
            { "HP", "Huawei Browser" },
            { "H3", "HUB Browser" },
            { "IO", "iBrowser" },
            { "IS", "iBrowser Mini" },
            { "IB", "IBrowse" },
            { "I6", "iDesktop PC Browser" },
            { "IC", "iCab" },
            { "I2", "iCab Mobile" },
            { "I1", "Iridium" },
            { "I3", "Iron Mobile" },
            { "I4", "IceCat" },
            { "ID", "IceDragon" },
            { "IV", "Isivioo" },
            { "I8", "IVVI Browser" },
            { "IW", "Iceweasel" },
            { "N3", "Incognito Browser" },
            { "IN", "Inspect Browser" },
            { "I9", "Insta Browser" },
            { "IE", "Internet Explorer" },
            { "I7", "Internet Browser Secure" },
            { "I5", "Indian UC Mini Browser" },
            { "Z0", "InBrowser" },
            { "IM", "IE Mobile" },
            { "IR", "Iron" },
            { "JB", "Japan Browser" },
            { "JS", "Jasmine" },
            { "JA", "JavaFX" },
            { "JL", "Jelly" },
            { "JI", "Jig Browser" },
            { "JP", "Jig Browser Plus" },
            { "JO", "Jio Browser" },
            { "J1", "JioPages" },
            { "KB", "K.Browser" },
            { "KF", "Keepsafe Browser" },
            { "KS", "Kids Safe Browser" },
            { "KI", "Kindle Browser" },
            { "KM", "K-meleon" },
            { "KO", "Konqueror" },
            { "KP", "Kapiko" },
            { "KN", "Kinza" },
            { "KW", "Kiwi" },
            { "KD", "Kode Browser" },
            { "KT", "KUTO Mini Browser" },
            { "KY", "Kylo" },
            { "KZ", "Kazehakase" },
            { "LB", "Cheetah Browser" },
            { "LA", "Lagatos Browser" },
            { "LR", "Lexi Browser" },
            { "LV", "Lenovo Browser" },
            { "LF", "LieBaoFast" },
            { "LG", "LG Browser" },
            { "LH", "Light" },
            { "L1", "Lilo" },
            { "LI", "Links" },
            { "LC", "LogicUI TV Browser" },
            { "IF", "Lolifox" },
            { "LO", "Lovense Browser" },
            { "LT", "LT Browser" },
            { "LU", "LuaKit" },
            { "LJ", "LUJO TV Browser" },
            { "LL", "Lulumi" },
            { "LS", "Lunascape" },
            { "LN", "Lunascape Lite" },
            { "LX", "Lynx" },
            { "L2", "Lynket Browser" },
            { "MD", "Mandarin" },
            { "M5", "MarsLab Web Browser" },
            { "M1", "mCent" },
            { "MB", "MicroB" },
            { "MC", "NCSA Mosaic" },
            { "MZ", "Meizu Browser" },
            { "ME", "Mercury" },
            { "M2", "Me Browser" },
            { "MF", "Mobile Safari" },
            { "MI", "Midori" },
            { "M3", "Midori Lite" },
            { "MO", "Mobicip" },
            { "MU", "MIUI Browser" },
            { "MS", "Mobile Silk" },
            { "MN", "Minimo" },
            { "MT", "Mint Browser" },
            { "MX", "Maxthon" },
            { "M4", "MaxTube Browser" },
            { "MA", "Maelstrom" },
            { "MM", "Mmx Browser" },
            { "NM", "MxNitro" },
            { "MY", "Mypal" },
            { "MR", "Monument Browser" },
            { "MW", "MAUI WAP Browser" },
            { "NW", "Navigateur Web" },
            { "NK", "Naked Browser" },
            { "NA", "Naked Browser Pro" },
            { "NR", "NFS Browser" },
            { "NB", "Nokia Browser" },
            { "NO", "Nokia OSS Browser" },
            { "NV", "Nokia Ovi Browser" },
            { "N2", "Norton Secure Browser" },
            { "NX", "Nox Browser" },
            { "N1", "NOMone VR Browser" },
            { "NE", "NetSurf" },
            { "NF", "NetFront" },
            { "NL", "NetFront Life" },
            { "NP", "NetPositive" },
            { "NS", "Netscape" },
            { "WR", "NextWord Browser" },
            { "NT", "NTENT Browser" },
            { "NU", "Nuanti Meta" },
            { "O9", "Ocean Browser" },
            { "NI", "Nuviu" },
            { "OC", "Oculus Browser" },
            { "O6", "Odd Browser" },
            { "O1", "Opera Mini iOS" },
            { "OB", "Obigo" },
            { "O2", "Odin" },
            { "2O", "Odin Browser" },
            { "H2", "OceanHero" },
            { "OD", "Odyssey Web Browser" },
            { "OF", "Off By One" },
            { "O5", "Office Browser" },
            { "HH", "OhHai Browser" },
            { "OE", "ONE Browser" },
            { "N4", "Onion Browser" },
            { "Y1", "Opera Crypto" },
            { "OX", "Opera GX" },
            { "OG", "Opera Neon" },
            { "OH", "Opera Devices" },
            { "OI", "Opera Mini" },
            { "OM", "Opera Mobile" },
            { "OP", "Opera" },
            { "ON", "Opera Next" },
            { "OO", "Opera Touch" },
            { "OA", "Orca" },
            { "OS", "Ordissimo" },
            { "OR", "Oregano" },
            { "O0", "Origin In-Game Overlay" },
            { "OY", "Origyn Web Browser" },
            { "O8", "OrNET Browser" },
            { "OV", "Openwave Mobile Browser" },
            { "O3", "OpenFin" },
            { "O4", "Open Browser" },
            { "4U", "Open Browser 4U" },
            { "5G", "Open Browser fast 5G" },
            { "O7", "Open TV Browser" },
            { "OW", "OmniWeb" },
            { "OT", "Otter Browser" },
            { "PL", "Palm Blazer" },
            { "PM", "Pale Moon" },
            { "PY", "Polypane" },
            { "PP", "Oppo Browser" },
            { "P6", "Opus Browser" },
            { "PR", "Palm Pre" },
            { "PU", "Puffin" },
            { "2P", "Puffin Web Browser" },
            { "PW", "Palm WebPro" },
            { "PA", "Palmscape" },
            { "P7", "Pawxy" },
            { "PE", "Perfect Browser" },
            { "P1", "Phantom.me" },
            { "PH", "Phantom Browser" },
            { "PX", "Phoenix" },
            { "PB", "Phoenix Browser" },
            { "P8", "PICO Browser" },
            { "PF", "PlayFree Browser" },
            { "PK", "PocketBook Browser" },
            { "PO", "Polaris" },
            { "PT", "Polarity" },
            { "LY", "PolyBrowser" },
            { "PI", "PrivacyWall" },
            { "P4", "Privacy Explorer Fast Safe" },
            { "P3", "Private Internet Browser" },
            { "P5", "Proxy Browser" },
            { "P2", "Pi Browser" },
            { "P0", "PronHub Browser" },
            { "PC", "PSI Secure Browser" },
            { "RW", "Reqwireless WebViewer" },
            { "PS", "Microsoft Edge" },
            { "QA", "Qazweb" },
            { "Q3", "Qmamu" },
            { "Q4", "Quick Search TV" },
            { "Q2", "QQ Browser Lite" },
            { "Q1", "QQ Browser Mini" },
            { "QQ", "QQ Browser" },
            { "QS", "Quick Browser" },
            { "QT", "Qutebrowser" },
            { "QU", "Quark" },
            { "QZ", "QupZilla" },
            { "QM", "Qwant Mobile" },
            { "QW", "QtWebEngine" },
            { "RE", "Realme Browser" },
            { "RK", "Rekonq" },
            { "RM", "RockMelt" },
            { "RB", "Roku Browser" },
            { "SB", "Samsung Browser" },
            { "3L", "Samsung Browser Lite" },
            { "SA", "Sailfish Browser" },
            { "S8", "Seewo Browser" },
            { "SC", "SEMC-Browser" },
            { "SE", "Sogou Explorer" },
            { "SO", "Sogou Mobile Browser" },
            { "RF", "SOTI Surf" },
            { "2S", "Soul Browser" },
            { "T0", "Soundy Browser" },
            { "SF", "Safari" },
            { "PV", "Safari Technology Preview" },
            { "S5", "Safe Exam Browser" },
            { "SW", "SalamWeb" },
            { "VN", "Savannah Browser" },
            { "SD", "SavySoda" },
            { "S9", "Secure Browser" },
            { "SV", "SFive" },
            { "SH", "Shiira" },
            { "K1", "Sidekick" },
            { "S1", "SimpleBrowser" },
            { "3S", "SilverMob US" },
            { "SY", "Sizzy" },
            { "K3", "Skye" },
            { "SK", "Skyfire" },
            { "SS", "Seraphic Sraf" },
            { "KK", "SiteKiosk" },
            { "SL", "Sleipnir" },
            { "S6", "Slimjet" },
            { "S7", "SP Browser" },
            { "9S", "Sony Small Browser" },
            { "8S", "Secure Private Browser" },
            { "X2", "SecureX" },
            { "T1", "Stampy Browser" },
            { "7S", "7Star" },
            { "SQ", "Smart Browser" },
            { "6S", "Smart Search & Web Browser" },
            { "LE", "Smart Lenovo Browser" },
            { "OZ", "Smooz" },
            { "SN", "Snowshoe" },
            { "B1", "Spectre Browser" },
            { "S2", "Splash" },
            { "SI", "Sputnik Browser" },
            { "SR", "Sunrise" },
            { "0S", "Sunflower Browser" },
            { "SP", "SuperBird" },
            { "SU", "Super Fast Browser" },
            { "5S", "SuperFast Browser" },
            { "HR", "Sushi Browser" },
            { "S3", "surf" },
            { "4S", "Surf Browser" },
            { "SG", "Stargon" },
            { "S0", "START Internet Browser" },
            { "S4", "Steam In-Game Overlay" },
            { "ST", "Streamy" },
            { "SX", "Swiftfox" },
            { "SZ", "Seznam Browser" },
            { "W1", "Sweet Browser" },
            { "2X", "SX Browser" },
            { "TP", "T+Browser" },
            { "TR", "T-Browser" },
            { "TO", "t-online.de Browser" },
            { "TA", "Tao Browser" },
            { "TH", "Thor" },
            { "1T", "Tor Browser" },
            { "TF", "TenFourFox" },
            { "TB", "Tenta Browser" },
            { "TE", "Tesla Browser" },
            { "TZ", "Tizen Browser" },
            { "TI", "Tint Browser" },
            { "TC", "TUC Mini Browser" },
            { "TU", "Tungsten" },
            { "TG", "ToGate" },
            { "TS", "TweakStyle" },
            { "TV", "TV Bro" },
            { "U0", "U Browser" },
            { "UB", "UBrowser" },
            { "UC", "UC Browser" },
            { "UH", "UC Browser HD" },
            { "UM", "UC Browser Mini" },
            { "UT", "UC Browser Turbo" },
            { "UI", "Ui Browser Mini" },
            { "UR", "UR Browser" },
            { "UZ", "Uzbl" },
            { "UE", "Ume Browser" },
            { "V0", "vBrowser" },
            { "VA", "Vast Browser" },
            { "V3", "VD Browser" },
            { "VE", "Venus Browser" },
            { "WD", "Vewd Browser" },
            { "N0", "Nova Video Downloader Pro" },
            { "VS", "Viasat Browser" },
            { "VI", "Vivaldi" },
            { "VV", "vivo Browser" },
            { "V2", "Vivid Browser Mini" },
            { "VB", "Vision Mobile Browser" },
            { "V4", "Vertex Surf" },
            { "VM", "VMware AirWatch" },
            { "WI", "Wear Internet Browser" },
            { "WP", "Web Explorer" },
            { "W3", "Web Browser & Explorer" },
            { "WE", "WebPositive" },
            { "WF", "Waterfox" },
            { "WB", "Wave Browser" },
            { "WA", "Wavebox" },
            { "WH", "Whale Browser" },
            { "WO", "wOSBrowser" },
            { "WT", "WeTab Browser" },
            { "1W", "World Browser" },
            { "WL", "Wolvic" },
            { "YG", "YAGI" },
            { "YJ", "Yahoo! Japan Browser" },
            { "YA", "Yandex Browser" },
            { "YL", "Yandex Browser Lite" },
            { "YN", "Yaani Browser" },
            { "Y2", "Yo Browser" },
            { "YB", "Yolo Browser" },
            { "YO", "YouCare" },
            { "YZ", "Yuzu Browser" },
            { "XR", "xBrowser" },
            { "XB", "X Browser Lite" },
            { "X0", "X-VPN" },
            { "X1", "xBrowser Pro Super Fast" },
            { "XN", "XNX Browser" },
            { "XT", "XtremeCast" },
            { "XS", "xStand" },
            { "XI", "Xiino" },
            { "XO", "Xooloo Internet" },
            { "XV", "Xvast" },
            { "ZE", "Zetakey" },
            { "ZV", "Zvu" },
            { "ZI", "Zirco Browser" },
            { "ZR", "Zordo Browser" },

            // detected browsers in older versions
            // { "IA", "Iceape" },  => pim
            // { "SM", "SeaMonkey" },  => pim
        };

        /// <summary>
        /// Browser families mapped to the short codes of the associated browsers
        /// </summary>
        protected static readonly Dictionary<string, string[]> BrowserFamilies = new Dictionary<string, string[]>
        {
            {"Android Browser"    , new []{"AN", "MU"}},
            {"BlackBerry Browser" , new []{"BB"}},
            {"Baidu"              , new []{"BD", "BS"}},
            {"Amiga"              , new []{"AV", "AW"}},
            {"Chrome"             , new []{"1B", "2B", "7S", "A0", "AC", "A4", "AE", "AH", "AI",
                                            "AO", "AS", "BA", "BM", "BR", "C2", "C3", "C5", "C4",
                                            "C6", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CL",
                                            "CM", "CN", "CP", "CR", "CV", "CW", "DA", "DD", "DG",
                                            "DR", "EC", "EE", "EU", "EW", "FA", "FS", "GB", "GI",
                                            "H2", "HA", "HE", "HH", "HS", "I3", "IR", "JB", "KN",
                                            "KW", "LF", "LL", "LO", "M1", "MA", "MD", "MR", "MS",
                                            "MT", "MZ", "NM", "NR", "O0", "O2", "O3", "OC", "PB",
                                            "PT", "QU", "QW", "RM", "S4", "S6", "S8", "S9", "SB",
                                            "SG", "SS", "SU", "SV", "SW", "SY", "SZ", "T1", "TA",
                                            "TB", "TG", "TR", "TS", "TU", "TV", "UB", "UR", "VE",
                                            "VG", "VI", "VM", "WP", "WH", "XV", "YJ", "YN", "FH",
                                            "B1", "BO", "HB", "PC", "LA", "LT", "PD", "HR", "HU",
                                            "HP", "IO", "TP", "CJ", "HQ", "HI", "NA", "BW", "YO",
                                            "DC", "G8", "DT", "AP", "AK", "UI", "SD", "VN", "4S",
                                            "2S", "RF", "LR", "SQ", "BV", "L1", "F0", "KS", "V0",
                                            "C8", "AZ", "MM", "BT", "N0", "P0", "F3", "VS", "DU",
                                            "D0", "P1", "O4", "8S", "H3", "TE", "WB", "K1", "P2",
                                            "XO", "U0", "B0", "VA", "X0", "NX", "O5", "R1", "I1",
                                            "HO", "A5", "X1", "18", "B5", "B6", "TC", "A6", "2X",
                                            "F4", "YG", "WR", "NA", "DM", "1M", "A7", "XN", "XT",
                                            "XB", "W1", "HT", "B8", "F5", "B9", "WA", "T0", "HC",
                                            "O6", "P7", "LJ", "LC", "O7", "N2", "A8", "P8", "RB",
                                            "1W", "EV", "I9", "V4", "H4", "1T", "M5", "0S", "0C",
                                            "ZR", "D6", "F6", "RC", "WD", "P3", "FT", "A9", "X2",
                                            "N3", "GD", "O9", "Q3", "F7", "K2", "P5", "H5", "V3",
                                            "K3", "Q4", "G2",
            }},
            {"Firefox"            , new []{"AX", "BI", "BF", "BH", "BN", "C0", "CU", "EI", "F1",
                                            "FB", "FE", "FF", "FM", "FR", "FY", "GZ", "I4", "IF",
                                            "IW", "LH", "LY", "MB", "MN", "MO", "MY", "OA", "OS",
                                            "PI", "PX", "QA", "S5", "SX", "TF", "TO", "WF", "ZV",
                                            "FP", "AD", "WL"

            }},
            {"Internet Explorer"  , new []{"BZ", "CZ", "IE", "IM", "PS"}},
            {"Konqueror"          , new []{"KO"}},
            {"NetFront"           , new []{"NF"}},
            {"NetSurf"            , new []{"NE"}},
            {"Nokia Browser"      , new []{"DO", "NB", "NO", "NV"}},
            {"Opera"              , new []{"O1", "OG", "OH", "OI", "OM", "ON", "OO", "OP", "OX", "Y1"}},
            {"Safari"             , new []{"MF", "S7", "SF", "SO", "PV" }},
            {"Sailfish Browser"   , new []{"SA"}},
        };

        /// <summary>
        /// Browsers that are available for mobile devices only
        /// </summary>
        protected static readonly string[] MobileOnlyBrowsers = {"36", "AH", "AI", "BL", "C1", "C4", "CB", "CW", "DB",
                                                                    "DD", "DT", "EU", "EZ", "FK", "FM", "FR", "FX", "GH",
                                                                    "GI", "GR", "HA", "HU", "IV", "JB", "KD", "M1", "MF",
                                                                    "MN", "MZ", "NX", "OC", "OI", "OM", "OZ", "PU", "PI",
                                                                    "PE", "QU", "RE", "S0", "S7", "SA", "SB", "SG", "SK",
                                                                    "ST", "SU", "T1", "UH", "UM", "UT", "VE", "VV", "WI",
                                                                    "WP", "YN", "IO", "IS", "HQ", "RW", "HI", "PN", "BW",
                                                                    "YO", "PK", "MR", "AP", "AK", "UI", "SD", "VN", "4S",
                                                                    "RF", "LR", "SQ", "BV", "L1", "F0", "KS", "V0", "C8",
                                                                    "AZ", "MM", "BT", "N0", "P0", "F3", "DU", "D0", "P1",
                                                                    "O4", "XO", "U0", "B0", "VA", "X0", "A5", "X1", "18",
                                                                    "B5", "B6", "TC", "A6", "2X", "F4", "YG", "WR", "NA",
                                                                    "DM", "1M", "A7", "XN", "XT", "XB", "W1", "HT", "B7",
                                                                    "B9", "T0", "I8", "O6", "P7", "O8", "4B", "A8", "P8",
                                                                    "1W", "EV", "Z0", "I9", "V4", "H4", "M5", "0S", "0C",
                                                                    "ZR", "D6", "F6",};

        public override Dictionary<string, string[]> ClientHintMapping => new Dictionary<string, string[]>
        {
             {"Chrome", new [] {"Google Chrome"}},
             {"Vewd Browser", new [] {"Vewd Core"}},
             {"DuckDuckGo Privacy Browser", new [] {"DuckDuckGo"}}
        };


        private const int MaxVersionParts = 5;


        private void Init()
        {

            FixtureFile = "regexes/client/browsers.yml";
            ParserName = DefaultParserName;
            regexList = GetRegexes();
        }
        private BrowserParser()
        {
            Init();
        }

        public BrowserParser(string ua = "", ClientHints clientHints = null)
           : base(ua, clientHints)
        {
            Init();
            browserHints = new BrowserHints(UserAgent, ClientHints);
        }

        /// <summary>
        /// Sets the client hints to parse
        /// </summary>
        public override void SetClientHints(ClientHints clientHints)
        {
            base.SetClientHints(clientHints);
            browserHints.SetClientHints(clientHints);
        }

        /// <summary>
        /// Sets the user agent to parse
        /// </summary>
        /// <param name="ua"></param>
        public override void SetUserAgent(string ua)
        {
            base.SetUserAgent(ua);
            browserHints.SetUserAgent(ua);
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetBrowserShortName(string name)
        {
            name = name.ToLower();
            foreach (var availableBrowser in AvailableBrowsers)
            {
                if (availableBrowser.Value.ToLower() == name)
                {
                    return availableBrowser.Key;
                }
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="browserLabel"></param>
        /// <param name="name"></param>
        /// <returns>string|null If null, "Unknown"</returns>
        public static string GetBrowserFamily(string browserLabel)
        {
            //if (AvailableBrowsers.TryGetValue(browserLabel, out var browserName))
            //{
            //    browserLabel = browserName;
            //}
            foreach (var family in BrowserFamilies)
            {
                if (!family.Value.Contains(browserLabel)) continue;
                return family.Key;
            }
            return null;
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

        public override ParseResult<ClientMatchResult> Parse()
        {
            var result = new ParseResult<ClientMatchResult>();
            var client = new BrowserMatchResult();

            var browserFromClientHints = ParseBrowserFromClientHints();
            var browserFromUserAgent = ParseBrowserFromUserAgent();

            // use client hints in favor of user agent data if possible
            if (!string.IsNullOrEmpty(browserFromClientHints.Name) && !string.IsNullOrEmpty(browserFromClientHints.Version))
            {
                client.Name = browserFromClientHints.Name;
                client.Version = browserFromClientHints.Version;
                client.ShortName = browserFromClientHints.ShortName;

                // If the version reported from the client hints is YYYY or YYYY.MM (e.g., 2022 or 2022.04),
                // then it is the Iridium browser
                // https://iridiumbrowser.de/news/
                if (!GetRegexEngine().Match(client.Version, "/^202[0-4]/")){
                    client.Name = "Iridium";
                    client.ShortName = "I1";
                }

                if ("Atom" == client.Name || "Huawei Browser" == client.Name)
                {
                    client.Version = browserFromUserAgent.Version;
                }

                if ("DuckDuckGo Privacy Browser" == client.Name)
                {
                    client.Version = string.Empty;
                }
                
                if ("Vewd Browser" == client.Name)
                {
                    client.Engine = browserFromUserAgent.Engine ?? string.Empty;
                    client.EngineVersion = browserFromUserAgent.EngineVersion ?? string.Empty;
                }

                // If client hints report Chromium, but user agent detects a chromium based browser, we favor this instead
                if ("Chromium" == client.Name
                    && !string.IsNullOrEmpty(browserFromUserAgent.Name)
                    && "Chromium" != browserFromUserAgent.Name
                    //&& "Chrome" == GetBrowserFamily(browserFromUserAgent.Name)
                    )
                {
                    client.Name = browserFromUserAgent.Name;
                    client.ShortName = browserFromUserAgent.ShortName;
                    client.Version = browserFromUserAgent.Version;
                }

                // Fix mobile browser names e.g. Chrome => Chrome Mobile
                if (client.Name + " Mobile" == browserFromUserAgent.Name)
                {
                    client.Name = browserFromUserAgent.Name;
                    client.ShortName = browserFromUserAgent.ShortName;
                }

                // If user agent detects another browser, but the family matches, we use the detected engine from user agent
                if (client.Name != browserFromUserAgent.Name
                    && GetBrowserFamily(client.Name) == GetBrowserFamily(browserFromUserAgent.Name))
                {
                    client.Engine = browserFromUserAgent.Engine ?? string.Empty;
                    client.EngineVersion = browserFromUserAgent.EngineVersion ?? string.Empty;
                }

                if(client.Name == browserFromUserAgent.Name){
                    client.Engine = browserFromUserAgent.Engine ?? string.Empty;
                    client.EngineVersion = browserFromUserAgent.EngineVersion ?? string.Empty;

                    // In case the user agent reports a more detailed version, we try to use this instead
                    if (!string.IsNullOrEmpty(browserFromUserAgent.Version)
                        && 0 == browserFromUserAgent.Version.IndexOf(client.Version)
                        //&& \version_compare($version, $browserFromUserAgent['version'], '<') //@todo
                        )
                    {
                        client.Version = browserFromUserAgent.Version;
                    }
                }

            }
            else
            {
                client.Name = browserFromUserAgent.Name;
                client.Version = browserFromUserAgent.Version;
                client.ShortName = browserFromUserAgent.ShortName;
                client.Engine = browserFromUserAgent.Engine;
                client.EngineVersion = browserFromUserAgent.EngineVersion;
            }
            client.Family = GetBrowserFamily(client.ShortName) ?? "Unknown";
            var appHash = browserHints.Parse();
            if (appHash.Success && appHash.Match.Name != client.Name)
            {
                client.Name = appHash.Match.Name;
                client.Version = string.Empty;
                client.ShortName = GetBrowserShortName(client.Name);
                if (IsMatchUserAgent("Chrome/.+ Safari/537.36"))
                {
                    client.Engine = "Blink";
                    client.Family = GetBrowserFamily(client.ShortName) ?? "Chrome";
                    client.EngineVersion = BuildEngineVersion(client.Engine);
                }

                if (string.IsNullOrEmpty(client.ShortName))
                {
                    // This Exception should never be thrown. If so a defined browser name is missing in $availableBrowsers
                    throw new Exception("Detected browser name " + client.Name + " was not found in AvailableBrowsers. Tried to parse user agent: " + UserAgent);
                }
            }

            if (string.IsNullOrEmpty(client.Name))
                return result;

            // exclude Blink engine version for browsers
            if ("Blink" == client.Engine && "Flow Browser" == client.Name)
            {
                client.EngineVersion = string.Empty;
            }

            // the browser simulate ua for Android OS
            if ("Every Browser" == client.Name) {
                client.Family = "Chrome";
                client.Engine = "Blink";
                client.EngineVersion = string.Empty;
            }

            client.Type = DefaultParserName;

            return result.Add(client);
        }

        /// <summary>
        /// Returns the browser that can be safely detected from client hints
        /// </summary>
        protected BrowserMatchResult ParseBrowserFromClientHints()
        {
            var name = string.Empty;
            var version = string.Empty;
            var @short = string.Empty;
            if (ClientHints != null)
            {
               var brands = ClientHints.GetBrandList();
                if (brands.Any())
                {
                    foreach (var brand in brands)
                    {
                        var brandName = ApplyClientHintMapping(brand.Key);
                        foreach (var availableBrowser in AvailableBrowsers)
                        {
                            if (FuzzyCompare(brandName, availableBrowser.Value) 
                                || FuzzyCompare(brandName + " Browser", availableBrowser.Value)
                                || FuzzyCompare(brandName, availableBrowser.Value + " Browser")
                                )
                            {
                                name = availableBrowser.Value;
                                @short   = availableBrowser.Key;
                                version = brand.Value;

                                break;
                            }
                        }
                        // If we detected a brand, that is not Chromium, we will use it, otherwise we will look further
                        if (!string.IsNullOrEmpty(name) && "Chromium" != name) {
                            break;
                        }
                    }
                    version = this.ClientHints.GetBrandVersion()  ?? version;
                }
            }

            return new BrowserMatchResult
            {
                Name = name, 
                ShortName = @short,
                Version = BuildVersion(version,Array.Empty<string>()) 
            };
        }

        /// <summary>
        /// Returns the browser that can be detected from user agent
        /// </summary>
        /// <returns></returns>
        protected BrowserMatchResult ParseBrowserFromUserAgent()
        {
            BrowserMatchResult result = new BrowserMatchResult();

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

            if (localBrowser == null) // || localMatches == null
                return result;

            var name = BuildByMatch(localBrowser.Name, localMatches);
            var browserShort =GetBrowserShortName(name);
            if (!string.IsNullOrEmpty(browserShort))
            {
                if (localBrowser.Engine == null)
                    localBrowser.Engine = new Engine();
                var version = BuildVersion(localBrowser.Version, localMatches);
                var engine = BuildEngine(localBrowser.Engine, version);
                var engineVersion = BuildEngineVersion(engine);
                return new BrowserMatchResult
                {
                    Type = ParserName,
                    Name = name,
                    Version = version,
                    ShortName = browserShort,
                    Engine = engine,
                    EngineVersion = engineVersion
                };
            }

            throw new Exception("Detected browser name "+name+" was not found in AvailableBrowsers. Tried to parse user agent: " + UserAgent);
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

                    if (ConvertToVersion(browserVersion).CompareTo(ConvertToVersion(ver)) >= 0)
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

        protected Version ConvertToVersion(string versionString)
        {
            if (string.IsNullOrWhiteSpace(versionString)) return new Version(0, 0);
            if (versionString.Count(c => c == '.') > MaxVersionParts)
            {
                versionString = string.Join(".", versionString.Split('.').Take(MaxVersionParts));
            }

            return Version.TryParse(versionString, out var version) ? version : new Version(0, 0);
            // return SemVersion.TryParse(versionString, out var version) ? version : new SemVersion(0);
        }

        protected string BuildEngineVersion(string engine)
        {
            var engineVersion = new VersionParser(UserAgent, engine);
            var result = engineVersion.Parse();
            return result.Success ? result.Match.Name : string.Empty;
        }
    }
}
