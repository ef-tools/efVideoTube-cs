using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PureLib.Common;

namespace efVideoTube.Models {
    public class Media {
        public static Dictionary<string, Media> SupportedMedia { get; private set; }
        public static VideoPlayer[] Players { get; private set; }
        
        public string Extension { get; private set; }
        public string MIME { get; private set; }
        public VideoPlayer Player { get; private set; }
        public VideoPlayer[] AvailablePlayers { get; private set; }

        static Media() {
            Media[] media = new Media[] {
                new Media(".mp4",  "video/mp4",      VideoPlayer.Html5,       VideoPlayer.Silverlight | VideoPlayer.Flash),
                new Media(".webm", "video/webm",     VideoPlayer.Html5),
                new Media(".wmv",  "video/x-ms-wmv", VideoPlayer.Silverlight),
                new Media(".flv",  "video/x-flv",    VideoPlayer.Flash),
                new Media(".m4a",  "audio/mp4",      VideoPlayer.Silverlight),
            };
            SupportedMedia = media.ToDictionary(m => m.Extension, m => m, StringComparer.OrdinalIgnoreCase);
            Players = (VideoPlayer[])Enum.GetValues(typeof(VideoPlayer));
        }

        public Media(string extension, string mime, VideoPlayer player, VideoPlayer optionalPlayers = VideoPlayer.None) {
            Extension = extension;
            MIME = mime;
            Player = player;

            VideoPlayer availablePlayers = (player | optionalPlayers);
            AvailablePlayers = Players.Where(p => availablePlayers.HasFlag(p)).ToArray();
        }
    }
}