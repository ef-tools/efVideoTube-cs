using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace efVideoTube.Models {
    [Flags]
    public enum VideoPlayer {
        None = 0,
        Html5 = 1,
        Silverlight = 2,
        Flash = 4,
    }
}