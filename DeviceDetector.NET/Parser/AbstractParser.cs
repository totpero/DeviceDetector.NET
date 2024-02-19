using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DeviceDetectorNET.Cache;
using DeviceDetectorNET.Class;
using DeviceDetectorNET.Class.Device;
using DeviceDetectorNET.RegexEngine;
using DeviceDetectorNET.Results;
using DeviceDetectorNET.Yaml;

namespace DeviceDetectorNET.Parser
{
    public abstract class AbstractParser<T, TResult>: IAbstractParser<TResult>
        where T : class, IEnumerable
//, IParseLibrary
        where TResult : class, IMatchResult, new()
    {
        /// <summary>
        /// Holds the path to the yml file containing regexes
        /// </summary>
        public string FixtureFile { get; protected set; }

        /// <summary>
        /// Holds the internal name of the parser
        /// Used for caching
        /// </summary>
        public string ParserName { get; protected set; }

        /// <summary>
        /// Holds the user agent the should be parsed
        /// </summary>
        public string UserAgent { get; private set; }

        /// <summary>
        /// Holds the client hints to be parsed
        /// </summary>
        public ClientHints ClientHints { get; protected set; }

        /// <summary>
        /// Contains a list of mappings from names we use to known client hint values
        /// </summary>
        public virtual Dictionary<string, string[]> ClientHintMapping { get; }

        /// <summary>
        /// Holds an array with method that should be available global
        /// </summary>
        protected string[] globalMethods;

        /// <summary>
        /// Holds an array with regexes to parse, if already loaded
        /// </summary>
        protected T regexList;

        /// <summary>
        /// Holds the concatenated regex for all items in regex list
        /// </summary>
        //protected List<T> overAllMatch;

        /// <summary>
        /// Indicates how deep versioning will be detected
        /// if $maxMinorParts is 0 only the major version will be returned
        /// </summary>
        protected static int maxMinorParts = -1;

        /// <summary>
        /// Versioning constant used to set max versioning to major version only
        /// Version examples are: 3, 5, 6, 200, 123, ...
        /// </summary>
        const int VERSION_TRUNCATION_MAJOR = 0;

        /// <summary>
        /// Versioning constant used to set max versioning to minor version
        /// Version examples are: 3.4, 5.6, 6.234, 0.200, 1.23, ...
        /// </summary>
        const int VERSION_TRUNCATION_MINOR = 1;

        /// <summary>
        /// Versioning constant used to set max versioning to path level
        /// Version examples are: 3.4.0, 5.6.344, 6.234.2, 0.200.3, 1.2.3, ...
        /// </summary>
        const int VERSION_TRUNCATION_PATCH = 2;

        /// <summary>
        /// Versioning constant used to set versioning to build number
        /// Version examples are: 3.4.0.12, 5.6.334.0, 6.234.2.3, 0.200.3.1, 1.2.3.0, ...
        /// </summary>
        const int VERSION_TRUNCATION_BUILD = 3;

        /// <summary>
        /// Versioning constant used to set versioning to unlimited (no truncation)
        /// </summary>
        public const int VERSION_TRUNCATION_NONE = -1;

        protected ICache Cache;

        protected IParser<T> YamlParser;

        protected IRegexEngine RegexEngine;

        public virtual ParseResult<TResult> Parse()
        {
            throw new NotImplementedException();
        }

        protected AbstractParser()
        {
            //regexList = new IEnumerable<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ua"></param>
        /// <param name="clientHints"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected AbstractParser(string ua = "", ClientHints clientHints = null)
        {
            //if (string.IsNullOrEmpty(ua)) throw new ArgumentNullException(nameof(ua));
            UserAgent = ua;
            if (clientHints != null)
            {
                ClientHints = clientHints;
            }
            //regexList = new List<T>();
        }

        /// <summary>
        /// Set how DeviceDetector should return versions
        /// </summary>
        /// <param name="type">Any of the VERSION_TRUNCATION_* constants</param>
        public static void SetVersionTruncation(int type)
        {
            var versions = new List<int> {
                VERSION_TRUNCATION_BUILD,
                VERSION_TRUNCATION_NONE,
                VERSION_TRUNCATION_MAJOR,
                VERSION_TRUNCATION_MINOR,
                VERSION_TRUNCATION_PATCH
            };
            if (versions.Contains(type))
            {
                maxMinorParts = type;
            }
        }

        /// <summary>
        /// Sets the user agent to parse
        /// </summary>
        /// <param name="ua"></param>
        public virtual void SetUserAgent(string ua)
        {
            //if (string.IsNullOrEmpty(ua)) throw new ArgumentNullException(nameof(ua));
            UserAgent = ua ?? string.Empty;
        }

        /// <summary>
        /// Sets the client hints to parse
        /// </summary>
        /// <param name="clientHints"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual void SetClientHints(ClientHints clientHints)
        {
            if (clientHints != null)
            {
                ClientHints = clientHints;
            }
        }

        /// <summary>
        /// Returns the internal name of the parser
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return ParserName;
        }

        /// <summary>
        /// Returns the result of the parsed yml file defined in $fixtureFile
        /// </summary>
        /// <returns></returns>
        protected T GetRegexes()
        {
            if (regexList.Any()) return regexList;
            var cacheKey = "DeviceDetector-" + DeviceDetector.VERSION + "regexes-" + GetName();
            //@todo:option none
            cacheKey = GetRegexEngine().Replace(cacheKey, "/([^a-z0-9_-]+)/i", string.Empty);
            var regexListCache = GetCache().Fetch(cacheKey);
            if (regexListCache != null)
            {
                regexList = (T)regexListCache;
            }
            if (regexList.Any()) return regexList;

            var regexesDir = GetRegexesDirectory();

            if (string.IsNullOrWhiteSpace(regexesDir))
            {
                var assembly = typeof(DeviceDetector).GetTypeInfo().Assembly;
                var filePath = FixtureFile.Replace("/", ".");
                var fullPath = $"{nameof(DeviceDetectorNET)}.{filePath}";

                using (Stream resource = assembly.GetManifestResourceStream(fullPath))
                {
                    regexList = GetYamlParser().ParseStream(resource);
                }
            }
            else
            {
                regexList = GetYamlParser().ParseFile(
                    regexesDir + FixtureFile
                );
            }

            GetCache().Save(cacheKey, regexList);
            return regexList;
        }

        /// <summary>
        /// Returns the provided name after applying client hint mappings.
        /// This is used to map names provided in client hints to the names we use.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string ApplyClientHintMapping(string name)
        {
            name = name.ToLower();

            foreach (var clientHints in ClientHintMapping)
            {
                foreach (var clientHint in clientHints.Value)
                {
                    if (name == clientHint.ToLower())
                    {
                        return clientHints.Key;
                    }
                }
            }

            return name;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected string GetRegexesDirectory()
        {
            return DeviceDetectorSettings.RegexesDirectory;
        }

        /// <summary>
        /// Matches the useragent against the given regex
        /// </summary>
        /// <param name="regex"></param>
        /// <returns></returns>
        protected bool IsMatchUserAgent(string regex)
        {
            // only match if useragent begins with given regex or there is no letter before it
            var match = GetRegexEngine().Match(UserAgent, FixUserAgentRegEx(regex));
            return match;
        }

        protected string[] MatchUserAgent(string regex)
        {
            // only match if useragent begins with given regex or there is no letter before it
            var match = GetRegexEngine().Matches(UserAgent, FixUserAgentRegEx(regex)).ToArray();
            return match;
        }

        private string FixUserAgentRegEx(string regex)
        {
            var cleanedRegex = regex.Replace("/", @"\/").Replace("++", "+").Replace(@"\_", "_").Replace(@"\_", @"\\_");
            // only match if useragent begins with given regex or there is no letter before it
            var result = $@"(?:^|[^A-Z0-9_-]|[^A-Z0-9-]_|sprd-|MZ-)(?:{cleanedRegex})";
            return result;
        }

        protected string BuildByMatch(string item, string[] matches)
        {
            var maxMatches = Math.Min(3, matches.Count() - 1);
            for (var nb = 1; nb <= maxMatches; nb++)
            {
                if (!item.Contains("$" + nb))
                {
                    continue;
                }

                var replace = matches[nb] ?? string.Empty;
                item = item.Replace("$"+nb, replace).Trim();
            }
            return item;
        }

        /// <summary>
        /// Builds the version with the given $versionString and $matches
        /// Example:
        /// $versionString = 'v$2'
        /// $matches = array('version_1_0_1', '1_0_1')
        /// return value would be v1.0.1
        /// </summary>
        /// <param name="versionString"></param>
        /// <param name="matches"></param>
        protected string BuildVersion(string versionString, string[] matches)
        {
            versionString = BuildByMatch(versionString ?? string.Empty, matches);
            versionString = versionString.Replace("_", ".").TrimEnd('.');

            var versionParts = versionString.Split('.');

            if (-1 == maxMinorParts || versionParts.Length - 1 <= maxMinorParts) return versionString;
            var newVersionParts = new string[1 + maxMinorParts];
            Array.Copy(versionParts, 0, newVersionParts, 0, newVersionParts.Length);
            versionString = string.Join(".", newVersionParts);
            return versionString;
        }

        /// <summary>
        /// Tests the useragent against a combination of all regexes
        ///
        /// All regexes returned by getRegexes() will be reversed and concated with '|'
        /// Afterwards the big regex will be tested against the user agent
        ///
        /// Method can be used to speed up detections by making a big check before doing checks for every single regex
        /// </summary>
        /// <returns></returns>
        protected bool PreMatchOverall()
        {
            var regexes = GetRegexes();

            var cacheKey = ParserName + DeviceDetector.VERSION + "-all";
            //@todo: default none
            cacheKey = GetRegexEngine().Replace(cacheKey, "/([^a-z0-9_-]+)/i", string.Empty);

            var regexListCache = GetCache().Fetch(cacheKey);
            string overAllMatch = regexListCache?.ToString() ?? string.Empty;


            if (string.IsNullOrEmpty(overAllMatch))
            {
                List<IParseLibrary> parses = new List<IParseLibrary>();
                // reverse all regexes, so we have the generic one first, which already matches most patterns
                if (regexes is IDictionary)
                {
                    var devices = regexes.Cast<KeyValuePair<string, DeviceModel>>().Select(d => d.Value).ToList();
                    parses.AddRange(devices.OfType<IParseLibrary>());
                    var models = devices.Where(e => e.Models != null).SelectMany(m => m.Models);
                    parses.AddRange(models.OfType<IParseLibrary>());
                }
                else
                {
                    parses = regexes.OfType<IParseLibrary>().ToList();
                }

                if (parses.Any())
                {
                    parses.Reverse();

                    overAllMatch = string.Join("|", parses.Where(p => !string.IsNullOrEmpty(p.Regex)).Select(r => r.Regex));
                }

                GetCache().Save(cacheKey, overAllMatch);
            }
            return IsMatchUserAgent(overAllMatch);
        }

        /// <summary>
        /// Sets the Cache class
        /// </summary>
        /// <param name="cacheProvider"></param>
        public void SetCache(ICache cacheProvider)
        {
            Cache = cacheProvider;
        }

        /// <summary>
        /// Returns Cache object
        /// </summary>
        /// <returns></returns>
        public ICache GetCache()
        {
            if (Cache != null)
            {
                return Cache;
            }
            Cache = new DictionaryCache();
            return Cache;
        }

        public void SetYamlParser(IParser<T> yaml)
        {
            if (!(yaml is YamlParser<T>)) throw new Exception("Yaml Parser not supported");
            YamlParser = yaml;
        }

        public IParser<T> GetYamlParser()
        {
            return YamlParser ?? (YamlParser = new YamlParser<T>());
        }

        public void SetRegexEngine(IRegexEngine regexEngine)
        {
            RegexEngine = regexEngine ?? throw new ArgumentNullException(nameof(regexEngine));
        }

        public IRegexEngine GetRegexEngine()
        {
            return RegexEngine ?? (RegexEngine = new MSRegexCompiledEngine());
        }

        /// <summary>
        /// Compares if two strings equals after lowering their case and removing spaces
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        protected bool FuzzyCompare(string value1, string value2)
        {
            return value1.Replace(" ", "").ToLower() == value2.Replace(" ", "").ToLower();
        }
    }
}
