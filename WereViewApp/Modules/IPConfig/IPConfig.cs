using WereViewApp.Modules.Session;
using System;
using System.Net;
using System.Web;

namespace WereViewApp.Modules.IPConfig {


    public class IPConfig {

        public double IPToValue(string ipAddress) {
            double Dot2LongIP = 0;
            int PrevPos = 0;
            int pos;
            int num;

            for (int i = 1; i <= 4; i++) {
                pos = ipAddress.IndexOf(".", PrevPos) + 1;
                if (i == 4) {
                    pos = ipAddress.Length + 1;
                }
                num = int.Parse(ipAddress.Substring(PrevPos, pos - PrevPos - 1));
                PrevPos = pos;
                Dot2LongIP = ((num % 256) * (256 ^ (4 - i))) + Dot2LongIP;

            }
            return Dot2LongIP;
        }

        /// <summary>
        /// method to get Client ip address
        /// </summary>
        /// <param name="GetLan"> set to true if want to get local(LAN) Connected ip address</param>
        /// <returns></returns>
        public static string GetVisitorIPAddress(bool GetLan = false) {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session[SessionNames.IpAddress] != null) {
                return (string)HttpContext.Current.Session[SessionNames.IpAddress];
            }
            string visitorIPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (String.IsNullOrEmpty(visitorIPAddress))
                visitorIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(visitorIPAddress))
                visitorIPAddress = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(visitorIPAddress) || visitorIPAddress.Trim() == "::1") {
                GetLan = true;
                visitorIPAddress = string.Empty;
            }

            if (GetLan && string.IsNullOrEmpty(visitorIPAddress)) {
                //This is for Local(LAN) Connected ID Address
                string stringHostName = Dns.GetHostName();
                //Get Ip Host Entry
                IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);
                //Get Ip Address From The Ip Host Entry Address List
                IPAddress[] arrIpAddress = ipHostEntries.AddressList;

                try {
                    visitorIPAddress = arrIpAddress[arrIpAddress.Length - 2].ToString();
                } catch {
                    try {
                        visitorIPAddress = arrIpAddress[0].ToString();
                    } catch {
                        try {
                            arrIpAddress = Dns.GetHostAddresses(stringHostName);
                            visitorIPAddress = arrIpAddress[0].ToString();
                        } catch {
                            visitorIPAddress = "127.0.0.1";
                        }
                    }
                }

            }

            if (HttpContext.Current.Session != null) {
                HttpContext.Current.Session[SessionNames.IpAddress] = visitorIPAddress;
            }
            return visitorIPAddress;
        }
        public static string GetIPAddress() {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session[SessionNames.IpAddress] != null) {
                return (string)HttpContext.Current.Session[SessionNames.IpAddress];
            }
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress)) {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0) {
                    return addresses[0];
                }
            }
            string finalIP = HttpContext.Current.Request.UserHostAddress;
            if (HttpContext.Current.Session != null) {
                HttpContext.Current.Session[SessionNames.IpAddress] = finalIP;
            }
            return finalIP;

        }


    }
}