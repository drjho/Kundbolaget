using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kundbolaget.EntityFramework.Context;
using Kundbolaget.Models.EntityModels;

namespace Kundbolaget.EntityFramework.Repositories
{
    public class DbCustomerRepository : IGenericRepository<Customer>
    {
        public Customer GetItem(int customerId)
        {
            using (var db = new StoreContext())
            {
                return db.Customers.Include(c => c.Addresses).SingleOrDefault(c => c.Id == customerId);
            }
        }

        public Customer[] GetItems()
        {
            using (var db = new StoreContext())
            {
                return db.Customers.Include(c => c.Addresses).ToArray();
            }
        }

        public void CreateItem(Customer newCustomer)
        {
            using (var db = new StoreContext())
            {
                db.Customers.Add(newCustomer);
                db.SaveChanges();
            }
        }

        public void UpdateItem(Customer updatedCustomer)
        {
            using (var db = new StoreContext())
            {
                db.Customers.Attach(updatedCustomer);
                var entry = db.Entry(updatedCustomer);
                entry.State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteItem(int customerId)
        {
            using (var db = new StoreContext())
            {
                var customer = db.Customers.SingleOrDefault(c => c.Id == customerId);
                db.Customers.Remove(customer);
                db.SaveChanges();
            }
        }
    }
}
