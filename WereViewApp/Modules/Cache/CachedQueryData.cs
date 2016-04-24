using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using WeReviewApp.Models.Context;
using WeReviewApp.Models.POCO.Identity;
using WeReviewApp.Models.POCO.IdentityCustomization;

namespace WeReviewApp.Modules.Cache {
    public class CachedQueriedData {
        #region Countries

        /// <summary>
        ///     Get data from cached if not possible hit database.
        ///     99% time hits the cache for static tables like Country.
        /// </summary>
        /// <returns></returns>
        public static List<Country> GetCountries() {
            var tableName = CacheNames.CountryTableName;
            var cache = GetTableContentsFromCache(tableName, _countries);
            if (cache == null) {
                //updates
                var countries = GetCountriesFromDb();
                _countries = countries;
                AppConfig.Caches.TableStatusSetUnChanged(tableName);
                return countries;
            }
            return (List<Country>) cache;
        }

        #endregion

        #region Fields

        private static List<Country> _countries;
        private static List<CountryLanguage> _languages;
        private static List<UserTimeZone> _timezones;
        private static CountryLanguage _engLanguage;

        #endregion

        #region Direct Database Hits

        private static List<Country> GetCountriesFromDb() {
            using (var db = new ApplicationDbContext()) {
                _countries =
                    db.Countries.Include(n => n.CountryLanguageRelations)
                      .Include(n => n.CountryTimezoneRelations)
                      .ToList();
                return _countries;
            }
        }

        private static List<CountryLanguage> GetLanguagesFromDb() {
            using (var db = new ApplicationDbContext()) {
                return db.CountryLanguages.Include(n => n.CountryLanguageRelations).ToList();
            }
        }

        #endregion

        #region Get any table cached data.

        /// <summary>
        ///     no caching
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="type"></param>
        /// <param name="columns"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable GetTable(string tableName, AppVar.ConnectionStringType type, string[] columns = null,
            string sql = null) {
            var connectionString = AppVar.GetConnectionString(type);
            return GetTable(tableName, connectionString, columns, sql);
        }

        /// <summary>
        ///     no caching
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="connectionString"></param>
        /// <param name="columns"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable GetTable(string tableName, string connectionString = null, string[] columns = null,
            string sql = null) {
            //var hash = DevHash.Get(tableName, connectionString, columns, sql);
            //string CahedName = tableName + "-adapter";
            var _sql = SQLGenerate.GetSimpleSQL(tableName, columns, sql);
            var dt = new DataTable();
            using (var connection = new SqlConnection(connectionString)) {
                using (var adapter = new SqlDataAdapter(_sql, connection)) {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        #endregion

        #region Saving and Getting data from cache.

        private static void SaveTableContentsInCache(string tableName, dynamic contents) {
            AppConfig.Caches.Set(tableName, contents);
            AppConfig.Caches.TableStatusSetUnChanged(tableName);
        }

        /// <summary>
        ///     Return from cache or from db, whatever necessary based what exist.
        ///     Always returns the updated one..
        ///     To make it expire use CacheProcessor.TableNotify
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="contents"></param>
        /// <returns>If expired/not exist then returns null</returns>
        private static dynamic GetTableContentsFromCache(string tableName, dynamic cached) {
            var tableHasNoUpdate = AppConfig.Caches.TableStatusCheck(tableName);
            bool cacheExist = cached != null;
            if (cacheExist && tableHasNoUpdate) {
                return cached; //no updates
            }
            return null;
        }

        /// <summary>
        ///     Return from cache or from db, whatever necessary based what exist.
        ///     Always returns the updated one..
        ///     To make it expire use CacheProcessor.TableNotify
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="contents"></param>
        /// <returns>If expired/not exist then returns null</returns>
        private static dynamic GetTableContentsFromCache(string tableName) {
            var tableHasNoUpdate = AppConfig.Caches.TableStatusCheck(tableName);
            var cached = AppConfig.Caches.Get(tableName);
            var cacheExist = cached != null;
            if (cacheExist && tableHasNoUpdate) {
                return cached; //no updates
            }
            return null;
        }

        #endregion

        #region Languages

        /// <summary>
        ///     Get data from cached if not possible hit database.
        ///     99% time hits the cache for static tables.
        /// </summary>
        /// <returns></returns>
        public static List<CountryLanguage> GetLanguages() {
            var tableName = CacheNames.LanguageTableName;

            var cache = GetTableContentsFromCache(tableName, _languages);
            if (cache == null) {
                //updates
                var languages = GetLanguagesFromDb();
                _languages = languages;
                AppConfig.Caches.TableStatusSetUnChanged(tableName);
                return languages;
            }
            return (List<CountryLanguage>) cache;
        }

        /// <summary>
        /// </summary>
        /// <returns>Return default language English</returns>
        public static CountryLanguage GetDefaultLanguage() {
            if (_engLanguage == null) {
                _engLanguage = GetLanguages().FirstOrDefault(n => n.Code == "en");
            }
            return _engLanguage;
        }

        /// <summary>
        ///     Get Languages based on CountryId
        ///     Returns from cached data if exist
        /// </summary>
        /// <returns>Don't return if only one</returns>
        public static List<CountryLanguage> GetLanguages(int? countryId, int countCheckAbove = 1) {
            if (countryId == null) {
                return null;
            }
            var tableName = CacheNames.LanguageTableName;
            var cacheTableName = tableName + "-Country-" + countryId;
            var cache = GetTableContentsFromCache(cacheTableName);

            var searchingColumn = "CountryLanguageID";
            if (cache != null) {
                return (List<CountryLanguage>) cache; //no updates cache exist.
            }
            var relations = GetCountries().FirstOrDefault(n => n.CountryID == countryId);
            if (relations.CountryLanguageRelations.Count() > countCheckAbove) {
                //returns in query
                using (var db = new ApplicationDbContext()) {
                    var listOfLanguages = relations.CountryLanguageRelations.Select(n => n.CountryLanguageID).ToArray();
                    var stringListOfLanguageIds = string.Join(",", listOfLanguages);
                    var sqlInQuery = "SELECT * FROM " + tableName + " WHERE " + searchingColumn + " IN (" +
                                     stringListOfLanguageIds + ")";
                    var languages = db.CountryLanguages.SqlQuery(sqlInQuery).ToList();
                    SaveTableContentsInCache(cacheTableName, languages);
                    return languages;
                }
            }
            return null;
        }

        /// <summary>
        ///     return from cache
        /// </summary>
        /// <returns></returns>
        public static List<UserTimeZone> GetTimezones() {
            if (_timezones == null) {
                using (var db = new ApplicationDbContext()) {
                    _timezones = db.UserTimeZones.ToList();
                }
            }
            return _timezones;
        }

        /// <summary>
        ///     return from cache
        /// </summary>
        /// <returns></returns>
        public static UserTimeZone GetTimezone(int? userTimeZoneId) {
            if (userTimeZoneId == null) {
                return null;
            }
            return GetTimezones().FirstOrDefault(n => n.UserTimeZoneID == userTimeZoneId);
        }

        /// <summary>
        ///     return from cache
        /// </summary>
        /// <returns></returns>
        public static UserTimeZone GetTimezone(ApplicationUser user) {
            return GetTimezone(user.UserTimeZoneID);
        }

        /// <summary>
        ///     return from cache
        /// </summary>
        /// <returns></returns>
        public static Country GetCountry(int? countryId) {
            if (countryId == null) {
                return null;
            }
            return GetCountries().FirstOrDefault(n => n.CountryID == countryId);
        }

        public static Country GetCountry(ApplicationUser user) {
            return GetCountry(user.CountryID);
        }

        /// <summary>
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static List<UserTimeZone> GetTimezones(int? countryId, int countCheckAbove = 1) {
            if (countryId == null) {
                return null;
            }
            var tableName = CacheNames.UsertimezoneTableName;
            var cacheTableName = tableName + "-Country-" + countryId;
            var searchingColumn = "UserTimeZoneID";
            var cache = GetTableContentsFromCache(cacheTableName);
            if (cache != null) {
                return (List<UserTimeZone>) cache; //no updates cache exist.
            }

            var relations = GetCountries().FirstOrDefault(n => n.CountryID == countryId);
            //var relations = db.Countries.Include(n=> n.CountryTimezoneRelations).FirstOrDefault(n => n.CountryID == countryId);
            if (relations.CountryTimezoneRelations.Count() > countCheckAbove) {
                using (var db = new ApplicationDbContext()) {
                    var list = relations.CountryTimezoneRelations.Select(n => n.UserTimeZoneID).ToArray();
                    var stringList = string.Join(",", list);
                    //returns in query
                    var sqlInQuery = "SELECT * FROM " + tableName + " WHERE " + searchingColumn + " IN (" + stringList +
                                     ")";
                    ;
                    var userTimeZones = db.UserTimeZones.SqlQuery(sqlInQuery).ToList();
                    SaveTableContentsInCache(cacheTableName, userTimeZones);
                    return userTimeZones;
                }
            }

            return null;
        }

        /// <summary>
        ///     Get Languages based on countryname
        /// </summary>
        /// <returns>Don't return if only one or not found country.</returns>
        public static List<CountryLanguage> GetLanguages(string countryName) {
            var country = GetCountries().FirstOrDefault(n => n.CountryName == countryName);
            if (country != null) {
                return GetLanguages(country.CountryID);
            }
            return null;
        }

        #endregion

        #region Removing Caches

        public static void RemoveCache() {
            var length = GetCountries().Count() - 1;
            AppConfig.Caches.Remove(CacheNames.CountryTableName);
            AppConfig.Caches.Remove(CacheNames.LanguageTableName);
            for (var i = 0; i < length; i++) {
                AppConfig.Caches.Remove(CacheNames.LanguageTableName + "-Country-" + i);
            }
        }

        public static void RemoveCache(string cacheName) {
            AppConfig.Caches.Remove(cacheName);
        }

        #endregion
    }
}