using System.Text;

namespace WeReviewApp.Modules {
    public static class DevHash {
        private static readonly StringBuilder _sb = new StringBuilder(10);

        /// <summary>
        ///     Checks nulls and returns only codes for existing ones.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string Get(params object[] o) {
            _sb.Clear();

            for (var i = 0; o[i] != null && i < o.Length; i++) {
                _sb.AppendLine(o[i].GetHashCode() + "_");
            }
            return _sb.ToString().GetHashCode().ToString();
        }
    }
}