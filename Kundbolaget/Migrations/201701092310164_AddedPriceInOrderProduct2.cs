namespace Kundbolaget.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPriceInOrderProduct2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderProducts", "Price", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderProducts", "Price");
        }
    }
}
