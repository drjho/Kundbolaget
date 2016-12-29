using Kundbolaget.EntityFramework.Context;
using Kundbolaget.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kundbolaget.EntityFramework.Repositories
{
    public class DbOrderProductRepository : IGenericRepository<OrderProduct>, IDisposable
    {
        private StoreContext db;

        public DbOrderProductRepository(StoreContext context)
        {
            db = context;
        }

        public void CreateItem(OrderProduct newItem)
        {
            db.OrderProducts.Add(newItem);
            db.SaveChanges();
        }

        public void DeleteItem(int id)
        {
            var o = db.OrderProducts.SingleOrDefault(OrderProduct => OrderProduct.Id == id);
            db.OrderProducts.Remove(o);
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public OrderProduct GetItem(int id)
        {
            return db.OrderProducts.SingleOrDefault(o => o.Id == id);
        }

        public OrderProduct[] GetItems()
        {
            return db.OrderProducts.ToArray();
        }

        public void UpdateItems(IEnumerable<OrderProduct> updatedItems)
        {
            for (int i = 0; i < updatedItems.Count(); i++)
            {
                db.OrderProducts.Attach(updatedItems.ElementAt(i));
                db.Entry(updatedItems.ElementAt(i)).State = EntityState.Modified;
            }
            db.SaveChanges();
        }

        public void UpdateItem(OrderProduct updatedItem)
        {
            db.OrderProducts.Attach(updatedItem);
            db.Entry(updatedItem).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}