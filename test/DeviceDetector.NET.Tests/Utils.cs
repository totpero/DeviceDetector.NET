using System.IO;

namespace DeviceDetectorNET.Tests
{
    public static class Utils
    {
        public static string CurrentDirectory()
        {
            var directoryInfo = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;

#if NETCOREAPP
            directoryInfo = directoryInfo.Parent;
#endif

            return directoryInfo?.FullName ?? "";
        }
    }
}