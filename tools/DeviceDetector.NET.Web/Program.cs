using DeviceDetectorNET.Icons.Matomo;
using Microsoft.Extensions.FileProviders;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
    .MinimumLevel.Debug()
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.WithDeviceDetector(services)
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"));

// The icons come from a git submodule: git submodule update --init icon-packs/matomo-icons
// Use dist/: many files in src/ are symbolic links, which on Windows are checked out as text files.
var iconsRootPath = Path.GetFullPath(
    Path.Combine(builder.Environment.ContentRootPath, "../../icon-packs/matomo-icons/dist"));
const string iconsUrlPath = "/assets/images/devicedetector";

builder.Services.AddMatomoDeviceDetectorIcons(options =>
{
    options.PhysicalRootPath = iconsRootPath;
    options.UrlBasePath = iconsUrlPath;
    options.FallbackIconPath = "/devices/unknown.png"; // the default, /Matomo.svg, is not part of the pack
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

if (Directory.Exists(iconsRootPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(iconsRootPath),
        RequestPath = iconsUrlPath
    });
}
else
{
    app.Logger.LogWarning("Icons not found in {IconsRootPath}", iconsRootPath);
}

//app.UseAuthorization();

app.MapControllers();

app.Run();
