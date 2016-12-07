namespace Kundbolaget.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class storagePlaceProductIdNullable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.StoragePlaces", name: "Product_Id", newName: "ProductId");
            RenameIndex(table: "dbo.StoragePlaces", name: "IX_Product_Id", newName: "IX_ProductId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.StoragePlaces", name: "IX_ProductId", newName: "IX_Product_Id");
            RenameColumn(table: "dbo.StoragePlaces", name: "ProductId", newName: "Product_Id");
        }
    }
}
