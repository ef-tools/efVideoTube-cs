using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PureLib.AspNet;
using PureLib.Common;

namespace efVideoTube.Controllers {
    public class GateController : Controller {
        public ActionResult Login(string returnUrl) {
            if (Request.CertAuth(ConfigurationManager.AppSettings["issuer"]))
                return returnUrl.IsNullOrEmpty() ? RedirectToAction("Index", "Home") : Redirect(returnUrl) as ActionResult;
            else
                return new HttpStatusCodeResult((int)HttpStatusCode.Unauthorized);
        }
    }
}
