namespace AWIC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedPaymentSucceededBoolProperty : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Donations", "PayemntSucceeded");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Donations", "PayemntSucceeded", c => c.Boolean(nullable: false));
        }
    }
}
