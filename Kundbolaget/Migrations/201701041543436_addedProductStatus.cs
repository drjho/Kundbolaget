namespace Kundbolaget.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedProductStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ProductStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "ProductStatus");
        }
    }
}
