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
            return new { path = path };
        }

        public static string GetMediaUrl(this HttpRequestBase request, string path) {
            return new Uri(new Uri("{0}://{1}{2}/".FormatWith(request.Url.Scheme,
                request.Url.Authority, request.ApplicationPath.TrimEnd('/'))), path).AbsolutePath;
        }

        public static class ActionName {
            public const string Index = "Index";
            public const string Player = "Player";
            public const string Subtitle = "Subtitle";
        }
    }
}