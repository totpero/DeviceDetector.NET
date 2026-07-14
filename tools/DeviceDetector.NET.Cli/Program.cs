using DeviceDetector.Net.Cli.Commands;
using DeviceDetector.Net.Cli.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace DeviceDetector.Net.Cli
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var services = new ServiceCollection();
            var registrar = new TypeRegistrar(services);

            var app = new CommandApp<ParseCommand>(registrar);
            app.Configure(config =>
            {
                config.SetApplicationName("ddetect");
                config.SetApplicationVersion(DeviceDetectorNET.DeviceDetector.VERSION);
            });

            return app.Run(args);
        }
    }
}
