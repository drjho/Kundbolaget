using Kundbolaget.EntityFramework.Context;
using Kundbolaget.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kundbolaget.EntityFramework.Repositories
{
    public class DbStoragePlaceRepository : IGenericRepository<StoragePlace>
    {
        StoreContext db;

        public DbStoragePlaceRepository()
        {
            db = new StoreContext();
        }

        public DbStoragePlaceRepository(StoreContext fakeContext)
        {
            db = fakeContext;
        }

        public void CreateItem(StoragePlace newItem)
        {
            db.StoragePlaces.Add(newItem);
            db.SaveChanges();
        }

        public void DeleteItem(int id)
        {
            var sp = db.StoragePlaces.SingleOrDefault(s => s.Id == id);
            db.StoragePlaces.Remove(sp);
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public StoragePlace GetItem(int id)
        {
            return db.StoragePlaces.Include(s => s.Warehouse).SingleOrDefault(s => s.Id == id);
        }

        public StoragePlace[] GetItems()
        {
            return db.StoragePlaces.Include(s => s.Warehouse).ToArray();
        }

        public void UpdateItem(StoragePlace updatedItem)
        {
            db.StoragePlaces.Attach(updatedItem);
            var entry = db.Entry(updatedItem);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}