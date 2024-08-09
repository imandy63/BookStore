using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BookStore.Identity
{
    public class AppDBContext: IdentityDbContext<AppUser>
    {
        public AppDBContext() : base("BookStore") { }
        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}