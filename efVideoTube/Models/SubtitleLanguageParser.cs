using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace efVideoTube.Models {
    public static class SubtitleLanguageParser {
        private const string defaultLanguage = sc;
        private const string sc = "zh-cn";
        private const string tc = "zh-tw";
        private const string jp = "ja-jp";
        private const string en = "en-us";

        private static readonly Dictionary<string, string> extLangMaps = new Dictionary<string, string>{
            { ".sc", sc }, { ".chs", sc },
            { ".tc", tc }, { ".cht", tc },
            { ".jp", jp }, { ".jpn", jp },
            { ".en", en }, { ".eng", en },
        };

        public static string Parse(string fileName) {
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            foreach (var extLang in extLangMaps) {
                if (fileNameWithoutExt.EndsWith(extLang.Key, StringComparison.OrdinalIgnoreCase))
                    return extLang.Value;
            }
            return defaultLanguage;
        }
    }
}