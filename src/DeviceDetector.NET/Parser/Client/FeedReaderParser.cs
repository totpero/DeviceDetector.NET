using System.Collections.Generic;
using DeviceDetector.NET.Class.Client;
using DeviceDetector.NET.Results.Client;

namespace DeviceDetector.NET.Parser.Client
{
    public class FeedReaderParser : ClientParserAbstract<List<FeedReader>, ClientMatchResult>
    {
        public FeedReaderParser()
        {
            FixtureFile = "regexes/client/feed_readers.yml";
            ParserName = "feed reader";
            regexList = GetRegexes();
        }
    }
}