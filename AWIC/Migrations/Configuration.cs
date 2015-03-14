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
                Email = User.ADMIN,
                EmailConfirmed = false,
                PasswordHash = "AK0O2maSDXqmvN/zipLunJ0C1rEoyTDm/D6EXje3TkTjUfxz6CPYNMsqvq+nGtHHZQ==",
                SecurityStamp = "83d4d10f-93d1-4967-8066-604cdc27a44b",
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEndDateUtc = null,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                UserName = User.ADMIN
            };;
            context.Users.AddOrUpdate(u => u.UserName, admin);
            context.SaveChanges();
        }
    }
}