using Kundbolaget.EntityFramework.Context;
using Kundbolaget.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kundbolaget.EntityFramework.Repositories
{
    public class DbPickingOrderRepository : IGenericRepository<PickingOrder>, IDisposable
    {
        private StoreContext db;

        public DbPickingOrderRepository(StoreContext context)
        {
            db = context;
        }

        public void CreateItem(PickingOrder newItem)
        {
            db.PickingOrders.Add(newItem);
            db.SaveChanges();
        }

        public void DeleteItem(int id)
        {
            var item = db.PickingOrders.SingleOrDefault(p => p.Id == id);
            if (item == null)
                return;
            db.PickingOrders.Remove(item);
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public PickingOrder GetItem(int id)
        {
            return db.PickingOrders.SingleOrDefault(o => o.Id == id);
        }

        public PickingOrder[] GetItems()
        {
            return db.PickingOrders.ToArray();
        }

        public void UpdateItems(IEnumerable<PickingOrder> updatedItems)
        {
            if (updatedItems.Count() > 0)
            {
                for (int i = 0; i < updatedItems.Count(); i++)
                {
                    db.PickingOrders.Attach(updatedItems.ElementAt(i));
                    db.Entry(updatedItems.ElementAt(i)).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
        }

        public void UpdateItem(PickingOrder updatedItem)
        {
            db.PickingOrders.Attach(updatedItem);
            db.Entry(updatedItem).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}