using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceDetectorNET
{
    public class ClientHints
    {
        /// <summary>
        /// Represents `Sec-CH-UA-Arch` header field: The underlying architecture's instruction
        /// </summary>
        public string Architecture { get; set; }

        /// <summary>
        /// Represents `Sec-CH-UA-Bitness` header field: The underlying architecture's bitness
        /// </summary>
        public string Bitness { get; set; }

        /// <summary>
        /// Represents `Sec-CH-UA-Mobile` header field: whether the user agent should receive a specifically "mobile" UX
        /// </summary>
        public bool Mobile { get; set; }

        /// <summary>
        /// Represents `Sec-CH-UA-Model` header field: the user agent's underlying device model
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Represents `Sec-CH-UA-Platform` header field: the platform's brand
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Represents `SSec-CH-UA-Platform-Version` header field: the platform's major version
        /// </summary>
        public string PlatformVersion { get; set; }

        /// <summary>
        /// Represents `Sec-CH-UA-Full-Version` header field: the platform's major version
        /// </summary>
        public string UaFullVersion { get; set; }

        /// <summary>
        /// Represents `Sec-CH-UA-Full-Version-List` header field: the full version for each brand in its brand list
        /// </summary>
        public string[] FullVersionList { get; set; }

        /// <summary>
        /// Represents `x-requested-with` header field: Android app id
        /// </summary>
        public string App { get; set; }
    }
}
