﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace efVideoTube.Models {
    public class Html5VideoModel : VideoModel {
        public Dictionary<string, string> SubtitleLanguages { get; set; }
    }
}