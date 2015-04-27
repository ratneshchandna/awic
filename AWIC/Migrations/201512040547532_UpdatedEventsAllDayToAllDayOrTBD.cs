namespace AWIC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedEventsAllDayToAllDayOrTBD : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "AllDayOrTBD", c => c.Boolean(nullable: false));
            DropColumn("dbo.Events", "AllDay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "AllDay", c => c.Boolean(nullable: false));
            DropColumn("dbo.Events", "AllDayOrTBD");
        }
    }
}
