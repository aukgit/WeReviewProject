using System;
using System.Net;
using System.Web;
using System.Linq;
using System.Data.Entity;
using WereViewApp.Models.Context;
using WereViewApp.Models.POCO.IdentityCustomization;
using WereViewApp.Modules.Session;

namespace WereViewApp.Modules.InternetProtocolRelations {
    public class IpConfigRelations {

        public static int GetCountryId(string ipAddress) {
            var value = IpToValue(ipAddress);
            using (var db = new ApplicationDbContext()) {
                var country = db.CountryDetectByIPs.FirstOrDefault(n => n.BeginingIP >= value && value <= n.EndingIP);
                if (country != null) {
                    return country.CountryID;
                }
            }
            return -1;
        }

        public static long IpToValue(string ipAddress) {
            if (!string.IsNullOrWhiteSpace(ipAddress)) {
                long result = 0;
                var ipArrary = ipAddress.Split('.');
                long w, x, y, z;
                bool isW, isX, isY, isZ;

                isW = long.TryParse(ipArrary[0], out w);
                isX = long.TryParse(ipArrary[1], out x);
                isY = long.TryParse(ipArrary[2], out y);
                isZ = long.TryParse(ipArrary[3], out z);
                if (isW == isX == isY == isZ == true) {
                    result = 16777216 * w + 65536 * x + 256 * y + z;
                    return result;
                }

            }

            return -1;
        }

        /// <summary>
        ///     method to get Client ip address
        /// </summary>
        /// <param name="getLan"> set to true if want to get local(LAN) Connected ip address</param>
        /// <returns></returns>
        public static string GetVisitorIpAddress(bool getLan = false) {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session[SessionNames.IpAddress] != null) {
                return (string)HttpContext.Current.Session[SessionNames.IpAddress];
            }
            var visitorIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (String.IsNullOrEmpty(visitorIpAddress))
                visitorIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(visitorIpAddress))
                visitorIpAddress = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(visitorIpAddress) || visitorIpAddress.Trim() == "::1") {
                getLan = true;
                visitorIpAddress = string.Empty;
            }

            if (getLan && string.IsNullOrEmpty(visitorIpAddress)) {
                //This is for Local(LAN) Connected ID Address
                var stringHostName = Dns.GetHostName();
                //Get Ip Host Entry
                var ipHostEntries = Dns.GetHostEntry(stringHostName);
                //Get Ip Address From The Ip Host Entry Address List
                var arrIpAddress = ipHostEntries.AddressList;

                try {
                    visitorIpAddress = arrIpAddress[arrIpAddress.Length - 2].ToString();
                } catch {
                    try {
                        visitorIpAddress = arrIpAddress[0].ToString();
                    } catch {
                        try {
                            arrIpAddress = Dns.GetHostAddresses(stringHostName);
                            visitorIpAddress = arrIpAddress[0].ToString();
                        } catch {
                            visitorIpAddress = "127.0.0.1";
                        }
                    }
                }
            }

            if (HttpContext.Current.Session != null) {
                HttpContext.Current.Session[SessionNames.IpAddress] = visitorIpAddress;
            }
            return visitorIpAddress;
        }

        public static string GetIpAddress() {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session[SessionNames.IpAddress] != null) {
                return (string)HttpContext.Current.Session[SessionNames.IpAddress];
            }
            var context = HttpContext.Current;
            var ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress)) {
                var addresses = ipAddress.Split(',');
                if (addresses.Length != 0) {
                    return addresses[0];
                }
            }
            var finalIp = HttpContext.Current.Request.UserHostAddress;
            if (HttpContext.Current.Session != null) {
                HttpContext.Current.Session[SessionNames.IpAddress] = finalIp;
            }
            return finalIp;
        }
    }
}