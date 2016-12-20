namespace Kundbolaget.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class revisedForPrepareOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderProducts", "AvailabeAmount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderProducts", "AvailabeAmount");
        }
    }
}
