using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace efVideoTube.Models {
    [Flags]
    public enum Player {
        None = 0,
        Html5Video = 1,
        Html5Audio = 2,
        Silverlight = 4,
        Flash = 8,
    }
}