using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WeReviewApp.Modules.Extensions {
    public static class GraphApiExtension {
        public static string GenerateAppSecretProof(this string accessToken) {
            //Creates a Facebook appsecret_proof value to be used for each graph api call when appsecret_proof has been enabled for the facebook app
            //Facebook appsecret_proof is SHA256 encrypted string of the current facebook access token using the facebook app secret value as the private key
            using (
                var algorithm =
                    new HMACSHA256(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["Facebook_AppSecret"]))) {
                var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(accessToken));
                var builder = new StringBuilder();
                for (var i = 0; i < hash.Length; i++) {
                    builder.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));
                }
                return builder.ToString();
            }
        }

        public static string GraphApiCall(this string baseGraphApiCall, params object[] args) {
            //returns a formatted Graph Api Call with a version prefix and appends a query string parameter containing the appsecret_proof value
            if (!string.IsNullOrEmpty(baseGraphApiCall)) {
                if (args != null &&
                    args.Count() > 0) {
                    //Determine if we need to concatenate appsecret_proof query string parameter or inject it as a single query string paramter
                    var graphApiCall = string.Empty;
                    if (baseGraphApiCall.Contains("?")) {
                        graphApiCall = string.Format(
                            baseGraphApiCall + "&appsecret_proof={" + (args.Count() - 1) + "}", args);
                    } else {
                        graphApiCall = string.Format(
                            baseGraphApiCall + "?appsecret_proof={" + (args.Count() - 1) + "}", args);
                    }

                    //prefix with Graph API Version
                    return string.Format("v2.1/{0}", graphApiCall);
                }
                throw new Exception(
                    "GraphAPICall requires at least one string parameter that contains the appsecret_proof value.");
            }
            return string.Empty;
        }
    }
}