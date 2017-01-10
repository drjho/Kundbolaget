namespace Kundbolaget.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullablePlannedDeliveryDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "PlannedDeliveryDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "PlannedDeliveryDate", c => c.DateTime(nullable: false));
        }
    }
}
