#region using block

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using WereViewApp.Models.POCO.IdentityCustomization;
using WereViewApp.Modules.Cache;
using WereViewApp.Modules.Cookie;

#endregion

namespace WereViewApp.Modules.TimeZone {
    public class Zone {
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
            AppConfig.Cookies.Remove(CookiesNames.ZoneInfo);
            AppConfig.Caches.Remove(CookiesNames.ZoneInfo + log);
        }

        #region Dynamic Timing

        public static string GetTimeDynamic() {
            var dynamic = DateTime.Now.Millisecond + DateTime.Now.Second + DateTime.Now.Minute +
                          DateTime.Now.Millisecond;

            return DateTime.Now.ToShortTimeString() + dynamic + (dynamic ^ dynamic);
        }

        #endregion

        #region Fields

        private static string _defaultTimeFormat = "hh:mm:ss tt";
        private static string _defaultDateFormat = "dd-MMM-yy";
        private static string _defaultDateTimeFormat = "dd-MMM-yy hh:mm:ss tt";

        #endregion

        #region Properties

        /// <summary>
        ///     hh:mm:ss tt
        /// </summary>
        public static string TimeFormat
        {
            get { return _defaultTimeFormat; }
            set { _defaultTimeFormat = value; }
        }

        /// <summary>
        ///     dd-MMM-yy
        /// </summary>
        public static string DateFormat
        {
            get { return _defaultDateFormat; }
            set { _defaultDateFormat = value; }
        }

        /// <summary>
        ///     dd-MMM-yy
        /// </summary>
        public static string DateTimeFormat
        {
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

        #region Get Zone from Cache

        /// <summary>
        ///     Optimized fist check on cache then database.
        ///     Get current logged time zone from database or from cache.
        /// </summary>
        /// <returns>Returns time zone of the user.</returns>
        public static TimeZoneInfo Get() {
            if (!HttpContext.Current.User.Identity.IsAuthenticated) {
                return null;
            }
            var log = HttpContext.Current.User.Identity.Name;
            return Get(log);
        }

        /// <summary>
        ///     Optimized fist check on cache then database.
        ///     Get time zone from database base on username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Returns time zone of the user.</returns>
        public static TimeZoneInfo Get(string username) {
            TimeZoneInfo timeZoneInfo = null;
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
                    timeZoneInfo = SystemTimeZones.FirstOrDefault(n => n.Id == timezoneDb.InfoID);
                }
                if (timeZoneInfo != null) {
                    // Save the time zone to the cache.
                    SaveTimeZone(timeZoneInfo, username);
                    return timeZoneInfo;
                }
            }
            return null;
        }

        /// <summary>
        ///     Get time zone from save cache or cookie of Current user.
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        private static TimeZoneInfo GetSavedTimeZone() {
            if (!HttpContext.Current.User.Identity.IsAuthenticated) {
                return null;
            }
            var log = HttpContext.Current.User.Identity.Name;
            return GetSavedTimeZone(log);
        }

        /// <summary>
        ///     Get time zone from save cache or cookie.
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        private static TimeZoneInfo GetSavedTimeZone(string log) {
            //save to cookie 
            if (!string.IsNullOrWhiteSpace(log)) {
                var cZone = (TimeZoneInfo) AppConfig.Caches.Get(CookiesNames.ZoneInfo + log);
                if (cZone == null) {
                    // try cookie.
                    var id = AppConfig.Cookies.Get(CookiesNames.ZoneInfo);
                    if (id != null) {
                        cZone = SystemTimeZones.FirstOrDefault(n => n.Id == id);
                        return cZone;
                    }
                    return null;
                }
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
        private static void SaveTimeZone(TimeZoneInfo timeZoneInfo) {
            if (!HttpContext.Current.User.Identity.IsAuthenticated) {
                return;
            }
            var log = HttpContext.Current.User.Identity.Name;
            SaveTimeZone(timeZoneInfo, log);
        }

        private static void SaveTimeZone(TimeZoneInfo timeZoneInfo, string log) {
            if (log == null || timeZoneInfo == null) {
                return;
            }
            //save to cookie 
            AppConfig.Cookies.Set(timeZoneInfo.Id, CookiesNames.ZoneInfo);
            AppConfig.Caches.Set(CookiesNames.ZoneInfo + log, timeZoneInfo);
        }

        #endregion

        #region Get times format based on zone

        /// <summary>
        ///     Get date to print as string.
        ///     Time zone by user logged in.
        ///     It will get the logged user and then get the time-zone and then print.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="format">if format null then default format.</param>
        /// <returns>Returns nice string format based on logged user's selected time zone.</returns>
        public static string GetTime(DateTime? dt, string format = null) {
            if (format == null) {
                format = TimeFormat;
            }
            return GetDateTimeDefault(dt, format);
        }

        /// <summary>
        ///     No time zone required.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="format">if format null then default format.</param>
        /// <returns>Returns nice string format based on logged user's selected time zone.</returns>
        public static string GetDate(DateTime? dt, string format = null) {
            if (format == null) {
                format = DateFormat;
            }
            return GetDateTimeDefault(dt, format);
        }

        /// <summary>
        ///     Get date to print as string.
        ///     Time zone by user logged in.
        ///     It will get the logged user and then get the time-zone and then print.
        /// </summary>
        /// <param name="dt">Returns "" if null</param>
        /// <param name="format">if format null then default format.</param>
        /// <returns>Returns nice string format based on logged user's selected time zone. If no logged user then empty string.</returns>
        public static string GetDateTime(DateTime? dt, string format = null) {
            if (format == null) {
                format = DateTimeFormat;
            }
            return GetDateTimeDefault(dt, format);
        }

        /// <summary>
        ///     Get date to print as string.
        ///     Time zone by user logged in.
        ///     It will get the logged user and then get the time-zone and then print.
        /// </summary>
        /// <param name="dt">Returns "" if null</param>
        /// <param name="format">if format null then default format.</param>
        /// <returns>Returns nice string format based on logged user's selected time zone. If no logged user then default datetime.</returns>
        public static string GetDateTimeDefault(DateTime? dt, string format = null) {
            if (format == null) {
                format = DateTimeFormat;
            }
            var timeZone = Get();
            return GetDateTime(timeZone, dt, format);
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
        /// <returns>Returns nice string format based on logged user's selected time zone.</returns>
        public static string GetTime(TimeZoneInfo timeZone, DateTime? dt, string format = null) {
            if (format == null) {
                format = TimeFormat;
            }
            return GetDateTime(timeZone, dt, format);
        }

        /// <summary>
        ///     No need to convert dates based on time zones.
        /// </summary>
        /// <param name="timeZone"></param>
        /// <param name="dt"></param>
        /// <param name="format">if format null then default format.</param>
        /// <returns>Returns nice string format based on logged user's selected time zone.</returns>
        public static string GetDate(TimeZoneInfo timeZone, DateTime? dt, string format = null) {
            if (format == null) {
                format = DateFormat;
            }
            return GetDateTime(timeZone, dt, format);
        }


        /// <summary>
        ///     Get date time to print as string.
        ///     Time zone by user logged in.
        ///     It will get the logged user and then get the time-zone and then print.
        /// </summary>
        /// <param name="format">if format null then default format.</param>
        /// <returns>Returns nice string format based on logged user's selected time zone.</returns>
        public static string GetCurrentDateTime(string format = null) {
            return GetDateTime(DateTime.Now, format);
        }

        /// <summary>
        ///     Get date time to print as string.
        ///     Time zone by user logged in.
        ///     It will get the logged user and then get the time-zone and then print.
        /// </summary>
        /// <param name="format">if format null then default format.</param>
        /// <returns>Returns nice string format based on logged user's selected time zone.</returns>
        public static string GetCurrentDate(string format = null) {
            return GetDate(DateTime.Now, format);
        }

        /// <summary>
        ///     Get date to print as string.
        ///     Time zone by user logged in.
        ///     It will get the logged user and then get the time-zone and then print.
        /// </summary>
        /// <param name="timeZone"></param>
        /// <param name="dt"></param>
        /// <param name="format">if format null then default format.</param>
        /// <returns>Returns nice string format based on logged user's selected time zone.</returns>
        public static string GetDateTime(TimeZoneInfo timeZone, DateTime? dt, string format = null) {
            if (dt == null) {
                return "";
            }
            var dt2 = (DateTime) dt;

            if (format == null) {
                format = DateTimeFormat;
            }
            if (timeZone == null) {
                return dt2.ToString(format);
            }
            //time zone found.
            var newDate = TimeZoneInfo.ConvertTime(dt2, timeZone);

            return newDate.ToString(format);
        }

        #endregion
    }
}