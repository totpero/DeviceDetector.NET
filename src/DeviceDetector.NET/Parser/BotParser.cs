using System.Collections.Generic;
using System.Text.RegularExpressions;
using DeviceDetector.NET.Class;
using DeviceDetector.NET.Results;

namespace DeviceDetector.NET.Parser
{
    public class BotParser : ParserAbstract<List<Bot>, BotMatchResult>
    {
        public BotParser()
        {
            FixtureFile = "regexes/bots.yml";
            ParserName = "bot";
            regexList = GetRegexes();
            DiscardDetails = true;
        }

        /// <summary>
        /// Enables information discarding
        /// </summary>
        public bool DiscardDetails { get; set; }

        /// <summary>
        /// Parses the current UA and checks whether it contains bot information
        ///
        /// @see bots.yml for list of detected bots
        ///
        /// Step 1: Build a big regex containing all regexes and match UA against it
        /// -> If no matches found: return
        /// -> Otherwise:
        /// Step 2: Walk through the list of regexes in bots.yml and try to match every one
        /// -> Return the matched data
        ///
        /// If $discardDetails is set to TRUE, the Step 2 will be skipped
        /// $bot will be set to TRUE instead
        ///
        /// NOTE: Doing the big match before matching every single regex speeds up the detection
        /// </summary>
        /// <returns></returns>
        public override ParseResult<BotMatchResult> Parse()
        {
            var result = new ParseResult<BotMatchResult>();
            if (PreMatchOverall())
            {
                foreach (var bot in regexList)
                {
                    var match = Regex.Match(UserAgent, bot.Regex, RegexOptions.IgnoreCase);
                    if (!match.Success) continue;
                    if (DiscardDetails)
                    {
                        result.Add(new BotMatchResult());
                        return result;
                    }

                    result.Add(new BotMatchResult
                    {
                        Name = bot.Name,
                        Category = bot.Category,
                        Url = bot.Url,
                        Producer = bot.Producer
                    });
                }
            }

            return result;
        }
    }
}