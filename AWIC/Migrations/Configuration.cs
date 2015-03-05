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

        }
    }
}
