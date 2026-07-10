# Client Hints

Modern Chromium-based browsers increasingly freeze/reduce the information exposed in the `User-Agent` string and instead expose structured data through [User-Agent Client Hints](https://developer.mozilla.org/en-US/docs/Web/HTTP/Client_hints) (`Sec-CH-UA-*` headers). DeviceDetector.NET can combine both sources to improve accuracy.

Client Hints are **optional** — everything works with just a User-Agent string, but passing Client Hints when available lets the parser prefer more precise brand/version/platform data.

## Requesting Client Hints from the browser

Your server must opt in by sending an `Accept-CH` response header listing the hints it wants:

```
Accept-CH: Sec-CH-UA-Full-Version-List, Sec-CH-UA-Model, Sec-CH-UA-Platform, Sec-CH-UA-Platform-Version, Sec-CH-UA-Arch, Sec-CH-UA-Bitness, Sec-CH-UA-Mobile, Sec-CH-UA-Form-Factors
```

On subsequent requests the browser will include the requested hints as headers.

## Building a `ClientHints` instance

`ClientHints.Factory` builds an instance from any `Dictionary<string, string>` of headers — it recognizes both the raw HTTP header names and normalized variants (`http-sec-ch-ua-model`, `sec-ch-ua-model`, `model`, ...):

```csharp
using DeviceDetectorNET;

var headers = Request.Headers.ToDictionary(a => a.Key, a => a.Value.ToArray().FirstOrDefault());
var clientHints = ClientHints.Factory(headers);

var dd = new DeviceDetector(userAgent, clientHints);
dd.Parse();
```

## Headers recognized

| Client Hint header | `ClientHints` property | Meaning |
|---|---|---|
| `Sec-CH-UA-Arch` | `Architecture` | Underlying CPU architecture |
| `Sec-CH-UA-Bitness` | `Bitness` | Underlying CPU architecture bitness |
| `Sec-CH-UA-Mobile` | `Mobile` | Whether the UA wants a "mobile" UX |
| `Sec-CH-UA-Model` | `Model` | Device model |
| `Sec-CH-UA-Platform` | `Platform` | OS/platform brand |
| `Sec-CH-UA-Platform-Version` | `PlatformVersion` | OS/platform version |
| `Sec-CH-UA-Full-Version` | `UaFullVersion` | Full browser version |
| `Sec-CH-UA-Full-Version-List` / `Sec-CH-UA` | `FullVersionList` | Brand → version pairs |
| `Sec-CH-UA-Form-Factors` | `FormFactors` | Device form factor names |
| `X-Requested-With` | `App` | Android app package id (when not `XMLHttpRequest`) |

## Reading values back

```csharp
clientHints.IsMobile();
clientHints.GetArchitecture();
clientHints.GetBitness();
clientHints.GetModel();
clientHints.GetOperatingSystem();
clientHints.GetOperatingSystemVersion();
clientHints.GetBrandList();     // IReadOnlyDictionary<string, string>
clientHints.GetBrandVersion();
clientHints.GetApp();
clientHints.GetFormFactors();   // List<string>
```

## Notes

- `ClientHints` is a straightforward C# port of the PHP `DeviceDetector\ClientHints` class — behavior (including which header wins when several convey the same information) matches upstream.
- If no relevant headers are present, `ClientHints.Factory` still returns a valid (empty) instance — pass it in unconditionally, there's no need to branch on whether the client sent hints.
