using System.Collections;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace DeviceDetector.NET.Yaml
{
    public class YamlParser<T> : IParser<T>
        where T : class, IEnumerable// IParseLibrary
    {
        public T ParseFile(string file)
        {
            using (var r = new StreamReader(file))
            {
                var deserializer = new DeserializerBuilder().Build();
                var parser = new YamlDotNet.Core.Parser(r);

                // Consume the stream start event "manually"
                parser.Expect<StreamStart>();

                while (parser.Accept<DocumentStart>())
                    // Deserialize the document
                {
                    return deserializer.Deserialize<T>(parser);
                }
                return null;
            }
        }
    }
}
