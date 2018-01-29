namespace DeviceDetectorNET.Parser
{
    public static class VersionTruncation
    {
        /// <summary>
        /// Versioning constant used to set max versioning to major version only
        /// Version examples are: 3, 5, 6, 200, 123, ...
        /// </summary>
        public const int VERSION_TRUNCATION_MAJOR = 0;

        /// <summary>
        /// Versioning constant used to set max versioning to minor version
        /// Version examples are: 3.4, 5.6, 6.234, 0.200, 1.23, ...
        /// </summary>
        public const int VERSION_TRUNCATION_MINOR = 1;

        /// <summary>
        /// Versioning constant used to set max versioning to path level
        /// Version examples are: 3.4.0, 5.6.344, 6.234.2, 0.200.3, 1.2.3, ...
        /// </summary>
        public const int VERSION_TRUNCATION_PATCH = 2;

        /// <summary>
        /// Versioning constant used to set versioning to build number
        /// Version examples are: 3.4.0.12, 5.6.334.0, 6.234.2.3, 0.200.3.1, 1.2.3.0, ...
        /// </summary>
        public const int VERSION_TRUNCATION_BUILD = 3;

        /// <summary>
        /// Versioning constant used to set versioning to unlimited (no truncation)
        /// </summary>
        public const int VERSION_TRUNCATION_NONE = -1;
    }
}
