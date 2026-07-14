using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DeviceDetector.Net.Cli.Commands
{
    public class ParseCommandSettings : CommandSettings
    {
        [CommandArgument(0, "[userAgents]")]
        [Description("One or more User-Agent strings to parse. If omitted, --file or stdin is used instead.")]
        public string[] UserAgents { get; set; } = System.Array.Empty<string>();

        [CommandOption("--file <PATH>")]
        [Description("Path to a file with one User-Agent string per line.")]
        public string File { get; set; }

        [CommandOption("--client-hints <JSON-OR-PATH>")]
        [Description("Inline JSON object or path to a JSON file of HTTP headers for Client Hints detection. Only valid with exactly one User-Agent.")]
        public string ClientHints { get; set; }

        [CommandOption("--json")]
        [Description("Output results as JSON instead of a table.")]
        public bool Json { get; set; }

        [CommandOption("--csv")]
        [Description("Output results as CSV instead of a table.")]
        public bool Csv { get; set; }

        [CommandOption("-o|--output <PATH>")]
        [Description("Destination file for --json/--csv output. Ignored for the default table output.")]
        public string Output { get; set; }

        [CommandOption("--skip-bot-detection")]
        [Description("Skip bot detection so bots are parsed as regular clients/devices.")]
        public bool SkipBotDetection { get; set; }

        [CommandOption("--no-color")]
        [Description("Disable colored console output.")]
        public bool NoColor { get; set; }

        public override ValidationResult Validate()
        {
            if (Json && Csv)
            {
                return ValidationResult.Error("--json and --csv cannot be used together.");
            }

            return ValidationResult.Success();
        }
    }
}
