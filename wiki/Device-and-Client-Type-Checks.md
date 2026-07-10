# Device and Client Type Checks

After calling `dd.Parse()`, `DeviceDetector` exposes a set of convenience boolean helpers so you rarely need to inspect raw match results just to answer "is this a phone?" or "is this a browser?".

## Device type checks

```csharp
dd.IsSmartphone();
dd.IsFeaturePhone();
dd.IsTablet();
dd.IsPhablet();
dd.IsConsole();
dd.IsPortableMediaPlayer();
dd.IsCarBrowser();
dd.IsTv();
dd.IsSmartDisplay();
dd.IsSmartSpeaker();
dd.IsCamera();
dd.IsWearable();
dd.IsPeripheral();
```

Two broader helpers combine several device types:

```csharp
dd.IsMobile();   // smartphone, feature phone, tablet, phablet, car browser, camera, portable media player, ...
dd.IsDesktop();  // desktop devices (device type unknown/not mobile and OS is a desktop OS)
```

## Client type checks

```csharp
dd.IsBrowser();
dd.IsFeedReader();
dd.IsMobileApp();
dd.IsPIM();          // Personal Information Manager
dd.IsLibrary();      // HTTP client libraries (curl, okhttp, Guzzle, ...)
dd.IsMediaPlayer();
```

## Extra helpers

```csharp
dd.IsBot();                    // see also the dedicated Bot Detection page
dd.IsTouchEnabled();
dd.HasAndroidTableFragment();  // Android "Tab" UA fragment
dd.HasAndroidMobileFragment(); // Android "Mobile" UA fragment
dd.HasAndroidVRFragment();     // Android "VR" UA fragment
dd.IsParsed();                 // whether Parse() has been called and produced a result
```

Generic check against any `ClientType`:

```csharp
using DeviceDetectorNET.Parser.Client;

dd.Is(ClientType.Browser);
dd.Is(ClientType.FeedReader);
dd.Is(ClientType.Library);
dd.Is(ClientType.MediaPlayer);
dd.Is(ClientType.MobileApp);
dd.Is(ClientType.PIM);
```

> `Is(ClientType)` (and every `IsX()` boolean helper) requires `Parse()` to have been called first — calling it beforehand throws `AccessViolationException`.

## Getting the underlying data

Beyond the booleans, the full parsed data is available through:

```csharp
dd.GetDeviceName();       // e.g. "smartphone"
dd.GetBrandName();
dd.GetModel();
dd.GetOs();                // ParseResult<OsMatchResult>
dd.GetClient();            // ParseResult<ClientMatchResult> — generic client info
dd.GetBrowserClient();     // ParseResult<BrowserMatchResult> — browser-specific info
dd.GetBot();               // ParseResult<BotMatchResult>, null unless IsBot()
```

Each `ParseResult<T>.Match` carries the strongly-typed match (name, version, and type-specific fields such as engine/family for browsers).

These map 1:1 onto the PHP methods (`$dd->isSmartphone()`, `$dd->getOs()`, ...) described in the [upstream README](https://github.com/matomo-org/device-detector#usage) — only the casing changes (PascalCase in .NET vs. camelCase in PHP).
