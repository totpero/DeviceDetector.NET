using DeviceDetectorNET.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceDetectorNET.Class.Client
{
    public class HintsDictionary : Dictionary<string, string>, IMatchResult
    {
        public string Name { get; set; }
    }
}
