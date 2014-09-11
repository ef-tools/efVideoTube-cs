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
                "-add \"{0}\"#audio \"{1}\"") },
            { ".webm", new AudioConfig(".webm", ConfigurationManager.AppSettings["mkvtoolnix"],
                "-o \"{1}\" --webm --language 1:eng --default-track 1:yes --forced-track 1:no -a 1 -D -S -T --no-global-tags --no-chapters ( \"{0}\" ) --track-order 0:1") }
        };

        public static bool CanExtract(this string path) {
            return _conversions.ContainsKey(Path.GetExtension(path));
        }

        public static bool Extract(ref string path, string physicalPath) {
            if (!path.CanExtract())
                return false;

            AudioConfig config = _conversions[Path.GetExtension(path)];
            path = "{0}{1}{2}".FormatWith(Global.TempAudioCategory,
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