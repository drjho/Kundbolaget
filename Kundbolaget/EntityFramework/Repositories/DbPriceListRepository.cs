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
        public PriceList GetItem(int id)
        {
            using (var db = new StoreContext())
            {
                return db.PriceLists.SingleOrDefault(p => p.Id == id);
            }
        }

        public PriceList[] GetItems()
        {
            using (var db = new StoreContext())
            {
                return db.PriceLists.ToArray();
            }
        }

        public void CreateItem(PriceList newItem)
        {
            using (var db = new StoreContext())
            {
                db.PriceLists.Add(newItem);
                db.SaveChanges();
            }
        }

        public void UpdateItem(PriceList updatedItem)
        {
            using (var db = new StoreContext())
            {
                db.PriceLists.Attach(updatedItem);
                var entry = db.Entry(updatedItem);
                entry.State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteItem(int id)
        {
            using (var db = new StoreContext())
            {
                var pricelist = db.PriceLists.SingleOrDefault(c => c.Id == id);
                db.PriceLists.Remove(pricelist);
                db.SaveChanges();
            }
        }
    }
}