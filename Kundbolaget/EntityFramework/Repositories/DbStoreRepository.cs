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
    public class DbStoreRepository : IStoreRepository
    {
        StoreContext db = new StoreContext();
        public Product[] GetProducts()
        {
            return db.Products.ToArray();
        }

        public void CreateProduct(Product newProduct)
        {
            db.Products.Add(newProduct);
            db.SaveChanges();
        }
        public void DeleteProduct(int id)
        {
            var product = db.Products.SingleOrDefault(p => p.Id == id);
            db.Products.Remove(product);
            db.SaveChanges();
        }
        public Product GetProduct(int id)
        {
            return db.Products.SingleOrDefault(p => p.Id == id);
        }
        public void UpdateProduct(Product updatedProduct)
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
        public void AddToStorage(CreateProductVM model)
        {
            var warehouse = db.Warehouses.SingleOrDefault(w => w.Id == model.WarehouseId);
            var openStoragePlace = warehouse.StoragePlace.First(s => s.Vacant);
            var product = db.Products.SingleOrDefault(p => p.Id == model.Id);
            openStoragePlace.ProductId = product.Id;
            openStoragePlace.Vacant = false;
            db.SaveChanges();
        }
    }
}