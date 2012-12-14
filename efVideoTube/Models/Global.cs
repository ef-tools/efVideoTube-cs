using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using PureLib.Common;

namespace efVideoTube.Models {
    public static class Global {
        public const string VttExt = ".vtt";
        public static Dictionary<string, string> SupportedVideoTypes { get; private set; }
        public static string[] SupportedSubTypes { get; private set; }
        public static Dictionary<string, string> CategoryPathMaps { get; private set; }

        static Global() {
            SupportedVideoTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
                { ".mp4", "video/mp4" },
                { ".webm", "video/webm" },
            };
            SupportedSubTypes = new string[] { ".srt", ".ass", ".ssa" };

            CategoryPathMaps = new Dictionary<string, string>();
            foreach (string pair in ConfigurationManager.AppSettings["categories"].Split('|')) {
                string[] map = pair.Split(',');
                CategoryPathMaps.Add(map.First(), map.Last());
            }
        }
    }
}