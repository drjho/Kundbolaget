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
    public class DbProductRepository : IGenericRepository<Product>
    {
        StoreContext db = new StoreContext();    
      
        public Product GetItem(int productId)
        {
            using (var db = new StoreContext())
            {
                return db.Products.Include(p => p.Name).SingleOrDefault(p => p.Id == productId);
            }
        }

        public Product[] GetItems()
        {
            using (var db = new StoreContext())
            {
                return db.Products.Include(p => p.Name).ToArray();
            }
        }

        public void CreateItem(Product newProduct)
        {
            using (var db = new StoreContext())
            {
                db.Products.Add(newProduct);
                db.SaveChanges();
            }
        }

        public void UpdateItem(Product updatedProduct)
        {
            using (var db = new StoreContext())
            {
                db.Products.Attach(updatedProduct);
                var entry = db.Entry(updatedProduct);
                entry.State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteItem(int productId)
        {
            using (var db = new StoreContext())
            {
                var product = db.Products.SingleOrDefault(p => p.Id == productId);
                db.Products.Remove(product);
                db.SaveChanges();
            }
        }
    }
}