using System.Runtime.Serialization;

namespace DeviceDetectorNET.Results.Client
{
    public class HintsResult: IMatchResult
    {
        [DataMember]
        public virtual string Name { get; set; }

    }
}
