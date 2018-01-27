using System.Collections;

namespace DeviceDetector.NET.Yaml
{
    public interface IParser<T>
        where T : IEnumerable //IParseLibrary
    {
        T ParseFile(string file);
    }
}
