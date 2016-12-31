namespace Kundbolaget.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WarehouseStorageplace : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.StoragePlaces");
            AddColumn("dbo.StoragePlaces", "Id", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.StoragePlaces", "Vacant", c => c.Boolean(nullable: false));
            AddPrimaryKey("dbo.StoragePlaces", "Id");
            DropColumn("dbo.StoragePlaces", "StoragePlaceId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StoragePlaces", "StoragePlaceId", c => c.String(nullable: false, maxLength: 128));
            DropPrimaryKey("dbo.StoragePlaces");
            DropColumn("dbo.StoragePlaces", "Vacant");
            DropColumn("dbo.StoragePlaces", "Id");
            AddPrimaryKey("dbo.StoragePlaces", "StoragePlaceId");
        }
    }
}
