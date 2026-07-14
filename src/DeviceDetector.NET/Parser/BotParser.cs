using System.Collections.Generic;
using DeviceDetectorNET.Class;
using DeviceDetectorNET.Results;

namespace DeviceDetectorNET.Parser
{
    /// <summary>
    /// Class BotParserAbstract
    /// Abstract class for all bot parsers
    /// </summary>
    public class BotParser : AbstractBotParser<List<Bot>>
    {
        public BotParser()
        {
            FixtureFile = "regexes/bots.yml";
            ParserName = "bot";
            regexList = GetRegexes();
            DiscardDetails = true;
        }  
    }
}