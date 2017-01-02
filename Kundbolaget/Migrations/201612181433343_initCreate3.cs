namespace Kundbolaget.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initCreate3 : DbMigration
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
                "dbo.AlcoholLicenses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Customer_Id);
            
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
                        OrganisationNumber = c.String(),
                        CustomerGroup_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerGroups", t => t.CustomerGroup_Id)
                .Index(t => t.CustomerGroup_Id);
            
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
                "dbo.CustomerGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
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
                        CustomerId = c.Int(nullable: false),
                        PlannedDeliveryDate = c.DateTime(nullable: false),
                        AddressId = c.Int(nullable: false),
                        Comment = c.String(),
                        ImportComments = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AddressId, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.AddressId);
            
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
                        TotalAmount = c.Int(nullable: false),
                        ReservedAmount = c.Int(nullable: false),
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
                        City = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        ZipCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PriceLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProductId = c.Int(),
                        RebatePerPallet = c.Int(nullable: false),
                        CustomerGroup_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerGroups", t => t.CustomerGroup_Id)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.ProductId)
                .Index(t => t.CustomerGroup_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PriceLists", "ProductId", "dbo.Products");
            DropForeignKey("dbo.PriceLists", "CustomerGroup_Id", "dbo.CustomerGroups");
            DropForeignKey("dbo.OrderProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.StoragePlaces", "WarehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.StoragePlaces", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderProducts", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Orders", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Customers", "CustomerGroup_Id", "dbo.CustomerGroups");
            DropForeignKey("dbo.CustomerAddresses", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.CustomerAddresses", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.AlcoholLicenses", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.PriceLists", new[] { "CustomerGroup_Id" });
            DropIndex("dbo.PriceLists", new[] { "ProductId" });
            DropIndex("dbo.StoragePlaces", new[] { "ProductId" });
            DropIndex("dbo.StoragePlaces", new[] { "WarehouseId" });
            DropIndex("dbo.Orders", new[] { "AddressId" });
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropIndex("dbo.OrderProducts", new[] { "OrderId" });
            DropIndex("dbo.OrderProducts", new[] { "ProductId" });
            DropIndex("dbo.CustomerAddresses", new[] { "AddressId" });
            DropIndex("dbo.CustomerAddresses", new[] { "CustomerId" });
            DropIndex("dbo.Customers", new[] { "CustomerGroup_Id" });
            DropIndex("dbo.AlcoholLicenses", new[] { "Customer_Id" });
            DropTable("dbo.PriceLists");
            DropTable("dbo.Warehouses");
            DropTable("dbo.StoragePlaces");
            DropTable("dbo.Products");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderProducts");
            DropTable("dbo.CustomerGroups");
            DropTable("dbo.CustomerAddresses");
            DropTable("dbo.Customers");
            DropTable("dbo.AlcoholLicenses");
            DropTable("dbo.Addresses");
        }
    }
}
