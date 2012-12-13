using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace efVideoTube.Models {
    public class ListModel {
        public string Current { get; set; }
        public string[] Folders { get; set; }
        public string[] Files { get; set; }
    }
}