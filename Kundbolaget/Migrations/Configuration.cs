namespace Kundbolaget.Migrations
{
    using Models.EntityModels;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Kundbolaget.EntityFramework.Context.StoreContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Kundbolaget.EntityFramework.Context.StoreContext context)
        {
            var products = new Product[]
                       {
                new Product { Id = 1, Name = "P1", Price = 100 },
                new Product { Id = 2, Name = "P2", Price = 100 },
                new Product { Id = 3, Name = "P3", Price = 100 },
                new Product { Id = 4, Name = "P4", Price = 100 }
                       };
            context.Products.AddOrUpdate(products);

        }
    }
}
