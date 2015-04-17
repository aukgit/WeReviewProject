using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using LinqKit;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.EntityModel.ExtenededWithCustomMethods;
using WereViewApp.Models.EntityModel.Structs;
using WereViewApp.Models.ViewModels;
using WereViewApp.Modules.DevUser;
using WereViewApp.WereViewAppCommon.Structs;

namespace WereViewApp.WereViewAppCommon {
    public class Algorithms {

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
                category.Apps = db.Apps.Include(n=> n.User).OrderByDescending(n=> n.AppID).Where(n => n.CategoryID == category.CategoryID).Take(eachSlotAppsNumber).ToList();
                if (category.Apps != null && category.Apps.Count > 0) {
                    GetEmbedImagesWithApp((List<App>)category.Apps, db,eachSlotAppsNumber,GalleryCategoryIDs.SearchIcon);
                }
            }
            return categories;
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

        #region Home Page Related Queries: Top, Latest, Suggested, Home Page Gallery, Advertise

        #region Latest Apps With Icons
        public List<App> GetLatestApps(WereViewAppEntities db, int max) {
            var apps = db.Apps
                .Include(n => n.Platform)
                .Include(n => n.User)
                .OrderByDescending(n => n.AppID)
                .Where(n => n.IsPublished && !n.IsBlocked)
                .Take(max).ToList();
            if (apps != null) {
                GetEmbedImagesWithApp(apps, db, max, GalleryCategoryIDs.HomePageIcon);
            }
            return apps;
        }

        #endregion

        #region Top Rated Apps with Icons
        /// <summary>
        /// include icons
        /// </summary>
        /// <param name="db"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public List<App> GetTopRatedApps(WereViewAppEntities db, int max) {
            var apps = db.Apps
                         .Include(n => n.Platform)
                         .Include(n => n.User)
                         .Where(n => n.IsPublished && !n.IsBlocked);

            addOrderingForSuggestions(ref apps, isMosRecent: false);
            var list = apps.Take(max).ToList();
            if (list != null) {
                GetEmbedImagesWithApp(list, db, max, GalleryCategoryIDs.HomePageIcon);
            }
            return list;
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

        #region Home page gallery

        /// <summary>
        /// Returns apps which are related to home page gallery 
        /// Use GalleryHomeFeaturedLocation to src display image.
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

        #region Get App From Cache
        public App GetAppFromStaticCache(long appID) {
            if (CommonVars.AppsFoundForSingleDisplay != null) {
                return CommonVars.AppsFoundForSingleDisplay.FirstOrDefault(n => n.AppID == appID);
            }
            return null;
        }
        #endregion

        #region Search Algorithm

        /// <summary>
        /// Returns the search no caching
        /// </summary>
        /// <param name="searchText">Plant Vs. Zombies</param>
        /// <param name="rating"></param>
        /// <param name="platform">apple</param>
        /// <param name="tags">tag1,tag2...</param>
        /// <param name="db"></param>
        /// <returns></returns>
        public List<App> GetSearchResults(string searchText, float? rating, string platform, string tags, int max, WereViewAppEntities db) {
            //var hash = DevHash.Get(searchText, rating, platform, tags);
            //var cacheReaderSaver = new CacheDataInFile(CommonVars.APP_SEARCH_RESULTS_ADDITIONALPATH);
            //var cache = cacheReaderSaver.ReadObjectFromBinaryFileAsCache(hash, CommonVars.APP_SEARCH_RESULTS_EXPIRE_IN_HOURS);
            //if (cache == null) {
            // cache doesn't exist
            var results = AppSearchAlgorithm(searchText, rating, platform, tags, CommonVars.SEARCH_RESULTS_MAX_RESULT_RETURN, db);
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

            IQueryable<App> appsSameName = null;
            IQueryable<App> appsSimilarNameAnd = null;

            var url = GenerateURLValid(searchText);
            var validUrlList = GetURLListExceptEscapeSequence(url);

            var platformObject = WereViewStatics.AppPlatformsCache.FirstOrDefault(n => n.PlatformName == platform);
            byte? platformId = null;
            if (platformObject != null) {
                platformId = platformObject.PlatformID;
            }
            if (searchText != null) {
                searchText = searchText.Trim();
                if (searchText.Length > 2) {
                    appsSameName = db.Apps.Where(n => n.AppName.StartsWith(searchText));
                }

                appsSimilarNameAnd = db.Apps;

                foreach (var singleValidURL in validUrlList) {

                    appsSimilarNameAnd = appsSimilarNameAnd
                                        .Where(n =>
                                            n.URLWithoutEscapseSequence.StartsWith(singleValidURL + "-") ||
                                            n.URLWithoutEscapseSequence.Contains("-" + singleValidURL + "-") ||
                                            n.URLWithoutEscapseSequence.EndsWith("-" + singleValidURL)
                                         );
                }
            }
            if (appsSameName == null) {
                appsSameName = db.Apps;
            }
            addConditionsForSearch(ref appsSameName, tagIds, rating, platformId);
            addOrderingForSuggestions(ref appsSameName, isMosRecent: true);
            executeAppsWithSameName = appsSameName.Include(n => n.User).ToList();

            var sameIds = executeAppsWithSameName.Select(n => n.AppID).ToArray();


            addConditionsForSearch(ref appsSimilarNameAnd, tagIds, rating, platformId);
            addOrderingForSuggestions(ref appsSimilarNameAnd, isMosRecent: true);
            addConditionOfRemovingPreviousFoundIDs(ref appsSimilarNameAnd, sameIds, null);

            int getSimilarMax = maxCount / 2;

            executeAppsWithSimilarNameAnd = appsSimilarNameAnd
                .Include(n => n.User)
                .Take(getSimilarMax)
                .ToList();
            return MergeSearchResultsLists(executeAppsWithSameName, executeAppsWithSimilarNameAnd, null, null, maxCount * 2);

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
        /// <param name="platformID"></param>
        void addConditionsForSearch(ref IQueryable<App> apps, List<long> tagsIDs = null, float? rating = null, byte? platformID = null) {
            if (tagsIDs != null) {
                apps = apps.Where(n => tagsIDs.All(tagId => n.TagAppRelations.Any(tagRel => tagRel.TagID == tagId)));
            }
            if (rating != null) {
                var rate = (float)rating;
                apps = apps.Where(a => a.AvgRating >= rate);
            }

            if (platformID != null) {
                var id = (byte)platformID;

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
            new Thread(() => {
                var appSavingFields = new AppSavingTextFields();
                appSavingFields.Developers = app.Developers;
                appSavingFields.IdeaBy = app.IdeaBy;
                appSavingFields.Publishers = app.Publishers;
                appSavingFields.Tags = app.Tags;
                appSavingFields.UploadGuid = app.UploadGuid;
                WereViewStatics.SavingAppInDirectory(appSavingFields);

            }).Start();
        }
        #endregion

        #region Review & Rating

        #region Fix Rating in App
        public void FixRatingInApp(long appId, WereViewAppEntities db, App app = null) {
            var reviewExist = db.Reviews.FirstOrDefault();
            if (reviewExist != null) {
                var avg = db.Reviews.Where(n => n.AppID == appId).Average(n => n.Rating);
                int i = db.Database.ExecuteSqlCommand("UPDATE APP SET AvgRating = @p0 WHERE AppID = @p1", avg, appId);
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
        /// <param name="AppID"></param>
        /// <param name="db"></param>
        /// <returns>Returns true if did review the app.</returns>
        public bool IsCurrentUserReviewedThisApp(long AppID, WereViewAppEntities db) {
            var userid = UserManager.GetLoggedUserId();
            return db.Reviews.Any(n => n.AppID == AppID && n.UserID == userid);
        }

        #endregion


        #region get user reviewed app
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="db"></param>
        /// <returns>Returns true if did review the app.</returns>
        public Review GetUserReviewedApp(long AppID, WereViewAppEntities db) {
            var userid = UserManager.GetLoggedUserId();
            return db.Reviews.FirstOrDefault(n => n.AppID == AppID && n.UserID == userid);
        }

        #endregion

        #endregion

        #region Fix Iframe Tag
        string getSqureIFrameTag(string str) {
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


        string getRawIframeString(string str) {
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
        /// Reading from binary
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
        public void AddNotification(long userId, byte NotificationTypeID, string msg, WereViewAppEntities db) {
            var type = WereViewStatics.GetNotificationType(NotificationTypeID);

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
        /// <param name="UserSettingsId">UserPointsSettingIDs.PostApp</param>
        /// <param name="db"></param>
        public void AddPoints(App app, byte UserSettingsId, WereViewAppEntities db) {
            var point = WereViewStatics.GetUserSettingPoint(UserSettingsId);
            var userId = UserManager.GetLoggedUserId();

            var userPoint = new UserPoint();
            userPoint.UserID = userId;
            userPoint.Point = point.Point;
            userPoint.Dated = DateTime.Now;
            userPoint.UserPointSettingID = point.UserPointSettingID;
            db.UserPoints.Add(userPoint);
            if (db.SaveChanges() > 0) {
                int i = db.Database
                    .ExecuteSqlCommand(
                    "UPDATE User SET TotalEarnedPoints = TotalEarnedPoints + @p0 WHERE UserId = @p1",
                    point.Point,
                    userId);

            }

        }



        #endregion

        #region Force App Review to Load
        public void ForceAppReviewToLoad(long appId) {
            var app = GetAppFromStaticCache(appId);
            if (app != null) {
                app.IsReviewLoaded = false;
            }
        }
        #endregion

        #region Load Review into app

        /// <summary>
        /// Only load reviews if needed.
        /// Based on app.IsReviewLoaded prop
        /// </summary>
        /// <param name="app"></param>
        /// <param name="maxReviewLoad"></param>
        /// <param name="db"></param>
        public void LoadReviewIntoApp(App app, int maxReviewLoad, WereViewAppEntities db) {
            if (app != null) {
                var appId = app.AppID;
                if (app.IsReviewLoaded == false) {
                    app.Reviews = db.Reviews
                        //.Include(n => n.ReviewLikeDislikes)
                                    .Include(n => n.User)
                                    .OrderByDescending(n => n.LikedCount)
                                    .OrderBy(n => n.DisLikeCount)
                                    .Where(n => n.AppID == appId)
                                    .Take(maxReviewLoad)
                                    .ToList();
                    app.ReviewsCount = (short)db.Reviews.Count(n => n.AppID == appId);
                    app.IsReviewLoaded = true;
                }
            }
        }
        #endregion

        #region Single App on App Page: Display
        /// <summary>
        /// First try to get the app from the static list.
        /// Static app list contain 500 of apps in memory.
        /// To remove cache static call RemoveSingleAppFromCacheOfStatic()
        /// If static app is not found.
        /// Then get the app from db and attach gallery images and icons with it.
        /// and then save it into the cache.
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="platformVersion"></param>
        /// <param name="category"></param>
        /// <param name="url"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public App GetSingleAppForDisplay(string platform, float platformVersion, string category, string url, int maxReviewLoad, WereViewAppEntities db) {
            if (platform != null && platformVersion != null && category != null && url != null) {
                if (CommonVars.AppsFoundForSingleDisplay == null) {
                    CommonVars.AppsFoundForSingleDisplay = new List<App>(800);
                }
                App app = null;
                var platformO = WereViewStatics.AppPlatformsCache.FirstOrDefault(n => n.PlatformName.Equals(platform));
                var categoryO = WereViewStatics.AppCategoriesCache.FirstOrDefault(n => n.CategoryName.Equals(category));
                if (platformO != null && categoryO != null) {
                    var platformID = platformO.PlatformID;
                    var categoryId = categoryO.CategoryID;
                    app = CommonVars.AppsFoundForSingleDisplay
                                    .FirstOrDefault(n =>
                                        n.URL.Equals(url) &&
                                        n.PlatformID == platformID &&
                                        n.CategoryID == categoryId);

                    // found in  the static cache.
                    if (app != null) {
                        var appId = app.AppID;
                        app.Category = categoryO;
                        app.Platform = platformO;
                        if (UserManager.IsAuthenticated()) {
                            var userId = UserManager.GetLoggedUserId();

                            var currentUserRated = db.Reviews.FirstOrDefault(n => n.AppID == appId && n.UserID == userId);
                            if (currentUserRated != null) {
                                app.CurrentUserRatedAppValue = currentUserRated.Rating;
                            } else {
                                app.CurrentUserRatedAppValue = null;
                            }
                        }
                        LoadReviewIntoApp(app, maxReviewLoad, db);
                        return app;
                    }
                    // app not found.
                    // search in db
                    app = db.Apps
                            .Include(n => n.User)
                            .Where(n => n.IsPublished && !n.IsBlocked)
                            .FirstOrDefault(n =>
                            n.URL.Equals(url) &&
                            n.PlatformID == platformID &&
                            n.CategoryID == categoryId);
                    if (app != null) {
                        app.Category = categoryO;
                        app.Platform = platformO;
                        app.YoutubeEmbedLink = getRawIframeString(app.YoutubeEmbedLink);
                        var appId = app.AppID;

                        LoadReviewIntoApp(app, maxReviewLoad, db);

                        if (UserManager.IsAuthenticated()) {
                            var userId = UserManager.GetLoggedUserId();

                            var currentUserRated = db.Reviews.FirstOrDefault(n => n.AppID == appId && n.UserID == userId);
                            if (currentUserRated != null) {
                                app.CurrentUserRatedAppValue = currentUserRated.Rating;
                            } else {
                                app.CurrentUserRatedAppValue = null;
                            }
                        }
                        ReadVirtualFields(app);
                        // adding gallery images with app
                        GetEmbedGalleryImagesWithCurrentApp(app, db);
                        if (CommonVars.AppsFoundForSingleDisplay.Count > 795) {
                            CommonVars.AppsFoundForSingleDisplay.RemoveRange(0, 200);
                        }
                        // saving app into the static
                        CommonVars.AppsFoundForSingleDisplay.Add(app);
                        return app;
                    }
                }
            }
            return null;
        }

        #endregion

        #region Increase App View Count

        public bool IncreaseViewCount(App app, WereViewAppEntities db) {
            var appid = app.AppID;
            int i = db.Database.ExecuteSqlCommand("UPDATE APP SET TotalViewed = TotalViewed+1 WHERE AppID = @p0", appid);
            if (i > 0) {
                app.TotalViewed += 1;
                return true;
            }
            return false;
        }

        #endregion

        #region Increase App Review Count

        public bool IncreaseReviewCount(long appId, WereViewAppEntities db, App app = null) {
            var appid = appId;

            var reviewCount = db.Reviews.Count(n => n.AppID == appid);
            if (reviewCount > 32000) {
                return true;
            }
            int i = db.Database.ExecuteSqlCommand("UPDATE APP SET ReviewsCount = @p0 WHERE AppID = @p1", reviewCount, appid);
            if (i > 0) {
                if (app != null) {
                    app.ReviewsCount += 1;
                }
                return true;
            }
            return false;
        }

        #endregion

        #region Remove single app from cache of static
        /// <summary>
        /// Only call when an app is edited.
        /// Remove the app from CommonVars.AppsFoundForSingleDisplay
        /// </summary>
        /// <param name="app"></param>
        /// <param name="db"></param>
        public void RemoveSingleAppFromCacheOfStatic(App app, WereViewAppEntities db) {
            if (CommonVars.AppsFoundForSingleDisplay != null) {
                var find = CommonVars.AppsFoundForSingleDisplay.FirstOrDefault(n => n.AppID == app.AppID);
                if (find != null) {
                    CommonVars.AppsFoundForSingleDisplay.Remove(find);
                }
            }
        }
        #endregion

        #region Get  URL Without Escapse Sequence.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">title-tile-2</param>
        /// <returns></returns>
        public List<string> GetURLListExceptEscapeSequence(string url) {
            if (url != null) {
                var urlList = url.Split('-');

                List<string> validURL = new List<string>(urlList.Length);

                foreach (var valid in urlList) {
                    if (!CommonVars.SearchingEscapeSequence.Any(escapse => escapse.Equals(valid))) {
                        bool isNumber = Regex.IsMatch(valid, @"^\d+$");
                        if (!isNumber) {
                            validURL.Add(valid);
                        }
                    }
                }
                return validURL;

            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">title-tile-2</param>
        /// <returns></returns>
        public string GetURLStringExceptEscapeSequence(string url) {
            if (url != null) {
                List<string> validURL = GetURLListExceptEscapeSequence(url);
                string returnStr = null;
                returnStr = string.Join("-", validURL);
                return returnStr;
            }
            return null;
        }
        #endregion

        #region Generate URL

        #region Generate Valid
        /// <summary>
        /// Create url but also check if existing one is there.
        /// and add 2 if same one exist.
        /// </summary>
        /// <param name="platformVersion"></param>
        /// <param name="categoryId"></param>
        /// <param name="title"></param>
        /// <param name="platformId"></param>
        /// <returns></returns>
        public string GenerateURLValid(double platformVersion, short categoryId, string title, byte platformId, WereViewAppEntities db, long currentAppId) {
            if (!string.IsNullOrEmpty(title)) {
                title = title.Trim();
                title = Regex.Replace(title, CommonVars.FRIENDLY_URL_REGEX, "-").ToLower();
            checkAgain:
                bool exist = false;
                if (currentAppId < 1) {
                    exist = db.Apps.Any(n => n.PlatformVersion == platformVersion && n.CategoryID == categoryId && n.URL == title && n.PlatformID == platformId);
                } else {
                    exist = db.Apps.Any(n => n.AppID != currentAppId && n.PlatformVersion == platformVersion && n.CategoryID == categoryId && n.URL == title && n.PlatformID == platformId);

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

        #endregion

        #region Generate URL not valid one
        /// <summary>
        /// Create url , don't check if exist one
        /// </summary>
        /// <param name="platformVersion"></param>
        /// <param name="categoryId"></param>
        /// <param name="title"></param>
        /// <param name="platformId"></param>
        /// <returns></returns>
        public string GenerateURLValid(string title) {
            if (!string.IsNullOrEmpty(title)) {
                title = title.Trim();
                title = Regex.Replace(title, CommonVars.FRIENDLY_URL_REGEX, "-").ToLower();
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
            var appid = app.AppID.ToString();
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
            if (apps != null) {
                var guidsStringList = GetGuidStringConcat(apps);
                string sql = string.Format("SELECT * FROM Gallery WHERE UploadGuid IN ({0}) AND GalleryCategoryID ={1}", guidsStringList, GalleryCategoryIDs.SuggestionIcon);
                if (string.IsNullOrEmpty(guidsStringList)) {
                    return;
                }
                var galleries = db.Database.SqlQuery<Gallery>(sql)
                      .Take(CommonVars.SUGGEST_HIGHEST_DISPLAY_NUMBER_SUGGESTIONS)
                      .ToList();


                foreach (var gallery in galleries) {

                    var tempApp = apps.FirstOrDefault(n => n.UploadGuid == gallery.UploadGuid);
                    string location = gallery.GetHtppUrl();

                    if (tempApp != null) {
                        tempApp.GallerySuggestionIconLocation = location;
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

                    app.GalleryGalleryImages = galleriesWithApp;
                }
            }

        }
        #endregion

        #region Any Image Embed.

        /// <summary>
        /// Don't work for App Gallery, Gallery Thumb.
        /// </summary>
        /// <param name="apps"></param>
        /// <param name="db"></param>
        /// <param name="totalTakeCount"></param>
        /// <param name="categoryId">
        /// if (tempApp != null) {
        ///    if (categoryId == GalleryCategoryIDs.HomePageFeatured) {
        ///        tempApp.GalleryHomeFeaturedLocation = location;
        ///    } else if (categoryId == GalleryCategoryIDs.HomePageIcon) {
        ///        tempApp.GalleryHomeIconLocation = location;
        ///    } else if (categoryId == GalleryCategoryIDs.SearchIcon) {
        ///        tempApp.GallerySearchIconLocation = location;
        ///    } else if (categoryId == GalleryCategoryIDs.SuggestionIcon) {
        ///        tempApp.GallerySuggestionIconLocation = location;
        ///    }
        /// }    
        /// </param>

        public void GetEmbedImagesWithApp(List<App> apps, WereViewAppEntities db, int totalTakeCount, int categoryId) {
            if (apps != null) {
                var guidsStringList = GetGuidStringConcat(apps);
                string sql = string.Format("SELECT * FROM Gallery WHERE UploadGuid IN ({0}) AND GalleryCategoryID ={1}", guidsStringList, categoryId);
                if (string.IsNullOrEmpty(guidsStringList)) {
                    return;
                }
                var galleries = db.Database.SqlQuery<Gallery>(sql)
                      .Take(totalTakeCount)
                      .ToList();

                foreach (var gallery in galleries) {
                    var tempApp = apps.FirstOrDefault(n => n.UploadGuid == gallery.UploadGuid);
                    string location = gallery.GetHtppUrl();
                    if (tempApp != null) {
                        if (categoryId == GalleryCategoryIDs.HomePageFeatured) {
                            tempApp.GalleryHomeFeaturedLocation = location;
                        } else if (categoryId == GalleryCategoryIDs.HomePageIcon) {
                            tempApp.GalleryHomeIconLocation = location;
                        } else if (categoryId == GalleryCategoryIDs.SearchIcon) {
                            tempApp.GallerySearchIconLocation = location;
                        } else if (categoryId == GalleryCategoryIDs.SuggestionIcon) {
                            tempApp.GallerySuggestionIconLocation = location;
                        }
                    }
                }
            }
        }
        #endregion

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
            var url = app.URLWithoutEscapseSequence;
            var validUrlList = url.Split('-');


            long userID = app.PostedByUserID;

            // same user same platform and category apps with 
            var appsCollectionNotAsSameId = db.Apps
                .Include(n => n.User)
                .Where(n => n.AppID != app.AppID)
                .Where(n => n.IsPublished && !n.IsBlocked);

            // like starts with query
            var appsSameNameAsCurrent = appsCollectionNotAsSameId
                                        .Where(n => n.URLWithoutEscapseSequence.StartsWith(url));


            IQueryable<App> appsNameSimilariesWithAnd = appsCollectionNotAsSameId;
            foreach (var singleValidURL in validUrlList) {

                appsNameSimilariesWithAnd = appsNameSimilariesWithAnd
                                    .Where(n => n.URLWithoutEscapseSequence.StartsWith(singleValidURL + "-") ||
                                                n.URLWithoutEscapseSequence.Contains("-" + singleValidURL + "-") ||
                                                n.URLWithoutEscapseSequence.EndsWith("-" + singleValidURL)
                                                );
            }

            IQueryable<App> appsNameSimilariesWithOr = appsCollectionNotAsSameId;
            var orPredicate = PredicateBuilder.False<App>();
            foreach (var singleValidURL in validUrlList) {

                orPredicate =
                    orPredicate.Or(n => n.URLWithoutEscapseSequence.StartsWith(singleValidURL + "-") ||
                                    n.URLWithoutEscapseSequence.Contains("-" + singleValidURL + "-") ||
                                    n.URLWithoutEscapseSequence.EndsWith("-" + singleValidURL)
                                    );
            }

            appsNameSimilariesWithOr = appsNameSimilariesWithOr.Where(orPredicate);

            // exclude blocked or not published
            var executeAlmostSameNameApps = appsSameNameAsCurrent
                                            .Take(CommonVars.SUGGEST_HIGHEST_TAKE)
                                            .ToList();

            var sameNameIds = executeAlmostSameNameApps.Select(n => n.AppID).ToArray();


            // add condition to reduce the redundant apps in the suggestion.
            addConditionOfRemovingPreviousFoundIDs(ref appsNameSimilariesWithAnd, sameNameIds);
            // add tag conditions
            addTagFindingCondition(ref appsNameSimilariesWithAnd, tagIds);
            // add ordering
            addOrderingForSuggestions(ref appsNameSimilariesWithAnd, isMosRecent: true);

            var executeSimilarNamesAppsAnd = appsNameSimilariesWithAnd
                // exclude blocked or not published
                                            .Where(n => n.IsPublished && !n.IsBlocked)
                                            .Take(CommonVars.SUGGEST_HIGHEST_TAKE)
                                            .ToList();


            var similarNameAndQueryIds = executeSimilarNamesAppsAnd
                                            .Select(n => n.AppID)
                                            .ToArray();

            List<App> executeSimilarAppsPostedByCurrentUser = null;
            long[] usersAppsIds = null;
            if (userID != -1) {

                // add condition to reduce the redundant apps in the suggestion.
                addConditionOfRemovingPreviousFoundIDs(ref appsNameSimilariesWithAnd, similarNameAndQueryIds);
                // add tag conditions
                addTagFindingCondition(ref appsNameSimilariesWithAnd, tagIds);
                // add ordering
                addOrderingForSuggestions(ref appsNameSimilariesWithAnd, isMosRecent: false);


                executeSimilarAppsPostedByCurrentUser = appsNameSimilariesWithAnd
                    // exclude blocked or not published
                                                .Where(n => n.IsPublished && !n.IsBlocked)
                                                .Where(n => n.PostedByUserID == userID)
                                                .Take(CommonVars.SUGGEST_HIGHEST_TAKE)
                                                .ToList();

                usersAppsIds = executeSimilarAppsPostedByCurrentUser.Select(n => n.AppID).ToArray();
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
            //        addOrderingForSuggestions(appsNameSimilariesWithOr, isMosRecent: false);
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
        /// <param name="FoundIds1"></param>
        /// <param name="FoundIds2"></param>
        /// <param name="FoundIds3"></param>
        /// <param name="FoundIds4"></param>
        /// <returns></returns>
        void addConditionOfRemovingPreviousFoundIDs(ref IQueryable<App> apps, long[] FoundIds1, long[] FoundIds2 = null, long[] FoundIds3 = null, long[] FoundIds4 = null) {
            apps = apps.Where(n => !FoundIds1.Any(id => id == n.AppID));
            if (FoundIds2 != null) {
                apps = apps.Where(n => !FoundIds2.Any(id => id == n.AppID));
            }

            if (FoundIds3 != null) {
                apps = apps.Where(n => !FoundIds3.Any(id => id == n.AppID));
            }

            if (FoundIds4 != null) {
                apps = apps.Where(n => !FoundIds4.Any(id => id == n.AppID));
            }

        }

        void addTagFindingCondition(ref IQueryable<App> apps, List<long> tagIds) {
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
        /// <param name="isMosRecent">True: Find records which is most recent + most viewed. False : means most viewed the most rated and new</param>
        void addOrderingForSuggestions(ref IQueryable<App> apps, bool isMosRecent) {
            if (isMosRecent) {
                apps = apps.OrderByDescending(n => n.AppID)
                           .OrderByDescending(n => n.TotalViewed)
                           .OrderByDescending(n => n.AvgRating)
                           .OrderByDescending(n => n.ReviewsCount);
            } else {
                // based on popularity
                apps = apps
                          .OrderByDescending(n => n.TotalViewed)
                          .OrderByDescending(n => n.AvgRating)
                          .OrderByDescending(n => n.AppID)
                          .OrderByDescending(n => n.ReviewsCount);
            }
            //return apps;
        }

        public List<App> FormalizeAppsListFromSeveralLogics(List<App> SimilarName, List<App> PostedByUser, List<App> AlmostSimilarNameWithAND, List<App> AlmostSimilarNameWithOr) {
            List<App> apps = new List<App>(CommonVars.SUGGEST_HIGHEST_DISPLAY_NUMBER_SUGGESTIONS + 10);

            if (SimilarName != null) {
                int conditionNumber = CommonVars.SUGGEST_HIGHEST_SAME_APP_NAME;
                int length = SimilarName.Count > conditionNumber ?
                            conditionNumber : SimilarName.Count;

                for (int i = 0; i < length; i++) {
                    var current = SimilarName[i];
                    apps.Add(current);
                }
            }

            if (PostedByUser != null && apps.Count < CommonVars.SUGGEST_HIGHEST_DISPLAY_NUMBER_SUGGESTIONS) {
                int conditionNumber = CommonVars.SUGGEST_HIGHEST_FROM_SAME_USER;
                int length = PostedByUser.Count > conditionNumber ?
                            conditionNumber : PostedByUser.Count;

                for (int i = 0; i < length; i++) {
                    var current = PostedByUser[i];
                    if (!apps.Any(n => n.AppID == current.AppID)) {
                        apps.Add(current);
                    }
                }
            }

            if (AlmostSimilarNameWithAND != null && apps.Count < CommonVars.SUGGEST_HIGHEST_DISPLAY_NUMBER_SUGGESTIONS) {
                int conditionNumber = CommonVars.SUGGEST_HIGHEST_AND_SIMILAR_QUERY;
                int length = AlmostSimilarNameWithAND.Count > conditionNumber ?
                            conditionNumber : AlmostSimilarNameWithAND.Count;

                for (int i = 0; i < length; i++) {
                    var current = AlmostSimilarNameWithAND[i];
                    if (!apps.Any(n => n.AppID == current.AppID)) {
                        apps.Add(current);
                    }
                }
            }

            if (AlmostSimilarNameWithOr != null && apps.Count < CommonVars.SUGGEST_HIGHEST_DISPLAY_NUMBER_SUGGESTIONS) {
                int conditionNumber = CommonVars.SUGGEST_HIGHEST_OR_SIMILAR_QUERY;
                int length = AlmostSimilarNameWithOr.Count > conditionNumber ?
                            conditionNumber : AlmostSimilarNameWithOr.Count;

                for (int i = 0; i < length; i++) {
                    var current = AlmostSimilarNameWithOr[i];
                    if (!apps.Any(n => n.AppID == current.AppID)) {
                        apps.Add(current);
                    }
                }
            }
            return apps;
        }
        #endregion


        #region Remove Output Cahces
        public void RemoveOutputCache(string url) {
            HttpResponse.RemoveOutputCacheItem(url);
        }

        public void RemoveOutputCacheApp(App app) {
            HttpResponse.RemoveOutputCacheItem("/" + app.GetAppURLWithoutHostName());
        }

        //public void RemoveOutputCacheReview(App app) {
        //    HttpResponse.RemoveOutputCacheItem(CommonVars.OUTPUTCAHE_REVIEWSDISPLAY_APPS + app.AppID);
        //}



        public void RemoveOutputCacheSuggested() {
            HttpResponse.RemoveOutputCacheItem(CommonVars.OUTPUTCAHE_SUGGESTED_APPS);
        }

        public void RemoveOutputCacheFeatured() {
            HttpResponse.RemoveOutputCacheItem(CommonVars.OUTPUTCAHE_FEATUREDAPPS_APPS);
        }

        public void RemoveOutputCacheLatest() {
            HttpResponse.RemoveOutputCacheItem(CommonVars.OUTPUTCAHE_LATESTAPPSLIST_APPS);
        }

        public void RemoveOutputCacheTopRated() {
            HttpResponse.RemoveOutputCacheItem(CommonVars.OUTPUTCAHE_TOPAPPSLIST_APPS);
        }
        #endregion
    }
}