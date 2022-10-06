using System.Text;
using System.Text.RegularExpressions;

namespace X.Sitemap.Internals {
    internal static class StringHelper {
        private static readonly Regex _HiddenChars = new("[^\u0009\u000A\u000D\u0020-\uD7FF\uE000-\uFFFD]", RegexOptions.Compiled);
        internal static Encoding Utf8WithoutBom { get; } = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

        /// <summary>Remove control characters from string.</summary>
        internal static string RemoveHiddenChars(this string input) {
            return _HiddenChars.Replace(input, replacement: string.Empty);
        }
    }
}
