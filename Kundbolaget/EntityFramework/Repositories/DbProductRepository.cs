using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kundbolaget.Models.EntityModels;
using Kundbolaget.EntityFramework.Context;
using System.Data.Entity;
using Kundbolaget.Models.ViewModels;

namespace Kundbolaget.EntityFramework.Repositories
{
    public class DbProductRepository : IGenericRepository<Product>, IDisposable
    {
        private StoreContext db; 

        public DbProductRepository(StoreContext context)
        {
            db = context;
        }

        public Product[] GetItems()
        {
            return db.Products.ToArray();
        }

        public void CreateItem(Product newProduct)
        {
            db.Products.Add(newProduct);
            db.SaveChanges();
        }
        public void DeleteItem(int id)
        {
            var product = db.Products.SingleOrDefault(p => p.Id == id);
            db.Products.Remove(product);
            db.SaveChanges();
        }

        public Product GetItem(string productOrderId)
        {
            return GetItems().SingleOrDefault(p => p.ProductOrderId == productOrderId);
        }

        public Product GetItem(int id)
        {
            return db.Products.SingleOrDefault(p => p.Id == id);
        }
        public void UpdateItem(Product updatedProduct)
        {
            db.Products.Attach(updatedProduct);
            var entry = db.Entry(updatedProduct);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }
        public List<Warehouse> GetWarehouses()
        {
            return db.Warehouses.ToList();
        }
        public Warehouse GetWarehouse(int id)
        {
            return db.Warehouses.SingleOrDefault(w => w.Id == id);
        }

        public void UpdateStoragePlace(StoragePlace updatedStoragePlace)
        {
            db.StoragePlaces.Attach(updatedStoragePlace);
            var entry = db.Entry(updatedStoragePlace);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}