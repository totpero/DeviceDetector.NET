using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using DeviceDetector.Net.Cli.Models;

namespace DeviceDetector.Net.Cli.Rendering
{
    public class CsvResultRenderer : IResultRenderer
    {
        private static readonly string[] Header =
        {
            "UserAgent", "DeviceType", "Brand", "Model", "OsName", "OsVersion",
            "ClientType", "ClientName", "ClientVersion", "IsBot", "BotName"
        };

        public void Render(IReadOnlyList<DetectionResult> results, string outputPath)
        {
            var builder = new StringBuilder();
            builder.AppendLine(string.Join(",", Header));

            foreach (var result in results)
            {
                builder.AppendLine(string.Join(",",
                    Escape(result.UserAgent),
                    Escape(result.DeviceType),
                    Escape(result.Brand),
                    Escape(result.Model),
                    Escape(result.OsName),
                    Escape(result.OsVersion),
                    Escape(result.ClientType),
                    Escape(result.ClientName),
                    Escape(result.ClientVersion),
                    result.IsBot.ToString(CultureInfo.InvariantCulture),
                    Escape(result.BotName)));
            }

            var csv = builder.ToString();

            if (string.IsNullOrEmpty(outputPath))
            {
                Console.Write(csv);
            }
            else
            {
                File.WriteAllText(outputPath, csv);
            }
        }

        private static string Escape(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var needsQuoting = value.IndexOfAny(new[] { ',', '"', '\n', '\r' }) >= 0;
            if (!needsQuoting)
            {
                return value;
            }

            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
    }
}
