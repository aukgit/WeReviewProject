using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevMvcComponent.Error;
using WeReviewApp.Models.Context;
using WeReviewApp.Models.EntityModel;
using WeReviewApp.Modules.Extensions.Context;

<<<<<<< HEAD
namespace WeReviewApp.BusinessLogics {
    public class BaseLogicComponent {
=======
namespace WeReviewApp.BusinessLogics.Component {
    public class BaseLogicComponent<TContext> where TContext : class, new() {
        private bool _identityDbInitialize, _weReviewDbInitialize, _initializeErrorCollector;
        internal ErrorCollector ErrorCollector;
        protected TContext db;
        protected BaseLogicComponent(bool identityDbInitialize, bool weReviewDbInitialize, 
            bool initializeErrorCollector) {
            _identityDbInitialize = identityDbInitialize;
            _weReviewDbInitialize = weReviewDbInitialize;
            _initializeErrorCollector = initializeErrorCollector;

            if (_identityDbInitialize) {
                IdentityDb = new ApplicationDbContext();
            }

            if (_weReviewDbInitialize) {
                WeReviewDb = new WereViewAppEntities();
            }
            if (_initializeErrorCollector) {
                ErrorCollector = new ErrorCollector();
            }
        }

        protected BaseLogicComponent() : this(false) {}
        protected BaseLogicComponent(bool initializeErrorCollector) : this(null,null, initializeErrorCollector) {}

        protected BaseLogicComponent(WereViewAppEntities weReviewDb,
            bool initializeErrorCollector) : this(null, weReviewDb, initializeErrorCollector) {
        }
        protected BaseLogicComponent(ApplicationDbContext identityDb,
           bool initializeErrorCollector)
            : this(identityDb, null, initializeErrorCollector) {
        }

        protected BaseLogicComponent(ApplicationDbContext identityDb)
            : this(identityDb, null, false) {
        }

        protected BaseLogicComponent(ApplicationDbContext identityDb, WereViewAppEntities weReviewDb,
            bool initializeErrorCollector) {
            _initializeErrorCollector = initializeErrorCollector;

            if (identityDb != null) {
                IdentityDb = identityDb;
            }

            if (weReviewDb != null) {
                WeReviewDb = weReviewDb;
            }

            Type dbType = typeof (TContext);
            if (dbType == typeof (WereViewAppEntities)) {
                db = weReviewDb as TContext;
            } else if (dbType == typeof(ApplicationDbContext)) {
                db = identityDb as TContext;
            }


            if (_initializeErrorCollector) {
                ErrorCollector = new ErrorCollector();
            }
        }

>>>>>>> 9100a12a5c57c668c4b01273ff40f68df67971e7
        protected ApplicationDbContext IdentityDb { get; set; }
        protected WereViewAppEntities WeReviewDb { get; set; }
        public Logics Logics { get; set; }
        internal ErrorCollector ErrorCollector;

        protected BaseLogicComponent(bool identityDbInitialize, bool weReviewDbInitialize, bool logicInitialize, bool initializeErrorCollector) {
            
        }
    }
}