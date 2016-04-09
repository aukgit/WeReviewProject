#region using block

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using WeReviewApp.Models.POCO.IdentityCustomization;
using WeReviewApp.Modules.Cache;
using WeReviewApp.Modules.Cookie;
using WeReviewApp.Modules.DevUser;

#endregion

namespace WeReviewApp.Modules.TimeZone {
    /// <summary>
    /// Timezone and date related codes.
    /// </summary>
    public class Zone {

        #region Fields

        private static string _defaultTimeFormat = "hh:mm:ss tt";
        private const string GmtConst = "GMT ";
        private static string _defaultDateFormat = "dd-MMM-yy";
        private static string _defaultDateTimeFormat = "dd-MMM-yy hh:mm:ss tt";

        #endregion

        #region Properties

        /// <summary>
        ///     hh:mm:ss tt
        /// </summary>
        public static string TimeFormat {
            get { return _defaultTimeFormat; }
            set { _defaultTimeFormat = value; }
        }

        /// <summary>
        ///     dd-MMM-yy
        /// </summary>
        public static string DateFormat {
            get { return _defaultDateFormat; }
            set { _defaultDateFormat = value; }
        }

        /// <summary>
        ///     dd-MMM-yy
        /// </summary>
        public static string DateTimeFormat {
            get { return _defaultDateTimeFormat; }
            set { _defaultDateTimeFormat = value; }
        }


        private static readonly ReadOnlyCollection<TimeZoneInfo> SystemTimeZones = TimeZoneInfo.GetSystemTimeZones();
        private static List<UserTimeZone> _dbTimeZones;

        #endregion

        #region Constructor

        public Zone() {
        }

        public Zone(string timeFormat, string dateFormat = null, string dateTimeFormat = null) {
            _defaultTimeFormat = timeFormat;
            if (dateFormat != null) {
                _defaultDateFormat = dateFormat;
            }
            if (dateTimeFormat != null) {
                _defaultDateTimeFormat = dateTimeFormat;
            }
        }

        #endregion

        #region Application Startup function for database

        public static void LoadTimeZonesIntoMemory() {
            _dbTimeZones = CachedQueriedData.GetTimezones();
        }

        #endregion

        /// <summary>
        ///     Flush cache information about user time-zone.
        /// </summary>
        /// <param name="log"></param>
        public static void RemoveTimeZoneCache(string log) {
            if (log == null) {
                return;
            }
            AppConfig.Caches.Remove(CookiesNames.ZoneInfo + log);
        }

        #region Dynamic Timing
        /// <summary>
        /// Returns a dynamic string value using time and other logics.
        /// </summary>
        /// <returns>Always get a unique string using date.</returns>
        public static string GetTimeDynamic() {
            var dynamic = DateTime.Now.Millisecond + DateTime.Now.Second + DateTime.Now.Minute +
                          DateTime.Now.Millisecond;

            return DateTime.Now.ToShortTimeString() + dynamic + (dynamic ^ dynamic);
        }

        #endregion


        #region Get Zone from Cache

        /// <summary>
        /// Get UserTimeZone from database using caching if possible.
        /// </summary>
        /// <param name="zone">Pass TimeZoneInfo to get the usertimezone from database.</param>
        /// <returns>Returns timezone from cache if possible if not found anywhere returns null.</returns>
        public static UserTimeZone Get(TimeZoneInfo zone) {
            var id = "timezone-id:" + zone.Id;
            var userTimeZone = (UserTimeZone)AppConfig.Caches.Get(id);
            if (userTimeZone == null) {
                userTimeZone = _dbTimeZones.FirstOrDefault(n => n.InfoID == zone.Id);
                AppConfig.Caches.Set(id, userTimeZone);
            }
            return userTimeZone;
        }
        /// <summary>
        /// Get timezone by userid.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Returns timezone from cache if possible if not found anywhere returns null.</returns>
        public static TimeZoneSet Get(long userId) {
            TimeZoneSet timeZoneInfo = null;
            var idString = "-id:" + userId;
            timeZoneInfo = GetSavedTimeZone(idString);
            if (timeZoneInfo != null) {
                //got time zone from cache.
                return timeZoneInfo;
            }
            //if cache time zone not exist.
            var user = UserManager.GetUser(userId);
            if (user != null) {
                var timezoneDb = _dbTimeZones.FirstOrDefault(n => n.UserTimeZoneID == user.UserTimeZoneID);
                if (timezoneDb != null) {
                    timeZoneInfo = new TimeZoneSet();
                    timeZoneInfo.UserTimezone = timezoneDb;
                    timeZoneInfo.TimeZoneInfo = SystemTimeZones.FirstOrDefault(n => n.Id == timezoneDb.InfoID);
                }
                if (timeZoneInfo != null && timeZoneInfo.TimeZoneInfo != null) {
                    // Save the time zone to the cache.
                    SaveTimeZone(timeZoneInfo, idString);
                    return timeZoneInfo;
                }
            }
            return null;
        }
        /// <summary>
        ///     Optimized fist check on cache then database.
        ///     Get current logged time zone from database or from cache.
        /// </summary>
        /// <returns>Returns time zone of the user.</returns>
        public static TimeZoneSet Get() {
            if (!HttpContext.Current.User.Identity.IsAuthenticated) {
                return null;
            }
            var log = HttpContext.Current.User.Identity.Name;
            return Get(log);
        }

        /// <summary>
        ///     Optimized fist check on cache then database.
        ///     Get time zone from database base on user name.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Returns time zone of the user.</returns>
        public static TimeZoneSet Get(string username) {
            TimeZoneSet timeZoneInfo = null;
            timeZoneInfo = GetSavedTimeZone(username);
            if (timeZoneInfo != null) {
                //got time zone from cache.
                return timeZoneInfo;
            }
            //if cache time zone not exist.
            var user = UserManager.GetUser(username);
            if (user != null) {
                var timezoneDb = _dbTimeZones.FirstOrDefault(n => n.UserTimeZoneID == user.UserTimeZoneID);
                if (timezoneDb != null) {
                    timeZoneInfo = new TimeZoneSet();
                    timeZoneInfo.UserTimezone = timezoneDb;
                    timeZoneInfo.TimeZoneInfo = SystemTimeZones.FirstOrDefault(n => n.Id == timezoneDb.InfoID);
                }
                if (timeZoneInfo != null && timeZoneInfo.TimeZoneInfo != null) {
                    // Save the time zone to the cache.
                    SaveTimeZone(timeZoneInfo, username);
                    return timeZoneInfo;
                }
            }
            return null;
        }


        /// <summary>
        ///     Get time zone from save cache.
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        private static TimeZoneSet GetSavedTimeZone(string log) {
            //save to cookie 
            if (!String.IsNullOrWhiteSpace(log)) {
                var cZone = (TimeZoneSet)AppConfig.Caches.Get(CookiesNames.ZoneInfo + log);
                return cZone; //fast
            }
            return null;
        }

        #endregion

        #region Save Zone in Cache

        /// <summary>
        ///     Saved for current logged user.
        /// </summary>
        /// <param name="timeZoneInfo"></param>
        private static void SaveTimeZone(TimeZoneSet timeZoneInfo) {
            if (!HttpContext.Current.User.Identity.IsAuthenticated) {
                return;
            }
            var log = HttpContext.Current.User.Identity.Name;
            SaveTimeZone(timeZoneInfo, log);
        }

        private static void SaveTimeZone(TimeZoneSet timeZoneInfo, string log) {
            if (log == null || timeZoneInfo == null) {
                return;
            }
            //save to cache 
            AppConfig.Caches.Set(CookiesNames.ZoneInfo + log, timeZoneInfo);
        }

        #endregion

        #region Get times format based on zone
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dt"></param>
        /// <param name="format"></param>
        /// <param name="addTimeZoneString">Add timezone string with Date. Eg. 26-Aug-2015 (GMT -07:00)</param>
        /// <returns></returns>
        public static string GetDateTime(
            long userId,
            DateTime? dt,
            string format = null,
            bool addTimeZoneString = true) {
            if (format == null) {
                format = DateTimeFormat;
            }
            var zone = Get(userId);
            return GetDateTime(zone, dt, format, addTimeZoneString);
        }

        /// <summary>
        ///     Get date to print as string.
        ///     Time zone by user logged in.
        ///     It will get the logged user and then get the time-zone and then print.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dt"></param>
        /// <param name="format">if format null then default format.</param>
        /// <param name="addTimeZoneString">Add timezone string with Date. Eg. 26-Aug-2015 (GMT -07:00)</param>
        /// <returns>Returns nice string format based on logged user's selected time zone.</returns>
        public static string GetTime(
            long userId,
            DateTime? dt,
            string format = null,
            bool addTimeZoneString = true) {
            if (format == null) {
                format = TimeFormat;
            }
            var zone = Get(userId);
            return GetDateTime(zone, dt, format);
        }
        /// <summary>
        ///     Get date to print as string.
        ///     Time zone by user logged in.
        ///     It will get the logged user and then get the time-zone and then print.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="format">if format null then default format.</param>
        /// <param name="addTimeZoneString">Add timezone string with Date. Eg. 26-Aug-2015 (GMT -07:00)</param>
        /// <returns>Returns nice string format based on logged user's selected time zone.</returns>
        public static string GetTime(
            DateTime? dt,
            string format = null,
            bool addTimeZoneString = true) {
            if (format == null) {
                format = TimeFormat;
            }
            return GetDateTime(dt, format, addTimeZoneString);
        }

        /// <summary>
        ///     No time zone required.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="format">if format null then default format.</param>
        /// <param name="addTimeZoneString">Add timezone string with Date. Eg. 26-Aug-2015 (GMT -07:00)</param>
        /// <returns>Returns nice string format based on logged user's selected time zone.</returns>
        public static string GetDate(
            DateTime? dt,
            string format = null,
            bool addTimeZoneString = true) {
            if (format == null) {
                format = DateFormat;
            }
            return GetDateTime(dt, format, addTimeZoneString);
        }

        /// <summary>
        ///     Get date to print as string.
        ///     Time zone by user logged in.
        ///     It will get the logged user and then get the time-zone and then print.
        /// </summary>
        /// <param name="dt">Returns "" if null</param>
        /// <param name="format">if format null then default format.</param>
        /// <param name="addTimeZoneString">Add timezone string with Date. Eg. 26-Aug-2015 (GMT -07:00)</param>
        /// <returns>Returns nice string format based on logged user's selected time zone. If no logged user then default datetime.</returns>
        public static string GetDateTime(
            DateTime? dt,
            string format = null,
            bool addTimeZoneString = true) {
            if (format == null) {
                format = DateTimeFormat;
            }
            var timeZone = Get();
            return GetDateTime(timeZone, dt, format, addTimeZoneString);
        }

        #endregion

        #region Based on timezone

        /// <summary>
        ///     Get date to print as string.
        ///     Time zone by user logged in.
        ///     It will get the logged user and then get the time-zone and then print.
        /// </summary>
        /// <param name="timeZone"></param>
        /// <param name="dt"></param>
        /// <param name="format">if format null then default format.</param>
        /// <param name="addTimeZoneString">Add timezone string with Date. Eg. 26-Aug-2015 (GMT -07:00)</param>
        /// <returns>Returns nice string format based on logged user's selected time zone.</returns>
        public static string GetTime(
            TimeZoneSet timeZone,
            DateTime? dt,
            string format = null,
            bool addTimeZoneString = true) {
            if (format == null) {
                format = TimeFormat;
            }
            return GetDateTime(timeZone, dt, format, addTimeZoneString);
        }

        /// <summary>
        ///     No need to convert dates based on time zones.
        /// </summary>
        /// <param name="timeZone"></param>
        /// <param name="dt"></param>
        /// <param name="format">if format null then default format.</param>
        /// <param name="addTimeZoneString">Add timezone string with Date. Eg. 26-Aug-2015 (GMT -07:00)</param>
        /// <returns>Returns nice string format based on logged user's selected time zone.</returns>
        public static string GetDate(
            TimeZoneSet timeZone,
            DateTime? dt,
            string format = null,
            bool addTimeZoneString = true) {
            if (format == null) {
                format = DateFormat;
            }
            return GetDateTime(timeZone, dt, format, addTimeZoneString);
        }


        /// <summary>
        ///     Get date time to print as string.
        ///     Time zone by user logged in.
        ///     It will get the logged user and then get the time-zone and then print.
        /// </summary>
        /// <param name="format">if format null then default format.</param>
        /// <param name="addTimeZoneString">Add timezone string with Date. Eg. 26-Aug-2015 (GMT -07:00)</param>
        /// <returns>Returns nice string format based on logged user's selected time zone.</returns>
        public static string GetCurrentDateTime(string format = null, bool addTimeZoneString = true) {
            return GetDateTime(DateTime.Now, format, addTimeZoneString);
        }

        /// <summary>
        ///     Get date time to print as string.
        ///     Time zone by user logged in.
        ///     It will get the logged user and then get the time-zone and then print.
        /// </summary>
        /// <param name="format">if format null then default format.</param>
        /// <param name="addTimeZoneString">Add timezone string with Date. Eg. 26-Aug-2015 (GMT -07:00)</param>
        /// <returns>Returns nice string format based on logged user's selected time zone.</returns>
        public static string GetCurrentDate(
            string format = null,
            bool addTimeZoneString = true) {
            return GetDate(DateTime.Now, format, addTimeZoneString);
        }

        /// <summary>
        ///     Get date to print as string.
        ///     Time zone by user logged in.
        ///     It will get the logged user and then get the time-zone and then print.
        /// </summary>
        /// <param name="timeZone"></param>
        /// <param name="dt"></param>
        /// <param name="format">if format null then default format.</param>
        /// <param name="addTimeZoneString">Add timezone string with Date. Eg. 26-Aug-2015 (GMT -07:00)</param>
        /// <returns>Returns nice string format based on logged user's selected time zone.</returns>
        public static string GetDateTime(
            TimeZoneSet timeZone,
            DateTime? dt,
            string format = null,
            bool addTimeZoneString = true) {
            if (dt == null) {
                return "";
            }
            var dt2 = (DateTime)dt;

            if (format == null) {
                format = DateTimeFormat;
            }
            if (timeZone == null || !timeZone.IsTimeZoneInfoExist()) {
                return dt2.ToString(format);
            }
            var currentZone = timeZone.TimeZoneInfo;
            //time zone found.
            var newDate = TimeZoneInfo.ConvertTime(dt2, currentZone);
            var additionalString = "";
            if (addTimeZoneString) {
                var userZone = timeZone.UserTimezone;
                additionalString = "(" + GmtConst  + userZone.TimePartOnly + ")";
            }
            return newDate.ToString(format) + additionalString;
        }

        #endregion
    }
}