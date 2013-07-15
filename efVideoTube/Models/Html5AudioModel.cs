using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace efVideoTube.Models {
    public class Html5AudioModel : MediaModel {
        public string Parent { get; set; }
        public string[] List { get; set; }
    }
}