using System.Collections.Generic;
using System.Linq;

namespace DeviceDetectorNET.Results
{
    public class ParseResult<TMatch>
        where TMatch : class
    {
        public ParseResult()
        {
            Success = false;
            Matches = new List<TMatch>();
        }

        public ParseResult(TMatch match, bool success = true)
            :this()
        {
            Matches.Add(match);
            Success = success;
        }

        public bool Success { get; private set; }
        public TMatch Match => Success ? Matches.FirstOrDefault() : null;
        public List<TMatch> Matches { get; set; }

        public ParseResult<TMatch> Add(TMatch match)
        {
            Matches.Add(match);
            Success = true;
            return this;
        }

        public ParseResult<TMatch> AddRange(IEnumerable<TMatch> matches)
        {
            Matches.AddRange(matches);
            Success = true;
            return this;
        }

        public override string ToString() => Success ? Match.ToString() : "No matches!";
    }
}
