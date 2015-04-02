namespace AWIC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDonationsModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Donations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AmountInCAD = c.Int(nullable: false),
                        Donor = c.String(),
                        DonorEmail = c.String(),
                        DonationDateAndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Donations");
        }
    }
}
