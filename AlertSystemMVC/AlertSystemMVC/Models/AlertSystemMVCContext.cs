using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AlertSystemMVC.Models
{
    public class AlertSystemMVCContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public AlertSystemMVCContext() : base("name=AlertSystemMVCContext")
        {
        }
        private void BeforeSave()
        {
            var entities = ChangeTracker.Entries();

            foreach (var entity in entities)
            {
                if (entity.Entity is AppBaseStamp)
                {
                    // BUG: var Id = HttpContext.Current != null && HttpContext.Current.User != null ? HttpContext.Current.User.Identity.GetUserId() : "Anonymous";
                    // FIX #9: The should be authenicated to Insert Or Modify the Model with AppBaseStamp as base. 29/01/2015
                    // #9: Validation error while registering new user. Module App/Register
                    if (entity.State == EntityState.Added || entity.State == EntityState.Modified)
                    {
                        var entityItem = ((AppBaseStamp)entity.Entity);
                        if (entity.State == EntityState.Added)
                        {
                            entityItem.Key = String.IsNullOrWhiteSpace(entityItem.Key) ? Guid.NewGuid().ToString() : entityItem.Key;
                        }
                    }
                }
                if (entity.Entity is DateTimeStamp)
                {
                    var entityItem = ((DateTimeStamp)entity.Entity);

                    if (entity.State == EntityState.Added)
                    {
                        entityItem.CreatedAt = DateTime.Now;
                        entityItem.UpdatedAt = DateTime.Now;
                        entityItem.ActiveFlag = true;
                    }
                    else if (entity.State == EntityState.Modified)
                    {
                        entityItem.UpdatedAt = DateTime.Now;
                    }
                }
            }
        }

        public override async Task<int> SaveChangesAsync()
        {
            try
            {
                BeforeSave();
                return await base.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public override int SaveChanges()
        {
            BeforeSave();
            return base.SaveChanges();
        }
        public System.Data.Entity.DbSet<AlertSystemMVC.Models.Machine> Machines { get; set; }

        public System.Data.Entity.DbSet<AlertSystemMVC.Models.Shift> Shifts { get; set; }

        public System.Data.Entity.DbSet<AlertSystemMVC.Models.Alert> Alerts { get; set; }

        public System.Data.Entity.DbSet<AlertSystemMVC.Models.ProductionSummary> ProductionSummaries { get; set; }
    }
}
