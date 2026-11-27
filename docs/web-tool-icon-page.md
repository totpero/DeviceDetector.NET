# Web tool: JSON at the root and a page with the detected icons

Working notes for [issue #113](https://github.com/totpero/DeviceDetector.NET/issues/113). Branch:
`web-tool-icon-page`.

## Problem

Running `tools/DeviceDetector.NET.Web` from Visual Studio opens the root address, which returned 404.

- `HomeController` only had the `/Home` route. `Properties/launchSettings.json` is ignored by git, so the
  page should not depend on a `launchUrl` set there.
- `Get` and `Icon` both used `[HttpGet]` without a template: `/Home` returned 500
  (`AmbiguousMatchException`) and `/Home/Icon` returned 404.
- `Icon` returned only the icon paths, and the app did not serve the icon files.
- The app pointed to `icon-packs/matomo-icons/src`. 114 of the browser, OS, brand and device images there
  are symbolic links, which Git on Windows checks out as text files when `core.symlinks` is `false`. The
  Chrome icon is one of them. `dist` has no links.
- The default `FallbackIconPath`, `/Matomo.svg`, is not a file of the pack.

## Status

- [x] Reproduce the errors
- [x] Return the detection result as JSON at `/`
- [x] Give `Icon` its own route and return an HTML page with the detected icons
- [x] Serve the icons from `dist`, with an existing file as fallback
- [x] Check the pages in the browser and build
- [x] Commit (branch `web-tool-icon-page`)
- [ ] Push, pull request and merge (only when asked)

Where to continue: push the branch and open the pull request.

## Decisions

- **Routes.** `Get` answers on `/`, `Icon` on `/Home/Icon`, and `/Home` itself has no route. The routes are
  set in the controller, not in `launchSettings.json`.
- **Icon page.** A Razor view, so the values taken from the request (user agent, client hints, device
  model) are HTML encoded.
- **Icon pack.** The app uses `dist` (real 48x48 PNG files, as the resolver's documentation asks) and
  `/devices/unknown.png` as fallback. The submodule is optional, so the app also starts without it.
- **Target framework.** The `Web`, `CacheBuilder` and `ReadmeGen` tools move to `net11.0`, in the same
  commit. This replaces the decision in [net11-upgrade.md](net11-upgrade.md) to keep them on `net10.0`. The
  workflows that build them already install the .NET 11 SDK, only the comment in `update-readme.yml` still
  says that ReadmeGen runs on `net10.0`.

## Verification

- `dotnet build DeviceDetector.NET.slnx`: 0 errors, 0 warnings.
- `/` returns the JSON result, `/Home/Icon` the icon page. The requests below ran while `Get` also
  answered on `/Home`. After that route was removed, only the build was checked.
- Icon page with Chrome on Windows (browser), a Samsung phone and an iPhone: every image is served.
  Googlebot and an unknown client show "Unknown" cards. A request without a user agent works.
- HTML in the user agent is encoded on the page.
- Without the icon pack (another content root) the app starts, logs a warning and both pages answer.
- Not run from Visual Studio itself: checked with `dotnet run`, with the same content root, over HTTP.

## Open points

- The unknown placeholders of the pack are fully transparent PNG files, so a card without a match shows an
  empty space above "Unknown".
- The library's default `FallbackIconPath`, `/Matomo.svg`, is not part of the matomo-icons pack. Only the
  web tool overrides it here, the package is unchanged.
