using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace efVideoTube.Models {
    public static class SubtitleLanguageParser {
        private static readonly CultureInfo en = new CultureInfo("en");
        private static readonly CultureInfo sc = new CultureInfo("zh-hans");
        private static readonly CultureInfo tc = new CultureInfo("zh-hant");
        private static readonly CultureInfo jp = new CultureInfo("ja");

        private static readonly CultureInfo[] langOrder = new CultureInfo[] { sc, tc, jp, en };
        private static readonly Dictionary<string, CultureInfo> extLangMaps = new Dictionary<string, CultureInfo> {
            { ".sc", sc }, { ".chs", sc }, { ".gb", sc },
            { ".tc", tc }, { ".cht", tc }, { ".big5", tc },
            { ".jp", jp }, { ".jpn", jp },
            { ".en", en }, { ".eng", en },
        };

        public static CultureInfo Parse(string fileName) {
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            foreach (var extLang in extLangMaps) {
                if (fileNameWithoutExt.EndsWith(extLang.Key, StringComparison.OrdinalIgnoreCase))
                    return extLang.Value;
            }
            return langOrder.First();
        }

        public static CultureInfo GetDefaultLanguage(this IEnumerable<CultureInfo> langs) {
            foreach (CultureInfo lang in langOrder) {
                if (langs.Contains(lang))
                    return lang;
            }
            return null;
        }
    }
}