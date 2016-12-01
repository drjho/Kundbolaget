namespace Kundbolaget.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerAdress : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Adresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Customer_Id = c.Int(),
                        Customer_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id1)
                .Index(t => t.Customer_Id)
                .Index(t => t.Customer_Id1);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        InvoiceAdress_Id = c.Int(),
                        VisitAdress_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Adresses", t => t.InvoiceAdress_Id)
                .ForeignKey("dbo.Adresses", t => t.VisitAdress_Id)
                .Index(t => t.InvoiceAdress_Id)
                .Index(t => t.VisitAdress_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Adresses", "Customer_Id1", "dbo.Customers");
            DropForeignKey("dbo.Customers", "VisitAdress_Id", "dbo.Adresses");
            DropForeignKey("dbo.Customers", "InvoiceAdress_Id", "dbo.Adresses");
            DropForeignKey("dbo.Adresses", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.Customers", new[] { "VisitAdress_Id" });
            DropIndex("dbo.Customers", new[] { "InvoiceAdress_Id" });
            DropIndex("dbo.Adresses", new[] { "Customer_Id1" });
            DropIndex("dbo.Adresses", new[] { "Customer_Id" });
            DropTable("dbo.Customers");
            DropTable("dbo.Adresses");
        }
    }
}
