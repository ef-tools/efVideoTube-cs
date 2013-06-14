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
        public static bool CanExtract(this string path) {
            return Path.GetExtension(path).Equals(".mp4", StringComparison.OrdinalIgnoreCase);
        }

        public static bool Extract(ref string path, string physicalPath) {
            if (!path.CanExtract())
                return false;

            path = Path.ChangeExtension(Global.TempAudioCategory 
                + path.Substring(path.IndexOf(Path.DirectorySeparatorChar)), ".m4a");
            string audioPhysicalPath;
            string category;
            Global.GetPhysicalPathAndCategory(path, out audioPhysicalPath, out category);
            if (File.Exists(audioPhysicalPath))
                return true;

            string audioFolder = Path.GetDirectoryName(audioPhysicalPath);
            if (!Directory.Exists(audioFolder))
                Directory.CreateDirectory(audioFolder);
            Process process = Process.Start(ConfigurationManager.AppSettings["mp4box"],
                "-add \"{0}\"#2 \"{1}\"".FormatWith(physicalPath, audioPhysicalPath));
            process.WaitForExit();
            return process.ExitCode == 0;
        }
    }
}