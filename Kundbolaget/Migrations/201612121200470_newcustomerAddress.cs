namespace Kundbolaget.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newcustomerAddress : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CustomerAddresses", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.CustomerAddresses", "CustomerId", "dbo.Customers");
            DropIndex("dbo.CustomerAddresses", new[] { "CustomerId" });
            DropIndex("dbo.CustomerAddresses", new[] { "AddressId" });
            AlterColumn("dbo.CustomerAddresses", "CustomerId", c => c.Int());
            AlterColumn("dbo.CustomerAddresses", "AddressId", c => c.Int());
            CreateIndex("dbo.CustomerAddresses", "CustomerId");
            CreateIndex("dbo.CustomerAddresses", "AddressId");
            AddForeignKey("dbo.CustomerAddresses", "AddressId", "dbo.Addresses", "Id");
            AddForeignKey("dbo.CustomerAddresses", "CustomerId", "dbo.Customers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerAddresses", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.CustomerAddresses", "AddressId", "dbo.Addresses");
            DropIndex("dbo.CustomerAddresses", new[] { "AddressId" });
            DropIndex("dbo.CustomerAddresses", new[] { "CustomerId" });
            AlterColumn("dbo.CustomerAddresses", "AddressId", c => c.Int(nullable: false));
            AlterColumn("dbo.CustomerAddresses", "CustomerId", c => c.Int(nullable: false));
            CreateIndex("dbo.CustomerAddresses", "AddressId");
            CreateIndex("dbo.CustomerAddresses", "CustomerId");
            AddForeignKey("dbo.CustomerAddresses", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CustomerAddresses", "AddressId", "dbo.Addresses", "Id", cascadeDelete: true);
        }
    }
}
