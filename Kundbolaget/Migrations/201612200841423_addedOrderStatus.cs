namespace Kundbolaget.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedOrderStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderStatus", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "ImportComments");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "ImportComments", c => c.String());
            DropColumn("dbo.Orders", "OrderStatus");
        }
    }
}
