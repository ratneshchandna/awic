namespace AWIC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAllDayAsEventProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "AllDay", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "AllDay");
        }
    }
}
