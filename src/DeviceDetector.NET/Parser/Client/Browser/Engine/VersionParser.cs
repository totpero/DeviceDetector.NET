using System.Collections.Generic;
using System.Text.RegularExpressions;
using DeviceDetector.NET.Class.Client;
using DeviceDetector.NET.Results;
using DeviceDetector.NET.Results.Client;

namespace DeviceDetector.NET.Parser.Client.Browser.Engine
{
    public class VersionParser : ClientParserAbstract<List<IClientParseLibrary>, ClientMatchResult>
    {
        private readonly string _engine;
        public VersionParser(string ua, string engine):base(ua)
        {
            _engine = engine;
        }

        public override ParseResult<ClientMatchResult> Parse()
        {
            var result = new ParseResult<ClientMatchResult>();
            if (string.IsNullOrEmpty(_engine))
            {
                return result;
            }
            var matches = Regex.Matches(UserAgent,_engine + @"\s*\/?\s*((?(?=\d+\.\d)\d+[.\d]*|\d{1,7}(?=(?:\D|$))))", RegexOptions.IgnoreCase);
            if (matches.Count <= 0) return result;
            foreach (Match match in matches)
            {
                foreach (Group group in match.Groups)
                {
                    if (!match.Value.Equals(group.Value))
                    {
                        result.Add(new ClientMatchResult { Name = group.Value });
                    }
                }
            }
            return result;
            //return base.Parse();
        }
    }
}