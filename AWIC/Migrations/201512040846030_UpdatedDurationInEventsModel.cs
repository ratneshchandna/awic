namespace AWIC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedDurationInEventsModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "Duration", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "Duration", c => c.Double(nullable: false));
        }
    }
}
