# Contributing

Contributions and feedback are welcome — pull requests, bug reports, and detection-accuracy reports all help.

## Before opening an issue

- If it's a **detection accuracy** issue (a UA classified incorrectly), first check whether the [upstream PHP project](https://github.com/matomo-org/device-detector) has the same problem — see [FAQ](FAQ#is-a-wrong-detection-a-devicedetectornet-bug-or-a-rules-issue). If PHP also gets it wrong, report it upstream instead; this repo picks up upstream fixes on its next [data sync](Updating-Device-Detector-Data).
- If it's a **.NET-specific bug** (porting mismatch vs. PHP, exception, API issue, packaging problem), open it here with:
  - The User-Agent string (and Client Hints, if relevant) that triggers it
  - Expected vs. actual result
  - Target framework and DeviceDetector.NET version

## Setting up the repo locally

```bash
git clone --recurse-submodules https://github.com/totpero/DeviceDetector.NET.git
cd DeviceDetector.NET
dotnet build DeviceDetector.NET.sln
```

The `--recurse-submodules` flag matters — the [`device-detector`](https://github.com/totpero/DeviceDetector.NET/tree/master/device-detector) PHP submodule is used as the reference for rule/fixture sync (see [Updating Device Detector Data](Updating-Device-Detector-Data)); without it that directory stays empty.

## Running the tests

```bash
dotnet test DeviceDetector.NET.Tests/DeviceDetector.NET.Tests.csproj -c Release --nologo
```

The test project uses **xUnit** with **Shouldly** assertions, and mirrors upstream PHP fixtures under `DeviceDetector.NET.Tests/fixtures/`. Tests run in parallel across fixtures — a failure surfaces as an `AggregateException`; expand it to see every individual mismatch, not just the first.

## Pull request guidelines

- Keep changes scoped: a rules/fixture sync PR shouldn't also refactor unrelated parser code, and vice versa — it makes review and any necessary revert much easier.
- If you touch YAML files under `DeviceDetector.NET/regexes/`, remember they are **embedded resources** — rebuild before testing, a stale build will silently keep using old rules.
- Run the full test suite before submitting; if a test fails that you believe was already broken before your change, say so explicitly in the PR description rather than leaving it unexplained.
- For CI status, see the [AppVeyor build](https://ci.appveyor.com/project/totpero/devicedetector-net) — configuration lives in [`appveyor.yml`](https://github.com/totpero/DeviceDetector.NET/blob/master/appveyor.yml).

## Community

- [Gitter chat](https://gitter.im/totpero/DeviceDetector.NET) for questions and discussion.
- For anything about the detection *rules themselves* (as opposed to the .NET port), the [upstream project's contributing guidelines](https://github.com/matomo-org/device-detector#contributing) apply.
