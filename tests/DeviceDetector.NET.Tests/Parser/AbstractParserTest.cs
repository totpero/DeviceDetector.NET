using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DeviceDetectorNET.Class;
using DeviceDetectorNET.Parser;
using DeviceDetectorNET.RegexEngine;
using DeviceDetectorNET.Results;
using Shouldly;
using Xunit;

namespace DeviceDetectorNET.Tests.Parser
{
    [Trait("Category", "AbstractParser")]
    public class AbstractParserTest
    {
        [Fact]
        public void RegexesForUserAgentReturnsAllRegexesIfNotMoreThanChunkSize()
        {
            var parser = new ChunkParser(2, "foo", "bar");

            parser.RegexesForUserAgent("nothing").ShouldBe(new[] { "foo", "bar" });
        }

        [Fact]
        public void RegexesForUserAgentSkipsChunksBeforeFirstMatchingChunk()
        {
            var parser = new ChunkParser(2, "one", "two", "three", "four", "five");

            parser.RegexesForUserAgent("two; five").ShouldBe(new[] { "one", "two", "three", "four", "five" });
            parser.RegexesForUserAgent("four; five").ShouldBe(new[] { "three", "four", "five" });
            parser.RegexesForUserAgent("five").ShouldBe(new[] { "five" });
            parser.RegexesForUserAgent("six").ShouldBeEmpty();
        }

        [Fact]
        public void RegexesForUserAgentKeepsFirstMatchOfLinearScan()
        {
            var regexes = Enumerable.Range(1, 25).Select(number => $"v{number}(?:[;)]|$)").ToArray();
            var parser = new ChunkParser(4, regexes);

            foreach (var userAgent in new[] { "v1", "v4", "v5", "v13", "v24; v2", "v25", "v7)", "v26" })
            {
                parser.FirstMatch(parser.RegexesForUserAgent(userAgent), userAgent)
                    .ShouldBe(parser.FirstMatch(regexes, userAgent), userAgent);
            }
        }

        [Fact]
        public void RegexesForUserAgentFallsBackToSingleRegexesOnEngineError()
        {
            var parser = new ChunkParser(2, "(?:a+)+$", "nomatch", "other", "b");
            parser.SetRegexEngine(new FailingRegexEngine("(?:a+)+$|nomatch"));

            parser.RegexesForUserAgent(new string('a', 30) + " b")
                .ShouldBe(new[] { "(?:a+)+$", "nomatch", "other", "b" });
        }

        private sealed class ChunkParser : AbstractParser<List<Os>, OsMatchResult>
        {
            public ChunkParser(int chunkSize, params string[] regexes)
            {
                // Unique name for every parser, as cached data is shared between parsers of the same name
                ParserName = "abstractparsertest" + Guid.NewGuid().ToString("N");
                regexList = regexes.Select(regex => new Os { Regex = regex }).ToList();
                regexChunkSize = chunkSize;
            }

            public string[] RegexesForUserAgent(string userAgent)
            {
                SetUserAgent(userAgent);

                return GetRegexesForUserAgent<Os>(item => item.Regex).Select(item => item.Regex).ToArray();
            }

            public string FirstMatch(IEnumerable<string> regexes, string userAgent)
            {
                SetUserAgent(userAgent);

                return regexes.FirstOrDefault(IsMatchUserAgent);
            }
        }

        /// <summary>
        /// Fails with a timeout for patterns containing the given text, like an engine that hits one of its limits
        /// </summary>
        private sealed class FailingRegexEngine : IRegexEngine
        {
            private readonly IRegexEngine _engine = new MSRegexCompiledEngine();
            private readonly string _failingPattern;

            public FailingRegexEngine(string failingPattern)
            {
                _failingPattern = failingPattern;
            }

            public bool Match(string input, string pattern)
            {
                if (pattern.Contains(_failingPattern))
                {
                    throw new RegexMatchTimeoutException(input, pattern, TimeSpan.Zero);
                }

                return _engine.Match(input, pattern);
            }

            public IEnumerable<string> Matches(string input, string pattern) => _engine.Matches(input, pattern);

            public IEnumerable<string> MatchesUniq(string input, string pattern) => _engine.MatchesUniq(input, pattern);

            public string Replace(string input, string pattern, string replacement) =>
                _engine.Replace(input, pattern, replacement);
        }
    }
}
