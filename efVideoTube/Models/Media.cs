using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PureLib.Common;

namespace efVideoTube.Models {
    public class Media {
        public static Dictionary<string, Media> SupportedMedia { get; private set; }
        public static Player[] Players { get; private set; }
        
        public string Extension { get; private set; }
        public string MIME { get; private set; }
        public Player Player { get; private set; }
        public Player[] AvailablePlayers { get; private set; }

        static Media() {
            Players = (Player[])Enum.GetValues(typeof(Player));
            SupportedMedia = new Media[] {
                new Media(".mp4",  "video/mp4",      Player.Html5Video,  Player.Silverlight | Player.Flash),
                new Media(".webm", "video/webm",     Player.Html5Video),
                new Media(".wmv",  "video/x-ms-wmv", Player.Silverlight),
                new Media(".flv",  "video/x-flv",    Player.Flash),
                new Media(".m4a",  "audio/mp4",      Player.Html5Audio,  Player.Silverlight),
                new Media(".mp3",  "audio/mpeg",     Player.Html5Audio,  Player.Silverlight),
            }.ToDictionary(m => m.Extension, m => m, StringComparer.OrdinalIgnoreCase);
        }

        public Media(string extension, string mime, Player player, Player optionalPlayers = Player.None) {
            Extension = extension;
            MIME = mime;
            Player = player;

            Player availablePlayers = (player | optionalPlayers);
            AvailablePlayers = Players.Where(p => availablePlayers.HasFlag(p)).ToArray();
        }
    }
}