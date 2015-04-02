namespace AWIC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPaymentSucceededBoolProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Donations", "PayemntSucceeded", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Donations", "PayemntSucceeded");
        }
    }
}
