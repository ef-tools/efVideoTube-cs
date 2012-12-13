using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using PureLib.Common;

namespace efVideoTube.Models {
    public static class Global {
        public const string VideoFilter = "*.mp4";
        public const string VttExt = "vtt";
        public static string[] SupportedSubTypes { get; private set; }
        public static Dictionary<string, string> CategoryPathMaps { get; private set; }

        static Global() {
            SupportedSubTypes = new string[] { ".srt", ".ass", ".ssa" };

            CategoryPathMaps = new Dictionary<string, string>();
            foreach (string pair in ConfigurationManager.AppSettings["categories"].Split('|')) {
                string[] map = pair.Split(',');
                CategoryPathMaps.Add(map.First(), map.Last());
            }
        }
    }
}