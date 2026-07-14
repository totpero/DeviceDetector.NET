# DeviceDetector.NET.Cli

Command-line tool for parsing User-Agent strings with [DeviceDetector.NET](https://github.com/totpero/DeviceDetector.NET).

## Install

```
dotnet tool install --global DeviceDetector.NET.Cli
```

## Usage

```
ddetect "Mozilla/5.0 (Linux; Android 10; SM-G960F) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.120 Mobile Safari/537.36"
```

Batch from a file (one User-Agent per line):

```
ddetect --file ua-list.txt
```

Batch via stdin:

```
cat ua-list.txt | ddetect
```

Client Hints (single User-Agent only):

```
ddetect "Mozilla/5.0 ..." --client-hints hints.json
```

Output as JSON or CSV:

```
ddetect "Mozilla/5.0 ..." --json
ddetect --file ua-list.txt --csv -o results.csv
```

## Options

| Option | Description |
|---|---|
| `[userAgents]` | One or more User-Agent strings (positional). |
| `--file <PATH>` | Read User-Agents, one per line, from a file. |
| `--client-hints <JSON-OR-PATH>` | Inline JSON or path to a JSON file of HTTP headers for Client Hints detection. Requires exactly one User-Agent. |
| `--json` | Output as JSON. |
| `--csv` | Output as CSV. |
| `-o, --output <PATH>` | Write `--json`/`--csv` output to a file instead of stdout. |
| `--skip-bot-detection` | Skip bot detection so bots are parsed as regular clients/devices. |
| `--no-color` | Disable colored console output. |
