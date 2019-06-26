using System.Collections.Generic;
using System.Text.RegularExpressions;
using DeviceDetectorNET.Class;
using DeviceDetectorNET.Results;

namespace DeviceDetectorNET.Parser
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public abstract class BotParserAbstract<T, TResult> : ParserAbstract<T, TResult>, IBotParserAbstract
        where T : class, IEnumerable<Bot>
        where TResult : class, IBotMatchResult, new()
    {
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
        public override ParseResult<TResult> Parse()
        {
            var result = new ParseResult<TResult>();
            if (PreMatchOverall())
            {
                foreach (var bot in regexList)
                {
                    var match = Regex.Match(UserAgent, bot.Regex, RegexOptions.IgnoreCase);
                    if (!match.Success) continue;
                    if (DiscardDetails)
                    {
                        result.Add(new TResult());
                        return result;
                    }

                    result.Add(new TResult
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