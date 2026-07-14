using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using DeviceDetector.Net.Cli.Models;
using DeviceDetector.Net.Cli.Rendering;
using DeviceDetectorNET;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DeviceDetector.Net.Cli.Commands
{
    public class ParseCommand : Command<ParseCommandSettings>
    {
        protected override int Execute(CommandContext context, ParseCommandSettings settings, CancellationToken cancellationToken)
        {
            if (settings.NoColor)
            {
                AnsiConsole.Profile.Capabilities.Ansi = false;
            }

            List<string> userAgents;
            try
            {
                userAgents = ResolveUserAgents(settings);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLineInterpolated($"[red]Error:[/] {ex.Message}");
                return 1;
            }

            if (userAgents.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]Error:[/] no User-Agent provided. Pass one as an argument, use --file, or pipe UAs via stdin.");
                return 1;
            }

            ClientHints clientHints = null;
            if (!string.IsNullOrEmpty(settings.ClientHints))
            {
                if (userAgents.Count != 1)
                {
                    AnsiConsole.MarkupLine("[red]Error:[/] --client-hints requires exactly one User-Agent.");
                    return 1;
                }

                try
                {
                    clientHints = LoadClientHints(settings.ClientHints);
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLineInterpolated($"[red]Error:[/] failed to load --client-hints: {ex.Message}");
                    return 1;
                }
            }

            var results = userAgents
                .Select(ua => Detect(ua, clientHints, settings.SkipBotDetection))
                .ToList();

            IResultRenderer renderer = settings.Json
                ? new JsonResultRenderer()
                : settings.Csv
                    ? new CsvResultRenderer()
                    : new TableResultRenderer();

            renderer.Render(results, settings.Output);

            return 0;
        }

        private static List<string> ResolveUserAgents(ParseCommandSettings settings)
        {
            if (settings.UserAgents.Length > 0)
            {
                return settings.UserAgents.ToList();
            }

            if (!string.IsNullOrEmpty(settings.File))
            {
                if (!File.Exists(settings.File))
                {
                    throw new FileNotFoundException($"file not found: {settings.File}");
                }

                return ReadNonEmptyLines(File.ReadAllLines(settings.File));
            }

            if (Console.IsInputRedirected)
            {
                var lines = new List<string>();
                string line;
                while ((line = Console.In.ReadLine()) != null)
                {
                    lines.Add(line);
                }

                return ReadNonEmptyLines(lines);
            }

            return new List<string>();
        }

        // Console.In.ReadLine() (unlike File.ReadAllLines) does not strip a leading UTF-8 BOM,
        // so a piped stdin source can leak it into the first line.
        private static readonly char Utf8ByteOrderMark = Convert.ToChar(0xFEFF);

        private static List<string> ReadNonEmptyLines(IEnumerable<string> lines)
        {
            return lines
                .Select(l => l.TrimStart(Utf8ByteOrderMark))
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();
        }

        private static ClientHints LoadClientHints(string clientHintsArg)
        {
            var json = File.Exists(clientHintsArg) ? File.ReadAllText(clientHintsArg) : clientHintsArg;
            var headers = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            return ClientHints.Factory(headers ?? new Dictionary<string, string>());
        }

        private static DetectionResult Detect(string userAgent, ClientHints clientHints, bool skipBotDetection)
        {
            var detector = new DeviceDetectorNET.DeviceDetector(userAgent, clientHints);
            detector.SkipBotDetection(skipBotDetection);
            detector.Parse();

            var os = detector.GetOs();
            var client = detector.GetClient();
            var isBot = detector.IsBot();
            var bot = detector.GetBot();

            return new DetectionResult
            {
                UserAgent = userAgent,
                DeviceType = detector.GetDeviceName(),
                Brand = detector.GetBrandName(),
                Model = detector.GetModel(),
                OsName = os.Success ? os.Match.Name : null,
                OsVersion = os.Success ? os.Match.Version : null,
                ClientType = client.Success ? client.Match.Type : null,
                ClientName = client.Success ? client.Match.Name : null,
                ClientVersion = client.Success ? client.Match.Version : null,
                IsBot = isBot,
                BotName = isBot && bot.Success ? bot.Match.Name : null
            };
        }
    }
}
