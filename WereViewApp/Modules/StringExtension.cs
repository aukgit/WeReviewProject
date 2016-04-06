namespace WereViewApp.Modules {
    public static class StringExtension {
        /// <summary>
        ///     Split the string into pieces.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length">If string len is less then return whole string. Null means whole len.</param>
        /// <returns></returns>
        public static string GetStringCutOff(this string str, int? length) {
            if (string.IsNullOrEmpty(str))
                return "";
            if (length == null) {
                length = str.Length;
            }
            if (str.Length <= length) {
                return str;
            }
            return str.Substring(0, (int) length);
        }

        /// <summary>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="starting">If previous mid was on 100 , start from 100</param>
        /// <param name="length">-1 means whole return last len.</param>
        /// <returns></returns>
        public static string GetStringCutOff(this string str, int starting, int length) {
            if (string.IsNullOrEmpty(str))
                return "";
            if (length == -1) {
                length = str.Length;
            }
            if (str.Length < starting) {
                return "";
            }
            if (str.Length <= length) {
                length = str.Length;
            }
            length = length - starting;

            return str.Substring(starting, length);
        }
    }
}