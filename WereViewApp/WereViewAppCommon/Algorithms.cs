using DevMvcComponent.Extensions;
using DevMvcComponent.Pagination;
using DevTrends.MvcDonutCaching;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WeReviewApp.Models.EntityModel;
using WeReviewApp.Models.EntityModel.ExtenededWithCustomMethods;
using WeReviewApp.Models.EntityModel.Structs;
using WeReviewApp.Models.ViewModels;
using WeReviewApp.Modules.Cache;
using WeReviewApp.Modules.DevUser;
using WeReviewApp.WereViewAppCommon.Structs;

namespace WeReviewApp.WereViewAppCommon {
    public class Algorithms {

        #region Viewable Apps : Apps which are published
        /// <summary>
        /// Get apps which can be viewable. 
        /// Apps are not block and published.
        /// </summary>
        /// <param name="db"></param>
        /// <returns>Returns IQueryable which are not block and published.</returns>
        public IQueryable<App> GetViewableApps(WereViewAppEntities db = null) {
            if (db == null) {
                db = new WereViewAppEntities();
            }
            return db.Apps.Where(n => n.IsBlocked == false && n.IsPublished == true);
        }
        #endregion

        #region Platform Controller

        #region Platform wise apps for category page
        /// <summary>
        /// Platform wise apps for Platform page
        /// </summary>
        /// <returns></returns>
        public List<Platform> GetPlatformWiseAppsForPlatformPage(WereViewAppEntities db = null, int eachSlotAppsNumber = 8) {
            if (db == null) {
                db = new WereViewAppEntities();
            }

            var platforms = WereViewStatics.AppPlatformsCache;
            foreach (var platform in platforms) {
                platform.Apps = GetViewableApps(db)
                                  .Include(n => n.User)
                                  .OrderByDescending(n => n.AppID)
                                  .Where(n => n.PlatformID == platform.PlatformID)
                                  .Take(eachSlotAppsNumber)
                                  .ToList();
                if (platform.Apps != null && platform.Apps.Count > 0) {
                    GetEmbedImagesWithApp((List<App>)platform.Apps, db, eachSlotAppsNumber, GalleryCategoryIDs.SearchIcon);
                }
            }
            return platforms;
        }
        #endregion

        #region specific apps
        /// <summary>
        /// Platform page : specific apps
        /// </summary>
        /// <returns></returns>
        public Platform GetPlatformPageApps(string platformName, PaginationInfo pageInfo, string cacheName, WereViewAppEntities db = null) {
            if (db == null) {
                db = new WereViewAppEntities();
            }

            var platform = WereViewStatics.AppPlatformsCache.FirstOrDefault(n => n.PlatformName.Equals(platformName, StringComparison.OrdinalIgnoreCase));
            if (platform != null) {
                var appsConditions = GetViewableApps(db)
                    .Include(n => n.User)
                    .OrderByDescending(n => n.AppID)
                    .Where(n => n.PlatformID == platform.PlatformID);

                var pagedApps = appsConditions.GetPageData(pageInfo, cacheName).ToList();

                if (pagedApps.Count > 0) {
                    GetEmbedImagesWithApp(pagedApps, db, (int)pageInfo.ItemsInPage, GalleryCategoryIDs.SearchIcon);
                }
                platform.Apps = pagedApps;

                return platform;
            }
            return null;
        }
        #endregion

        #endregion

        #region Category Controller

        #region Category wise apps for category page
        /// <summary>
        /// Category wise apps for category page
        /// </summary>
        /// <returns></returns>
        public List<Category> GetCategoryWiseAppsForCategoryPage(WereViewAppEntities db = null, int eachSlotAppsNumber = 8) {
            if (db == null) {
                db = new WereViewAppEntities();
            }

            var categories = WereViewStatics.AppCategoriesCache;
            foreach (var category in categories) {
                category.Apps = GetViewableApps(db)
                                  .Include(n => n.User)
                                  .OrderByDescending(n => n.AppID)
                                  .Where(n => n.CategoryID == category.CategoryID)
                                  .Take(eachSlotAppsNumber)
                                  .ToList();
                if (category.Apps != null && category.Apps.Count > 0) {
                    GetEmbedImagesWithApp((List<App>)category.Apps, db, eachSlotAppsNumber, GalleryCategoryIDs.SearchIcon);
                }
            }
            return categories;
        }
        #endregion

        #region Category page : specific apps
        /// <summary>
        /// Category page : specific apps
        /// </summary>
        /// <param name="slug">Slug of the category</param>
        /// <param name="pageInfo"></param>
        /// <param name="cacheName"></param>
        /// <param name="db"></param>
        /// <returns>Returns category item with apps contained inside.</returns>
        public Category GetCategoryPageApps(string slug, PaginationInfo pageInfo, string cacheName, WereViewAppEntities db = null) {
            if (db == null) {
                db = new WereViewAppEntities();
            }
            var category = WereViewStatics.AppCategoriesCache.FirstOrDefault(n => n.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
            if (category != null) {
                var appsConditions = GetViewableApps(db)
                    .Include(n => n.User)
                    .OrderByDescending(n => n.AppID)
                    .Where(n => n.CategoryID == category.CategoryID);

                var pagedApps = appsConditions.GetPageData(pageInfo, cacheName).ToList();

                if (pagedApps.Count > 0) {
                    GetEmbedImagesWithApp(pagedApps, db, (int)pageInfo.ItemsInPage, GalleryCategoryIDs.SearchIcon);
                }
                category.Apps = pagedApps;

                return category;
            }
            return null;
        }
        #endregion

        #endregion

        #region Get top users

        public List<string> GetTopDevelopers(WereViewAppEntities db, int topDevelopersLimit = 30) {
            if (db == null) {
                db = new WereViewAppEntities();
            }
            var topDeveloperNames = GetViewableApps(db)
                            .Include(n => n.User)
                            .Select(n => new {
                                username = n.User.UserName
                            })
                            .GroupBy(n => n.username)
                            .Select(app => new { Username = app.Key, Count = app.Count() })
                            .OrderByDescending(n => n.Count)
                            .Take(topDevelopersLimit)
                            .Select(n => n.Username)
                            .ToList();

            return topDeveloperNames;
        }

        #endregion

        #region Lame Gallery Queries


        public Gallery HomeIcon(App app, WereViewAppEntities db = null) {
            if (db == null) {
                db = new WereViewAppEntities();
            }
            if (app != null) {
                return db.Galleries.FirstOrDefault(n => n.UploadGuid == app.UploadGuid && n.GalleryCategoryID == GalleryCategoryIDs.HomePageIcon);
            }
            return null;
        }

        public Gallery SearchIcon(App app, WereViewAppEntities db = null) {
            if (db == null) {
                db = new WereViewAppEntities();
            }
            if (app != null) {
                return db.Galleries.FirstOrDefault(n => n.UploadGuid == app.UploadGuid && n.GalleryCategoryID == GalleryCategoryIDs.SearchIcon);
            }
            return null;
        }

        public Gallery SuggestIcon(App app, WereViewAppEntities db = null) {
            if (db == null) {
                db = new WereViewAppEntities();
            }
            if (app != null) {
                return db.Galleries.FirstOrDefault(n => n.UploadGuid == app.UploadGuid && n.GalleryCategoryID == GalleryCategoryIDs.SuggestionIcon);
            }
            return null;
        }

        public List<long> GetTagIds(App app, WereViewAppEntities db = null) {
            if (db == null) {
                db = new WereViewAppEntities();
            }
            if (app.Tags != null) {

                return db.TagAppRelations.Where(n => n.AppID == app.AppID).Select(n => n.TagID).ToList();


            }
            return null;
        }

        public List<long> GetTagIds(WereViewAppEntities db, string tags) {

            if (tags != null && tags.Length > 4) {
                tags = tags.Trim();
                return db.Tags.Where(n => n.TagDisplay.Equals(tags)).Select(n => n.TagID).ToList();
            }

            return null;
        }
        #endregion

        #region Get App From Cache
        public App GetAppFromStaticCache(long appId) {
            if (CommonVars.StaticAppsList != null) {
                return CommonVars.StaticAppsList.FirstOrDefault(n => n.AppID == appId);
            }
            return null;
        }
        #endregion

        #region Search Algorithm

        /// <summary>
        /// Returns  IQueryable of Tag where urlStringExceptEscapeSequence=> words matches with the display string of Tag.
        /// Example : given parameter urlStringExceptEscapeSequence "Hello-World"
        /// Will return tags which are (equal to 'Hello' or 'World')
        /// </summary>
        /// <param name="urlStringExceptEscapeSequence"></param>
        /// <param name="?"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public IQueryable<Tag> GetTagIds(string urlStringExceptEscapeSequence,  WereViewAppEntities db) {
            var listWords = GetUrlListExceptEscapeSequence(urlStringExceptEscapeSequence);
            var tags = db.Tags.Where(n => listWords.Any(word => word == n.TagDisplay));
            return tags;
        }
        /// <summary>
        /// Search apps based on UrlEscapseString based on given searchString.
        /// </summary>
        /// <param name="searchString">Give a string "Hello World v2" , it will search for 'Hello' and 'World'</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public IQueryable<App> GetSimpleAppSearchResults(IQueryable<App> apps,string searchString) {
            // convert any given "Hello World v2" =>  "Hello-World"
            var appHyphenUrl = GenerateHyphenUrlString(searchString);
            var appUrlEscapseString = GetUrlStringExceptEscapeSequence(appHyphenUrl); // "Hello World v2" =>  "Hello-World"
            var urlListOfEscapseString = GetUrlListExceptEscapeSequence(appUrlEscapseString); // list of words from split '-'

            var query = apps.Where(app => 
                                        urlListOfEscapseString.All(
                                        searchWord=> 
                                            app.UrlWithoutEscapseSequence.Contains(searchWord)));
            return query;
        }


        /// <summary>
        /// Returns the search no caching
        /// </summary>
        /// <param name="searchText">Plant Vs. Zombies</param>
        /// <param name="rating"></param>
        /// <param name="platform">apple</param>
        /// <param name="tags">tag1,tag2...</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public List<App> GetSearchResults(string searchText, float? rating, string platform, string tags, int max, WereViewAppEntities db = null) {
            if (db == null) {
                db = new WereViewAppEntities();
            }
            //var hash = DevHash.Get(searchText, rating, platform, tags);
            //var cacheReaderSaver = new CacheDataInFile(CommonVars.APP_SEARCH_RESULTS_ADDITIONALPATH);
            //var cache = cacheReaderSaver.ReadObjectFromBinaryFileAsCache(hash, CommonVars.APP_SEARCH_RESULTS_EXPIRE_IN_HOURS);
            //if (cache == null) {
            // cache doesn't exist
            var results = AppSearchAlgorithm(searchText, rating, platform, tags, CommonVars.SearchResultsMaxResultReturn, db);
            //cacheReaderSaver.SaveInBinaryAsync(hash, results);
            GetEmbedImagesWithApp(results, db, max, GalleryCategoryIDs.SearchIcon);

            return results;
            //} else {
            //    //cache exist
            //    return (List<App>)cache;
            //}

        }

        public List<App> AppSearchAlgorithm(string searchText, float? rating, string platform, string tags, int maxCount, WereViewAppEntities db) {
            List<App> executeAppsWithSameName = null;
            List<App> executeAppsWithSimilarNameAnd = null;

            var tagIds = GetTagIds(db, tags);
            var minSearchChars = 3;
            IQueryable<App> appsSameName = null;
            IQueryable<App> appsSimilarNameAnd = null;
            var isSearchable = false;
            var viewableApps = GetViewableApps(db);

            var url = GenerateHyphenUrlString(searchText);
            var validUrlList = GetUrlListExceptEscapeSequence(url);
            byte? platformId = null;
            if (platform != null) {
                var platformObject = WereViewStatics.AppPlatformsCache.FirstOrDefault(n => n.PlatformName == platform);
                if (platformObject != null) {
                    platformId = platformObject.PlatformID;
                }
            }
            if (searchText != null) {
                searchText = searchText.Trim();
                if (searchText.Length >= minSearchChars) {
                    isSearchable = true;
                    appsSameName = viewableApps.Where(n => n.AppName.StartsWith(searchText));
                    appsSimilarNameAnd = viewableApps;

                    foreach (var singleValidUrl in validUrlList) {
                        appsSimilarNameAnd = appsSimilarNameAnd
                                            .Where(n =>
                                                n.UrlWithoutEscapseSequence.StartsWith(singleValidUrl + "-") ||
                                                n.UrlWithoutEscapseSequence.Contains("-" + singleValidUrl + "-") ||
                                                n.UrlWithoutEscapseSequence.EndsWith("-" + singleValidUrl)
                                             );
                    }
                }
            }
            if (isSearchable) {
                if (appsSameName == null) {
                    appsSameName = viewableApps;
                }
                AddConditionsForSearch(ref appsSameName, tagIds, rating, platformId);
                AddOrderingForSuggestions(ref appsSameName, isMosRecentOrBasedOnPopularity: true);
                executeAppsWithSameName = appsSameName.Include(n => n.User).ToList();

                var sameIds = executeAppsWithSameName.Select(n => n.AppID).ToArray();


                AddConditionsForSearch(ref appsSimilarNameAnd, tagIds, rating, platformId);
                AddOrderingForSuggestions(ref appsSimilarNameAnd, isMosRecentOrBasedOnPopularity: true);
                AddConditionOfRemovingPreviousFoundIDs(ref appsSimilarNameAnd, sameIds, null);

                var getSimilarMax = maxCount / 2;

                executeAppsWithSimilarNameAnd = appsSimilarNameAnd
                    .Include(n => n.User)
                    .Take(getSimilarMax)
                    .ToList();
                return MergeSearchResultsLists(executeAppsWithSameName, executeAppsWithSimilarNameAnd, null, null, maxCount * 2);
            }
            return null;
        }
        /// <summary>
        /// Merge the list of search results.
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <param name="list3"></param>
        /// <param name="list4"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public List<App> MergeSearchResultsLists(List<App> list1 = null, List<App> list2 = null, List<App> list3 = null, List<App> list4 = null, int maxCount = -1) {
            var newList = new List<App>(maxCount);
            if (list1 != null) {
                foreach (var app in list1) {
                    newList.Add(app);
                }
            }

            if (list2 != null) {
                foreach (var app in list2) {
                    if (!newList.Any(n => n.AppID == app.AppID)) {
                        newList.Add(app);
                    }
                }
            }

            if (list3 != null) {
                foreach (var app in list3) {
                    if (!newList.Any(n => n.AppID == app.AppID)) {
                        newList.Add(app);
                    }
                }
            }

            if (list4 != null) {
                foreach (var app in list4) {
                    if (!newList.Any(n => n.AppID == app.AppID)) {
                        newList.Add(app);
                    }
                }
            }

            return newList;
        }

        /// <summary>
        /// Additional Same conditions like tag, rating searching adds with every query.
        /// </summary>
        /// <param name="apps"></param>
        /// <param name="tagsIDs"></param>
        /// <param name="rating"></param>
        /// <param name="platformId"></param>
        void AddConditionsForSearch(ref IQueryable<App> apps, List<long> tagsIDs = null, float? rating = null, byte? platformId = null) {
            if (tagsIDs != null) {
                apps = apps.Where(n => tagsIDs.All(tagId => n.TagAppRelations.Any(tagRel => tagRel.TagID == tagId)));
            }
            if (rating != null) {
                var rate = (float)rating;
                apps = apps.Where(a => a.AvgRating >= rate);
            }

            if (platformId != null) {
                var id = (byte)platformId;

                apps = apps.Where(a => a.PlatformID >= id);
            }


        }

        #endregion

        #region Saving Virtual Fields In App
        /// <summary>
        /// Async saving into files as binary
        /// </summary>
        /// <param name="app"></param>
        public void SaveVirtualFields(App app) {
            var appSavingFields = new AppSavingTextFields();
            appSavingFields.Developers = app.Developers;
            appSavingFields.IdeaBy = app.IdeaBy;
            appSavingFields.Publishers = app.Publishers;
            appSavingFields.Tags = app.Tags;
            appSavingFields.UploadGuid = app.UploadGuid;
            WereViewStatics.SavingAppInDirectory(appSavingFields);
        }
        #endregion

        #region Review & Rating

        #region Fix Rating in App
        public void FixRatingInApp(long appId, WereViewAppEntities db, App app = null) {
            var reviewExist = db.Reviews.FirstOrDefault();
            if (reviewExist != null) {
                var avg = db.Reviews.Where(n => n.AppID == appId).Average(n => n.Rating);
                var i = db.Database.ExecuteSqlCommand("UPDATE APP SET AvgRating = @p0 WHERE AppID = @p1", avg, appId);
                if (i > 0) {
                    if (app != null) {
                        app.AvgRating = avg;
                    }
                }
            }
        }
        #endregion

        #region After Writing Review : Fix Rating and Update Rating and Review Count

        /// <summary>
        /// Updates avgRating in app
        /// Updates ReviewsCount in App in db and in static
        /// </summary>
        /// <param name="review"></param>
        /// <param name="isNew"></param>
        /// <param name="db"></param>
        public void AfterReviewIsSavedFixRatingNReviewCountInApp(Review review, bool isNew, WereViewAppEntities db) {
            var app = GetAppFromStaticCache(review.AppID);
            if (isNew) {
                IncreaseReviewCount(review.AppID, db, app);
            }
            FixRatingInApp(review.AppID, db, app);
            //RemoveOutputCacheReview(review.AppID);
        }
        #endregion

        #region Does current user reviewed the app before
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="db"></param>
        /// <returns>Returns true if did review the app.</returns>
        public bool IsCurrentUserReviewedThisApp(long appId, WereViewAppEntities db) {
            var userid = UserManager.GetLoggedUserId();
            return db.Reviews.Any(n => n.AppID == appId && n.UserID == userid);
        }

        #endregion

        #region get user reviewed app
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="db"></param>
        /// <returns>Returns true if did review the app.</returns>
        public Review GetUserReviewedApp(long appId, WereViewAppEntities db) {
            var userid = UserManager.GetLoggedUserId();
            return db.Reviews.FirstOrDefault(n => n.AppID == appId && n.UserID == userid);
        }

        #endregion

        #endregion

        #region Fix Iframe Tag
        string GetSqureIFrameTag(string str) {
            if (!string.IsNullOrWhiteSpace(str)) {
                //|embed|object|frameset|frame|iframe|
                //str = str.ToLower();
                str = str.Replace("<iframe", "[iframe");
                str = str.Replace("</iframe>", "[/iframe]");
                str = str.Replace(">", "]");
                return str;
            }
            return str;
        }


        string GetRawIframeString(string str) {
            if (!string.IsNullOrWhiteSpace(str)) {
                //|embed|object|frameset|frame|iframe|                
                str = str.Replace("[iframe", "<iframe");
                str = str.Replace("[/iframe]", "</iframe>");
                str = str.Replace("]", ">");
                return str;
            }
            return str;
        }
        #endregion

        #region Reading Virtual Fields In App
        /// <summary>
        /// Reading from binary : read only fields from text which are not saved in database.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public App ReadVirtualFields(App app) {
            var retriveFields = WereViewStatics.ReadAppFromDirectory(app.UploadGuid);
            if (retriveFields != null) {
                app.Tags = retriveFields.Tags;
                app.IdeaBy = retriveFields.IdeaBy;
                app.Developers = retriveFields.Developers;
                app.Publishers = retriveFields.Publishers;
            }
            return app;
        }
        #endregion

        #region Notification Add & Transfer notifcation to seen table
        /// <summary>
        /// Add notification to user 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="UserSettingsId">UserPointsSettingIDs.PostApp</param>
        /// <param name="db"></param>
        public void AddNotification(long userId, byte notificationTypeId, string msg, WereViewAppEntities db) {
            var type = WereViewStatics.GetNotificationType(notificationTypeId);

            var notify = new Notification();
            notify.UserID = userId;
            notify.NotificationTypeID = type.NotificationTypeID;
            notify.Dated = DateTime.Now;
            notify.Message = msg;
            db.Notifications.Add(notify);
            db.SaveChanges();
        }

        /// <summary>
        /// Transfer notification to seen table
        /// </summary>
        /// <param name="app"></param>
        /// <param name="UserSettingsId">UserPointsSettingIDs.PostApp</param>
        /// <param name="db"></param>
        public void SeenNotificationTransfer(long notifyId, WereViewAppEntities db) {
            var notify = db.Notifications.FirstOrDefault(n => n.NotificationID == notifyId);
            if (notify != null) {
                var seen = new LatestSeenNotification();
                seen.Dated = notify.Dated;
                seen.Message = notify.Message;
                seen.UserID = notify.UserID;
                seen.NotificationTypeID = notify.NotificationTypeID;
                db.LatestSeenNotifications.Add(seen);
                db.Notifications.Remove(notify);
                db.SaveChanges();
            }
        }
        #endregion

        #region Add user points
        /// <summary>
        /// Add points + Update user profile
        /// </summary>
        /// <param name="app"></param>
        /// <param name="userSettingsId">UserPointsSettingIDs.PostApp</param>
        /// <param name="db"></param>
        public void AddPoints(App app, byte userSettingsId, WereViewAppEntities db) {
            var point = WereViewStatics.GetUserSettingPoint(userSettingsId);
            var userId = UserManager.GetLoggedUserId();

            var userPoint = new UserPoint();
            userPoint.UserID = userId;
            userPoint.Point = point.Point;
            userPoint.Dated = DateTime.Now;
            userPoint.UserPointSettingID = point.UserPointSettingID;
            db.UserPoints.Add(userPoint);
            if (db.SaveChanges() > 0) {
                var i = db.Database
                    .ExecuteSqlCommand(
                    "UPDATE User SET TotalEarnedPoints = TotalEarnedPoints + @p0 WHERE UserId = @p1",
                    point.Point,
                    userId);

            }

        }



        #endregion

        #region Single app disply : site.com/Apps/Apple-8/Games/plant-vs-zombies, reviews loading algorithm
        #region Single App on App Page: Display
        /// <summary>
        /// First try to get the app from the static list.
        /// Static app list contain 500 of apps in memory.
        /// To remove cache static 
        /// call RemoveSingleAppFromCacheOfStatic().
        /// If static app is not found.
        /// Then get the app from db and attach 
        /// gallery images and icons with it.
        /// and then save it into the cache.
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="platformVersion"></param>
        /// <param name="categorySlug"></param>
        /// <param name="url"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public App GetSingleAppForDisplay(string platform, float? platformVersion, string categorySlug, string url, int maxReviewLoad, WereViewAppEntities db) {
            if (platform != null && platformVersion != null && categorySlug != null && url != null) {
                if (CommonVars.StaticAppsList == null) {
                    CommonVars.StaticAppsList = new List<App>(800);
                }
                App app = null;
                var platformO = WereViewStatics.AppPlatformsCache.FirstOrDefault(n => n.PlatformName.Equals(platform, StringComparison.OrdinalIgnoreCase));
                var categoryO = WereViewStatics.AppCategoriesCache.FirstOrDefault(n => n.Slug.Equals(categorySlug, StringComparison.OrdinalIgnoreCase));
                if (platformO != null && categoryO != null) {
                    var platformId = platformO.PlatformID;
                    var categoryId = categoryO.CategoryID;
                    app = CommonVars.StaticAppsList
                                    .FirstOrDefault(n =>
                                        n.Url.Equals(url) &&
                                        n.PlatformID == platformId &&
                                        n.CategoryID == categoryId);

                    if (app != null) {
                        // app found in  the static cache.
                        var appId = app.AppID;
                        app.Category = categoryO;
                        app.Platform = platformO;
                        if (UserManager.IsAuthenticated()) {
                            // if current user is rated this app , then put the rating in the CurrentUserRatedAppValue 
                            var userId = UserManager.GetLoggedUserId();

                            var currentUserRated =
                                    db.Reviews
                                      .FirstOrDefault(n =>
                                         n.AppID == appId &&
                                         n.UserID == userId);
                            if (currentUserRated != null) {
                                app.CurrentUserRatedAppValue = currentUserRated.Rating;
                            } else {
                                app.CurrentUserRatedAppValue = null;
                            }
                        }
                        // only load if needed : 
                        // determinate by the flag : app.IsReviewAlreadyLoaded == false
                        // Also load app review like dislikes
                        LoadReviewAndThenReviewLikeDislikesBasedOnUserIntoApp(app, 0, maxReviewLoad, db);
                        return app;
                    }
                    // app not found in cache so search in db:
                    app = GetViewableApps(db) //means not blocked and published
                            .Include(n => n.User)
                            .FirstOrDefault(n =>
                            n.Url.Equals(url) &&
                            n.PlatformID == platformId &&
                            n.CategoryID == categoryId);
                    if (app != null) {
                        // app  found in db.
                        app.Category = categoryO;
                        app.Platform = platformO;
                        app.YoutubeEmbedLink = GetRawIframeString(app.YoutubeEmbedLink);
                        var appId = app.AppID;

                        // Loading app reviews into app
                        // only load if needed : determinate by the flag
                        // app.IsReviewAlreadyLoaded == false
                        // Also load app review like dislikes
                        LoadReviewAndThenReviewLikeDislikesBasedOnUserIntoApp(app, 0, maxReviewLoad, db);

                        //Get current user app-rating.
                        if (UserManager.IsAuthenticated()) {
                            var userId = UserManager.GetLoggedUserId();

                            // current user rating for the app
                            var currentUserRated = db.Reviews.FirstOrDefault(n => n.AppID == appId && n.UserID == userId);
                            if (currentUserRated != null) {
                                app.CurrentUserRatedAppValue = currentUserRated.Rating;
                            } else {
                                app.CurrentUserRatedAppValue = null;
                            }
                        }
                        // read app virtual fields, like tags as string and so on
                        ReadVirtualFields(app);


                        // injecting gallery images location inside the app
                        GetEmbedGalleryImagesWithCurrentApp(app, db);
                        // add youtube cover image location inject.
                        GetEmbedImagesWithApp(ref app, db, 1, GalleryCategoryIDs.YoutubeCoverImage);

                        // clear few old cache if close overflowing
                        if (CommonVars.StaticAppsList.Count > 795) {
                            CommonVars.StaticAppsList.RemoveRange(0, 200);
                        }
                        // saving app into the static
                        CommonVars.StaticAppsList.Add(app);
                        return app;
                    }
                }
            }
            return null;
        }

        #endregion

        #region Single app edit - retrieval
        /// <summary>
        /// Returns an app if it is created by this same user.
        /// Try to use the cache if possible.
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public App GetEditingApp(long appId, WereViewAppEntities db) {
            var userId = UserManager.GetLoggedUserId();
            App app = null;
            if (CommonVars.StaticAppsList != null) {
                app = CommonVars.StaticAppsList.FirstOrDefault(n => n.AppID == appId && n.PostedByUserID == userId);
            }

            var cacheId = CacheNames.EditingApp + appId + "-" + userId;

            if (app == null) {
                app = (App)AppConfig.Caches[cacheId];
                if (app == null) {
                    app = db.Apps
                        .FirstOrDefault(n => n.AppID == appId && n.PostedByUserID == userId);
                    AppConfig.Caches[cacheId] = app;
                }
            }
            return app;
        }

        #endregion

        #region Reviews loading algorithm.
        #region App-Details Page : Review Load app + Review Like Dislikes

        /// <summary>
        /// Only load reviews if needed.
        /// Based on app.IsReviewAlreadyLoaded prop
        /// Also generate ReviewCount Value
        /// To make it force to load make sure "app.IsReviewAlreadyLoaded == false"
        /// Also load Review Like Dislikes efficiently
        /// </summary>
        /// <param name="app"></param>
        /// <param name="skip">how many to skip</param>
        /// <param name="maxReviewLoad"></param>
        /// <param name="loadAppIfNotExist">Try load the app from cache or from database if not exist</param>
        /// <param name="db"></param>
        /// <param name="appId">Must pass app id when loadAppIfNotExist = true</param>
        /// <returns>App with reviews if successfully done</returns>
        public App LoadReviewAndThenReviewLikeDislikesBasedOnUserIntoApp(App app, int skip, int maxReviewLoad, WereViewAppEntities db = null, bool loadAppIfNotExist = false, long appId = -1) {
            if (loadAppIfNotExist && app == null) {
                // forcing + app is not exist
                app = GetAppFromStaticCache(appId);
                if (app == null) {
                    // app doesn't exist in the cache 

                    // now we don't need to pull the whole app
                    // just create a new one to hold these reviews.
                    app = new App() {
                        AppID = appId
                    };

                }
            }
            if (app != null && app.IsReviewAlreadyLoaded == false) {
                appId = app.AppID;
                var reviews = db.Reviews
                    //.Include(n => n.ReviewLikeDislikes)
                                .Include(n => n.User)
                                .OrderByDescending(n => n.LikedCount)
                                .ThenBy(n => n.DisLikeCount)
                                .Where(n => n.AppID == appId)
                                .Skip(skip).Take(maxReviewLoad);
                app.Reviews = reviews
                              .ToList();
                if (skip == 0) {
                    app.ReviewDisplayingCount = app.Reviews.Count;
                }
                app.ReviewsCount = (short)db.Reviews.Count(n => n.AppID == appId);
                app.IsReviewAlreadyLoaded = true;
                // load review like dislikes by this authenticated user.
                // app.ReviewLikeDislikesCollection will have the like dislikes
                // Loads all likes and dislikes over reviews (given reviews inside the app) for current user only.
                LoadReviewsLikeDislikeBasedOnUserIntoApp(app, db);
                return app;
            }
            return null;
        }
        /// <summary>
        /// Load ReviewLikeDislikes efficiently if app.IsReviewAlreadyLoaded == true.
        /// Loads all likes and dislikes over reviews (given reviews inside the app) for current user only.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="db"></param>
        public void LoadReviewsLikeDislikeBasedOnUserIntoApp(App app, WereViewAppEntities db) {
            if (app != null && UserManager.IsAuthenticated()) {
                if (app.IsReviewAlreadyLoaded) {
                    if (app.Reviews.Count > 0) {
                        app.ReviewLikeDislikesCollection = GetReviewsLikeDislikeBasedOnUser(app.Reviews, db);
                    }
                }
            }
        }

        /// <summary>
        /// Load ReviewLikeDislikes efficiently if app.IsReviewAlreadyLoaded == true and user is authenticated.
        /// Loads all likes and dislikes over reviews (given reviews inside the app) for current user only.
        /// </summary>
        /// <param name="reviews"></param>
        /// <param name="db"></param>
        /// <returns>Returns a collection of ReviewLikeDislike or null.</returns>
        public List<ReviewLikeDislike> GetReviewsLikeDislikeBasedOnUser(IEnumerable<Review> reviews, WereViewAppEntities db) {
            if (UserManager.IsAuthenticated()) {
                var currentUserId = UserManager.GetLoggedUserId();
                if (reviews != null) {
                    var reviewIdsCsv = reviews.GetAsCommaSeperatedValues(n => n.ReviewID);  // all review ids
                    // getting the like dislike based on reviews those are loaded 
                    // and if and only if current user has done any.
                    var sql = string.Format("SELECT * FROM ReviewLikeDislike WHERE ReviewID IN ({0}) AND UserID = {1}", reviewIdsCsv, currentUserId);
                    //var reviewLikeDislikes = db.Database.SqlQuery<ReviewLikeDislike>(sql);
                    var likeDislikes = db.Database.SqlQuery<ReviewLikeDislike>(sql).ToList();
                    return likeDislikes;
                }
            }
            return null;
        }
        #endregion

        #region Force App Review to Load
        public void ForceAppReviewToLoad(long appId) {
            var app = GetAppFromStaticCache(appId);
            if (app != null) {
                app.IsReviewAlreadyLoaded = false;
            }
            RemoveDonutCaching("Partials", "ReviewsDisplay", new { @id = appId });
        }
        #endregion
        #endregion

        #endregion

        #region View Count Increase ++ : App , Review
        #region Increase App View Count

        /// <summary>
        /// Increases the app view count.
        /// Threaded method, make sure running from async Action
        /// </summary>
        /// <param name="app"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public void IncreaseViewCount(App app, WereViewAppEntities db) {
            var appid = app.AppID;
            new Thread(() => {
                db.Database.ExecuteSqlCommand("UPDATE APP SET TotalViewed = TotalViewed+1 WHERE AppID = @p0", appid);
            }).Start();
            app.TotalViewed += 1;
        }

        #endregion

        #region Increase App Review Count

        public bool IncreaseReviewCount(long appId, WereViewAppEntities db, App app = null) {
            var appid = appId;

            var reviewCount = db.Reviews.Count(n => n.AppID == appid);
            if (reviewCount > 32000) {
                return true;
            }
            var i = db.Database.ExecuteSqlCommand("UPDATE APP SET ReviewsCount = @p0 WHERE AppID = @p1", reviewCount, appid);
            if (i > 0) {
                if (app != null) {
                    app.ReviewsCount += 1;
                }
                return true;
            }
            return false;
        }

        #endregion
        #endregion

        #region Remove single app from cache of static

        /// <summary>
        /// Only call when an app is edited.
        /// Remove the app from CommonVars.StaticAppsList
        /// </summary>
        /// <param name="appId"></param>
        public void RemoveSingleAppFromCacheOfStatic(long appId) {
            if (CommonVars.StaticAppsList != null) {
                var find = CommonVars.StaticAppsList.FirstOrDefault(n => n.AppID == appId);
                if (find != null) {
                    CommonVars.StaticAppsList.Remove(find);
                }
            }
        }
        #endregion

        #region Get Url Without Escape Sequence.
        /// <summary>
        /// title-tile-2 will return 2 list items [title,tile] numbers will be gone.
        /// </summary>
        /// <param name="url">title-tile-2</param>
        /// <returns></returns>
        public List<string> GetUrlListExceptEscapeSequence(string url) {
            if (url != null) {
                var urlList = url.Split('-');

                var validUrl = new List<string>(urlList.Length);

                foreach (var valid in urlList) {
                    if (!CommonVars.SearchingEscapeSequence.Any(escapse => escapse.Equals(valid))) {
                        var isNumber = Regex.IsMatch(valid, @"^\d+$");
                        if (!isNumber) {
                            validUrl.Add(valid);
                        }
                    }
                }
                return validUrl;

            }
            return null;
        }
        /// <summary>
        /// "title-tile-2" => "title-tile"
        /// Use GenerateHyphenUrlString() to get the hyphen string.
        /// Returns url without the number and escape sequence (it is for version matching and suggestions)
        /// [Example : title-tile-2 => title-tile]
        /// Plant vs Zombines v2 should have a suggestion of Plant Vs. Zombines v1, v3 and so on.
        /// </summary>
        /// <param name="url">title-tile-2</param>
        /// <returns>title-title</returns>
        public string GetUrlStringExceptEscapeSequence(string url) {
            if (url != null) {
                
                var validUrl = GetUrlListExceptEscapeSequence(url);
                string returnStr = null;
                returnStr = string.Join("-", validUrl);
                return returnStr;
            }
            return null;
        }
        #endregion

        #region AppTitle + Generate URL

        public static char ToLower(ref char c) {
            // if upper case
            if (c <= 'Z' && c >= 'A') {
                c = (char)(c - 'A' + 'a');// lowercase
            }
            return c;
        }
        public static char ToUpper(ref char c) {
            // if lower case
            if (c <= 'z' && c >= 'a') {
                c = (char)(c - 'a' + 'A');// uppercase
            }
            return c;
        }
        /// <summary>
        /// Get each word's first character upper case.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string GetAllUpperCaseTitle(string title) {
            if (!string.IsNullOrEmpty(title)) {
                var list = title.Split(" ".ToCharArray());
                var finalizedArray = new string[list.Length];
                var finalIndex = 0;
                foreach (var str in list) {
                    var strArray = str.ToCharArray();
                    int mid = strArray.Length / 2,
                        lastIndex = strArray.Length - 1;
                    ToUpper(ref strArray[0]); // uppercase
                    for (var i = 0; i < mid; i++) {
                        if (i == 0) {
                            ToLower(ref strArray[lastIndex]); // lowercase
                            ToLower(ref strArray[mid]); // lowercase
                        } else {
                            lastIndex--;
                            ToLower(ref strArray[i]); // lowercase
                            ToLower(ref strArray[lastIndex]); // lowercase
                        }
                    }
                    finalizedArray[finalIndex++] = new string(strArray);

                }
                var output = string.Join(" ", finalizedArray);
                finalizedArray = null;
                list = null;
                GC.Collect();
                return output;

            }
            return title;
        }

        #region Generate Url + Hyphen Urls

        /// <summary>
        /// Create url but also check if existing one is there.
        /// and add 2 if same one exist.
        /// </summary>
        /// <param name="platformVersion"></param>
        /// <param name="categoryId"></param>
        /// <param name="title"></param>
        /// <param name="platformId"></param>
        /// <param name="db">Must pass a db, otherwise it will throw an exception.</param>
        /// <param name="currentAppId">Put -1 if the app is not created. If created then give the app id.</param>
        /// <returns>Returns url using hyphen(-). E.g. title app name => title-app-name. Note: Must get a valid url.</returns>
        public string GenerateHyphenUrlStringValid(double platformVersion, short categoryId, string title, byte platformId, WereViewAppEntities db, long currentAppId) {
            if (!string.IsNullOrEmpty(title)) {
                title = title.Trim();
                title = Regex.Replace(title, CommonVars.FriendlyUrlRegex, "-").ToLower();
            checkAgain:
                var exist = false;
                if (currentAppId < 1) {
                    exist = db.Apps.Any(n => n.PlatformVersion == platformVersion && n.CategoryID == categoryId && n.Url == title && n.PlatformID == platformId);
                } else {
                    exist = db.Apps.Any(n => n.AppID != currentAppId && n.PlatformVersion == platformVersion && n.CategoryID == categoryId && n.Url == title && n.PlatformID == platformId);
                }

                if (exist) {
                    // url already exist then change it to 2
                    title += "-2";
                    goto checkAgain;
                }
                return title;
            }
            return title;
        }
        /// <summary>
        /// Create url but also check if existing one is there.
        /// and add 2 if same one exist.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="db">Must pass a db, otherwise it will throw an exception.</param>
        /// <returns>Returns url using hyphen(-). E.g. title app name => title-app-name. Note: Must get a valid url.</returns>
        public string GenerateHyphenUrlStringValid(App app, WereViewAppEntities db) {
            return GenerateHyphenUrlStringValid(app.PlatformVersion, app.CategoryID, app.AppName, app.PlatformID, db, app.AppID);
        }

        #endregion

        #region Generate URL not valid one
        /// <summary>
        /// "Hello World v2" => "Hello-World-v2"
        /// Create url , don't check if exist one.
        /// It is used for searching. Whatever search phrase is given , it converts it to url and look exact match at the database level.
        /// </summary>
        /// <param name="title">Give the search string.</param>
        /// <returns>Returns url using hyphen(-). E.g. "title app name" => "title-app-name"</returns>
        public string GenerateHyphenUrlString(string title) {
            if (!string.IsNullOrEmpty(title)) {
                title = title.Trim();
                title = Regex.Replace(title, CommonVars.FriendlyUrlRegex, "-").ToLower();
                return title;
            }
            return title;
        }

        #endregion

        #endregion

        #region Upload Guids in 'IN Query format'
        string GetGuidStringConcat(List<App> apps) {
            var guids = apps.AsParallel().Select(n => "'" + n.UploadGuid.ToString() + "'").ToArray();
            //var guids = apps.AsEnumerable().Select(n =>  n.UploadGuid.ToString() ).ToArray();
            var guidsStringList = string.Join(",", guids); // guid1,guid2...

            return guidsStringList;
        }
        #endregion

        #region UI : Bredcrum
        /// <summary>
        /// Get Breadcrumb from current url.
        /// </summary>
        /// <returns></returns>
        public MvcHtmlString GetBredcrumbsBasedOnCurrentUrl(string styleClass = "") {
            var url = GetCurrentUrlWithoutHostNameWithoutSlash();
            //<ol class="breadcrumb" style="margin-bottom: 5px;">
            //    <li><a href="#">Home</a></li>
            //    <li><a href="#">Library</a></li>
            //    <li class="active">Data</li>
            //</ol>
            var hostUrl = AppVar.Url + "/";

            var builder = new StringBuilder(40);

            builder.Append("<ol class=\"breadcrumb " + styleClass + "\">");
            var length = url.Length;
            builder.Append("<li><a href=\"" + hostUrl + "\" title=\"" + AppVar.Name + "\"><i class=\"fa fa-home\"></i></a></li>");
            string urlUpto,
                    currentDirectory,
                    pointingUrl;
            var i = 0;
            for (i = 0; i < length; i++) {
                if (url[i] == '/') {
                    // hello/world/new
                    urlUpto = url.Substring(0, i);
                    currentDirectory = GetCurrentWebDirectory(urlUpto);
                    pointingUrl = hostUrl + urlUpto;
                    builder.Append("<li><a href=\"" + pointingUrl + "\">" + currentDirectory + "</a></li>");
                }
            }
            //last index
            urlUpto = url.Substring(0, i);
            currentDirectory = GetCurrentWebDirectory(urlUpto);
            pointingUrl = hostUrl + urlUpto;
            builder.Append("<li><a href=\"" + pointingUrl + "\">" + currentDirectory + "</a></li>");
            builder.Append("</ol>");
            var bredcrumHtml = new MvcHtmlString(builder.ToString());
            builder = null;
            GC.Collect();
            return bredcrumHtml;
        }
        /// <summary>
        /// site.com/hello/world
        /// returns world
        /// </summary>
        /// <param name="url"></param>
        /// <returns>site.com/hello/world, returns world</returns>
        private string GetCurrentWebDirectory(string url) {
            var found = url.LastIndexOf('/');
            if (found == -1) {
                return url; // in current directory.
            }
            return url.Substring(found + 1);
        }
        #endregion

        #region Get current url
        /// <summary>
        /// Get the current url.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentUrlWithHostName() {
            return AppVar.Url + HttpContext.Current.Request.RawUrl;
        }
        /// <summary>
        /// Get the extract part of what is exist after site.com
        /// </summary>
        /// <returns></returns>
        public string GetCurrentUrlWithoutHostName() {
            return HttpContext.Current.Request.RawUrl;
        }

        /// <summary>
        /// Get the extract part of what is exist after site.com
        /// </summary>
        /// <returns></returns>
        public string GetCurrentUrlWithoutHostNameWithoutSlash() {
            var url = GetCurrentUrlWithoutHostName();
            if (url != null) {
                if (url[url.Length - 1] == '/') {
                    url = url.Remove(url.Length - 1, 1);
                }
                return url.Remove(0, 1);
            }
            return "";
        }
        #endregion

        #region List Of Apps: Top, Latest, Home, Gallery, Advertise,  Suggested, Developers and so on.

        #region Get apps filtered by : site.com/Apps/Apple-8/Games

        /// <summary>
        /// Get a list of apps by this platform and category (games or so on)
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="platformVersion"></param>
        /// <param name="categorySlug"></param>
        /// <param name="page">Current displaying list page</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public List<App> GetAppsFilteredByPlatformAndCategory(string platform, double? platformVersion, string categorySlug, int page, dynamic ViewBag, WereViewAppEntities db) {
            IQueryable<App> apps = null;
            Category categoryO;
            short categoryId = -1, platformId = -1;

            if (platform != null) {
                //var cacheName = "GetAppsFilteredByPlatformAndCategory.names." + page + platform;
                var platformO = WereViewStatics.AppPlatformsCache.FirstOrDefault(n => n.PlatformName.Equals(platform, StringComparison.OrdinalIgnoreCase));
                if (platformO != null) {
                    platformId = platformO.PlatformID;
                    apps = GetViewableApps(db)
                           .Where(n => n.PlatformID == platformId);
                    if (platformVersion != null) {
                        apps = apps.Where(n => n.PlatformVersion == platformVersion);
                    }
                }
                if (!string.IsNullOrEmpty(categorySlug)) {
                    categoryO = WereViewStatics.AppCategoriesCache.FirstOrDefault(n => n.Slug.Equals(categorySlug, StringComparison.OrdinalIgnoreCase));
                    categoryId = categoryO.CategoryID;
                    if (platformO != null) {
                        apps = apps.Where(n => n.CategoryID == categoryId);
                    }
                }
                if (platformO != null) {
                    // add ordered by
                    var pageInfo = new PaginationInfo {
                        ItemsInPage = AppConfig.Setting.PageItems,
                    };
                    apps = apps.Include(n => n.User)
                                .OrderByDescending(n => n.AppID);
                    var paged = apps.GetPageData(pageInfo).ToList();
                    GetEmbedImagesWithApp(paged, db, (int)AppConfig.Setting.PageItems, GalleryCategoryIDs.SearchIcon);
                    var eachUrl = GetCurrentUrlWithHostName() + "?page=@page";
                    ViewBag.paginationHtml = new HtmlString(Pagination.GetList(pageInfo, eachUrl, "",
                    maxNumbersOfPagesShow: 8));
                    return paged;
                }
            }
            return null;
        }

        #endregion

        #region Home Page Related Queries: Top, Latest, Suggested, Home Page Gallery, Advertise

        #region Latest Apps With Icons
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="max"></param>
        /// <param name="isFromHomePage">True attaches HomeIcon or else SearchIcon</param>
        /// <returns></returns>
        public List<App> GetLatestApps(WereViewAppEntities db, int max, bool isFromHomePage = true) {
            var apps = GetViewableApps(db)
                .Include(n => n.Platform)
                .Include(n => n.User)
                .OrderByDescending(n => n.AppID)
                .Take(max)
                .ToList();
            if (apps != null) {
                var homePageIconOrSearchIconId = GalleryCategoryIDs.HomePageIcon;
                homePageIconOrSearchIconId = !isFromHomePage
                    ? GalleryCategoryIDs.SearchIcon
                    : homePageIconOrSearchIconId;
                GetEmbedImagesWithApp(apps, db, max, homePageIconOrSearchIconId);
            }
            return apps;
        }

        public List<App> GetLatestApps(WereViewAppEntities db, bool pagination, int page, out HtmlString paginationListItems, bool isFromHomePage = true) {
            var apps = GetViewableApps(db)
                .Include(n => n.Platform)
                .Include(n => n.User)
                .OrderByDescending(n => n.AppID);

            var pageInfo = new PaginationInfo {
                ItemsInPage = AppConfig.Setting.PageItems,
                PageNumber = page,
                PagesExists = -1
            };

            var pagedApps = apps.GetPageData(pageInfo, CacheNames.LastestAppsArchived, true)
                                .ToList();
            if (pagedApps != null && pagedApps.Count > 0) {
                var homePageIconOrSearchIconId = GalleryCategoryIDs.HomePageIcon;
                homePageIconOrSearchIconId = !isFromHomePage
                    ? GalleryCategoryIDs.SearchIcon
                    : homePageIconOrSearchIconId;
                GetEmbedImagesWithApp(pagedApps, db, (int)AppConfig.Setting.PageItems, homePageIconOrSearchIconId);
            }

            var eachUrl = "/Apps?Page=@page";
            paginationListItems = new HtmlString(Pagination.GetList(pageInfo, eachUrl, "",
                           maxNumbersOfPagesShow: 8));
            return pagedApps;
        }

        #endregion

        #region Top Rated Apps with Icons

        /// <summary>
        /// include icons
        /// </summary>
        /// <param name="db"></param>
        /// <param name="max"></param>
        /// <param name="isFromHomePage">If true then attaches HomeIcon or else attach search icon</param>
        /// <returns></returns>
        public List<App> GetTopRatedApps(WereViewAppEntities db, int max, bool isFromHomePage = true) {
            var apps = GetViewableApps(db)
                        .Include(n => n.Platform)
                        .Include(n => n.User);
            //isMosRecentOrBasedOnPopularity : false means based on popularity
            AddOrderingForSuggestions(ref apps, isMosRecentOrBasedOnPopularity: false);
            var topRatedApps = apps.Take(max).ToList();
            if (topRatedApps != null) {
                var homePageIconOrSearchIconId = GalleryCategoryIDs.HomePageIcon;
                homePageIconOrSearchIconId = !isFromHomePage
                    ? GalleryCategoryIDs.SearchIcon
                    : homePageIconOrSearchIconId;

                GetEmbedImagesWithApp(topRatedApps, db, max, homePageIconOrSearchIconId);
            }
            return topRatedApps;
        }
        #endregion

        #region Advertise
        public List<DisplayGalleryImages> GetAdvertises(WereViewAppEntities db, int max) {
            var galleryDisplays = db.Galleries
                .Where(n =>
                    n.GalleryCategoryID == GalleryCategoryIDs.Advertise)
                .Take(max)
                .AsParallel()
                .AsEnumerable()
                .Select(n => new DisplayGalleryImages {
                    GalleryID = n.GalleryID,
                    GalleryImageLocation = n.GetHtppUrl(null),
                    Sequence = n.Sequence,
                    Title = n.Title,
                    Subtitle = n.Subtitle
                })
                .ToList();
            if (galleryDisplays != null) {
                galleryDisplays = galleryDisplays.OrderBy(n => n.Sequence).ToList();

            }
            return galleryDisplays;

        }
        #endregion

        #region Home page : gallery

        /// <summary>
        /// Returns apps which are related to home page gallery 
        /// Use HomeFeaturedBigImageLocation to src display image.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public List<App> GetHomePageGalleryImages(WereViewAppEntities db, int max) {
            var appsRelatedToHomePage = db.FeaturedImages.Include(n => n.App).ToList();
            if (appsRelatedToHomePage != null) {
                var apps = appsRelatedToHomePage
                    .Select(n => n.App)
                    .Where(n => n.IsPublished && !n.IsBlocked)
                    .ToList();
                if (apps != null) {
                    GetEmbedImagesWithApp(apps, db, max, GalleryCategoryIDs.HomePageFeatured);
                    return apps;
                }
            }
            return null;
        }


        /// <summary>
        /// Returns apps which are related to home page gallery 
        /// Use IsFeatured = true to src display image.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public List<App> GetFeaturedAppsWithImages(App app, WereViewAppEntities db, int max) {
            //app = GetAppFromStaticCache(app.AppID);

            var tagsIds = GetTagIds(app, db);
            if (tagsIds != null) {
                var appsRelatedToHomePage = db.FeaturedImages
                    .Include(n => n.App)
                    .Include(n => n.App.User)
                    .Where(n => n.IsFeatured)
                    .Where(feature =>
                                tagsIds.Any(tagId =>
                                    feature.App
                                    .TagAppRelations
                                    .Any(tagRel => tagRel.TagID == tagId)))
                    .ToList();


                if (appsRelatedToHomePage != null) {

                    var apps = appsRelatedToHomePage
                        .Select(n => n.App)
                        .Where(n => n.IsPublished && !n.IsBlocked)
                        .ToList();

                    if (apps != null && apps.Count > 0) {
                        GetEmbedImagesWithApp(apps, db, max, GalleryCategoryIDs.SuggestionIcon);
                        return apps;
                    } else if (apps != null && apps.Count == 0) {
                        //appsRelatedToHomePage = db.FeaturedImages
                        //               .Include(n => n.App)
                        //               .Where(n => n.IsFeatured)
                        //               .Where(feature =>
                        //                           app
                        //                           .URLWithoutEscapseSequence
                        //                           .Split('-')
                        //                           .Any(url =>
                        //                               feature.App
                        //                              .URLWithoutEscapseSequence.StartsWith(url + "-") ||
                        //                               feature.App
                        //                              .URLWithoutEscapseSequence.Contains("-" + url + "-") ||
                        //                               feature.App
                        //                              .URLWithoutEscapseSequence.EndsWith("-" + url)
                        //                            ))
                        //                            .ToList();
                        //apps = appsRelatedToHomePage
                        //           .Select(n => n.App)
                        //           .Where(n => n.IsPublished && !n.IsBlocked)
                        //           .ToList();
                        //if (apps != null && apps.Count > 0) {
                        //    GetEmbedImagesWithApp(apps, db, max, GalleryCategoryIDs.SuggestionIcon);
                        //}
                        return null;
                    }
                }
            }
            return null;
        }
        #endregion

        #endregion

        #region Developers: Get Suggested apps

        /// <summary>
        /// Return final suggested apps from cache if possible.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="db"></param>
        /// <param name="maxNumberOfApps"></param>
        /// <returns></returns>
        public List<App> GetDevelopersAppsByApp(App app, WereViewAppEntities db, int maxNumberOfApps = 40) {
            if (app == null) {
                return null;
            }
            var appid = app.AppID;
            var developersApps = GetViewableApps(db)
                                    .Include(n => n.User)
                                    .Where(n => n.PostedByUserID == app.PostedByUserID && n.AppID != appid)
                                    .OrderByDescending(n => n.AppID)
                                    .Take(maxNumberOfApps)
                                    .ToList();
            GetEmbededSuggestedIconsWithApps(developersApps, db);
            return developersApps;
        }
        #endregion

        #region Get Final Suggested Apps
        /// <summary>
        /// Return final suggested apps from cache if possible.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public List<App> GetFinalSuggestedAppsCache(App app, WereViewAppEntities db) {
            //app = GetAppFromStaticCache(app.AppID);
            if (app == null) {
                return null;
            }
            //var appid = app.AppID.ToString();
            List<App> suggestedApps = null;

            //var cacheReaderSaver = new CacheDataInFile(CommonVars.APP_SUGGESTED_ADDITIONALPATH);
            //var cache = cacheReaderSaver.ReadObjectFromBinaryFileAsCache(appid, CommonVars.APP_SEARCH_RESULTS_EXPIRE_IN_HOURS);
            //if (cache == null) {
            // cache doesn't exist
            suggestedApps = GetSuggestedApps(app, db);
            GetEmbededSuggestedIconsWithApps(suggestedApps, db);
            // cacheReaderSaver.SaveInBinaryAsync(appid, suggestedApps);

            //} else {
            //    //cache exist
            //    suggestedApps = (List<App>)cache;

            //}


            return suggestedApps;

        }
        #endregion

        #region Get Suggested Apps
        /// <summary>
        /// Virtual fields must be available
        /// Use this method to embed gallery icons with 
        /// </summary>
        /// <param name="app"></param>
        /// <returns>Returns a list of Apps which is related to current app.</returns>
        public List<App> GetSuggestedApps(App app, WereViewAppEntities db = null) {
            if (db == null) {
                db = new WereViewAppEntities();
            }
            if (app == null) {
                return null;
            }
            var tagIds = GetTagIds(app, db);
            var url = app.UrlWithoutEscapseSequence;
            var validUrlList = url.Split('-');


            var userId = app.PostedByUserID;

            // same user same platform and category apps with 
            var appsCollectionNotAsSameId = GetViewableApps(db)
                .Include(n => n.User)
                .Where(n => n.AppID != app.AppID)
                .Where(n => n.IsPublished && !n.IsBlocked);

            // like starts with query
            var appsSameNameAsCurrent = appsCollectionNotAsSameId
                                        .Where(n => n.UrlWithoutEscapseSequence.StartsWith(url));


            var appsNameSimilariesWithAnd = appsCollectionNotAsSameId;
            foreach (var singleValidUrl in validUrlList) {

                appsNameSimilariesWithAnd = appsNameSimilariesWithAnd
                                    .Where(n => n.UrlWithoutEscapseSequence.StartsWith(singleValidUrl + "-") ||
                                                n.UrlWithoutEscapseSequence.Contains("-" + singleValidUrl + "-") ||
                                                n.UrlWithoutEscapseSequence.EndsWith("-" + singleValidUrl)
                                                );
            }

            //IQueryable<App> appsNameSimilariesWithOr = appsCollectionNotAsSameId;
            //var orPredicate = PredicateBuilder.False<App>();
            //foreach (var singleValidUrl in validUrlList) {

            //    orPredicate =
            //        orPredicate.Or(n => n.UrlWithoutEscapseSequence.StartsWith(singleValidUrl + "-") ||
            //                        n.UrlWithoutEscapseSequence.Contains("-" + singleValidUrl + "-") ||
            //                        n.UrlWithoutEscapseSequence.EndsWith("-" + singleValidUrl)
            //                        );
            //}

            //appsNameSimilariesWithOr = appsNameSimilariesWithOr.Where(orPredicate);

            // exclude blocked or not published
            var executeAlmostSameNameApps = appsSameNameAsCurrent
                                            .Take(CommonVars.SuggestHighestTake)
                                            .ToList();

            var sameNameIds = executeAlmostSameNameApps.Select(n => n.AppID).ToArray();


            // add condition to reduce the redundant apps in the suggestion.
            AddConditionOfRemovingPreviousFoundIDs(ref appsNameSimilariesWithAnd, sameNameIds);
            // add tag conditions
            AddTagFindingCondition(ref appsNameSimilariesWithAnd, tagIds);
            // add ordering
            AddOrderingForSuggestions(ref appsNameSimilariesWithAnd, isMosRecentOrBasedOnPopularity: true);

            var executeSimilarNamesAppsAnd = appsNameSimilariesWithAnd
                // exclude blocked or not published
                                            .Where(n => n.IsPublished && !n.IsBlocked)
                                            .Take(CommonVars.SuggestHighestTake)
                                            .ToList();


            var similarNameAndQueryIds = executeSimilarNamesAppsAnd
                                            .Select(n => n.AppID)
                                            .ToArray();

            List<App> executeSimilarAppsPostedByCurrentUser = null;
            long[] usersAppsIds = null;
            if (userId != -1) {

                // add condition to reduce the redundant apps in the suggestion.
                AddConditionOfRemovingPreviousFoundIDs(ref appsNameSimilariesWithAnd, similarNameAndQueryIds);
                // add tag conditions
                AddTagFindingCondition(ref appsNameSimilariesWithAnd, tagIds);
                // add ordering
                AddOrderingForSuggestions(ref appsNameSimilariesWithAnd, isMosRecentOrBasedOnPopularity: false);


                executeSimilarAppsPostedByCurrentUser = appsNameSimilariesWithAnd
                    // exclude blocked or not published
                                                .Where(n => n.PostedByUserID == userId)
                                                .Take(CommonVars.SuggestHighestTake)
                                                .ToList();

                //usersAppsIds = executeSimilarAppsPostedByCurrentUser.Select(n => n.AppID).ToArray();
            }



            List<App> executeSimilarNamesAppsOr = null;



            //if ((executeSimilarNamesAppsAnd != null && executeSimilarNamesAppsAnd.Count < 6) || executeSimilarNamesAppsAnd == null) {

            //    if (userID == -1) {
            //        // user doesn't exist
            //        // add condition to reduce the redundant apps in the suggestion.
            //        addConditionOfRemovingPreviousFoundIDs(appsNameSimilariesWithOr, sameNameIds, similarNameAndQueryIds, usersAppsIds);
            //        // add tag conditions
            //        addTagFindingCondition(appsNameSimilariesWithOr, tagIds);
            //        // add ordering
            //        addOrderingForSuggestions(appsNameSimilariesWithOr, isMosRecentOrBasedOnPopularity: false);
            //    } else {
            //        // user exist
            //        // all other is already added in the user section
            //        addConditionOfRemovingPreviousFoundIDs(appsNameSimilariesWithOr, usersAppsIds);
            //    }


            //    executeSimilarNamesAppsOr = appsNameSimilariesWithOr
            //        // exclude blocked or not published
            //                                .Where(n => n.IsPublished && !n.IsBlocked)
            //                                .Take(CommonVars.SUGGEST_HIGHEST_TAKE)
            //                                .ToList();
            //}

            return FormalizeAppsListFromSeveralLogics(executeAlmostSameNameApps, executeSimilarAppsPostedByCurrentUser, executeSimilarNamesAppsAnd, executeSimilarNamesAppsOr);
        }

        /// <summary>
        /// If previous any found with same id then exclude those.
        /// </summary>
        /// <param name="apps"></param>
        /// <param name="foundIds1"></param>
        /// <param name="foundIds2"></param>
        /// <param name="foundIds3"></param>
        /// <param name="foundIds4"></param>
        /// <returns></returns>
        void AddConditionOfRemovingPreviousFoundIDs(ref IQueryable<App> apps, long[] foundIds1, long[] foundIds2 = null, long[] foundIds3 = null, long[] foundIds4 = null) {
            apps = apps.Where(n => !foundIds1.Any(id => id == n.AppID));
            if (foundIds2 != null) {
                apps = apps.Where(n => !foundIds2.Any(id => id == n.AppID));
            }

            if (foundIds3 != null) {
                apps = apps.Where(n => !foundIds3.Any(id => id == n.AppID));
            }

            if (foundIds4 != null) {
                apps = apps.Where(n => !foundIds4.Any(id => id == n.AppID));
            }

        }

        void AddTagFindingCondition(ref IQueryable<App> apps, List<long> tagIds) {
            if (tagIds != null) {
                apps = apps.Where(singleApp =>
                                tagIds.Any(tagId =>
                                    singleApp
                                    .TagAppRelations
                                    .Any(tagRel => tagRel.TagID == tagId)));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apps"></param>
        /// <param name="isMosRecentOrBasedOnPopularity">True: Find records which is most recent + most viewed. False : means most viewed the most rated and new</param>
        void AddOrderingForSuggestions(ref IQueryable<App> apps, bool isMosRecentOrBasedOnPopularity) {
            if (isMosRecentOrBasedOnPopularity) {
                apps = apps.OrderByDescending(n => n.AppID)
                           .ThenByDescending(n => n.TotalViewed)
                           .ThenByDescending(n => n.AvgRating)
                           .ThenByDescending(n => n.ReviewsCount);
            } else {
                // based on popularity
                apps = apps
                          .OrderByDescending(n => n.TotalViewed)
                          .ThenByDescending(n => n.AvgRating)
                          .ThenByDescending(n => n.AppID)
                          .ThenByDescending(n => n.ReviewsCount);
            }
            //return apps;
        }

        public List<App> FormalizeAppsListFromSeveralLogics(List<App> similarName, List<App> postedByUser, List<App> almostSimilarNameWithAnd, List<App> almostSimilarNameWithOr) {
            var apps = new List<App>(CommonVars.SuggestHighestDisplayNumberSuggestions + 10);

            if (similarName != null) {
                var conditionNumber = CommonVars.SuggestHighestSameAppName;
                var length = similarName.Count > conditionNumber ?
                            conditionNumber : similarName.Count;

                for (var i = 0; i < length; i++) {
                    var current = similarName[i];
                    apps.Add(current);
                }
            }

            if (postedByUser != null && apps.Count < CommonVars.SuggestHighestDisplayNumberSuggestions) {
                var conditionNumber = CommonVars.SuggestHighestFromSameUser;
                var length = postedByUser.Count > conditionNumber ?
                            conditionNumber : postedByUser.Count;

                for (var i = 0; i < length; i++) {
                    var current = postedByUser[i];
                    if (!apps.Any(n => n.AppID == current.AppID)) {
                        apps.Add(current);
                    }
                }
            }

            if (almostSimilarNameWithAnd != null && apps.Count < CommonVars.SuggestHighestDisplayNumberSuggestions) {
                var conditionNumber = CommonVars.SuggestHighestAndSimilarQuery;
                var length = almostSimilarNameWithAnd.Count > conditionNumber ?
                            conditionNumber : almostSimilarNameWithAnd.Count;

                for (var i = 0; i < length; i++) {
                    var current = almostSimilarNameWithAnd[i];
                    if (!apps.Any(n => n.AppID == current.AppID)) {
                        apps.Add(current);
                    }
                }
            }

            if (almostSimilarNameWithOr != null && apps.Count < CommonVars.SuggestHighestDisplayNumberSuggestions) {
                var conditionNumber = CommonVars.SuggestHighestOrSimilarQuery;
                var length = almostSimilarNameWithOr.Count > conditionNumber ?
                            conditionNumber : almostSimilarNameWithOr.Count;

                for (var i = 0; i < length; i++) {
                    var current = almostSimilarNameWithOr[i];
                    if (!apps.Any(n => n.AppID == current.AppID)) {
                        apps.Add(current);
                    }
                }
            }
            return apps;
        }
        #endregion

        #endregion

        #region Embed Images & Icons Code

        #region Get Suggested Embed Icons
        /// <summary>
        /// First call GetSuggestedApps() get the list.
        /// Put the list in this method to get embed suggested icons
        /// Embed Suggested Icons with Apps
        /// </summary>
        /// <param name="apps"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public void GetEmbededSuggestedIconsWithApps(List<App> apps, WereViewAppEntities db) {
            if (apps != null && apps.Count > 0) {
                var guidsStringList = GetGuidStringConcat(apps);
                if (guidsStringList == "") {
                    return;
                }
                var sql = string.Format("SELECT * FROM Gallery WHERE UploadGuid IN ({0}) AND GalleryCategoryID ={1}", guidsStringList, GalleryCategoryIDs.SuggestionIcon);
                if (string.IsNullOrEmpty(guidsStringList)) {
                    return;
                }
                var galleries = db.Database.SqlQuery<Gallery>(sql)
                      .Take(CommonVars.SuggestHighestDisplayNumberSuggestions)
                      .ToList();


                foreach (var gallery in galleries) {

                    var tempApp = apps.FirstOrDefault(n => n.UploadGuid == gallery.UploadGuid);
                    var location = gallery.GetHtppUrl();

                    if (tempApp != null) {
                        tempApp.SuggestionIconLocation = location;
                    }
                }
            }
        }
        #endregion

        #region Gallery Images Embed.
        /// <summary>
        /// Final One:
        /// Embed gallery images with current app.
        /// Include gallery image location + icon.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="db"></param>
        public void GetEmbedGalleryImagesWithCurrentApp(App app, WereViewAppEntities db) {
            if (app != null) {
                var guid = app.UploadGuid;
                var galleriesWithApp = db.Galleries.Where(n => n.UploadGuid == guid &&
                                                              n.GalleryCategoryID == GalleryCategoryIDs.AppPageGallery)
                                                              .AsParallel()
                                                              .AsEnumerable()
                                                              .Select(n => new DisplayGalleryImages {
                                                                  GalleryImageLocation = n.GetHtppUrl(null),
                                                                  GalleryIconLocation = n.GetHtppUrl(GalleryCategoryIDs.GalleryIcon),
                                                                  GalleryID = n.GalleryID,
                                                                  Sequence = n.Sequence,
                                                                  Title = n.Title,
                                                                  Subtitle = n.Subtitle

                                                              })
                                                              .Take(AppVar.Setting.GalleryMaxPictures)
                                                              .ToList();

                if (galleriesWithApp != null) {
                    galleriesWithApp = galleriesWithApp.OrderBy(n => n.Sequence).ToList();

                    app.AppDetailsGalleryImages = galleriesWithApp;
                }
            }

        }
        #endregion

        #region Any Image Embed.
        /// <summary>
        /// Don't work for App Gallery, Gallery Thumb.
        /// GalleryCategoryIDs.HomePageFeatured
        /// GalleryCategoryIDs.HomePageIcon
        /// GalleryCategoryIDs.SearchIcon
        /// GalleryCategoryIDs.SuggestionIcon
        /// GalleryCategoryIDs.YoutubeCoverImage
        /// </summary>
        /// <param name="apps"></param>
        /// <param name="db"></param>
        /// <param name="totalTakeCount"></param>
        /// <param name="categoryId">
        /// if (tempApp != null) {
        ///    if (categoryId == GalleryCategoryIDs.HomePageFeatured) {
        ///        tempApp.HomeFeaturedBigImageLocation = location;
        ///    } else if (categoryId == GalleryCategoryIDs.HomePageIcon) {
        ///        tempApp.HomePageIconLocation = location;
        ///    } else if (categoryId == GalleryCategoryIDs.SearchIcon) {
        ///        tempApp.SearchIconLocation = location;
        ///    } else if (categoryId == GalleryCategoryIDs.SuggestionIcon) {
        ///        tempApp.SuggestionIconLocation = location;
        ///    } else if (categoryId == GalleryCategoryIDs.YoutubeCoverImage) {
        ///        tempApp.YoutubeCoverImageLocation = location;
        ///    }
        /// }    
        /// </param>

        public void GetEmbedImagesWithApp(ref App app, WereViewAppEntities db, int totalTakeCount, int categoryId) {
            var list = new List<App>(1);
            list.Add(app);
            GetEmbedImagesWithApp(list, db, totalTakeCount, categoryId);
        }
        /// <summary>
        /// Don't work for App Gallery, Gallery Thumb.
        /// GalleryCategoryIDs.HomePageFeatured
        /// GalleryCategoryIDs.HomePageIcon
        /// GalleryCategoryIDs.SearchIcon
        /// GalleryCategoryIDs.SuggestionIcon
        /// GalleryCategoryIDs.YoutubeCoverImage
        /// </summary>
        /// <param name="apps"></param>
        /// <param name="db"></param>
        /// <param name="totalTakeCount"></param>
        /// <param name="categoryId">
        /// if (tempApp != null) {
        ///    if (categoryId == GalleryCategoryIDs.HomePageFeatured) {
        ///        tempApp.HomeFeaturedBigImageLocation = location;
        ///    } else if (categoryId == GalleryCategoryIDs.HomePageIcon) {
        ///        tempApp.HomePageIconLocation = location;
        ///    } else if (categoryId == GalleryCategoryIDs.SearchIcon) {
        ///        tempApp.SearchIconLocation = location;
        ///    } else if (categoryId == GalleryCategoryIDs.SuggestionIcon) {
        ///        tempApp.SuggestionIconLocation = location;
        ///    } else if (categoryId == GalleryCategoryIDs.YoutubeCoverImage) {
        ///        tempApp.YoutubeCoverImageLocation = location;
        ///    }
        /// }    
        /// </param>

        public void GetEmbedImagesWithApp(List<App> apps, WereViewAppEntities db, int totalTakeCount, int categoryId) {
            if (apps != null && apps.Count > 0) {
                var guidsStringList = GetGuidStringConcat(apps);
                if (guidsStringList == "") {
                    return;
                }
                var sql = string.Format("SELECT TOP " + totalTakeCount + " * FROM Gallery WHERE UploadGuid IN ({0}) AND GalleryCategoryID ={1}", guidsStringList, categoryId);
                if (string.IsNullOrEmpty(guidsStringList)) {
                    return;
                }
                var galleries = db.Database.SqlQuery<Gallery>(sql)
                      .Take(totalTakeCount)
                      .ToList();

                foreach (var gallery in galleries) {
                    var tempApp = apps.FirstOrDefault(n => n.UploadGuid == gallery.UploadGuid);
                    var location = gallery.GetHtppUrl();
                    if (tempApp != null) {
                        if (categoryId == GalleryCategoryIDs.HomePageFeatured) {
                            tempApp.HomeFeaturedBigImageLocation = location;
                        } else if (categoryId == GalleryCategoryIDs.HomePageIcon) {
                            tempApp.HomePageIconLocation = location;
                        } else if (categoryId == GalleryCategoryIDs.SearchIcon) {
                            tempApp.SearchIconLocation = location;
                        } else if (categoryId == GalleryCategoryIDs.SuggestionIcon) {
                            tempApp.SuggestionIconLocation = location;
                        } else if (categoryId == GalleryCategoryIDs.YoutubeCoverImage) {
                            tempApp.YoutubeCoverImageLocation = location;
                        }
                    }
                }
            }
        }
        #endregion

        #endregion

        #region Remove Output Cahces
        public void RemoveOutputCache(string url) {
            HttpResponse.RemoveOutputCacheItem(url);
        }

        public void RemoveOutputCacheApp(App app) {
            HttpResponse.RemoveOutputCacheItem("/" + app.GetAppUrlWithoutHostName());
        }

        //public void RemoveOutputCacheReview(App app) {
        //    HttpResponse.RemoveOutputCacheItem(CommonVars.OUTPUTCAHE_REVIEWSDISPLAY_APPS + app.AppID);
        //}



        public void RemoveOutputCacheSuggested() {
            HttpResponse.RemoveOutputCacheItem(CommonVars.OutputcaheSuggestedApps);
        }

        public void RemoveOutputCacheFeatured() {
            HttpResponse.RemoveOutputCacheItem(CommonVars.OutputcaheFeaturedappsApps);
        }

        public void RemoveOutputCacheLatest() {
            HttpResponse.RemoveOutputCacheItem(CommonVars.OutputcaheLatestappslistApps);
        }

        public void RemoveOutputCacheTopRated() {
            HttpResponse.RemoveOutputCacheItem(CommonVars.OutputcaheTopappslistApps);
        }


        public void RemoveDonutCaching(string controllerName) {
            var cacheManager = new OutputCacheManager();
            cacheManager.RemoveItems(controllerName);
        }
        public void RemoveDonutCaching(string controllerName, string action) {
            var cacheManager = new OutputCacheManager();
            cacheManager.RemoveItems(controllerName, action);
        }
        public void RemoveDonutCaching(string controllerName, string action, object routes) {
            var cacheManager = new OutputCacheManager();
            cacheManager.RemoveItems(controllerName, action, routes);
        }
        #endregion

        #region Remove Cache

        public void RemoveCachingApp(long appId) {
            RemoveSingleAppFromCacheOfStatic(appId);
            var userId = UserManager.GetLoggedUserId();
            var cacheId = CacheNames.EditingApp + appId + "-" + userId;
            AppConfig.Caches[cacheId] = null;

        }
        #endregion
    }
}