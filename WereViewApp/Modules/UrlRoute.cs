using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeReviewApp.Modules
{
    public static class UrlRoute {
        public static string HostUrl = AppVar.Url;
        /// <summary>
        /// /User/Reviews/@user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static string ReviewsbyUser(string username) {
            return HostUrl + "/User/Reviews/" + username;
        }
        /// <summary>
        /// /Report/Review/@id
        /// </summary>
        /// <param name="reviewId"></param>
        /// <returns></returns>
        public static string ReportReviewUrl(long reviewId){
            return HostUrl + "/Report/Review/" + reviewId;
        }
    }
}