﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace efVideoTube.Models {
    public static class SubtitleLanguageParser {
        private const string en = "English";
        private const string sc = "简体中文";
        private const string tc = "繁體中文";
        private const string jp = "日本語";

        private static readonly string[] langOrder = new string[] { sc, tc, jp, en };
        private static readonly Dictionary<string, string> extLangMaps = new Dictionary<string, string> {
            { ".sc", sc }, { ".chs", sc }, { ".gb", sc },
            { ".tc", tc }, { ".cht", tc }, { ".big5", tc },
            { ".jp", jp }, { ".jpn", jp },
            { ".en", en }, { ".eng", en },
        };

        public static string Parse(string fileName) {
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            foreach (var extLang in extLangMaps) {
                if (fileNameWithoutExt.EndsWith(extLang.Key, StringComparison.OrdinalIgnoreCase))
                    return extLang.Value;
            }
            return langOrder.First();
        }

        public static string GetDefaultLanguage(this IEnumerable<string> langs) {
            foreach (string lang in langOrder) {
                if (langs.Contains(lang))
                    return lang;
            }
            return null;
        }
    }
}