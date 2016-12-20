using Kundbolaget.EntityFramework.Context;
using Kundbolaget.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kundbolaget.EntityFramework.Repositories
{
    public class DbOrderRepository : IGenericRepository<Order>, IDisposable
    {
        StoreContext db = new StoreContext();

        public void CreateItem(Order newItem)
        {
            db.Orders.Add(newItem);
            db.SaveChanges();
        }

        public void DeleteItem(int id)
        {
            var o = db.Orders.SingleOrDefault(order => order.Id == id);
            db.Orders.Remove(o);
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public Order GetItem(int id)
        {
            return db.Orders.SingleOrDefault(o => o.Id == id);
        }

        public Order[] GetItems()
        {
            return db.Orders.ToArray();
        }

        public void UpdateItem(Order updatedItem)
        {
            db.Orders.Attach(updatedItem);
            db.Entry(updatedItem).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void HandleOrder(Order order)
        {
            var orders = db.Orders.Where(o => o.AddressId == order.AddressId && o.CustomerId == order.CustomerId).Include(o => o.OrderProducts);

            foreach (var item in orders)
            {
                if (order.DesiredDeliveryDate.Equals(item.DesiredDeliveryDate) && order.Comment.Equals(item.Comment))
                    return;
            }

            db.OrderProducts.AddRange(order.OrderProducts);
            db.Orders.Add(order);

            //TODO: Reserve product!

            db.SaveChanges();

        }
    }
}