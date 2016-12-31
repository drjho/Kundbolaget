using Kundbolaget.EntityFramework.Context;
using Kundbolaget.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kundbolaget.EntityFramework.Repositories
{
    public class DbAddressRepository : IGenericRepository<Address>
    {
        public void CreateItem(Address newAddress)
        {
            using (var db = new StoreContext())
            {
                db.Addresses.Add(newAddress);
                db.SaveChanges();
            }
        }

        public void DeleteItem(int addressId)
        {
            using (var db = new StoreContext())
            {
                var address = db.Addresses.SingleOrDefault(c => c.Id == addressId);
                db.Addresses.Remove(address);
                db.SaveChanges();
            }
        }

        public Address GetItem(int addressId)
        {
            using (var db = new StoreContext())
            {
                return db.Addresses.SingleOrDefault(c => c.Id == addressId);
            }
        }

        public Address[] GetItems()
        {
            using (var db = new StoreContext())
            {
                return db.Addresses.ToArray();
            }
        }

        public void UpdateItem(Address updatedAddress)
        {
            using (var db = new StoreContext())
            {
                db.Addresses.Attach(updatedAddress);
                var entry = db.Entry(updatedAddress);
                entry.State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}