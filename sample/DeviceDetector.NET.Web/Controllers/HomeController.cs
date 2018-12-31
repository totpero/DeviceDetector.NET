using System;
using System.Web.Hosting;
using System.Web.Mvc;
using DeviceDetectorNET;

using DeviceDetectorNET.Parser;

namespace DeviceDetector.NET.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            DeviceDetectorNET.DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);
            //DeviceDetectorSettings.RegexesDirectory = Server.MapPath("~/bin/");
            DeviceDetectorSettings.RegexesDirectory = HostingEnvironment.MapPath("~/bin/");

            var userAgent = Request.UserAgent;
            var result = DeviceDetectorNET.DeviceDetector.GetInfoFromUserAgent(userAgent);

            if (!result.Success) return Content("Unknown");

            return Content(result.ToString().Replace(Environment.NewLine, "<br />"));
        }       
    }
}