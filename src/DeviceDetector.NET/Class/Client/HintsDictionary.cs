using DeviceDetectorNET.Results;
using System.Collections.Generic;

namespace DeviceDetectorNET.Class.Client
{
    public class HintsDictionary : Dictionary<string, string>, IMatchResult
    {
        public string Name { get; set; }
    }
}
