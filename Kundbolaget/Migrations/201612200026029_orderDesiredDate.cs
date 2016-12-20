namespace Kundbolaget.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orderDesiredDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "DesiredDeliveryDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "DesiredDeliveryDate");
        }
    }
}
