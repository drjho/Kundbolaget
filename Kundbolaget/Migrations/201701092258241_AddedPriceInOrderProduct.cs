namespace Kundbolaget.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPriceInOrderProduct : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PriceLists", "Price", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PriceLists", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
