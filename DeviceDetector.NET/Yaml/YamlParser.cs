using System.Collections;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace DeviceDetectorNET.Yaml
{
    public class YamlParser<T> : IParser<T>
        where T : class, IEnumerable// IParseLibrary
    {
        public T ParseFile(string file)
        {
            using (var r = new StreamReader(file))
            {
                return ParseStreamReader(r);
            }
        }

        public T ParseStream(Stream stream)
        {
            using (var r = new StreamReader(stream))
            {
                return ParseStreamReader(r);
            }
        }

        private T ParseStreamReader(StreamReader streamReader)
        {
            var deserializer = new DeserializerBuilder().Build();
            var parser = new YamlDotNet.Core.Parser(streamReader);

            // Consume the stream start event "manually"
            parser.TryConsume<StreamStart>(out _);

            while (parser.TryConsume<DocumentStart>(out _))
            // Deserialize the document
            {
                var type = typeof(T);
                return deserializer.Deserialize<T>(parser);
            }

            return null;
        }
    }
}
