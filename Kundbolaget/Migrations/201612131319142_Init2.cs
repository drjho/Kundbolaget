namespace Kundbolaget.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StreetName = c.String(nullable: false),
                        Number = c.Int(nullable: false),
                        PostalCode = c.String(nullable: false),
                        Area = c.String(nullable: false),
                        Country = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomerAddresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(),
                        AddressId = c.Int(),
                        AddressType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CorporateStucture = c.String(),
                        CreditLimit = c.Int(nullable: false),
                        DaysToDelievery = c.Int(nullable: false),
                        CustomerAuditCode = c.Int(nullable: false),
                        OrganisationNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(),
                        OrderedAmount = c.Int(nullable: false),
                        DeliveredAmount = c.Int(nullable: false),
                        AcceptedAmount = c.Int(nullable: false),
                        Comment = c.String(),
                        OrderId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                        DesiredDeliveryDate = c.DateTime(nullable: false),
                        PlannedDeliveryDate = c.DateTime(nullable: false),
                        CustomerAddressId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerAddresses", t => t.CustomerAddressId, cascadeDelete: true)
                .Index(t => t.CustomerAddressId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ConsumerPackage = c.Int(nullable: false),
                        Volume = c.Int(nullable: false),
                        StoragePackage = c.Int(nullable: false),
                        Alcohol = c.Single(nullable: false),
                        ConsumerPerStorage = c.Int(nullable: false),
                        ProductGroup = c.Int(nullable: false),
                        AuditCode = c.Int(nullable: false),
                        VatCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StoragePlaces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WarehouseId = c.Int(),
                        ProductId = c.Int(),
                        AisleNr = c.Int(nullable: false),
                        Side = c.Int(nullable: false),
                        Spot = c.Int(nullable: false),
                        ShelfNr = c.Int(nullable: false),
                        ArrivalDate = c.DateTime(nullable: false),
                        Vacant = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .ForeignKey("dbo.Warehouses", t => t.WarehouseId)
                .Index(t => t.WarehouseId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Warehouses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        City = c.String(),
                        Country = c.String(),
                        ZipCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.StoragePlaces", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.StoragePlaces", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderProducts", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "CustomerAddressId", "dbo.CustomerAddresses");
            DropForeignKey("dbo.CustomerAddresses", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.CustomerAddresses", "AddressId", "dbo.Addresses");
            DropIndex("dbo.StoragePlaces", new[] { "ProductId" });
            DropIndex("dbo.StoragePlaces", new[] { "WarehouseId" });
            DropIndex("dbo.Orders", new[] { "CustomerAddressId" });
            DropIndex("dbo.OrderProducts", new[] { "OrderId" });
            DropIndex("dbo.OrderProducts", new[] { "ProductId" });
            DropIndex("dbo.CustomerAddresses", new[] { "AddressId" });
            DropIndex("dbo.CustomerAddresses", new[] { "CustomerId" });
            DropTable("dbo.Warehouses");
            DropTable("dbo.StoragePlaces");
            DropTable("dbo.Products");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderProducts");
            DropTable("dbo.Customers");
            DropTable("dbo.CustomerAddresses");
            DropTable("dbo.Addresses");
        }
    }
}
