namespace AWIC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEventModelRestrictions : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "EventDescription", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "EventDescription", c => c.String());
        }
    }
}
