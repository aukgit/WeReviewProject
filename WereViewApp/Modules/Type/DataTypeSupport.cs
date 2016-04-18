using System;

namespace WeReviewApp.Modules.Type {
    public class DataTypeSupport {
        /// <summary>
        ///     Returns true type is primitive type or guid or string or datetime.
        /// </summary>
        /// <param name="o">Pass the object of any type.</param>
        /// <returns>
        ///     Returns true type is primitive type or guid or string or datetime. If complex or custom class then returns
        ///     false.
        /// </returns>
        public static bool Support(object o) {
            var checkLong = o is long;
            var checkInt = o is int || o is short || o is int || o is long;
            var checkDecimal = o is float || o is decimal || o is double;
            var checkString = o is string;
            var checkGuid = o is Guid;
            var checkBool = o is bool || o is bool;
            var checkDateTime = o is DateTime;
            var checkByte = o is byte || o is byte;

            if (checkString || checkByte || checkLong || checkInt || checkDecimal || checkGuid || checkBool ||
                checkDateTime)
                return true;
            return false;
        }

        public static bool IsNumber(string o2) {
            object o = o2;

            return o is sbyte || o is byte || o is short || o is ushort || o is int || o is uint || o is long ||
                   o is ulong || o is float || o is double || o is decimal;
        }
    }
}