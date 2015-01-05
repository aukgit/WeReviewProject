using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace WereViewApp.Modules.Extensions.Context {
    public abstract class DevDbContext : DbContext {
        public DevDbContext() {
        }

        public DevDbContext(string connectionStringName)
            : base(connectionStringName) {
        }

        public DevDbContext(DbCompiledModel compiledModel)
            : base(compiledModel) {
        }

        public DevDbContext(string connectionStringName, DbCompiledModel compiledModel)
            : base(connectionStringName, compiledModel) {
        }

        public DevDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection) {
        }

        public DevDbContext(ObjectContext objectContext, bool contextOwnsConnection)
            : base(objectContext, contextOwnsConnection) {
        }

        public DevDbContext(DbConnection existingConnection, DbCompiledModel compiledModel, bool contextOwnsConnection)
            : base(existingConnection, compiledModel, contextOwnsConnection) {
        }

        /// <summary>
        ///     Save changes and sends an email to the developer if any error occurred.
        /// </summary>
        /// <returns>>=0 :executed correctly. -1: error occurred.</returns>
        public override int SaveChanges() {
            try {
                return base.SaveChanges();
            } catch (Exception ex) {
                //async email
                AppVar.Mailer.HandleError(ex, "SaveChanges", "Error SaveChanges()");
                return -1;
            }
        }

        /// <summary>
        ///     Save changes and sends an email to the developer if any error occurred.
        /// </summary>
        /// <param name="entity">A single entity while saving if any error occurred send the info to the developer as well.</param>
        /// <returns>>=0 :executed correctly. -1: error occurred.</returns>
        public int SaveChanges(object entity) {
            try {
                return base.SaveChanges();
            } catch (Exception ex) {
                //async email
                AppVar.Mailer.HandleError(ex, "SaveChanges", "Error SaveChanges()", entity);
                return -1;
            }
        }
    }
}