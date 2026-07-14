using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceDetectorNET.Class
{
    /// <summary>
    /// Faithful port of PHP's <c>version_compare()</c> used throughout device-detector.
    /// Returns -1, 0 or 1 just like PHP (a left-hand version that is lower yields -1).
    /// </summary>
    public static class VersionComparer
    {
        /// <summary>
        /// Compares two version strings the same way PHP's version_compare does.
        /// </summary>
        public static int Compare(string version1, string version2)
        {
            var parts1 = Canonicalize(version1);
            var parts2 = Canonicalize(version2);

            var min = Math.Min(parts1.Count, parts2.Count);
            for (var i = 0; i < min; i++)
            {
                var c = ComparePart(parts1[i], parts2[i]);
                if (c != 0)
                {
                    return c;
                }
            }

            if (parts1.Count == parts2.Count)
            {
                return 0;
            }

            // The shared prefix is equal: a trailing numeric segment makes that side greater,
            // a trailing special form (dev/alpha/beta/rc/pl) is compared against a numeric placeholder.
            if (parts1.Count > parts2.Count)
            {
                return IsDigits(parts1[min]) ? 1 : Math.Sign(FormOrder(parts1[min]).CompareTo(FormOrder("#")));
            }

            return IsDigits(parts2[min]) ? -1 : Math.Sign(FormOrder("#").CompareTo(FormOrder(parts2[min])));
        }

        private static int ComparePart(string a, string b)
        {
            var da = IsDigits(a);
            var db = IsDigits(b);

            if (da && db)
            {
                return CompareNumeric(a, b);
            }

            // FormOrder treats an all-digit segment as the "#" form (order 0), which also covers the
            // mixed numeric/string case exactly like PHP.
            return Math.Sign(FormOrder(a).CompareTo(FormOrder(b)));
        }

        private static int CompareNumeric(string a, string b)
        {
            a = a.TrimStart('0');
            b = b.TrimStart('0');
            if (a.Length != b.Length)
            {
                return a.Length < b.Length ? -1 : 1;
            }

            return Math.Sign(string.CompareOrdinal(a, b));
        }

        private static int FormOrder(string form)
        {
            if (IsDigits(form))
            {
                return 0; // "#"
            }

            switch (form.ToLowerInvariant())
            {
                case "dev": return -6;
                case "alpha":
                case "a": return -5;
                case "beta":
                case "b": return -4;
                case "rc": return -3;
                case "#": return 0;
                case "pl":
                case "p": return 1;
                default: return -7;
            }
        }

        /// <summary>
        /// Splits a version into comparable segments, inserting boundaries between non-alphanumeric
        /// characters and between digit/letter transitions (matching PHP's php_canonicalize_version).
        /// </summary>
        private static List<string> Canonicalize(string version)
        {
            var parts = new List<string>();
            if (string.IsNullOrEmpty(version))
            {
                return parts;
            }

            var sb = new StringBuilder();
            char? prevClass = null; // 'd' = digit, 'a' = alpha

            foreach (var c in version)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    if (sb.Length > 0)
                    {
                        parts.Add(sb.ToString());
                        sb.Clear();
                    }

                    prevClass = null;
                    continue;
                }

                var cls = char.IsDigit(c) ? 'd' : 'a';
                if (prevClass != null && cls != prevClass && sb.Length > 0)
                {
                    parts.Add(sb.ToString());
                    sb.Clear();
                }

                sb.Append(c);
                prevClass = cls;
            }

            if (sb.Length > 0)
            {
                parts.Add(sb.ToString());
            }

            return parts;
        }

        private static bool IsDigits(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            foreach (var c in s)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
