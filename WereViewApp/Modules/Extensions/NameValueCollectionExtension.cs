using System.Collections.Specialized;

namespace WeReviewApp.Modules.Extensions {
    public static class NameValueCollectionExtension {
        public static string Get(this NameValueCollection pair, string key, string defaultValue = "") {
            if (pair != null) {
                var val = pair[key] ?? defaultValue;
                return val;
            }
            return defaultValue;
        }

        public static int GetAsInt(this NameValueCollection pair, string key, int defaultValue = 0) {
            var val = Get(pair, key, null);
            if (val != null) {
                var valueToReturn = 0;
                if (int.TryParse(val, out valueToReturn)) {
                    return valueToReturn;
                }
            }
            return defaultValue;
        }

        public static long GetAsLong(this NameValueCollection pair, string key, long defaultValue = 0) {
            var val = Get(pair, key, null);
            if (val != null) {
                long valueToReturn = 0;
                if (long.TryParse(val, out valueToReturn)) {
                    return valueToReturn;
                }
            }
            return defaultValue;
        }

        public static decimal GetAsDecimal(this NameValueCollection pair, string key, decimal defaultValue = 0) {
            var val = Get(pair, key, null);
            if (val != null) {
                decimal valueToReturn = 0;
                if (decimal.TryParse(val, out valueToReturn)) {
                    return valueToReturn;
                }
            }
            return defaultValue;
        }

        public static bool GetAsBool(this NameValueCollection pair, string key, bool defaultValue = false) {
            var val = Get(pair, key, null);
            if (val != null) {
                bool valueToReturn;
                if (bool.TryParse(val, out valueToReturn)) {
                    return valueToReturn;
                }
            }
            return defaultValue;
        }
    }
}