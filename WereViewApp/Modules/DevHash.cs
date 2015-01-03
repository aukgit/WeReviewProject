using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WereViewApp.Modules {
    public static class DevHash {
       static StringBuilder _sb = new StringBuilder(10);

        /// <summary>
        /// Checks nulls and returns only codes for existing ones.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string Get(params object[] o) {
            _sb.Clear();

            for (int i = 0; o[i] != null && i < o.Length; i++) {
                _sb.AppendLine(o[i].GetHashCode().ToString() + "_");
            }
            return _sb.ToString().GetHashCode().ToString();
        }
    }
}