using System.Collections.Generic;
using DeviceDetector.Net.Cli.Models;
using Spectre.Console;

namespace DeviceDetector.Net.Cli.Rendering
{
    public class TableResultRenderer : IResultRenderer
    {
        public void Render(IReadOnlyList<DetectionResult> results, string outputPath)
        {
            if (!string.IsNullOrEmpty(outputPath))
            {
                AnsiConsole.MarkupLine("[yellow]Warning:[/] --output is ignored for the default table renderer; use --json or --csv to write to a file.");
            }

            var table = new Table();
            table.AddColumn("User-Agent");
            table.AddColumn("Device Type");
            table.AddColumn("Brand");
            table.AddColumn("Model");
            table.AddColumn("OS");
            table.AddColumn("Client");
            table.AddColumn("Bot");

            foreach (var result in results)
            {
                table.AddRow(
                    Truncate(result.UserAgent, 50).EscapeMarkup(),
                    (result.DeviceType ?? string.Empty).EscapeMarkup(),
                    (result.Brand ?? string.Empty).EscapeMarkup(),
                    (result.Model ?? string.Empty).EscapeMarkup(),
                    FormatOs(result).EscapeMarkup(),
                    FormatClient(result).EscapeMarkup(),
                    (result.IsBot ? result.BotName ?? "yes" : string.Empty).EscapeMarkup());
            }

            AnsiConsole.Write(table);
        }

        private static string FormatOs(DetectionResult result)
        {
            if (string.IsNullOrEmpty(result.OsName))
            {
                return string.Empty;
            }

            return string.IsNullOrEmpty(result.OsVersion)
                ? result.OsName
                : $"{result.OsName} {result.OsVersion}";
        }

        private static string FormatClient(DetectionResult result)
        {
            if (string.IsNullOrEmpty(result.ClientName))
            {
                return string.Empty;
            }

            var version = string.IsNullOrEmpty(result.ClientVersion) ? string.Empty : $" {result.ClientVersion}";
            var type = string.IsNullOrEmpty(result.ClientType) ? string.Empty : $" ({result.ClientType})";
            return $"{result.ClientName}{version}{type}";
        }

        private static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            {
                return value ?? string.Empty;
            }

            return value.Substring(0, maxLength - 1) + "…";
        }
    }
}
