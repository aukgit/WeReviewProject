﻿using DevMVCComponent.Database;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WereViewApp.AlgorithmsWereViewApp;
using WereViewApp.Models.Context;
using WereViewApp.Models.EntityModel;
using WereViewApp.Models.EntityModel.Structs;
using WereViewApp.Modules.Cache;
using WereViewApp.Modules.DevUser;

namespace WereViewApp.Controllers {
    public class ProfileController : Controller {
        private const int MaxNumbersOfPagesShow = 8;

        [OutputCache(CacheProfile = "Day", VaryByParam = "page")]
        public ActionResult Index(int page = 1) {
            //var db2 = new ApplicationDbContext();

            var users = UserManager
                        .GetAllUsersAsIQueryable();

                        
            var cachePagesString = AppConfig.Caches.Get(CacheNames.ProfilePaginationDataCount);
            int count = -1;
            if (cachePagesString == null) {
                count = users.Select(n => n.Id).Count();
            } else {
                count = (int)cachePagesString;
            }
            // add ordered by
            users = users.OrderByDescending(n => n.Id);
            var pageInfo = new PaginationInfo() {
                ItemsInPage = AppConfig.Setting.PageItems + 40,
                PageNumber = page,
                PagesExists = count
            };
            var usersForThisPage = users.GetPageData(pageInfo, CacheNames.ProfilePaginationDataCount,false).ToList();
            const string eachUrl = "/Profile?page=@page";
            ViewBag.paginationHtml = Pagination.GetList(pageInfo, eachUrl, "", maxNumbersOfPagesShow: MaxNumbersOfPagesShow);
            return View(usersForThisPage);
        }
        // GET: Profile/username
        [OutputCache(CacheProfile = "Day", VaryByParam = "username,page")]
        public ActionResult GetProfile(string username, int page = 1) {
            if (!string.IsNullOrWhiteSpace(username)) {
                var algorithms = new Algorithms();

                var user = UserManager.GetUser(username);
                if (user != null) {
                    using (var db = new WereViewAppEntities()) {
                        var apps = db.Apps
                            .Where(n => n.PostedByUserID == user.UserID)
                            .Include(n => n.User)
                            .OrderByDescending(n => n.AppID);

                        var pageInfo = new PaginationInfo() {
                            ItemsInPage = AppConfig.Setting.PageItems,
                            PageNumber = page
                        };
                        var appsForThisPage = apps.GetPageData(pageInfo, CacheNames.ProfilePaginationDataForSpecificProfile, true).ToList();
                        algorithms.GetEmbedImagesWithApp(appsForThisPage, db, (int)pageInfo.ItemsInPage, GalleryCategoryIDs.SearchIcon);
                        ViewBag.Apps = appsForThisPage;
                        string eachUrl = "/Profile/" + user.UserName + "/@page";
                        ViewBag.paginationHtml = Pagination.GetList(pageInfo, eachUrl, "", maxNumbersOfPagesShow: MaxNumbersOfPagesShow);
                        return View(user);
                    }
                }
            }
            ViewBag.Reason = "User not found.";
            return View("_NotExist");
        }
    }
}