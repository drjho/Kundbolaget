namespace Kundbolaget.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerInAlocolicese : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AlcoholLicenses", name: "Customer_Id", newName: "CustomerId");
            RenameIndex(table: "dbo.AlcoholLicenses", name: "IX_Customer_Id", newName: "IX_CustomerId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.AlcoholLicenses", name: "IX_CustomerId", newName: "IX_Customer_Id");
            RenameColumn(table: "dbo.AlcoholLicenses", name: "CustomerId", newName: "Customer_Id");
        }
    }
}
