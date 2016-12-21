namespace Kundbolaget.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ordercommentsadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Comments", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Comments");
        }
    }
}
