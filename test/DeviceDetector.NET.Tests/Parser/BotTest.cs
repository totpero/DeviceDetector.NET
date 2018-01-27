using FluentAssertions;
using Xunit;
using DeviceDetector.NET.Class;
using DeviceDetector.NET.Parser;

namespace DeviceDetector.NET.Tests.Parser
{
    [Trait("Category","Bot")]
    public class BotTest
    {
        [Fact]
        public void TestGetInfoFromUaBot()
        {
            var expected = new Bot
            {

                Name = "Googlebot",
                Category = "Search bot",
                Url = "http://www.google.com/bot.html",
                Producer = new Producer
                {
                    Name = "Google Inc.",
                    Url = "http://www.google.com"
                }
            };

            var botParser = new BotParser {DiscardDetails = false};
            botParser.SetUserAgent("Googlebot/2.1 (http://www.googlebot.com/bot.html)");
            var result = botParser.Parse();
            result.Match.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void TestParseNoDetails()
        {
            var botParser = new BotParser();
            botParser.SetUserAgent("Googlebot/2.1 (http://www.googlebot.com/bot.html)");
            var result = botParser.Parse();

            result.Success.Should().BeTrue();
        }

        [Fact]
        public void TestParseNoBot()
        {
            var botParser = new BotParser();
            botParser.SetUserAgent("Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1; SV1; SE 2.x)");
            var result = botParser.Parse();

            result.Success.Should().BeFalse();
        }
    }
}
