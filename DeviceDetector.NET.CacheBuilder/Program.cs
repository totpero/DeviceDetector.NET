using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using DeviceDetectorNET;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DeviceDetector.Net.CacheBuilder
{
    internal class Options
    {
        [Option('i', "input", Required = true, HelpText = "Text file of user agent strings with one string per line")]
        public string InputFile { get; set; }

        [Option('o', "output", Default = "DeviceDetector.db", HelpText = "Filename of cache database")]
        public string OutputFile { get; set; }

        [Option("yaml", HelpText = "Directory of YAML files")]
        public string YamlDirectory { get; set; }

        [Option('a', "append",
            Default = false,
            HelpText = "Append to cache database (default overwrites)")]
        public bool Append { get; set; }

        [Option("cacheTimeoutDays",
            Default = 365,
            HelpText = "Days to keep results in parse cache")]
        public int CacheTimeout { get; set; }

        [Option("skipBotDetection",
            Default = false,
            HelpText = "DeviceDetector skipBotDetection flag (impacts cache key)")]
        public bool SkipBotDetection { get; set; }
    }

    internal static class Program
    {
        private static Options CommandLineOptions { get; set; }

        private static int Main(string[] args)
        {

            var commandLineResult = Parser.Default.ParseArguments<Options>(args)
                                          .MapResult(RunOptions, HandleParseError);

            if (commandLineResult != 0) return commandLineResult;
            using var serviceProvider = new ServiceCollection()
                                        .AddLogging(config =>
                                            config.ClearProviders().AddConsole().SetMinimumLevel(LogLevel.Information))
                                        .BuildServiceProvider();

            SetDeviceDetectorSettings(serviceProvider);

            if (!CommandLineOptions.Append && File.Exists(CommandLineOptions.OutputFile))
                File.Delete(CommandLineOptions.OutputFile);

            var userAgentStrings = File.ReadAllLines(CommandLineOptions.InputFile);
            var builder = new CacheBuilder(userAgentStrings, CommandLineOptions.SkipBotDetection, serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<CacheBuilder>());
            builder.BuildCache();

            return 0;
        }

        private static void SetDeviceDetectorSettings(ServiceProvider dependencyServiceProvider)
        {
            DeviceDetectorSettings.ParseCacheDBDirectory = Path.GetDirectoryName(CommandLineOptions.OutputFile);
            DeviceDetectorSettings.ParseCacheDBFilename = Path.GetFileName(CommandLineOptions.OutputFile);
            DeviceDetectorSettings.ParseCacheDBExpiration = new TimeSpan(CommandLineOptions.CacheTimeout, 0, 0, 0);
            DeviceDetectorSettings.RegexesDirectory = CommandLineOptions.YamlDirectory;
            DeviceDetectorSettings.Logger =
                dependencyServiceProvider?.GetService<ILoggerFactory>()?.CreateLogger("ParseCache");
        }

        private static int HandleParseError(IEnumerable<Error> errs)
        {
            var result = -2;
            Console.WriteLine("errors {0}", errs.Count());
            if (errs.Any(x => x is HelpRequestedError || x is VersionRequestedError))
                result = -1;

            return result;
        }

        private static int RunOptions(Options opts)
        {
            CommandLineOptions = opts;
            return 0;
        }
    }
}