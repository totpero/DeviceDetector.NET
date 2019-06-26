using System.Collections;
using System.IO;

namespace DeviceDetectorNET.Yaml
{
    public interface IParser<T>
        where T : IEnumerable //IParseLibrary
    {
        T ParseFile(string file);
        T ParseStream(Stream steam);
    }
}
