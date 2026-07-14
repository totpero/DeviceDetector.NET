# Icon packs: verify/update workflow

`DeviceDetector.NET.Icons` (Simbiat convention) and `DeviceDetector.NET.Icons.Matomo` (Matomo convention)
never vendor icon image assets — see both projects' READMEs. The `icon-packs/matomo-icons` and
`icon-packs/device-detector-icons` git submodules exist only as read-only references for the maintainer, to
re-verify the folder/naming assumptions each resolver hardcodes as the upstream packs evolve. Nothing under
`icon-packs/` is ever copied into a shipped package.

## Bumping a pin

```bash
git -C icon-packs/matomo-icons fetch
git -C icon-packs/matomo-icons checkout <tag-or-commit>
git add icon-packs/matomo-icons
git commit -m "Bump matomo-icons submodule to <tag>"
```

Same commands for `icon-packs/device-detector-icons`.

## What to re-verify after a bump

1. **Folder set unchanged.**
   - Matomo (`MatomoIconResolver`): `browsers/`, `os/`, `brand/`, `devices/` under `dist/` still exist and
     are still the only ones relevant (unrelated ones — `flags/`, `plugins/`, `searchEngines/`, `SEO/`,
     `socials/`, `aiAssistants/` — stay out of scope).
   - Simbiat (`IconResolver`): `bot/`, `bot/category/`, `client/type/`, `client/browser/`,
     `client/browser/family/`, `client/browser/engine/`, `client/os/`, `client/os/family/`,
     `device/brand/`, `device/type/` still exist.
2. **Key scheme per folder unchanged**, e.g. `ls icon-packs/matomo-icons/dist/browsers | head` should still
   show two/three-letter short codes (`FF.png`), not full names; `ls icon-packs/matomo-icons/dist/brand |
   head` should still show full brand names (`Acer.svg`), not short codes.
3. **New `DeviceType`/`Devices` members.** If `src/DeviceDetector.NET/Parser/Device/Devices.cs` gained new
   `DeviceTypes` or `DeviceBrands` entries since the last bump (from a PHP `device-detector` submodule sync),
   check whether the icon pack has matching new files before assuming coverage — the resolvers only return
   the configured `FallbackIconPath` for anything not covered.
4. **Extension set.** If a probe with `ls icon-packs/matomo-icons/dist/browsers | sed 's/.*\.//' | sort -u`
   shows extensions outside `MatomoIconResolverOptions`'s default `ExtensionPriority`
   (`png, svg, jpg, gif, ico`), update the default.
5. **`Devices.GetDeviceName()` spelling.** `MatomoIconResolverOptions`'s default `NameReplacements` maps the
   five multi-word device-type names (`"car browser"`, `"feature phone"`, `"smart display"`,
   `"portable media player"`, `"smart speaker"`) to their snake_case matomo-icons file names. If
   `Devices.cs`'s `DeviceTypes` dictionary keys change wording, update this map too.
