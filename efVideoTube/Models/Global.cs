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
        public static Dictionary<string, string> SupportedMediaTypes { get; private set; }
        public static string[] SupportedSubtitleTypes { get; private set; }
        public static Dictionary<string, string> CategoryPathMaps { get; private set; }

        static Global() {
            SupportedMediaTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
                { ".mp4", "video/mp4" },
                { ".webm", "video/webm" },
                { ".m4a", "audio/mp4" },
            };
            SupportedSubtitleTypes = new string[] { ".srt", ".ass", ".ssa" };

            CategoryPathMaps = new Dictionary<string, string>();
            foreach (string pair in ConfigurationManager.AppSettings["categories"].Split('|')) {
                string[] map = pair.Split(',');
                CategoryPathMaps.Add(map.First(), map.Last());
            }
        }

        public static bool IsVideo(this string path) {
            return SupportedMediaTypes[Path.GetExtension(path)].StartsWith("video/");
        }

        public static object GetRouteValues(this string path) {
            return new { id = Path.GetFileName(path), path = path };
        }

        public static class ActionName {
            public const string Index = "Index";
            public const string Player = "Player";
            public const string Media = "Media";
            public const string Subtitle = "Subtitle";
        }
    }
}