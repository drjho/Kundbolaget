namespace Kundbolaget.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedPickingOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PickingOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoragePlaceId = c.Int(nullable: false),
                        PickingAmount = c.Int(nullable: false),
                        PickedAmount = c.Int(nullable: false),
                        Comment = c.String(),
                        OrderProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderProducts", t => t.OrderProductId, cascadeDelete: true)
                .ForeignKey("dbo.StoragePlaces", t => t.StoragePlaceId, cascadeDelete: true)
                .Index(t => t.StoragePlaceId)
                .Index(t => t.OrderProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PickingOrders", "StoragePlaceId", "dbo.StoragePlaces");
            DropForeignKey("dbo.PickingOrders", "OrderProductId", "dbo.OrderProducts");
            DropIndex("dbo.PickingOrders", new[] { "OrderProductId" });
            DropIndex("dbo.PickingOrders", new[] { "StoragePlaceId" });
            DropTable("dbo.PickingOrders");
        }
    }
}
