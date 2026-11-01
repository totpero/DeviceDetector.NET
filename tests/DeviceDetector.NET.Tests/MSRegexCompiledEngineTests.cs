using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DeviceDetectorNET.RegexEngine;
using Xunit;

namespace DeviceDetectorNET.Tests;

public class MSRegexCompiledEngineTests
{
    [Theory]
    [InlineData(null, "[")]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData("", "[")]
    [InlineData("", "^$")]
    public void EmptyInputDoesNotEvaluatePattern(string input, string pattern)
    {
        Assert.False(new MSRegexCompiledEngine().Match(input, pattern));
    }

    [Theory]
    [InlineData("Mozilla/5.0 Chrome/140.0", @"chrome/(\d+)\.\d+")]
    [InlineData("Firefox/140.0", @"chrome/(\d+)\.\d+")]
    [InlineData("abc\n", "^abc$")]
    public void MatchAgreesWithStaticRegex(string input, string pattern)
    {
        Assert.Equal(Regex.Match(input, pattern, RegexOptions.IgnoreCase).Success,
            new MSRegexCompiledEngine().Match(input, pattern));
    }

    [Fact]
    public void InvalidPatternStillThrows()
    {
        Assert.ThrowsAny<ArgumentException>(() => new MSRegexCompiledEngine().Match("nonempty", "["));
    }

    [Fact]
    public void NullPatternNamesThePublicPatternArgument()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new MSRegexCompiledEngine().Match("nonempty", null));
        Assert.Equal("pattern", exception.ParamName);
    }

    [Theory]
    [InlineData("en-US", true)]
    [InlineData("en-US", false)]
    [InlineData("tr-TR", true)]
    [InlineData("tr-TR", false)]
    public void MatchAndCapturesShareTheExistingCompiledRegex(string firstCulture, bool matchFirst)
    {
        var previous = CultureInfo.CurrentCulture;
        var engine = new MSRegexCompiledEngine();
        var pattern = "^Isharedcachetest$(?#" + Guid.NewGuid().ToString("N") + ")";
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(firstCulture);
            var expected = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            if (matchFirst)
                Assert.True(engine.Match("Isharedcachetest", pattern));
            else
                Assert.Equal(new[] { "Isharedcachetest" }, engine.Matches("Isharedcachetest", pattern));
            foreach (var culture in new[] { "en-US", "tr-TR", "en-US", "tr-TR" })
            {
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(culture);
                foreach (var input in new[] { "isharedcachetest", "ısharedcachetest", "Isharedcachetest" })
                {
                    var success = expected.IsMatch(input);
                    Assert.Equal(success, engine.Match(input, pattern));
                    Assert.Equal(success ? new[] { input } : Array.Empty<string>(), engine.Matches(input, pattern));
                }
            }
        }
        finally { CultureInfo.CurrentCulture = previous; }
    }

    [Fact]
    public void ConcurrentRequestsUseTheExistingCompiledRegex()
    {
        var pattern = "^Isharedconcurrenttest$(?#" + Guid.NewGuid().ToString("N") + ")";
        var previousCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            Assert.True(new MSRegexCompiledEngine().Match("Isharedconcurrenttest", pattern));
        }
        finally { CultureInfo.CurrentCulture = previousCulture; }
        Parallel.For(0, 16, worker =>
        {
            var previous = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(worker % 2 == 0 ? "en-US" : "tr-TR");
                var engine = new MSRegexCompiledEngine();
                for (int repeat = 0; repeat < 50; repeat++)
                {
                    Assert.True(engine.Match("isharedconcurrenttest", pattern));
                    Assert.False(engine.Match("ısharedconcurrenttest", pattern));
                }
            }
            finally { CultureInfo.CurrentCulture = previous; }
        });
    }
}
