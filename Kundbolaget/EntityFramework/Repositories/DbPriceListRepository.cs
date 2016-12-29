using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Kundbolaget.EntityFramework.Context;
using Kundbolaget.Models.EntityModels;

namespace Kundbolaget.EntityFramework.Repositories
{
    public class DbPriceListRepository : IGenericRepository<PriceList>
    {
        StoreContext db;

        public DbPriceListRepository()
        {
            db = new StoreContext();
        }

        public DbPriceListRepository(StoreContext fakeContext)
        {
           db = fakeContext;
        }

        public PriceList GetItem(int id)
        {         
                return db.PriceLists.SingleOrDefault(p => p.Id == id);         
        }

        public PriceList[] GetItems()
        {
                return db.PriceLists.ToArray();       
        }

        public void CreateItem(PriceList newItem)
        {
                db.PriceLists.Add(newItem);
                db.SaveChanges();
        }

        public void UpdateItem(PriceList updatedItem)
        {

                db.PriceLists.Attach(updatedItem);
                var entry = db.Entry(updatedItem);
                entry.State = EntityState.Modified;
                db.SaveChanges();
            
        }

        public void DeleteItem(int id)
        {
                var pricelist = db.PriceLists.SingleOrDefault(c => c.Id == id);
                db.PriceLists.Remove(pricelist);
                db.SaveChanges();          
        }

        public void AddToCustomerGroup(CustomerGroup model)
        {
            var pricelist = db.PriceLists.Attach(GetItem(model.Id));
            db.PriceLists.Add(pricelist);
            db.SaveChanges();
        }

        public void AddProductToPriceList(Product model)
        {
            var pricelist = db.Products.Attach(model);

        }
    }
}