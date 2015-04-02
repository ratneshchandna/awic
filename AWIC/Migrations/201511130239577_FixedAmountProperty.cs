namespace AWIC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedAmountProperty : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Donations", "AmountInCAD", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Donations", "AmountInCAD", c => c.Int(nullable: false));
        }
    }
}
