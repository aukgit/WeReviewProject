using System.Web;

namespace WeReviewApp.Modules.Extensions {
    public static class HttpSessionStateBaseExtension {
        public static object Get<T>(this HttpSessionStateBase pair, string key, T defaultValue) {
            if (pair != null) {
                var val = pair[key] ?? defaultValue;
                return val;
            }
            return defaultValue;
        }

        public static string GetAsString(this HttpSessionStateBase pair, string key, string defaultValue = "") {
            return (string)Get(pair, key, defaultValue);
        }

        public static int GetAsInt(this HttpSessionStateBase pair, string key, int defaultValue = 0) {
            var val = GetAsString(pair, key, null);
            if (val != null) {
                var valueToReturn = 0;
                if (int.TryParse(val, out valueToReturn)) {
                    return valueToReturn;
                }
            }
            return defaultValue;
        }

        public static long GetAsLong(this HttpSessionStateBase pair, string key, long defaultValue = 0) {
            var val = GetAsString(pair, key, null);
            if (val != null) {
                long valueToReturn = 0;
                if (long.TryParse(val, out valueToReturn)) {
                    return valueToReturn;
                }
            }
            return defaultValue;
        }

        public static decimal GetAsDecimal(this HttpSessionStateBase pair, string key, decimal defaultValue = 0) {
            var val = GetAsString(pair, key, null);
            if (val != null) {
                decimal valueToReturn = 0;
                if (decimal.TryParse(val, out valueToReturn)) {
                    return valueToReturn;
                }
            }
            return defaultValue;
        }

        public static bool GetAsBool(this HttpSessionStateBase pair, string key, bool defaultValue = false) {
            var val = GetAsString(pair, key, null);
            if (val != null) {
                bool valueToReturn;
                if (bool.TryParse(val, out valueToReturn)) {
                    return valueToReturn;
                }
            }
            return defaultValue;
        }

        public static void RemoveKeys(this HttpSessionStateBase pair, string[] keys) {
            if (pair != null) {
                foreach (var key in keys) {
                    pair.Remove(key);
                }
            }
        }

    }
}