using System.IO;
using System.Reflection;

namespace DeviceDetectorNET.Tests
{
    public static class Utils
    {
        public static string CurrentDirectory()
        {
            var location = typeof(DeviceDetectorTest).GetTypeInfo().Assembly.Location;
            var directory = Path.GetDirectoryName(location);

            return directory ?? "";
        }
    }
}