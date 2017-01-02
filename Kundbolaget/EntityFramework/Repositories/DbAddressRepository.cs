using Kundbolaget.EntityFramework.Context;
using Kundbolaget.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kundbolaget.EntityFramework.Repositories
{
    public class DbAddressRepository : IGenericRepository<Address>, IDisposable
    {
        private StoreContext db;

        public DbAddressRepository()
        {
            db = new StoreContext();

        }

        public DbAddressRepository(StoreContext fakeContext)
        {
            db = fakeContext;
        }

        public void CreateItem(Address newAddress)
        {
            db.Addresses.Add(newAddress);
            db.SaveChanges();
        }

        public void DeleteItem(int addressId)
        {
            var address = db.Addresses.SingleOrDefault(c => c.Id == addressId);
            db.Addresses.Remove(address);
            db.SaveChanges();
        }

        public Address GetItem(int addressId)
        {
            return db.Addresses.SingleOrDefault(c => c.Id == addressId);
        }

        public Address[] GetItems()
        {
            return db.Addresses.ToArray();
        }

        public void UpdateItem(Address updatedAddress)
        {
            db.Addresses.Attach(updatedAddress);
            var entry = db.Entry(updatedAddress);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}