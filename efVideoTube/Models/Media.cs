using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PureLib.Common;

namespace efVideoTube.Models {
    public class Media {
        public static Dictionary<string, Media> SupportedMedia { get; private set; }
        
        public string Extension { get; private set; }
        public string MIME { get; private set; }
        public VideoPlayer Player { get; private set; }

        static Media() {
            Media[] media = new Media[] {
                new Media(".mp4",  "video/mp4",      VideoPlayer.Html5),
                new Media(".webm", "video/webm",     VideoPlayer.Html5),
                new Media(".flv",  "video/x-flv",    VideoPlayer.Flash),
                new Media(".wmv",  "video/x-ms-wmv", VideoPlayer.Silverlight),
                new Media(".m4a",  "audio/mp4",      VideoPlayer.None),
            };
            SupportedMedia = media.ToDictionary(m => m.Extension, m => m, StringComparer.OrdinalIgnoreCase);
        }

        public Media(string extension, string mime, VideoPlayer player) {
            Extension = extension;
            MIME = mime;
            Player = player;
        }
    }
}