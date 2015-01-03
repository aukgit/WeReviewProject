using WereViewApp.Models.Context;
using WereViewApp.Models.POCO.IdentityCustomization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data.Entity;

namespace WereViewApp.Modules.Menu {
    public class GenerateMenu : IDisposable {
        private StringBuilder _sb = new StringBuilder(150);
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// 0 - name
        /// 1 - drop li items
        /// </summary>
        const string dropDownTemplate = @"<li class='dropdown'>
                                            <a href='#' class='dropdown-toggle' data-toggle='dropdown'>{0} <span class='caret'></span></a>
                                            <ul class='dropdown-menu' role='menu'>
                                              {1}
                                            </ul>
                                          </li>";
        const string dropDownUlTemplete = @"<ul class='dropdown-menu' role='menu'>
                                              {0}
                                            </ul>";
        /// <summary>
        /// 0-href
        /// 1-title attribute
        /// 2-Link display text
        /// </summary>
        const string htmlListItem = @"<li title='{1}'><a href='{0}' title='{1}'>{2}</a></li>";


        public List<NavigationItem> Items { get; set; }
        public Navigation GetMenuItem(string menuName) {
            return db.Navigations.Include(w => w.NavigationItems).FirstOrDefault(n => n.Name == menuName);
        }

        public Navigation GetMenuItem(int navigationId) {
            return db.Navigations.Include(w => w.NavigationItems).FirstOrDefault(n => n.NavigationID == navigationId);
        }

        /// <summary>
        /// Not recursive yet.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="hasDropDown"></param>
        /// <returns></returns>
        public string GenerateRecursiveMenuItems(List<NavigationItem> list, bool firstTime = true, bool hasDropDown = false) {
            if (firstTime) {
                _sb.Clear();
            }
            if (list.Count > 0) {
                string appUrl = AppVar.Url;

                foreach (var item in list) {
                    string url = appUrl + item.RelativeURL;
                    _sb.Append(string.Format(htmlListItem, url, item.Title, item.Title));
                }
            }
            return _sb.ToString();
        }

        public void Dispose() {
            db.Dispose();
        }
    }
}