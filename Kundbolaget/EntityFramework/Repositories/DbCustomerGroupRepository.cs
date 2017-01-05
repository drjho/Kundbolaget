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
        StoreContext db;

        public DbCustomerGroupRepository()
        {
            db = new StoreContext();
        }

        public DbCustomerGroupRepository(StoreContext fakeContext)
        {
            db = fakeContext;
        }

        public CustomerGroup GetItem(int id)
        {
            return db.CustomerGroups.SingleOrDefault(c => c.Id == id);
        }

        public CustomerGroup[] GetItems()
        {
            return db.CustomerGroups.ToArray();
        }

        public void CreateItem(CustomerGroup newItem)
        {
            db.CustomerGroups.Add(newItem);
            db.SaveChanges();

        }

        public void UpdateItem(CustomerGroup updatedItem)
        {

            db.CustomerGroups.Attach(updatedItem);
            var entry = db.Entry(updatedItem);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteItem(int id)
        {
            db.CustomerGroups.Remove(GetItem(id));
            db.SaveChanges();
        }

        public void AddToProductGroup(CustomerGroup model)
        {
            var group = db.CustomerGroups.SingleOrDefault(g => g.Id == model.Id);
        }

    }
}