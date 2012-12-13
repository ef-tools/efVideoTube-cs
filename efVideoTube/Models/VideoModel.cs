using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace efVideoTube.Models {
    public class VideoModel {
        public string Video { get; set; }
        public Dictionary<string, string> SubtitleLanguages { get; set; }
    }
}