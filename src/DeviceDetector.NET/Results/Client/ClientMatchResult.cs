using System;

namespace DeviceDetectorNET.Results.Client
{
    public class ClientMatchResult:IClientMatchResult
    {
        public virtual string Type { get; set; }
        public virtual string Name { get; set; }
        public string Version { get; set; }

        public override string ToString() =>
          $"Type: {Type}; " +
          $"{Environment.NewLine} " +
          $"Name: {Name};" +
          $"{Environment.NewLine} " +
          $"Version: {Version};" +
          $"{Environment.NewLine} ";
    }
}