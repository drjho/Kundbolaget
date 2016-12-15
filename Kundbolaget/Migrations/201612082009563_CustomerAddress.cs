namespace Kundbolaget.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerAddress : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Addresses", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.OrderProducts", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderProducts", "ProductId", "dbo.Products");
            DropIndex("dbo.Addresses", new[] { "CustomerId" });
            DropIndex("dbo.OrderProducts", new[] { "ProductId" });
            DropIndex("dbo.OrderProducts", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            CreateTable(
                "dbo.CustomerAddresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddressType = c.Int(nullable: false),
                        Address_Id = c.Int(),
                        Customer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .Index(t => t.Address_Id)
                .Index(t => t.Customer_Id);
            
            DropColumn("dbo.Addresses", "Type");
            DropColumn("dbo.Addresses", "CustomerId");
            DropTable("dbo.OrderProducts");
            DropTable("dbo.Orders");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                        DesiredDeliveryDate = c.DateTime(nullable: false),
                        PlannedDeliveryDate = c.DateTime(nullable: false),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        OrderedAmount = c.Int(nullable: false),
                        DeliveredAmount = c.Int(nullable: false),
                        AcceptedAmount = c.Int(nullable: false),
                        Comment = c.String(),
                        OrderId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Addresses", "CustomerId", c => c.Int(nullable: false));
            AddColumn("dbo.Addresses", "Type", c => c.Int(nullable: false));
            DropForeignKey("dbo.CustomerAddresses", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.CustomerAddresses", "Address_Id", "dbo.Addresses");
            DropIndex("dbo.CustomerAddresses", new[] { "Customer_Id" });
            DropIndex("dbo.CustomerAddresses", new[] { "Address_Id" });
            DropTable("dbo.CustomerAddresses");
            CreateIndex("dbo.Orders", "CustomerId");
            CreateIndex("dbo.OrderProducts", "OrderId");
            CreateIndex("dbo.OrderProducts", "ProductId");
            CreateIndex("dbo.Addresses", "CustomerId");
            AddForeignKey("dbo.OrderProducts", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
            AddForeignKey("dbo.OrderProducts", "OrderId", "dbo.Orders", "Id");
            AddForeignKey("dbo.Orders", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Addresses", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
        }
    }
}
