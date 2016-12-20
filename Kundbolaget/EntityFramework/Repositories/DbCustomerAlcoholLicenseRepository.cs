using Kundbolaget.EntityFramework.Context;
using Kundbolaget.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kundbolaget.EntityFramework.Repositories
{
    public class DbCustomerAlcoholLicenseRepository : IGenericRepository<CustomerAlcoholLicense>
    {
        StoreContext db = new StoreContext();
        public void CreateItem(CustomerAlcoholLicense newCustomerAlcoholLicense)
        {
            db.CustomerAlcoholLicenses.Add(newCustomerAlcoholLicense);
            db.SaveChanges();
        }

        public void DeleteItem(int customerId)
        {
            db.CustomerAlcoholLicenses.Remove(GetItem(customerId));
            db.SaveChanges();
        }

        public CustomerAlcoholLicense GetItem(int customerId)
        {
            return db.CustomerAlcoholLicenses.SingleOrDefault(c => c.Id == customerId);
        }

        public CustomerAlcoholLicense[] GetItems()
        {
            return db.CustomerAlcoholLicenses.ToArray();
        }

        public void UpdateItem(CustomerAlcoholLicense updatedCustomerAlcoholLicense)
        {
            db.CustomerAlcoholLicenses.Attach(updatedCustomerAlcoholLicense);
            var entry = db.Entry(updatedCustomerAlcoholLicense);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }

        public AlcoholLicense[] GetAlcoholLicense()
        {
            return db.AlcoholLicenses.ToArray();
        }

        public Customer[] GetCustomers()
        {
            return db.Customers.ToArray();
        }
    }


}