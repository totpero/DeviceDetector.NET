using System.Collections;

namespace DeviceDetectorNET.Yaml
{
    public interface IParser<T>
        where T : IEnumerable //IParseLibrary
    {
        T ParseFile(string file);
    }
}
