using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WereViewApp.Modules {
    public static class SQLGenerate {
        /// <summary>
        /// Returns a simple SQL Query.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <param name="sql">if not null then return this one.</param>
        /// <returns></returns>
        public static string GetSimpleSQL(string tableName, string[] columns = null, string sql = null) {
            if (sql != null) {
                return sql;
            }
            string columnString = "";

            if (columns == null) {
                columnString = "*";
            } else {
                columnString = string.Join(",", columns);
            }

            return "SELECT " + columnString + " FROM " + tableName;
        }

    }
}