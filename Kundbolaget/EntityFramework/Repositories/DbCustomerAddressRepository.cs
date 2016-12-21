using Kundbolaget.EntityFramework.Context;
using Kundbolaget.Models.EntityModels;
using Kundbolaget.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kundbolaget.EntityFramework.Repositories
{
    public class DbCustomerAddressRepository : IGenericRepository<CustomerAddress>
    {
        StoreContext db;

        public DbCustomerAddressRepository()
        {
            db = new StoreContext();
        }

        public DbCustomerAddressRepository(StoreContext fakeContext)
        {
            db = fakeContext;
        }

        public void CreateItem(CustomerAddress newCustomerAddress)
        {
            db.CustomerAddresses.Add(newCustomerAddress);
            db.SaveChanges();
        }

        public void DeleteItem(int id)
        {
            db.CustomerAddresses.Remove(GetItem(id));
            db.SaveChanges();
        }

        public CustomerAddress[] GetItems(int customerId)
        {
            return db.CustomerAddresses.Where(ca => ca.CustomerId == customerId).ToArray();
        }

        public CustomerAddress GetItem(int id)
        {
            return db.CustomerAddresses.SingleOrDefault(c => c.Id == id);
        }

        public CustomerAddress[] GetItems()
        {
            return db.CustomerAddresses.ToArray();
        }

        public void UpdateItem(CustomerAddress updatedCustomerAddress)
        {
            db.CustomerAddresses.Attach(updatedCustomerAddress);
            var entry = db.Entry(updatedCustomerAddress);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }

        public Address[] GetAddress()
        {
            return db.Addresses.ToArray();
        }

        public Customer[] GetCustomers()
        {
            return db.Customers.ToArray();
        }
    }
}