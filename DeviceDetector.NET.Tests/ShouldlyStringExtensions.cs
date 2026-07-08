using System;
using Shouldly;

namespace DeviceDetectorNET.Tests
{
    /// <summary>
    /// Case-insensitive string equality assertion, mirroring the semantics of
    /// FluentAssertions' <c>Should().BeEquivalentTo</c> for strings.
    /// </summary>
    internal static class ShouldlyStringExtensions
    {
        public static void ShouldBeIgnoringCase(this string actual, string expected, string customMessage = null)
        {
            if (!string.Equals(actual, expected, StringComparison.OrdinalIgnoreCase))
            {
                // fails with the original values (and optional message) in the Shouldly output
                actual.ShouldBe(expected, customMessage);
            }
        }
    }
}
