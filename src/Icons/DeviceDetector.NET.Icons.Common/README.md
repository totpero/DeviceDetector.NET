DeviceDetector.NET.Icons.Common
================================

Shared icon-path-resolution primitives used by
[`DeviceDetector.NET.Icons`](https://github.com/totpero/DeviceDetector.NET) and
[`DeviceDetector.NET.Icons.Matomo`](https://github.com/totpero/DeviceDetector.NET). Not meant to be
referenced directly by application code — install one of those two packages instead, depending on which
icon pack (Simbiat's or the official Matomo one) you're using.

## Adding a new icon pack

A new icon-pack project shares `IIconProbeOptions`, `IconResolverOptionsValidation` and `IconPathProber`
from here, but does **not** share a base resolver class or a generic DI-registration helper — each pack's
folder layout and fallback rules differ enough that forcing a shared resolver base would fight the design
more than it would help. Instead, follow this convention by copying its shape, not by inheriting code:

1. `<Pack>IconResolverOptions : IIconProbeOptions`, with pack-specific defaults for `ExtensionPriority`,
   `NameReplacements` and `FallbackIconPath`.
2. `<Pack>IconResolver`, constructed with `IconResolverOptionsValidation.Validate(...)` then a private
   `IconPathProber` built from `options.FileExists ?? IconResolverOptionsValidation.DefaultFileExists(...)`.
3. A `ServiceCollectionExtensions.Add<Pack>DeviceDetectorIcons(this IServiceCollection, Action<TOptions>)`
   extension method, registering the resolver as a singleton — same shape as
   `DeviceDetectorNET.Icons.Matomo.ServiceCollectionExtensions.AddMatomoDeviceDetectorIcons`.
4. A `<Pack>.Tests` project mirroring the structure of `DeviceDetector.NET.Icons.Matomo.Tests` (construction,
   options defaults, lookups, `FileExists` override, result-type overloads, DI, end-to-end).
