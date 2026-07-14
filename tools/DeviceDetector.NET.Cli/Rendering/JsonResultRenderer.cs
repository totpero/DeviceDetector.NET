using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using DeviceDetector.Net.Cli.Models;

namespace DeviceDetector.Net.Cli.Rendering
{
    public class JsonResultRenderer : IResultRenderer
    {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public void Render(IReadOnlyList<DetectionResult> results, string outputPath)
        {
            var json = JsonSerializer.Serialize(results, Options);

            if (string.IsNullOrEmpty(outputPath))
            {
                Console.WriteLine(json);
            }
            else
            {
                File.WriteAllText(outputPath, json);
            }
        }
    }
}
