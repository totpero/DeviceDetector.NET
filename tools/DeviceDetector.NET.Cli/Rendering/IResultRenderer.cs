using System.Collections.Generic;
using DeviceDetector.Net.Cli.Models;

namespace DeviceDetector.Net.Cli.Rendering
{
    public interface IResultRenderer
    {
        void Render(IReadOnlyList<DetectionResult> results, string outputPath);
    }
}
