using System.Collections;

namespace DeviceDetectorNET
{
    public static class IEnumerableExtensions
    {
        public static bool Any(this IEnumerable source)
        {
            if (source == null) return false;
            var e = source.GetEnumerator();
            return e.MoveNext();
        }
        public static int Count(this IEnumerable source)
        {
            switch (source)
            {
                case null:
                    return 0;
                case ICollection col:
                    return col.Count;
            }

            var c = 0;
            var e = source.GetEnumerator();
            while (e.MoveNext())
                c++;
            return c;
        }
    }
}