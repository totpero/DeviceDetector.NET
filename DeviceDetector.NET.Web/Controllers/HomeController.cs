using DeviceDetectorNET.Web.Models;
using DeviceDetectorNET.Parser;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace DeviceDetectorNET.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            DeviceDetectorNET.DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

            var userAgent = Request.Headers["User-Agent"];

            //var headers = Request.Headers.ToDictionary(a => a.Key, a => a.Value);

            var result = DeviceDetectorNET.DeviceDetector.GetInfoFromUserAgent(userAgent);

            var output = result.Success ? result.ToString().Replace(Environment.NewLine, "<br />") : "Unknown";

            return View(new IndexModel { Content = output });
        }
    }
}
