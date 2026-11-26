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
- [x] Review and merge the pull request ([#110](https://github.com/totpero/DeviceDetector.NET/pull/110),
      `1fc445c`), branch deleted
- [x] Replace AppVeyor with a GitHub Actions test job ([issue #111](https://github.com/totpero/DeviceDetector.NET/issues/111),
      branch `ci-github-actions`)
- [x] Commit and pull request (branch `ci-github-actions`)
- [ ] The test job passes on the pull request, merge and delete the branch
- [ ] Delete the AppVeyor project and its webhook (repository owner)

Where to continue: the pull request of the branch above ([issue #111](https://github.com/totpero/DeviceDetector.NET/issues/111)),
then the open points at the end of this file.

## CI: AppVeyor replaced by GitHub Actions

The AppVeyor builds failed with `NETSDK1045` since November (first at `f04d54a`, before this work):
AppVeyor no longer applied `appveyor.yml` and built with its UI settings (Visual Studio 2019, .NET 5
SDK). Most likely it lost access to the GitHub API, as it also stopped posting commit statuses.
Re-authorizing GitHub on 2026-11-26 did not help (build 2.0.290), so AppVeyor is removed.

`dotnetcore.yml` has two jobs:

- `build` (Linux): restore, build and pack of the `.slnx`, as before.
- `test` (Windows): `dotnet test` of the `.sln` on `net10.0` and `net11.0`. It runs on Windows because the
  tests build the paths of their fixtures with Windows separators. All the test projects run (AppVeyor ran
  only the library and Serilog ones). No submodule is needed: the one test that uses the real Matomo icons
  passes without them.

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
  fail (`MSB1011`), so the CI names them: it builds `DeviceDetector.NET.slnx` and tests
  `DeviceDetector.NET.sln`, which also checks that both stay valid. Add or remove projects in both.
- The workflows use the .NET 11 SDK (plus the .NET 10 one where `net10.0` code runs).

## Verification

- Visual Studio 2026 MSBuild, rebuild of `.slnx` (Debug) and `.sln` (Release): 0 errors, 0 warnings.
- `dotnet test DeviceDetector.NET.slnx -c Release`: 326 passed on `net10.0` and 326 on `net11.0`.
- The CI steps (restore, build, pack of the `.slnx`) pass. The packages have the expected frameworks and
  dependencies, and the CLI tool contains `net10.0` and `net11.0`.
- An app using the local packages runs on .NET 8, 9, 10 and 11, including the Serilog enricher.
- The command of the new `test` job, on a clean clone without submodules: 326 passed on `net10.0` and 326
  on `net11.0`. The job itself has not run on GitHub yet.

## Open points

- .NET 8 and .NET 9 reached
  [end of support on November 10, 2026](https://devblogs.microsoft.com/dotnet/dotnet-8-9-end-of-support/).
  Dropping them (a breaking change for the packages) would also remove the per framework versions.
- Package versions and release notes are unchanged, so merging does not publish new NuGet packages (the
  release workflow only publishes when a `<Version>` changes).
