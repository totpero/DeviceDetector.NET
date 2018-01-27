using System.IO;

namespace DeviceDetector.NET.Tests
{
    public static class Utils
    {
        public static string CurrentDirectory()
        {
            var directoryInfo = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;
            return directoryInfo?.FullName ?? "";
        }
    }
}