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
        private readonly BrowserHints _browserHints;

        /// <summary>
        /// Known browsers mapped to their internal short codes
        /// </summary>
        protected static readonly IReadOnlyDictionary<string, string> AvailableBrowsers = new Dictionary<string, string>
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
            { "3B", "360 Secure Browser" },
            { "36", "360 Phone Browser" },
            { "7B", "7654 Browser" },
            { "AA", "Avant Browser" },
            { "AB", "ABrowse" },
            { "4A", "Acoo Browser" },
            { "BW", "AdBlock Browser" },
            { "A7", "Adult Browser" },
            { "8A", "Ai Browser" },
            { "A9", "Airfind Secure Browser" },
            { "AF", "ANT Fresco" },
            { "AG", "ANTGalio" },
            { "AL", "Aloha Browser" },
            { "AH", "Aloha Browser Lite" },
            { "A8", "ALVA" },
            { "9A", "AltiBrowser" },
            { "AM", "Amaya" },
            { "A3", "Amaze Browser" },
            { "A5", "Amerigo" },
            { "AO", "Amigo" },
            { "AN", "Android Browser" },
            { "3A", "AOL Explorer" },
            { "AE", "AOL Desktop" },
            { "AD", "AOL Shield" },
            { "A4", "AOL Shield Pro" },
            { "2A", "Aplix" },
            { "A6", "AppBrowzer" },
            { "0A", "AppTec Secure Browser" },
            { "AP", "APUS Browser" },
            { "AR", "Arora" },
            { "AX", "Arctic Fox" },
            { "AV", "Amiga Voyager" },
            { "AW", "Amiga Aweb" },
            { "PN", "APN Browser" },
            { "6A", "Arachne" },
            { "RA", "Arc Search" },
            { "R5", "Armorfly Browser" },
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
            { "7A", "Awesomium" },
            { "5B", "Basic Web Browser" },
            { "BA", "Beaker Browser" },
            { "BM", "Beamrise" },
            { "F7", "BF Browser" },
            { "BB", "BlackBerry Browser" },
            { "6B", "Bluefy" },
            { "H1", "BrowseHere" },
            { "B8", "Browser Hup Pro" },
            { "BD", "Baidu Browser" },
            { "BS", "Baidu Spark" },
            { "BG", "Bang" },
            { "B9", "Bangla Browser" },
            { "BI", "Basilisk" },
            { "BV", "Belva Browser" },
            { "B5", "Beyond Private Browser" },
            { "BE", "Beonex" },
            { "B2", "Berry Browser" },
            { "BT", "Bitchute Browser" },
            { "9B", "BizBrowser" },
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
            { "M9", "Browser Mini" },
            { "4B", "BrowsBit" },
            { "BY", "Biyubi" },
            { "BF", "Byffox" },
            { "B4", "BXE Browser" },
            { "CA", "Camino" },
            { "5C", "Catalyst" },
            { "XP", "Catsxp" },
            { "0C", "Cave Browser" },
            { "CL", "CCleaner" },
            { "C8", "CG Browser" },
            { "CJ", "ChanjetCloud" },
            { "C6", "Chedot" },
            { "C9", "Cherry Browser" },
            { "C0", "Centaury" },
            { "CQ", "Cliqz" },
            { "CC", "Coc Coc" },
            { "C4", "CoolBrowser" },
            { "C2", "Colibri" },
            { "6C", "Columbus Browser" },
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
            { "7C", "Classilla" },
            { "CN", "CoolNovo" },
            { "4C", "Colom Browser" },
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
            { "8C", "Cromite" },
            { "RC", "Crow Browser" },
            { "CT", "Crusta" },
            { "CG", "Craving Explorer" },
            { "CZ", "Crazy Browser" },
            { "CU", "Cunaguaro" },
            { "CV", "Chrome Webview" },
            { "YC", "CyBrowser" },
            { "DB", "dbrowser" },
            { "PD", "Peeps dBrowser" },
            { "DK", "Dark Web" },
            { "DP", "Dark Web Private" },
            { "D1", "Debuggable Browser" },
            { "DC", "Decentr" },
            { "DE", "Deepnet Explorer" },
            { "DG", "deg-degan" },
            { "DA", "Deledao" },
            { "DT", "Delta Browser" },
            { "D0", "Desi Browser" },
            { "DS", "DeskBrowse" },
            { "D3", "Dezor" },
            { "II", "Diigo Browser" },
            { "D2", "DoCoMo" },
            { "DF", "Dolphin" },
            { "DZ", "Dolphin Zero" },
            { "DO", "Dorado" },
            { "DR", "Dot Browser" },
            { "DL", "Dooble" },
            { "DI", "Dillo" },
            { "DU", "DUC Browser" },
            { "DD", "DuckDuckGo Privacy Browser" },
            { "E1", "East Browser" },
            { "EC", "Ecosia" },
            { "EW", "Edge WebView" },
            { "EV", "Every Browser" },
            { "EI", "Epic" },
            { "EL", "Elinks" },
            { "EN", "EinkBro" },
            { "EB", "Element Browser" },
            { "EE", "Elements Browser" },
            { "EO", "Eolie" },
            { "EX", "Explore Browser" },
            { "EZ", "eZ Browser" },
            { "E2", "EudoraWeb" },
            { "EU", "EUI Browser" },
            { "EP", "GNOME Web" },
            { "G1", "G Browser" },
            { "ES", "Espial TV Browser" },
            { "FG", "fGet" },
            { "FA", "Falkon" },
            { "FX", "Faux Browser" },
            { "F8", "Fire Browser" },
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
            { "F9", "FOSS Browser" },
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
            { "G3", "Good Browser" },
            { "GE", "Google Earth" },
            { "GP", "Google Earth Pro" },
            { "GO", "GOG Galaxy" },
            { "GR", "GoBrowser" },
            { "GK", "GoKu" },
            { "G2", "GO Browser" },
            { "RN", "GreenBrowser" },
            { "HW", "Habit Browser" },
            { "H7", "Halo Browser" },
            { "HB", "Harman Browser" },
            { "HS", "HasBrowser" },
            { "HA", "Hawk Turbo Browser" },
            { "HQ", "Hawk Quick Browser" },
            { "HE", "Helio" },
            { "HN", "Herond Browser" },
            { "HX", "Hexa Web Browser" },
            { "HI", "HeyTapBrowser" },
            { "H8", "Hi Browser" },
            { "HO", "hola! Browser" },
            { "H4", "Holla Web Browser" },
            { "H5", "HotBrowser" },
            { "HJ", "HotJava" },
            { "H6", "HONOR Browser" },
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
            { "4I", "iNet Browser" },
            { "I1", "Iridium" },
            { "I3", "Iron Mobile" },
            { "I4", "IceCat" },
            { "ID", "IceDragon" },
            { "IV", "Isivioo" },
            { "I8", "IVVI Browser" },
            { "IW", "Iceweasel" },
            { "2I", "Impervious Browser" },
            { "N3", "Incognito Browser" },
            { "IN", "Inspect Browser" },
            { "I9", "Insta Browser" },
            { "IE", "Internet Explorer" },
            { "I7", "Internet Browser Secure" },
            { "5I", "Internet Webbrowser" },
            { "3I", "Intune Managed Browser" },
            { "I5", "Indian UC Mini Browser" },
            { "Z0", "InBrowser" },
            { "IG", "Involta Go" },
            { "IM", "IE Mobile" },
            { "IR", "Iron" },
            { "JB", "Japan Browser" },
            { "JS", "Jasmine" },
            { "JA", "JavaFX" },
            { "JL", "Jelly" },
            { "JI", "Jig Browser" },
            { "JP", "Jig Browser Plus" },
            { "JO", "JioSphere" },
            { "JZ", "JUZI Browser" },
            { "KB", "K.Browser" },
            { "KF", "Keepsafe Browser" },
            { "K7", "KeepSolid Browser" },
            { "KS", "Kids Safe Browser" },
            { "KI", "Kindle Browser" },
            { "KM", "K-meleon" },
            { "KJ", "K-Ninja" },
            { "KO", "Konqueror" },
            { "KP", "Kapiko" },
            { "KE", "Keyboard Browser" },
            { "KN", "Kinza" },
            { "K4", "Kitt" },
            { "KW", "Kiwi" },
            { "KD", "Kode Browser" },
            { "KU", "KUN" },
            { "KT", "KUTO Mini Browser" },
            { "KY", "Kylo" },
            { "KZ", "Kazehakase" },
            { "LB", "Cheetah Browser" },
            { "LD", "Ladybird" },
            { "LA", "Lagatos Browser" },
            { "GN", "Legan Browser" },
            { "LR", "Lexi Browser" },
            { "LV", "Lenovo Browser" },
            { "LF", "LieBaoFast" },
            { "LG", "LG Browser" },
            { "LH", "Light" },
            { "L4", "Lightning Browser Plus" },
            { "L1", "Lilo" },
            { "LI", "Links" },
            { "RI", "Liri Browser" },
            { "LC", "LogicUI TV Browser" },
            { "IF", "Lolifox" },
            { "L3", "Lotus" },
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
            { "MP", "Maple" },
            { "M5", "MarsLab Web Browser" },
            { "M7", "MaxBrowser" },
            { "M1", "mCent" },
            { "MB", "MicroB" },
            { "MC", "NCSA Mosaic" },
            { "MZ", "Meizu Browser" },
            { "ME", "Mercury" },
            { "M2", "Me Browser" },
            { "MF", "Mobile Safari" },
            { "MI", "Midori" },
            { "M3", "Midori Lite" },
            { "M6", "MixerBox AI" },
            { "MO", "Mobicip" },
            { "MU", "Mi Browser" },
            { "MS", "Mobile Silk" },
            { "MK", "Mogok Browser" },
            { "M8", "Motorola Internet Browser" },
            { "MN", "Minimo" },
            { "MT", "Mint Browser" },
            { "MX", "Maxthon" },
            { "M4", "MaxTube Browser" },
            { "MA", "Maelstrom" },
            { "3M", "Mises" },
            { "MM", "Mmx Browser" },
            { "NM", "MxNitro" },
            { "MY", "Mypal" },
            { "MR", "Monument Browser" },
            { "MW", "MAUI WAP Browser" },
            { "N7", "Naenara Browser" },
            { "NW", "Navigateur Web" },
            { "NK", "Naked Browser" },
            { "NA", "Naked Browser Pro" },
            { "NR", "NFS Browser" },
            { "N5", "Ninetails" },
            { "NB", "Nokia Browser" },
            { "NO", "Nokia OSS Browser" },
            { "NV", "Nokia Ovi Browser" },
            { "N2", "Norton Private Browser" },
            { "NX", "Nox Browser" },
            { "N1", "NOMone VR Browser" },
            { "N6", "NOOK Browser" },
            { "NE", "NetSurf" },
            { "NF", "NetFront" },
            { "NL", "NetFront Life" },
            { "NP", "NetPositive" },
            { "NS", "Netscape" },
            { "WR", "NextWord Browser" },
            { "N8", "Ninesky" },
            { "NT", "NTENT Browser" },
            { "NU", "Nuanti Meta" },
            { "NI", "Nuviu" },
            { "O9", "Ocean Browser" },
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
            { "OL", "OnBrowser Lite" },
            { "OE", "ONE Browser" },
            { "N4", "Onion Browser" },
            { "1N", "ONIONBrowser" },
            { "Y1", "Opera Crypto" },
            { "OX", "Opera GX" },
            { "OG", "Opera Neon" },
            { "OH", "Opera Devices" },
            { "OI", "Opera Mini" },
            { "OM", "Opera Mobile" },
            { "OP", "Opera" },
            { "ON", "Opera Next" },
            { "OO", "Opera Touch" },
            { "OU", "Orbitum" },
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
            { "5O", "Open Browser Lite" },
            { "O7", "Open TV Browser" },
            { "OW", "OmniWeb" },
            { "OT", "Otter Browser" },
            { "4O", "Owl Browser" },
            { "JR", "OJR Browser" },
            { "PL", "Palm Blazer" },
            { "PM", "Pale Moon" },
            { "PY", "Polypane" },
            { "8P", "Prism" },
            { "PP", "Oppo Browser" },
            { "P6", "Opus Browser" },
            { "PR", "Palm Pre" },
            { "2E", "Pocket Internet Explorer" },
            { "7I", "Puffin Cloud Browser" },
            { "6I", "Puffin Incognito Browser" },
            { "PU", "Puffin Secure Browser" },
            { "2P", "Puffin Web Browser" },
            { "PW", "Palm WebPro" },
            { "PA", "Palmscape" },
            { "P7", "Pawxy" },
            { "0P", "Peach Browser" },
            { "PE", "Perfect Browser" },
            { "K6", "Perk" },
            { "P1", "Phantom.me" },
            { "PH", "Phantom Browser" },
            { "PX", "Phoenix" },
            { "PB", "Phoenix Browser" },
            { "5P", "Photon" },
            { "N9", "Pintar Browser" },
            { "P9", "PirateBrowser" },
            { "P8", "PICO Browser" },
            { "PF", "PlayFree Browser" },
            { "PK", "PocketBook Browser" },
            { "PO", "Polaris" },
            { "PT", "Polarity" },
            { "LY", "PolyBrowser" },
            { "9P", "Presearch" },
            { "BP", "Privacy Browser" },
            { "PI", "PrivacyWall" },
            { "P4", "Privacy Explorer Fast Safe" },
            { "X5", "Privacy Pioneer Browser" },
            { "P3", "Private Internet Browser" },
            { "P5", "Proxy Browser" },
            { "7P", "Proxyium" },
            { "6P", "Proxynet" },
            { "2F", "ProxyFox" },
            { "2M", "ProxyMax" },
            { "P2", "Pi Browser" },
            { "P0", "PronHub Browser" },
            { "PC", "PSI Secure Browser" },
            { "RW", "Reqwireless WebViewer" },
            { "RO", "Roccat" },
            { "PS", "Microsoft Edge" },
            { "QA", "Qazweb" },
            { "QI", "Qiyu" },
            { "QJ", "QJY TV Browser" },
            { "Q3", "Qmamu" },
            { "Q4", "Quick Search TV" },
            { "Q2", "QQ Browser Lite" },
            { "Q1", "QQ Browser Mini" },
            { "QQ", "QQ Browser" },
            { "QS", "Quick Browser" },
            { "QT", "Qutebrowser" },
            { "QU", "Quark" },
            { "Q6", "QuarkPC" },
            { "Q7", "Quetta" },
            { "QZ", "QupZilla" },
            { "QM", "Qwant Mobile" },
            { "Q5", "QtWeb" },
            { "QW", "QtWebEngine" },
            { "R3", "Rakuten Browser" },
            { "R4", "Rakuten Web Search" },
            { "R2", "Raspbian Chromium" },
            { "RT", "RCA Tor Explorer" },
            { "RE", "Realme Browser" },
            { "RK", "Rekonq" },
            { "RM", "RockMelt" },
            { "RB", "Roku Browser" },
            { "SB", "Samsung Browser" },
            { "3L", "Samsung Browser Lite" },
            { "SA", "Sailfish Browser" },
            { "R0", "SberBrowser" },
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
            { "ZB", "Singlebox" },
            { "SY", "Sizzy" },
            { "K3", "Skye" },
            { "SK", "Skyfire" },
            { "KL", "SkyLeap" },
            { "SS", "Seraphic Sraf" },
            { "KK", "SiteKiosk" },
            { "SL", "Sleipnir" },
            { "8B", "SlimBoat" },
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
            { "K5", "Spark" },
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
            { "RY", "Surfy Browser" },
            { "SG", "Stargon" },
            { "S0", "START Internet Browser" },
            { "5A", "Stealth Browser" },
            { "S4", "Steam In-Game Overlay" },
            { "ST", "Streamy" },
            { "SX", "Swiftfox" },
            { "W7", "Swiftweasel" },
            { "SZ", "Seznam Browser" },
            { "W1", "Sweet Browser" },
            { "2X", "SX Browser" },
            { "TP", "T+Browser" },
            { "TR", "T-Browser" },
            { "TO", "t-online.de Browser" },
            { "TT", "TalkTo" },
            { "TA", "Tao Browser" },
            { "T2", "tararia" },
            { "TH", "Thor" },
            { "1T", "Tor Browser" },
            { "TF", "TenFourFox" },
            { "TB", "Tenta Browser" },
            { "TE", "Tesla Browser" },
            { "TZ", "Tizen Browser" },
            { "TI", "Tint Browser" },
            { "TL", "TrueLocation Browser" },
            { "TC", "TUC Mini Browser" },
            { "TK", "TUSK" },
            { "TU", "Tungsten" },
            { "TG", "ToGate" },
            { "T3", "Total Browser" },
            { "TQ", "TQ Browser" },
            { "TS", "TweakStyle" },
            { "TV", "TV Bro" },
            { "T4", "TV-Browser Internet" },
            { "U0", "U Browser" },
            { "UB", "UBrowser" },
            { "UC", "UC Browser" },
            { "UH", "UC Browser HD" },
            { "UM", "UC Browser Mini" },
            { "UT", "UC Browser Turbo" },
            { "UI", "Ui Browser Mini" },
            { "UP", "UPhone Browser" },
            { "UR", "UR Browser" },
            { "UZ", "Uzbl" },
            { "UE", "Ume Browser" },
            { "V0", "vBrowser" },
            { "VA", "Vast Browser" },
            { "V3", "VD Browser" },
            { "VR", "Veera" },
            { "VE", "Venus Browser" },
            { "WD", "Vewd Browser" },
            { "V5", "VibeMate" },
            { "N0", "Nova Video Downloader Pro" },
            { "VS", "Viasat Browser" },
            { "VI", "Vivaldi" },
            { "VV", "vivo Browser" },
            { "V2", "Vivid Browser Mini" },
            { "VB", "Vision Mobile Browser" },
            { "V4", "Vertex Surf" },
            { "VM", "VMware AirWatch" },
            { "V6", "VMS Mosaic" },
            { "VK", "Vonkeror" },
            { "VU", "Vuhuv" },
            { "WI", "Wear Internet Browser" },
            { "WP", "Web Explorer" },
            { "W3", "Web Browser & Explorer" },
            { "W5", "Webian Shell" },
            { "W4", "WebDiscover" },
            { "WE", "WebPositive" },
            { "W6", "Weltweitimnetz Browser" },
            { "WX", "Wexond" },
            { "WF", "Waterfox" },
            { "WB", "Wave Browser" },
            { "WA", "Wavebox" },
            { "WH", "Whale Browser" },
            { "W2", "Whale TV Browser" },
            { "WO", "wOSBrowser" },
            { "3W", "w3m" },
            { "WT", "WeTab Browser" },
            { "1W", "World Browser" },
            { "WL", "Wolvic" },
            { "WK", "Wukong Browser" },
            { "WY", "Wyzo" },
            { "YG", "YAGI" },
            { "YJ", "Yahoo! Japan Browser" },
            { "YA", "Yandex Browser" },
            { "Y4", "Yandex Browser Corp" },
            { "YL", "Yandex Browser Lite" },
            { "YN", "Yaani Browser" },
            { "Y2", "Yo Browser" },
            { "YB", "Yolo Browser" },
            { "YO", "YouCare" },
            { "Y3", "YouBrowser" },
            { "YZ", "Yuzu Browser" },
            { "XR", "xBrowser" },
            { "X3", "MMBOX XBrowser" },
            { "XB", "X Browser Lite" },
            { "X0", "X-VPN" },
            { "X1", "xBrowser Pro Super Fast" },
            { "XN", "XNX Browser" },
            { "XT", "XtremeCast" },
            { "XS", "xStand" },
            { "XI", "Xiino" },
            { "X4", "XnBrowse" },
            { "XO", "Xooloo Internet" },
            { "XV", "Xvast" },
            { "ZE", "Zetakey" },
            { "ZV", "Zvu" },
            { "ZI", "Zirco Browser" },
            { "ZR", "Zordo Browser" },
            { "ZT", "ZTE Browser" },

            // detected browsers in older versions
            // { "IA", "Iceape" },  => pim
            // { "SM", "SeaMonkey" },  => pim
        };

        /// <summary>
        /// Browser families mapped to the short codes of the associated browsers
        /// </summary>
        protected static readonly IReadOnlyDictionary<string, string[]> BrowserFamilies = new Dictionary<string, string[]>
        {
            {"Android Browser"    , new []{ "AN" }},
            {"BlackBerry Browser" , new []{ "BB" }},
            {"Baidu"              , new []{ "BD", "BS", "H6" }},
            {"Amiga"              , new []{ "AV", "AW"}},
            {"Chrome"             , new []{
                "CH", "2B", "7S", "A0", "AC", "A4", "AE", "AH", "AI",
                "AO", "AS", "BA", "BM", "BR", "C2", "C3", "C5", "C4",
                "C6", "CC", "CD", "CE", "CF", "CG", "1B", "CI", "CL",
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
                "HP", "IO", "TP", "CJ", "HQ", "HI", "PN", "BW", "YO",
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
                "K3", "Q4", "G2", "R2", "WX", "XP", "3I", "BG", "R0",
                "JO", "OL", "GN", "W4", "QI", "E1", "RI", "8B", "5B",
                "K4", "WK", "T3", "K5", "MU", "9P", "K6", "VR", "N9",
                "M9", "F9", "0P", "0A", "JR", "D3", "TK", "BP", "2F",
                "2M", "K7", "1N", "8A", "H7", "X3", "T4", "X4", "5O",
                "8C", "3M", "6I", "2P", "PU", "7I", "X5", "AL", "3P",
                "W2", "ZB", "HN", "Q6", "Q7", "H8"
            }},
            {"Firefox"            , new []{
                "FF", "BI", "BF", "BH", "BN", "C0", "CU", "EI", "F1",
                "FB", "FE", "AX", "FM", "FR", "FY", "I4", "IF", "8P",
                "IW", "LH", "LY", "MB", "MN", "MO", "MY", "OA", "OS",
                "PI", "PX", "QA", "S5", "SX", "TF", "TO", "WF", "ZV",
                "FP", "AD", "2I", "P9", "KJ", "WY", "VK", "W5",
                "7C", "N7", "W7",
            }},
            {"Internet Explorer"  , new []{ "BZ", "CZ", "IE", "IM", "PS", "3A", "4A", "RN", "2E" }},
            {"Konqueror"          , new []{ "KO" }},
            {"NetFront"           , new []{ "NF" }},
            {"NetSurf"            , new []{ "NE" }},
            {"Nokia Browser"      , new []{ "DO", "NB", "NO", "NV" }},
            {"Opera"              , new []{ "O1", "OG", "OH", "OI", "OM", "ON", "OO", "OP", "OX", "Y1" }},
            {"Safari"             , new []{ "MF", "S7", "SF", "SO", "PV" }},
            {"Sailfish Browser"   , new []{ "SA" }},
        };

        /// <summary>
        /// Browsers that are available for mobile devices only
        /// </summary>
        protected static readonly string[] MobileOnlyBrowsers =
        {
            "36", "AH", "AI", "BL", "C1", "C4", "CB", "CW", "DB",
            "3M", "DT", "EU", "EZ", "FK", "FM", "FR", "FX", "GH",
            "GI", "GR", "HA", "HU", "IV", "JB", "KD", "M1", "MF",
            "MN", "MZ", "NX", "OC", "OI", "OM", "OZ", "2P", "PI",
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
            "ZR", "D6", "F6", "P3", "FT", "A9", "X2", "NI", "FG",
            "TH", "N3", "GD", "O9", "Q3", "F7", "K2", "N4", "P5",
            "H5", "V3", "G2", "BG", "OL", "II", "TL", "M6", "Y3",
            "M7", "GN", "JR", "IG", "HW", "4O", "OU", "5P", "KE",
            "5A", "TT", "6P", "G3", "7P", "VU", "F8", "L4", "DK",
            "DP", "KL", "K4", "N6", "KU", "WK", "M8", "UP", "ZT",
            "9P", "N8", "VR", "N9", "M9", "F9", "0P", "0A", "2F",
            "2M", "K7", "1N", "8A", "H7", "X3", "X4", "5O", "6I",
            "7I", "X5", "3P", "2E"
        };

        public override IReadOnlyDictionary<string, string[]> ClientHintMapping => new Dictionary<string, string[]>
        {
            { "Chrome", new[] { "Google Chrome" } },
            { "Chrome Webview", new[] { "Android WebView" } },
            { "DuckDuckGo Privacy Browser", new[] { "DuckDuckGo" } },
            { "Mi Browser", new[] { "Miui Browser", "XiaoMiBrowser" } },
            { "Edge WebView", new[] { "Microsoft Edge WebView2" } },
            { "Microsoft Edge", new[] { "Edge" } },
            { "Norton Private Browser", new[] { "Norton Secure Browser" } },
            { "Vewd Browser", new[] { "Vewd Core" } }
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
            _browserHints = new BrowserHints(UserAgent, ClientHints);
        }

        /// <summary>
        /// Sets the client hints to parse
        /// </summary>
        public override void SetClientHints(ClientHints clientHints)
        {
            base.SetClientHints(clientHints);
            _browserHints.SetClientHints(clientHints);
        }

        /// <summary>
        /// Sets the user agent to parse
        /// </summary>
        /// <param name="ua"></param>
        public override void SetUserAgent(string ua)
        {
            base.SetUserAgent(ua);
            _browserHints.SetUserAgent(ua);
        }

        /// <summary>
        /// Returns list of all available browsers
        /// </summary>
        /// <returns></returns>
        public static IReadOnlyDictionary<string, string> GetAvailableBrowsers()
        {
            return AvailableBrowsers;
        }

        /// <summary>
        /// Returns list of all available browser families
        /// </summary>
        public static IReadOnlyDictionary<string, string[]> GetAvailableBrowserFamilies()
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
                if (GetRegexEngine().Match(client.Version, "/^202[0-4]/")){
                    client.Name = "Iridium";
                    client.ShortName = "I1";
                }

                // https://bbs.360.cn/thread-16096544-1-1.html
                if (GetRegexEngine().Match(client.Version, "/^15/") && GetRegexEngine().Match(browserFromUserAgent.Version, "/^114/"))
                {
                    client.Name = "360 Secure Browser";
                    client.ShortName = "3B";
                    client.Engine = browserFromUserAgent.Engine ?? string.Empty;
                    client.EngineVersion = browserFromUserAgent.EngineVersion ?? string.Empty;
                }

                // If client hints report the following browsers, we use the version from useragent
                if (!string.IsNullOrEmpty(browserFromUserAgent.Version)
                    && !new[] { "A0", "AL", "HP", "JR", "MU", "OM", "OP", "VR" }.Contains(client.ShortName))
                {
                    client.Version = browserFromUserAgent.Version;
                }
                
                if ("Vewd Browser" == client.Name)
                {
                    client.Engine = browserFromUserAgent.Engine ?? string.Empty;
                    client.EngineVersion = browserFromUserAgent.EngineVersion ?? string.Empty;
                }

                // If client hints report Chromium, but user agent detects a chromium based browser, we favor this instead
                if (("Chromium" == client.Name || "Chrome Webview" == client.Name)
                    && !string.IsNullOrEmpty(browserFromUserAgent.Name)
                    && !new []{ "CR", "CV", "AN" }.Contains(browserFromUserAgent.ShortName)
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

                if(client.Name == browserFromUserAgent.Name)
                {
                    client.Engine = browserFromUserAgent.Engine ?? string.Empty;
                    client.EngineVersion = browserFromUserAgent.EngineVersion ?? string.Empty;

                }

                // In case the user agent reports a more detailed version, we try to use this instead
                if (!string.IsNullOrEmpty(browserFromUserAgent.Version)
                    && 0 == browserFromUserAgent.Version?.IndexOf(client.Version)
                    //&& \version_compare($version, $browserFromUserAgent['version'], '<') //@todo
                   )
                {
                    client.Version = browserFromUserAgent.Version;
                }

                if ("DuckDuckGo Privacy Browser" == client.Name)
                {
                    client.Version = string.Empty;
                }

                // In case client hints report a more detailed engine version, we try to use this instead
                if ("Blink" == client.Engine && "Iridium" != client.Name
                    //&& \version_compare($engineVersion, $browserFromClientHints['version'], '<') //@todo
                    )
                {
                    client.EngineVersion = browserFromClientHints.Version;
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
            var appHash = _browserHints.Parse();
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

            if (string.IsNullOrEmpty(client.Name) || IsMatchUserAgent("/Cypress|PhantomJS/"))
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

            // This browser simulates user-agent of Firefox
            if ("TV-Browser Internet" == client.Name && "Gecko" == client.Engine)
            {
                client.Family = "Chrome";
                client.Engine = "Blink";
                client.EngineVersion = string.Empty;
            }

            if ("Wolvic" == client.Name && "Blink" == client.Engine)
            {
                client.Family = "Chrome";
            }

            if ("Wolvic" == client.Name && "Gecko" == client.Engine)
            {
                client.Family = "Firefox";
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
                        if (!string.IsNullOrEmpty(name) && "Chromium" != name && "Microsoft Edge" != name) {
                            break;
                        }
                    }
                }
                version = ClientHints.GetBrandVersion() ?? version;
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
