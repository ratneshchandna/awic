namespace AWIC.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using AWIC.DAL;
    using AWIC.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<AWICDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "AWICDbContext";
        }

        protected override void Seed(AWICDbContext context)
        {
            var admin = new User
            {
                Email = System.Configuration.ConfigurationManager.AppSettings["OrganisationEmailAddress"],
                EmailConfirmed = false,
                PasswordHash = System.Configuration.ConfigurationManager.AppSettings["OrganisationAdminPasswordHash"],
                SecurityStamp = System.Configuration.ConfigurationManager.AppSettings["OrganisationAdminSecurityStamp"],
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEndDateUtc = null,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                UserName = System.Configuration.ConfigurationManager.AppSettings["OrganisationAdminUsername"]
            };;
            context.Users.AddOrUpdate(u => u.UserName, admin);
            context.SaveChanges();
        }
    }
}