using DeviceDetectorNET.Results.Client;
using DeviceDetectorNET.Results.Device;
using System;
using System.Runtime.Serialization;

namespace DeviceDetectorNET.Results
{
    [Serializable]
    [DataContract]
    public class DeviceDetectorResult
    {
        public DeviceDetectorResult()
        {
            OsFamily = "Unknown";
            BrowserFamily = "Unknown";
        }
        [DataMember]
        public string UserAgent { get; set; }
        [DataMember]
        public BotMatchResult Bot { get; set; }
        [DataMember]
        public OsMatchResult Os { get; set; }
        [DataMember]
        public ClientMatchResult Client { get; set; }
        [DataMember]
        public DeviceMatchResult Device { get; set; }
        [DataMember]
        public string DeviceType { get; set; }
        [DataMember]
        public string DeviceBrand { get; set; }
        [DataMember]
        public string DeviceModel { get; set; }

        [DataMember]
        public string OsFamily { get; set; }
        [DataMember]
        public string BrowserFamily { get; set; }

        public override string ToString() =>
        $"UserAgent: {UserAgent}; " +
        $"{Environment.NewLine} " +
        $"Device: {Device}" +
        $"DeviceType: {DeviceType}" +
        $"{Environment.NewLine} " +
        $"DeviceBrand: {DeviceBrand}" +
        $"{Environment.NewLine} " +
        $"DeviceModel: {DeviceModel}" +
        $"{Environment.NewLine} " +
        $"BrowserFamily: {BrowserFamily}" +
        $"{Environment.NewLine} " +
        $"Bot: {Bot}" +
        $"{Environment.NewLine} " +
        $"Os: {Os}" +
        $"{Environment.NewLine} " +
        $"Client: {Client}";
    }
}