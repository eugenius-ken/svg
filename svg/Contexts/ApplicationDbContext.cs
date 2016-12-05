using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using svg.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace svg.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            RoleManager.Create(new IdentityRole("Administrator"));
            RoleManager.Create(new IdentityRole("User"));

            var admin = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Administrator@urfu.ru"
            };

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "User@urfu.ru"
            };

            UserManager.Create(admin, "7753191Aa-");
            UserManager.Create(user, "7753191Aa-");

            UserManager.AddToRole(admin.Id, "Administrator");
            UserManager.AddToRole(user.Id, "User");
        }
    }

}