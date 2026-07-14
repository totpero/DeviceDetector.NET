DeviceDetector.NET.Icons
=========================

Resolves icon file paths for [DeviceDetector.NET](https://github.com/totpero/DeviceDetector.NET)
detection results — bots, browsers, operating systems, clients (feed readers, media players, PIMs,
libraries, mobile apps) and device brands/types — with filesystem-existence-based extension fallback.

This package does **not** bundle icon image assets. Point it at your own copy of an icon pack, such as:
- Unofficial [Simbiat/DeviceDetectorIcons](https://github.com/Simbiat/DeviceDetectorIcons) pack (the
  resolution algorithm below is a C# port of this project's `DDCIcons.php`, MIT licensed,
  © 2021 Dmitrii Kustov — see [Attribution](#attribution))
- Official [Matomo icons](https://github.com/matomo-org/matomo-icons/) pack

## Install

```
dotnet add package DeviceDetector.NET.Icons
```

## Usage

```csharp
using DeviceDetectorNET;
using DeviceDetectorNET.Icons;

var resolver = new IconResolver(new IconResolverOptions
{
    // Folder that directly contains the icon pack's bot/, client/ and device/ subfolders.
    PhysicalRootPath = "/var/www/wwwroot/assets/images/devicedetector"
});

var parseResult = DeviceDetector.GetInfoFromUserAgent(userAgentString);
var icons = resolver.GetIcons(parseResult.Match);

// icons.BotIcon, icons.OsIcon, icons.ClientIcon, icons.BrandIcon, icons.DeviceTypeIcon
```

Or register it for dependency injection:

```csharp
using DeviceDetectorNET.Icons;

builder.Services.AddDeviceDetectorIcons(options =>
{
    options.PhysicalRootPath = Path.Combine(builder.Environment.WebRootPath, "assets/images/devicedetector");
});
```

Individual lookups are also available, mirroring the upstream PHP API:

```csharp
resolver.GetBrowser("Chrome", family: "Chrome", engine: "Blink");
resolver.GetOs("Ubuntu", family: "Linux");
resolver.GetBrand("Apple", type: "smartphone");
```

### Hosting icons behind an `IFileProvider` or CDN

By default, existence is checked with `File.Exists` against `PhysicalRootPath`. To back it with
something else (an ASP.NET Core `IFileProvider`, a CDN manifest, etc.), set `FileExists` instead:

```csharp
var resolver = new IconResolver(new IconResolverOptions
{
    FileExists = relativePath => fileProvider.GetFileInfo(relativePath).Exists,
    UrlBasePath = "https://cdn.example.com/devicedetector-icons"
});
```

## Attribution

The path-resolution, extension-priority-fallback and name-sanitization logic in this package is a
C# port of [`Simbiat/DeviceDetectorIcons`](https://github.com/Simbiat/DeviceDetectorIcons)'s
`DDCIcons.php` class:

> MIT License
>
> Copyright (c) 2021 Dmitrii Kustov
>
> Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
> associated documentation files (the "Software"), to deal in the Software without restriction,
> including without limitation the rights to use, copy, modify, merge, publish, distribute,
> sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
> furnished to do so, subject to the following conditions: The above copyright notice and this
> permission notice shall be included in all copies or substantial portions of the Software. THE
> SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
> LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
> NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
> DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
> OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
