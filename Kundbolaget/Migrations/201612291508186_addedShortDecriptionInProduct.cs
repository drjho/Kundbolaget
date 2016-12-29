namespace Kundbolaget.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedShortDecriptionInProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PickingOrders", "ReservedAmount", c => c.Int(nullable: false));
            DropColumn("dbo.PickingOrders", "PickingAmount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PickingOrders", "PickingAmount", c => c.Int(nullable: false));
            DropColumn("dbo.PickingOrders", "ReservedAmount");
        }
    }
}
