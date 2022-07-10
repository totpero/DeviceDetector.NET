using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace DeviceDetectorNET.Results
{
    [Serializable]
    [DataContract]
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

        [DataMember]
        public string ParserName { get; set; }

        [DataMember]
        public bool Success { get; private set; }
        public TMatch Match => Success ? Matches.FirstOrDefault() : null;
        [DataMember] 
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
