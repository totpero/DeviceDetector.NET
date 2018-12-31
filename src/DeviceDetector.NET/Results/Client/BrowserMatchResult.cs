using System;

namespace DeviceDetectorNET.Results.Client
{
    public class BrowserMatchResult : ClientMatchResult
    {
        public string ShortName { get; set; }
        public string Engine { get; set; }
        public string EngineVersion { get; set; }

        public override string ToString() => 
            base.ToString() +
            $"ShortName: {ShortName}; " +
            $"{Environment.NewLine} " +
            $"Engine: {Engine}; " +
            $"{Environment.NewLine} " +
            $"EngineVersion: {EngineVersion};" ;
    }
}