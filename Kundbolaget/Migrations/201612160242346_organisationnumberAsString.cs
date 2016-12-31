namespace Kundbolaget.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class organisationnumberAsString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "OrganisationNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "OrganisationNumber", c => c.Int(nullable: false));
        }
    }
}
