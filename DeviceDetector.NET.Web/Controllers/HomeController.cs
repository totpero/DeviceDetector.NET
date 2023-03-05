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
            DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

            var userAgent = Request.Headers["User-Agent"];

            //var headers = Request.Headers.ToDictionary(a => a.Key, a => a.Value);

            var result = DeviceDetector.GetInfoFromUserAgent(userAgent);

            var output = result.Success ? result.ToString().Replace(Environment.NewLine, "<br />") : "Unknown";

            return View(new IndexModel { Content = output });
        }
    }
}
