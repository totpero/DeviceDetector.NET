DeviceDetector.NET
==============

[![Awesome](https://cdn.rawgit.com/sindresorhus/awesome/d7305f38d29fed78fa85652e3a63e154dd8e8829/media/badge.svg)](https://github.com/quozd/awesome-dotnet#misc) 
[![Build status](https://ci.appveyor.com/api/projects/status/baf9r5iqnp4flwkm?svg=true)](https://ci.appveyor.com/project/totpero/devicedetector-net)
[![NuGet](https://img.shields.io/nuget/v/DeviceDetector.NET.svg?style=flat-square)](https://www.nuget.org/packages/DeviceDetector.NET)
[![Join the chat at https://gitter.im/totpero/DeviceDetector.NET](https://badges.gitter.im/totpero/DeviceDetector.NET.svg)](https://gitter.im/totpero/DeviceDetector.NET?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![Code Health](https://landscape.io/github/totpero/DeviceDetector.NET/master/landscape.svg?style=flat)](https://landscape.io/github/totpero/DeviceDetector.NET/master)
[![Waffle.io - Columns and their card count](https://badge.waffle.io/totpero/DeviceDetector.NET.svg?columns=all)](https://waffle.io/totpero/DeviceDetector.NET)

## Description

The Universal Device Detection library for .NET that parses User Agents and detects devices (desktop, tablet, mobile, tv, cars, console, etc.), clients (browsers, feed readers, media players, PIMs, ...), operating systems, brands and models.
This is a port of the popular PHP [device-detector](https://github.com/matomo-org/device-detector) library to C#. For the most part you can just follow the documentation for device-detector with no issue.


## Usage

Using DeviceDetector.NET with nuget is quite easy. Just add DeviceDetector.NET to your projects requirements. And use some code like this one:


```csharp
using DeviceDetectorNET;

// OPTIONAL: Set version truncation to none, so full versions will be returned
// By default only minor versions will be returned (e.g. X.Y)
// for other options see VERSION_TRUNCATION_* constants in DeviceParserAbstract class
// add using DeviceDetectorNET.Parser;
DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

var dd = new DeviceDetector(userAgent);

// OPTIONAL: Set caching method
// By default static cache is used, which works best within one php process (memory array caching)
// To cache across requests use caching in files or memcache
// add using DeviceDetectorNET.Cache;
dd.SetCache(new DictionaryCache());

// OPTIONAL: If called, GetBot() will only return true if a bot was detected  (speeds up detection a bit)
dd.DiscardBotInformation();

// OPTIONAL: If called, bot detection will completely be skipped (bots will be detected as regular devices then)
dd.SkipBotDetection();

dd.Parse();

if(dd.IsBot()) {
	// handle bots,spiders,crawlers,...
	var botInfo = dd.GetBot();
} else {
	var clientInfo = dd.GetClient(); // holds information about browser, feed reader, media player, ...
	var osInfo = dd.GetOs();
	var device = dd.GetDeviceName();
	var brand  = dd.GetBrandName();
	var model  = dd.GetModel();
}

```

Instead of using the full power of DeviceDetector it might in some cases be better to use only specific parsers.
If you aim to check if a given useragent is a bot and don't require any of the other information, you can directly use the bot parser.

```csharp
using DeviceDetectorNET.Parser;

var botParser = new BotParser();
botParser.SetUserAgent(userAgent);

// OPTIONAL: discard bot information. Parse() will then return true instead of information
botParser.DiscardDetails = true;

var result = botParser.Parse();

if (result != null) {
    // do not do anything if a bot is detected
    return;
}

// handle non-bot requests

```


##### The default regexes directory path can be changed like this:
```csharp
DeviceDetectorSettings.RegexesDirectory = @"C:\YamlRegexsFiles\";
```


.....



## What Device Detector is able to detect

The lists below are auto generated and updated from time to time. Some of them might not be complete.

*Last update: 2019/03/30*

### List of detected operating systems:

AIX, Android, AmigaOS, Apple TV, Arch Linux, BackTrack, Bada, BeOS, BlackBerry OS, BlackBerry Tablet OS, Brew, CentOS, Chrome OS, CyanogenMod, Debian, DragonFly, Fedora, Firefox OS, Fire OS, FreeBSD, Gentoo, Google TV, HP-UX, Haiku OS, IRIX, Inferno, KaiOS, Knoppix, Kubuntu, GNU/Linux, Lubuntu, VectorLinux, Mac, Maemo, Mandriva, MeeGo, MocorDroid, Mint, MildWild, MorphOS, NetBSD, MTK / Nucleus, Nintendo, Nintendo Mobile, OS/2, OSF1, OpenBSD, PlayStation Portable, PlayStation, Red Hat, RISC OS, Remix OS, RazoDroiD, Sabayon, SUSE, Sailfish OS, Slackware, Solaris, Syllable, Symbian, Symbian OS, Symbian OS Series 40, Symbian OS Series 60, Symbian^3, ThreadX, Tizen, Ubuntu, WebTV, Windows, Windows CE, Windows IoT, Windows Mobile, Windows Phone, Windows RT, Xbox, Xubuntu, YunOs, iOS, palmOS, webOS

### List of detected browsers:

360 Phone Browser, 360 Browser, Avant Browser, ABrowse, ANT Fresco, ANTGalio, Aloha Browser, Amaya, Amigo, Android Browser, AOL Shield, Arora, Amiga Voyager, Amiga Aweb, Atomic Web Browser, Avast Secure Browser, Beaker Browser, BlackBerry Browser, Baidu Browser, Baidu Spark, Beonex, Bunjalloo, B-Line, Brave, BriskBard, BrowseX, Camino, Coc Coc, Comodo Dragon, Coast, Charon, Chrome Frame, Headless Chrome, Chrome, Chrome Mobile iOS, Conkeror, Chrome Mobile, CoolNovo, CometBird, ChromePlus, Chromium, Cyberfox, Cheshire, Cunaguaro, dbrowser, Deepnet Explorer, Dolphin, Dorado, Dooble, Dillo, Epic, Elinks, Element Browser, GNOME Web, Espial TV Browser, Firebird, Fluid, Fennec, Firefox, Firefox Focus, Flock, Firefox Mobile, Fireweb, Fireweb Navigator, Galeon, Google Earth, HotJava, Iceape, IBrowse, iCab, iCab Mobile, Iridium, IceDragon, Isivioo, Iceweasel, Internet Explorer, IE Mobile, Iron, Jasmine, Jig Browser, Kindle Browser, K-meleon, Konqueror, Kapiko, Kylo, Kazehakase, Liebao, LG Browser, Links, LuaKit, Lunascape, Lynx, MicroB, NCSA Mosaic, Mercury, Mobile Safari, Midori, MIUI Browser, Mobile Silk, Maxthon, Nokia Browser, Nokia OSS Browser, Nokia Ovi Browser, NetSurf, NetFront, NetFront Life, NetPositive, Netscape, NTENT Browser, Obigo, Odyssey Web Browser, Off By One, ONE Browser, Opera Mini, Opera Mobile, Opera, Opera Next, Opera Touch, Oregano, Openwave Mobile Browser, OmniWeb, Otter Browser, Palm Blazer, Pale Moon, Oppo Browser, Palm Pre, Puffin, Palm WebPro, Palmscape, Phoenix, Polaris, Polarity, Microsoft Edge, QQ Browser, Qutebrowser, QupZilla, Qwant Mobile, Rekonq, RockMelt, Samsung Browser, Sailfish Browser, SEMC-Browser, Sogou Explorer, Safari, Shiira, Skyfire, Seraphic Sraf, Sleipnir, SeaMonkey, Snowshoe, Sunrise, SuperBird, Streamy, Swiftfox, TenFourFox, Tenta Browser, Tizen Browser, TweakStyle, UC Browser, Vivaldi, Vision Mobile Browser, WebPositive, Waterfox, wOSBrowser, WeTab Browser, Yandex Browser, Xiino

### List of detected browser engines:

WebKit, Blink, Trident, Text-based, Dillo, iCab, Elektra, Presto, Gecko, KHTML, NetFront, Edge, NetSurf

### List of detected libraries:

aiohttp, curl, Faraday, Go-http-client, Google HTTP Java Client, Guzzle (PHP HTTP Client), HTTP_Request2, Java, libdnf, Mechanize, OkHttp, Perl, Python Requests, Python urllib, urlgrabber (yum), Wget, WWW-Mechanize

### List of detected media players:

Audacious, Banshee, Boxee, Clementine, Deezer, FlyCast, Foobar2000, iTunes, Kodi, MediaMonkey, Miro, NexPlayer, Nightingale, QuickTime, Songbird, Stagefright, SubStream, VLC, Winamp, Windows Media Player, XBMC

### List of detected mobile apps:

AndroidDownloadManager, AntennaPod, Apple News, BeyondPod, bPod, Castro, Castro 2, DoggCatcher, Facebook, Facebook Messenger, FeedR, Google Play Newsstand, Google Plus, iCatcher, Instacast, Line, Overcast, Pinterest, Player FM, Pocket Casts, Podcast & Radio Addict, Podcast Republic, Podcasts, Podcat, Podcatcher Deluxe, Podkicker, Sina Weibo, WeChat, WhatsApp, Yahoo! Japan, Yelp Mobile, YouTube  and *mobile apps using [AFNetworking](https://github.com/AFNetworking/AFNetworking)*

### List of detected PIMs (personal information manager):

Airmail, Barca, DAVdroid, Lotus Notes, MailBar, Microsoft Outlook, Outlook Express, Postbox, The Bat!, Thunderbird

### List of detected feed readers:

Akregator, Apple PubSub, BashPodder, Breaker, Downcast, FeedDemon, Feeddler RSS Reader, gPodder, JetBrains Omea Reader, Liferea, NetNewsWire, Newsbeuter, NewsBlur, NewsBlur Mobile App, PritTorrent, Pulp, ReadKit, Reeder, RSS Bandit, RSS Junkie, RSSOwl, Stringer

### List of brands with detected devices:

3Q, 4Good, Acer, Advance, AGM, Ainol, Airness, Airties, Aiwa, Akai, Alcatel, AllCall, Allview, Altech UEC, altron, Amazon, AMGOO, Amoi, Apple, Archos, Arnova, ARRIS, Ask, Asus, Audiovox, AVH, Avvio, Axxion, Azumi Mobile, BangOlufsen, Barnes & Noble, BBK, Becker, Beeline, Beetel, BenQ, BenQ-Siemens, BGH, Bird, Bitel, Blackview, Blaupunkt, Blu, Bluboo, Bluegood, Bmobile, bogo, Boway, bq, Bravis, Brondi, Bush, CAGI, Capitel, Captiva, Carrefour, Casio, Cat, Celkon, Changhong, Cherry Mobile, China Mobile, CnM, Coby Kyros, Comio, Compal, Compaq, ComTrade Tesla, Concord, ConCorde, Condor, Coolpad, Cowon, CreNova, Crescent, Cricket, Crius Mea, Crosscall, Cube, CUBOT, Cyrus, Danew, Datang, Datsun, Dbtel, Dell, Denver, Desay, DEXP, Dialog, Dicam, Digicel, Digiland, Digma, DMM, DNS, DoCoMo, Doogee, Doov, Dopod, Doro, Dune HD, E-Boda, Easypix, EBEST, Echo Mobiles, ECS, EE, EKO, Eks Mobility, Elephone, Energizer, Energy Sistem, Ericsson, Ericy, Essential, Essentielb, Eton, eTouch, Evertek, Evolio, Evolveo, Explay, Extrem, Ezio, Ezze, Fairphone, FiGO, Fly, FNB, Forstar, Foxconn, Freetel, Fujitsu, G-TiDE, Garmin-Asus, Gateway, Gemini, Geotel, Ghia, Gigabyte, Gigaset, Gionee, GOCLEVER, Goly, GoMobile, Google, Gradiente, Grape, Grundig, Hafury, Haier, HannSpree, Hasee, Hi-Level, Hisense, Homtom, Hosin, HP, HTC, Huawei, Humax, Hyrican, Hyundai, i-Joy, i-mate, i-mobile, iBall, iBerry, IconBIT, iHunt, Ikea, iKoMo, IMO Mobile, Impression, iNew, Infinix, Inkti, InnJoo, Innostream, Inoi, INQ, Intek, Intex, Inverto, iOcean, iPro, iTel, iView, JAY-Tech, Jiayu, Jolla, K-Touch, Kalley, Karbonn, Kazam, KDDI, Kempler & Strauss, Kiano, Kingsun, Kodak, Kogan, Komu, Konka, Konrow, Koobee, KOPO, Koridy, KRONO, Krüger&Matz, KT-Tech, Kumai, Kyocera, LAIQ, Landvo, Lanix, Lava, LCT, Leagoo, Ledstar, LeEco, Lemhoov, Lenco, Lenovo, Leotec, Le Pan, Lexand, Lexibook, LG, Lingwin, Loewe, Logicom, LYF, M.T.T., M4tel, Majestic, Manta Multimedia, Maxwest, Mecer, Mediacom, MediaTek, Medion, MEEG, MegaFon, Meizu, Memup, Metz, MEU, MicroMax, Microsoft, Mio, Miray, Mitsubishi, MIXC, MLLED, Mobiistar, Mobistel, Modecom, Mofut, Motorola, Movic, Mpman, MSI, MTC, MTN, MyPhone, Myria, MyWigo, Navon, NEC, Neffos, Netgear, NeuImage, Newgen, Nexian, Nextbit, NextBook, NGM, Nikon, Nintendo, Noain, Noblex, Nokia, Nomi, Nous, NUU Mobile, Nvidia, NYX Mobile, O+, O2, Obi, Odys, Onda, OnePlus, OPPO, Opsson, Orange, Ouki, OUYA, Overmax, Oysters, Palm, Panacom, Panasonic, Pantech, PCBOX, PCD, PCD Argentina, PEAQ, Pentagram, Philips, phoneOne, Pioneer, Ployer, Plum, Point of View, Polaroid, PolyPad, Pomp, Positivo, PPTV, Prestigio, Primepad, ProScan, PULID, Qilive, QMobile, Qtek, Quantum, Quechua, Ramos, RCA Tablets, Readboy, Rikomagic, RIM, Riviera, Roku, Rover, RT Project, Sagem, Samsung, Sanei, Santin BiTBiZ, Sanyo, Savio, Sega, Selevision, Selfix, Sencor, Sendo, Senseit, SFR, Sharp, Siemens, Silent Circle, Sky, Skyworth, Smart, Smartfren, Smartisan, Softbank, Sonim, Sony, Sony Ericsson, Spice, Star, STF Mobile, STK, Stonex, Storex, Sumvision, SunVan, SuperSonic, Supra, SWISSMOBILITY, Symphony, T-Mobile, TB Touch, TCL, TechniSat, TechnoTrend, TechPad, Teclast, Tecno Mobile, Telefunken, Telego, Telenor, Telit, Tesco, Tesla, teXet, ThL, Thomson, TIANYU, Timovi, TiPhone, Tolino, Top House, Toplux, Toshiba, Touchmate, TrekStor, Trevi, Tunisie Telecom, Turbo-X, TVC, U.S. Cellular, Uhappy, Ulefone, UMIDIGI, Unimax, Uniscope, Unknown, Unnecto, Unonu, Unowhy, UTStarcom, Vastking, Vernee, Vertex, Vertu, Verykool, Vestel, Videocon, Videoweb, ViewSonic, Vinsoc, Vitelcom, Vivo, Vizio, VK Mobile, Vodafone, Vonino, Vorago, Voto, Voxtel, Vulcan, Walton, Web TV, Weimei, WellcoM, Wexler, Wiko, Wileyfox, Wink, Wolder, Wolfgang, Wonu, Woo, Woxter, X-View, Xiaomi, Xion, Xolo, Yarvik, Yes, Yezz, Ytone, Yu, Yuandao, Yusun, Zeemi, Zen, Zenek, Zonda, Zopo, ZTE, Zuum, Zync, öwn

### List of detected bots:

360Spider, Aboundexbot, Acoon, AddThis.com, ADMantX, aHrefs Bot, Alexa Crawler, Alexa Site Audit, Amorank Spider, Analytics SEO Crawler, ApacheBench, Applebot, Arachni, archive.org bot, Ask Jeeves, Backlink-Check.de, BacklinkCrawler, Baidu Spider, BazQux Reader, BingBot, BitlyBot, Blekkobot, BLEXBot Crawler, Bloglovin, Blogtrottr, Bountii Bot, Browsershots, BUbiNG, Butterfly Robot, CareerBot, Castro 2, Catchpoint, ccBot crawler, Charlotte, Cliqzbot, CloudFlare Always Online, CloudFlare AMP Fetcher, Collectd, CommaFeed, CSS Certificate Spider, Cốc Cốc Bot, Datadog Agent, Dataprovider, Daum, Dazoobot, Discobot, Domain Re-Animator Bot, DotBot, DuckDuckGo Bot, Easou Spider, EMail Exractor, EmailWolf, evc-batch, ExaBot, ExactSeek Crawler, Ezooms, Facebook External Hit, Feedbin, FeedBurner, Feedly, Feedspot, Feed Wrangler, Fever, Findxbot, Flipboard, Generic Bot, Generic Bot, Genieo Web filter, Gigablast, Gigabot, Gluten Free Crawler, Gmail Image Proxy, Goo, Googlebot, Google PageSpeed Insights, Google Partner Monitoring, Google Search Console, Google Structured Data Testing Tool, Grapeshot, Heritrix, Heureka Feed, HTTPMon, HubPages, HubSpot, ICC-Crawler, ichiro, IIS Site Analysis, Inktomi Slurp, IP-Guide Crawler, IPS Agent, Kouio, Larbin web crawler, Let's Encrypt Validation, Lighthouse, Linkdex Bot, LinkedIn Bot, LTX71, Lycos, Magpie-Crawler, MagpieRSS, Mail.Ru Bot, masscan, Mastodon Bot, Meanpath Bot, MetaInspector, MetaJobBot, Mixrank Bot, MJ12 Bot, Mnogosearch, MojeekBot, Monitor.Us, Munin, Nagios check_http, NalezenCzBot, Netcraft Survey Bot, netEstate, NetLyzer FastProbe, NetResearchServer, Netvibes, NewsBlur, NewsGator, NLCrawler, Nmap, Nutch-based Bot, Octopus, Omgili bot, Openindex Spider, OpenLinkProfiler, OpenWebSpider, Orange Bot, Outbrain, PagePeeker, PaperLiBot, Phantomas, PHP Server Monitor, Picsearch bot, Pingdom Bot, Pinterest, PocketParser, Pompos, PritTorrent, QuerySeekerSpider, Quora Link Preview, Qwantify, Rainmeter, RamblerMail Image Proxy, Reddit Bot, Riddler, Rogerbot, ROI Hunter, SafeDNSBot, Scooter, ScoutJet, Scrapy, Screaming Frog SEO Spider, ScreenerBot, Semrush Bot, Sensika Bot, Sentry Bot, SEOENGBot, SEOkicks-Robot, Seoscanners.net, Server Density, Seznam Bot, Seznam Email Proxy, Seznam Zbozi.cz, ShopAlike, ShopWiki, SilverReader, SimplePie, SISTRIX Crawler, Site24x7 Website Monitoring, SiteSucker, Sixy.ch, Skype URI Preview, Slackbot, Snapchat Proxy, Sogou Spider, Soso Spider, Sparkler, Speedy, Spinn3r, Sputnik Bot, sqlmap, SSL Labs, StatusCake, Superfeedr Bot, Survey Bot, Tarmot Gezgin, TelgramBot, TinEye Crawler, Tiny Tiny RSS, TLSProbe, Trendiction Bot, TurnitinBot, TweetedTimes Bot, Tweetmeme Bot, Twitterbot, UkrNet Mail Proxy, UniversalFeedParser, Uptimebot, Uptime Robot, URLAppendBot, Vagabondo, Visual Site Mapper Crawler, VK Share Button, W3C CSS Validator, W3C I18N Checker, W3C Link Checker, W3C Markup Validation Service, W3C MobileOK Checker, W3C Unified Validator, Wappalyzer, WebbCrawler, WebPageTest, WebSitePulse, WebThumbnail, WeSEE:Search, Willow Internet Crawler, WordPress, Wotbox, YaCy, Yahoo! Cache System, Yahoo! Link Preview, Yahoo! Slurp, Yahoo Gemini, Yandex Bot, Yeti/Naverbot, Yottaa Site Monitor, Youdao Bot, Yourls, Yunyun Bot, Zao, zgrab, Zookabot, ZumBot