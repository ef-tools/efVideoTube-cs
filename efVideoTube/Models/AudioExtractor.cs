using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using PureLib.Common;

namespace efVideoTube.Models {
    public static class AudioExtractor {
        private static Dictionary<string, AudioConfig> _conversions = new Dictionary<string, AudioConfig>(StringComparer.OrdinalIgnoreCase) {
            { ".mp4", new AudioConfig(".m4a", ConfigurationManager.AppSettings["mp4box"],
                ConfigurationManager.AppSettings["mp4boxArgsFormat"]) },
            { ".webm", new AudioConfig(".webm", ConfigurationManager.AppSettings["mkvtoolnix"],
                ConfigurationManager.AppSettings["mkvtoolnixArgsFormat"]) }
        };

        public static bool CanExtract(string path) {
            return _conversions.ContainsKey(Path.GetExtension(path));
        }

        public static bool Extract(ref string path, string physicalPath) {
            if (!File.Exists(physicalPath) || !CanExtract(path))
                return false;

            AudioConfig config = _conversions[Path.GetExtension(path)];
            path = "{0}{1}{2}".FormatWith(Global.VideoCacheCategory,
                Path.DirectorySeparatorChar, Path.ChangeExtension(path, config.DestExt));
            string audioPhysicalPath;
            string category;
            Global.GetPhysicalPathAndCategory(path, out audioPhysicalPath, out category);
            if (File.Exists(audioPhysicalPath))
                return true;

            string audioFolder = Path.GetDirectoryName(audioPhysicalPath);
            if (!Directory.Exists(audioFolder))
                Directory.CreateDirectory(audioFolder);
            Process process = Process.Start(config.Muxer, config.MuxerArgsFormat.FormatWith(physicalPath, audioPhysicalPath));
            process.WaitForExit();
            return process.ExitCode == 0;
        }

        private class AudioConfig {
            public string DestExt { get; private set; }
            public string Muxer { get; private set; }
            public string MuxerArgsFormat { get; private set; }

            public AudioConfig(string destExt, string muxer, string muxerArgsFormat) {
                DestExt = destExt;
                Muxer = muxer;
                MuxerArgsFormat = muxerArgsFormat;
            }
        }
    }
}