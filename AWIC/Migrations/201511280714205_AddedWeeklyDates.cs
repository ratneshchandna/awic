namespace AWIC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedWeeklyDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "WeeklyDates", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "WeeklyDates");
        }
    }
}