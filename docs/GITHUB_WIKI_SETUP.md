# Configurare Wiki GitHub pentru DeviceDetector.NET

Acest document explică pas cu pas cum se activează, configurează și întreține secțiunea **Wiki** a repository-ului `totpero/DeviceDetector.NET` pe GitHub, plus o structură de pagini recomandată pentru acest proiect.

## 1. Activarea Wiki-ului în repository

1. Intră pe pagina repository-ului: `https://github.com/totpero/DeviceDetector.NET`
2. Click pe **Settings** (necesită drepturi de admin pe repo).
3. În secțiunea **Features**, bifează caseta **Wikis**.
4. Opțional, sub **Settings → Wikis** (sau direct din tab-ul **Wiki**), poți restricționa editarea:
   - **"Restrict editing to collaborators only"** — recomandat pentru un proiect de librărie, ca să eviți spam/vandalism.
   - Lasă bifat doar dacă vrei să permiți și contribuții externe (issue: risc de conținut nepotrivit, dar poate crește implicarea comunității).
5. Salvează. Un nou tab **Wiki** apare în bara de navigare a repo-ului.

## 2. Structura wiki-ului este un repo Git separat

GitHub tratează wiki-ul ca pe un repository Git independent, clonabil:

```bash
git clone https://github.com/totpero/DeviceDetector.NET.wiki.git
```

- Fiecare pagină e un fișier Markdown (`.md`) în rădăcina acestui repo.
- Numele fișierului = titlul paginii, cu spații înlocuite prin `-` (ex: `Client-Hints-Support.md`).
- Pagina inițială obligatorie se numește `Home.md`.
- Poți edita local (VS Code, orice editor) și face `git push` — mult mai comod decât editorul web pentru schimbări mari sau cu multe pagini.

> Notă: `git clone` funcționează abia după ce ai creat cel puțin o pagină din interfața web (repo-ul wiki nu există până la primul commit).

## 3. Conținutul este deja generat în `wiki/`

Paginile efective au fost scrise și verificate contra codului sursă curent, în directorul [`wiki/`](../wiki) din acest repo (nu în repo-ul de wiki separat — vezi secțiunea 4 pentru publicare):

- [`Home.md`](../wiki/Home.md) — prezentare, tabel de navigare, licență
- [`Getting-Started.md`](../wiki/Getting-Started.md) — instalare NuGet, exemplu `Parse()`, helper `GetInfoFromUserAgent`
- [`Device-and-Client-Type-Checks.md`](../wiki/Device-and-Client-Type-Checks.md) — toate metodele `IsX()` + `Is(ClientType)`
- [`Client-Hints.md`](../wiki/Client-Hints.md) — `ClientHints.Factory(...)`, header-e recunoscute
- [`Caching.md`](../wiki/Caching.md) — cele 3 straturi de cache (`ICache`, `ParseCache`/LiteDB, `LRUCachedDeviceDetector`)
- [`Regex-Engines.md`](../wiki/Regex-Engines.md) — `MSRegexEngine`/`MSRegexCompiledEngine` vs. pachetul `DeviceDetector.NET.RegexEngine.PCRE`
- [`Bot-Detection.md`](../wiki/Bot-Detection.md) — `IsBot()`, `DiscardBotInformation()`, `SkipBotDetection()`, `BotParser` direct
- [`ASP.NET-Core-Integration.md`](../wiki/ASP.NET-Core-Integration.md) — demo `DeviceDetector.NET.Web` + pattern de dependency injection (issue #83)
- [`Serilog.Enrichers.AspNetCore.DeviceDetector.md`](../wiki/Serilog.Enrichers.AspNetCore.DeviceDetector.md) — pachetul Serilog dedicat, caching per-request, setup complet
- [`CacheBuilder-Tool.md`](../wiki/CacheBuilder-Tool.md) — opțiunile CLI ale `DeviceDetector.NET.CacheBuilder`
- [`Updating-Device-Detector-Data.md`](../wiki/Updating-Device-Detector-Data.md) — workflow-ul de sincronizare cu submodulul PHP
- [`FAQ.md`](../wiki/FAQ.md)
- [`Contributing.md`](../wiki/Contributing.md)
- [`_Sidebar.md`](../wiki/_Sidebar.md) — meniu lateral persistent, recunoscut automat de GitHub
- [`_Footer.md`](../wiki/_Footer.md) — subsol persistent, recunoscut automat de GitHub

Linkurile interne dintre pagini folosesc deja formatul corect pentru wiki: `[Text afișat](Nume-Pagina)` — fără `.md`, fără `https://`.

## 4. Publicarea conținutului din `wiki/` pe wiki-ul GitHub

Fișierele din [`wiki/`](../wiki) nu ajung automat pe `github.com/totpero/DeviceDetector.NET/wiki` — wiki-ul e un repo Git separat, deci trebuie copiate și împinse acolo o singură dată (apoi la fiecare actualizare):

1. Activează Wiki-ul din **Settings → Features** (pasul 1) și creează manual o primă pagină din UI (altfel repo-ul de wiki nu există încă și `git clone` dă eroare).
2. Clonează repo-ul de wiki alături de acest repo:
   ```bash
   cd ..
   git clone https://github.com/totpero/DeviceDetector.NET.wiki.git
   ```
3. Copiază fișierele generate:
   ```bash
   cp DeviceDetector.NET/wiki/*.md DeviceDetector.NET.wiki/
   ```
4. Commit + push:
   ```bash
   cd DeviceDetector.NET.wiki
   git add .
   git commit -m "Publish generated documentation"
   git push
   ```
5. Verifică pe `https://github.com/totpero/DeviceDetector.NET/wiki`.

Pentru actualizări viitoare: editează fișierele din `wiki/` în acest repo (ca sursă de adevăr, versionată alături de cod), apoi repetă pașii 3–4 pentru a le republica. Editarea directă din browser-ul wiki-ului rămâne utilă pentru modificări mici ad-hoc, dar riscă să diverge de conținutul din `wiki/` dacă nu e adusă înapoi în acest repo.

## 5. Sincronizare opțională README ↔ Wiki

Dacă vrei ca anumite secțiuni din [README.md](../README.md) (ex. lista metodelor `IsX()`) să nu diverge de conținutul wiki-ului, două opțiuni:

- **Manual**: la fiecare update relevant din README, actualizezi manual pagina corespunzătoare din wiki. Suficient pentru un proiect de mărimea asta.
- **Automatizat**: un GitHub Action care, la push pe `master` cu modificări în README, copiază/generează pagini în `DeviceDetector.NET.wiki` repo (necesită un token cu drept de scriere pe wiki, ex. `GITHUB_TOKEN` cu `contents: write` nu e suficient — wiki-ul e alt repo, deci ai nevoie de un PAT sau deploy key dedicat). Recomandat doar dacă README-ul se schimbă frecvent; altfel e complexitate inutilă.

## 6. Bune practici

- Nu duplica documentația de API detaliată din [device-detector](https://github.com/matomo-org/device-detector) (proiectul PHP sursă) — leagă către ea și documentează doar diferențele/specificul .NET.
- Ține `Home.md` scurt: e prima impresie, nu un manual complet.
- Folosește blocuri de cod cu limbaj specificat (` ```csharp `) pentru highlighting corect.
- Activează notificări pentru wiki (Watch → Custom → Wiki) dacă vrei să vezi contribuții externe, în caz că ai permis editarea publică.
