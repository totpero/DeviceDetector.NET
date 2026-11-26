# .NET 11, NuGet updates and the .slnx solution

Working notes for [issue #109](https://github.com/totpero/DeviceDetector.NET/issues/109). Branch:
`net11-support`.

## Problem

After the NuGet updates the build failed in Visual Studio 2026 (18.13) and with `dotnet build`:

- `Serilog.AspNetCore` 11.0.0 only targets `net10.0`/`net11.0`. On `net8.0`/`net9.0` NuGet takes the
  `netstandard2.1` assets and `HttpContextAccessor` is missing.
- `Spectre.Console.Cli` 0.57.2 made `Command<T>.Execute` public.
- `Microsoft.Extensions.*` and `System.Text.Json` 11.0.0 do not support `net8.0`/`net9.0` and warn.

## Status

- [x] Fix the build
- [x] Add .NET 11
- [x] Add `DeviceDetector.NET.slnx`
- [x] Update the CI configuration, README and wiki
- [x] Verify
- [x] Commit and pull request (branch `net11-support`)
- [ ] Review and merge the pull request, then delete the branch

Where to continue: the pull request of the branch above, then the open points at the end of this file.

## Decisions

- **Package versions per framework.** `net8.0` and `net9.0` use the latest packages that support them:
  `Microsoft.Extensions.*.Abstractions` 10.0.15 and `Serilog.AspNetCore` 10.0.0. All other frameworks
  use the updated 11.0.0 versions. When updating packages from Visual Studio, keep the `net8.0`/`net9.0`
  entries on 10.x.
- **`net11.0`** is added to the libraries. The BitFaster cache and PCRE engine projects, the CLI tool and
  the test projects target `net10.0` and `net11.0`. The `Web`, `CacheBuilder` and `ReadmeGen` tools stay
  on `net10.0`.
- On `net11.0` the logging and dependency injection abstractions are part of the framework, so NuGet
  leaves them out of the `net11.0` dependencies of the packages.
- **Both solution files are kept.** With two of them in the root, `dotnet` commands without an argument
  fail (`MSB1011`), so GitHub Actions builds `DeviceDetector.NET.slnx` and AppVeyor builds
  `DeviceDetector.NET.sln`; this also checks that both stay valid. Add or remove projects in both.
- The workflows and AppVeyor use the .NET 11 SDK (plus the .NET 10 one where `net10.0` code runs).

## Verification

- Visual Studio 2026 MSBuild, rebuild of `.slnx` (Debug) and `.sln` (Release): 0 errors, 0 warnings.
- `dotnet test DeviceDetector.NET.slnx -c Release`: 326 passed on `net10.0` and 326 on `net11.0`.
- The CI steps (restore, build, pack of the `.slnx`) pass. The packages have the expected frameworks and
  dependencies, and the CLI tool contains `net10.0` and `net11.0`.
- An app using the local packages runs on .NET 8, 9, 10 and 11, including the Serilog enricher.

## Open points

- .NET 8 and .NET 9 reached
  [end of support on November 10, 2026](https://devblogs.microsoft.com/dotnet/dotnet-8-9-end-of-support/).
  Dropping them (a breaking change for the packages) would also remove the per framework versions.
- Package versions and release notes are unchanged, so merging does not publish new NuGet packages (the
  release workflow only publishes when a `<Version>` changes).
