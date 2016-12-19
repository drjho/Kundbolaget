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
        StoreContext db = new StoreContext();

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

        public int ReserveItem(int? productId, int orderedAmount)
        {
            int remainAmount = orderedAmount;
            // This assume that there is only 1 warehouse!!!
            var storagePlaces = db.StoragePlaces.Where(sp => sp.ProductId == productId);
            foreach (var storagePlace in storagePlaces)
            {
                db.StoragePlaces.Attach(storagePlace);
                storagePlace.ReservedAmount += Math.Min(storagePlace.AvailableAmount, remainAmount);
                var entry = db.Entry(storagePlace);
                entry.State = EntityState.Modified;
                remainAmount -= storagePlace.ReservedAmount;
                if (remainAmount < 1)
                    break;
            }
            db.SaveChanges();
            return orderedAmount - remainAmount;
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