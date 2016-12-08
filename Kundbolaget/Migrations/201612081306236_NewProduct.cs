namespace Kundbolaget.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ConsumerPackage", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "Volume", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "StoragePackage", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "Alcohol", c => c.Single(nullable: false));
            AddColumn("dbo.Products", "ConsumerPerStorage", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "ProductGroup", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "AuditCode", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "VatCode", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "Price");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Price", c => c.Double(nullable: false));
            DropColumn("dbo.Products", "VatCode");
            DropColumn("dbo.Products", "AuditCode");
            DropColumn("dbo.Products", "ProductGroup");
            DropColumn("dbo.Products", "ConsumerPerStorage");
            DropColumn("dbo.Products", "Alcohol");
            DropColumn("dbo.Products", "StoragePackage");
            DropColumn("dbo.Products", "Volume");
            DropColumn("dbo.Products", "ConsumerPackage");
        }
    }
}
