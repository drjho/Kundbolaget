using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Kundbolaget.EntityFramework.Context;
using Kundbolaget.Models.EntityModels;

namespace Kundbolaget.EntityFramework.Repositories
{
    public class DbCustomerGroupRepository : IGenericRepository<CustomerGroup>
    {
        StoreContext db = new StoreContext();
        public CustomerGroup GetItem(int id)
        {
            using (var db = new StoreContext())
            {
                return db.CustomerGroups.SingleOrDefault(c => c.Id == id);
            }
        }

        public CustomerGroup[] GetItems()
        {
            using (var db = new StoreContext())
            {
                return db.CustomerGroups.ToArray();
            }
        }

        public void CreateItem(CustomerGroup newItem)
        {
            using (var db = new StoreContext())
            {
                db.CustomerGroups.Add(newItem);
                db.SaveChanges();
            }
        }

        public void UpdateItem(CustomerGroup updatedItem)
        {
            using (var db = new StoreContext())
            {
                db.CustomerGroups.Attach(updatedItem);
                var entry = db.Entry(updatedItem);
                entry.State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteItem(int id)
        {
            using (var db = new StoreContext())
            {
                var customerGroup = db.CustomerGroups.Include(p => p.ListOfCustomers).SingleOrDefault(c => c.Id == id);
                db.Customers.RemoveRange(customerGroup.ListOfCustomers);
                db.CustomerGroups.Remove(customerGroup);
                db.SaveChanges();
            }
        }

        public void AddToProductGroup(CustomerGroup model)
        {
            var group = db.CustomerGroups.SingleOrDefault(g => g.Id == model.Id);
            
        }

    }
}