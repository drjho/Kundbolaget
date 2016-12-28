using Kundbolaget.EntityFramework.Context;
using Kundbolaget.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kundbolaget.EntityFramework.Repositories
{
    public class DbStoragePlaceRepository : IGenericRepository<StoragePlace>, IDisposable
    {
        StoreContext db;

        public DbStoragePlaceRepository(StoreContext context)
        {
            db = context;
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

        public void UpdateItems(IEnumerable<StoragePlace> updatedItems)
        {
            for (int i = 0; i < updatedItems.Count(); i++)
            {
                db.StoragePlaces.Attach(updatedItems.ElementAt(i));
                db.Entry(updatedItems.ElementAt(i)).State = EntityState.Modified;
            }
            db.SaveChanges();
        }

        public void UpdateItem(StoragePlace updatedItem)
        {
            db.StoragePlaces.Attach(updatedItem);
            var entry = db.Entry(updatedItem);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }

        public int GetAvailableAmount(int? productId)
        {
            return db.StoragePlaces.Where(sp => sp.ProductId == productId).Sum(sp => sp.TotalAmount-sp.ReservedAmount);
        }

        public bool AddProduct(int warehouseId, int productId, int amount)
        {
            var storagePlace = db.StoragePlaces.Where(s => s.WarehouseId == warehouseId).FirstOrDefault(s => s.Vacant);
            if (storagePlace == null)
                return false;
            //var product = db.Products.SingleOrDefault(p => p.Id == productId);
            //if (product == null)
            //    return false;
            //storagePlace.ProductId = productId;
            //storagePlace.TotalAmount = amount;
            //storagePlace.Vacant = false;
            //UpdateItem(storagePlace);
            UpdateProduct(storagePlace.Id, productId, amount, 0);
            return true;
        }

        public bool UpdateProduct(int id, int productId, int newAmount, int newReservedAmount)
        {
            var storagePlace = db.StoragePlaces.SingleOrDefault(s => s.Id == id);
            if (storagePlace == null)
                return false;
            var product = db.Products.SingleOrDefault(p => p.Id == productId);
            if (product == null)
                return false;
            if (newAmount == 0)
                return RemoveProduct(id);
            storagePlace.ProductId = product.Id;
            storagePlace.TotalAmount = newAmount;
            storagePlace.ReservedAmount = newReservedAmount;
            storagePlace.Vacant = false;
            UpdateItem(storagePlace);
            return true;
        }

        public bool RemoveProduct(int id)
        {
            var storagePlace = db.StoragePlaces.SingleOrDefault(s => s.Id == id);
            if (storagePlace == null)
                return false;
            storagePlace.ProductId = null;
            storagePlace.TotalAmount = 0;
            storagePlace.ReservedAmount = 0;
            storagePlace.Vacant = true;
            UpdateItem(storagePlace);
            return true;
        }
    }
}