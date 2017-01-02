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
    public class DbCustomerRepository : IGenericRepository<Customer>, IDisposable
    {
        StoreContext db;

        public DbCustomerRepository()
        {
            db = new StoreContext();
        }

        public DbCustomerRepository(StoreContext context)
        {
            db = context;
        }

        public Customer GetItem(int customerId)
        {
            return db.Customers.SingleOrDefault(c => c.Id == customerId);
        }

        public Customer[] GetItems()
        {
            return db.Customers.ToArray();
        }

        public void CreateItem(Customer newCustomer)
        {

            db.Customers.Add(newCustomer);
            db.SaveChanges();
        }

        public void UpdateItem(Customer updatedCustomer)
        {
            db.Customers.Attach(updatedCustomer);
            var entry = db.Entry(updatedCustomer);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteItem(int customerId)
        {

            var customer = db.Customers.SingleOrDefault(c => c.Id == customerId);
            db.Customers.Remove(customer);
            db.SaveChanges();

        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
