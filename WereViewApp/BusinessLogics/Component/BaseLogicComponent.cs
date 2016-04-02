using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevMvcComponent.Error;
using WeReviewApp.Models.Context;
using WeReviewApp.Models.EntityModel;

namespace WeReviewApp.BusinessLogics {
    public class BaseLogicComponent {
        protected ApplicationDbContext IdentityDb { get; set; }
        protected WereViewAppEntities WeReviewDb { get; set; }
        public Logics Logics { get; set; }
        internal ErrorCollector ErrorCollector;

        protected BaseLogicComponent(bool identityDbInitialize, bool weReviewDbInitialize, bool logicInitialize, bool initializeErrorCollector) {
            
        }
    }
}