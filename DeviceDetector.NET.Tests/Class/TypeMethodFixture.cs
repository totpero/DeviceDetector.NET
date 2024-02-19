using System;

namespace DeviceDetectorNET.Tests.Class;

public class TypeMethodFixture
{
    public string user_agent { get; set; }
    public Tuple<bool,bool,bool,bool,bool,bool> check { get; set; }
}