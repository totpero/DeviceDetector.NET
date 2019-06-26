using DeviceDetectorNET.Web.Models;
using DeviceDetectorNET.Parser;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DeviceDetectorNET.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            DeviceDetectorNET.DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

            var userAgent = Request.Headers["User-Agent"];
            var result = DeviceDetectorNET.DeviceDetector.GetInfoFromUserAgent(userAgent);

            var output = result.Success ? result.ToString().Replace(Environment.NewLine, "<br />") : "Unknown";

            return View(new IndexModel { Content = output });
        }
    }
}
