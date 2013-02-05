using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace efVideoTube.Models {
    public class Html5VideoModel : MediaModel {
        public Dictionary<string, string> SubtitleLanguages { get; set; }
        public string Previous { get; set; }
        public string Next { get; set; }
    }
}