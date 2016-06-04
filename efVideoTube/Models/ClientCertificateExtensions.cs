using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using PureLib.Common;

namespace efVideoTube.Models {
    public static class ClientCertificateExtensions {
        public static bool CertAuth(this HttpRequestBase request, string issuer) {
            string userName = ParseUserName(request.ClientCertificate, issuer);
            if (userName.IsNullOrEmpty())
                return false;

            FormsAuthentication.SetAuthCookie(userName, false, request.ApplicationPath);
            return true;
        }

        private static string ParseUserName(HttpClientCertificate cert, string issuer) {
            if (Debugger.IsAttached)
                return "Debug";

            if (cert != null && cert.IsValid && !cert.Subject.IsNullOrEmpty() && (issuer.IsNullOrEmpty() ||
                    cert.BinaryIssuer.ToHexString().Equals(issuer, StringComparison.OrdinalIgnoreCase)))
                return cert.Subject.Substring("CN=".Length);

            if (issuer.IsNullOrEmpty())
                return "Guest";

            return null;
        }
    }
}
