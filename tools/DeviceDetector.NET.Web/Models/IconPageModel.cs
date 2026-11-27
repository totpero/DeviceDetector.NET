namespace DeviceDetectorNet.Web.Models
{
    public record DetectedIcon(string Category, string Name, string Url);

    public record IconPageModel(string UserAgent, IReadOnlyList<DetectedIcon> Icons);
}
