using DeviceDetectorNET;
using DeviceDetectorNET.Icons.Matomo;
using DeviceDetectorNET.Parser;
using DeviceDetectorNET.Results;
using DeviceDetectorNet.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeviceDetectorNet.Web.Controllers
{
    [ApiController]

    [Route("[controller]")]
    public class HomeController(ILogger<HomeController> logger, MatomoIconResolver iconResolver) : Controller
    {
        [HttpGet("/")]
        public object Get()
        {
            return Detect();
        }

        [HttpGet("Icon")]
        public IActionResult Icon()
        {
            var match = Detect().Match;
            var paths = iconResolver.GetIcons(match);

            var icons = new List<DetectedIcon>
            {
                new("Operating system", Describe(match.Os?.Name, match.Os?.Version), paths.OsIcon),
                new("Client", Describe(match.Client?.Name, match.Client?.Version), paths.ClientIcon),
                new("Brand", Describe(match.DeviceBrand), paths.BrandIcon),
                new("Device type", Describe(match.DeviceType), paths.DeviceTypeIcon)
            };

            if (match.IsBoot)
            {
                icons.Insert(0, new("Bot", Describe(match.Bot.Name), paths.BotIcon));
            }

            return View(new IconPageModel(match.UserAgent, icons));
        }

        private ParseResult<DeviceDetectorResult> Detect()
        {
            DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

            var userAgent = Request.Headers.UserAgent; // change this to the useragent you want to parse
            var headers = Request.Headers.ToDictionary(a => a.Key, a => a.Value.ToArray().FirstOrDefault());
            var clientHints = ClientHints.Factory(headers);  // client hints are optional

            var result = DeviceDetector.GetInfoFromUserAgent(userAgent, clientHints);

            var output = result.Success ? result.ToString().Replace(Environment.NewLine, "<br />") : "Unknown";
            logger.LogDebug(output);

            return result;
        }

        private static string Describe(params string?[] parts)
        {
            var text = string.Join(" ", parts.Where(part => !string.IsNullOrWhiteSpace(part)));
            return text.Length > 0 ? text : "Unknown";
        }
    }
}