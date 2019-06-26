using DeviceDetectorNET.Results.Client;
using System;

namespace DeviceDetectorNET.Results
{
    public class DeviceDetectorResult
    {
        public DeviceDetectorResult()
        {
            OsFamily = "Unknown";
            BrowserFamily = "Unknown";
        }
        public string UserAgent { get; set; }
        public BotMatchResult Bot { get; set; }
        public OsMatchResult Os { get; set; }
        public ClientMatchResult Client { get; set; }
        public string DeviceType { get; set; }
        public string DeviceBrand { get; set; }
        public string DeviceModel { get; set; }
        public string OsFamily { get; set; }
        public string BrowserFamily { get; set; }

        public override string ToString() =>
        $"UserAgent: {UserAgent}; " +
        $"{Environment.NewLine} " +
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