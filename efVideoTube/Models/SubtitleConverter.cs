using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using PureLib.Common;

namespace efVideoTube.Models {
    public static class SubtitleConverter {
        public const string VttExt = ".vtt";
        public static string[] SupportedSubtitles { get; private set; }

        static SubtitleConverter() {
            SupportedSubtitles = new string[] { ".srt", ".ass", ".ssa" };
        }

        public static bool CanExtract(string path) {
            return SupportedSubtitles.Contains(Path.GetExtension(path));
        }

        public static bool Extract(ref string path, string physicalPath) {
            if (!File.Exists(physicalPath) || !CanExtract(path))
                return false;

            path = "{0}{1}{2}".FormatWith(Global.VideoCacheCategory, Path.DirectorySeparatorChar, path + VttExt);
            string subtitlePhysicalPath;
            string category;
            Global.GetPhysicalPathAndCategory(path, out subtitlePhysicalPath, out category);
            if (File.Exists(subtitlePhysicalPath))
                return true;

            string subtitleFolder = Path.GetDirectoryName(subtitlePhysicalPath);
            if (!Directory.Exists(subtitleFolder))
                Directory.CreateDirectory(subtitleFolder);
            Process process = Process.Start(ConfigurationManager.AppSettings["ass2srt"], ConfigurationManager.AppSettings["ass2srtArgsFormat"].FormatWith(physicalPath, subtitlePhysicalPath));
            process.WaitForExit();
            return process.ExitCode == 0;
        }
    }
}