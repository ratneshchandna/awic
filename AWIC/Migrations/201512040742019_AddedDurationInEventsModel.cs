namespace AWIC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDurationInEventsModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Duration", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "Duration");
        }
    }
}