using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using AWIC.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AWIC.DAL
{
    public class AWICDbContext : IdentityDbContext<User>
    {
        public AWICDbContext()
            : base("AWICDBConnection", throwIfV1Schema: false)
        {
        }

        public static AWICDbContext Create()
        {
            return new AWICDbContext();
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Donations> Donations { get; set; }
    }
}