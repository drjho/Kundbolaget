namespace Kundbolaget.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddressCustomer2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Adresses", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Warehouses", "Adress_Id", "dbo.Adresses");
            DropForeignKey("dbo.StoragePlaces", "Warehouse_Id", "dbo.Warehouses");
            DropIndex("dbo.Adresses", new[] { "Customer_Id" });
            DropIndex("dbo.Warehouses", new[] { "Adress_Id" });
            DropIndex("dbo.StoragePlaces", new[] { "Warehouse_Id" });
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StreetName = c.String(nullable: false),
                        Number = c.Int(nullable: false),
                        PostalCode = c.String(nullable: false),
                        Area = c.String(nullable: false),
                        Type = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false));
            DropTable("dbo.Adresses");
            DropTable("dbo.Warehouses");
            DropTable("dbo.StoragePlaces");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StoragePlaces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AisleNr = c.Int(nullable: false),
                        Spot = c.Int(nullable: false),
                        ShelfNr = c.Int(nullable: false),
                        ArrivalDate = c.DateTime(nullable: false),
                        Vacant = c.Boolean(nullable: false),
                        Warehouse_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Warehouses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        City = c.String(),
                        Country = c.String(),
                        ZipCode = c.Int(nullable: false),
                        Adress_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Adresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeId = c.Int(nullable: false),
                        StreetName = c.String(),
                        Number = c.Int(nullable: false),
                        PostalCode = c.String(),
                        Area = c.String(),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Addresses", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Addresses", new[] { "CustomerId" });
            AlterColumn("dbo.Customers", "Name", c => c.String());
            DropTable("dbo.Addresses");
            CreateIndex("dbo.StoragePlaces", "Warehouse_Id");
            CreateIndex("dbo.Warehouses", "Adress_Id");
            CreateIndex("dbo.Adresses", "Customer_Id");
            AddForeignKey("dbo.StoragePlaces", "Warehouse_Id", "dbo.Warehouses", "Id");
            AddForeignKey("dbo.Warehouses", "Adress_Id", "dbo.Adresses", "Id");
            AddForeignKey("dbo.Adresses", "Customer_Id", "dbo.Customers", "Id");
        }
    }
}
