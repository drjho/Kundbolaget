using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Kundbolaget.EntityFramework.Context;
using Kundbolaget.Models.EntityModels;

namespace Kundbolaget.EntityFramework.Repositories
{
    public class DbWarehouseRepository : IGenericRepository<Warehouse> 
    {
        StoreContext db = new StoreContext();

        public void CreateItem(Warehouse newWarehouse)
        {
                db.Warehouses.Add(newWarehouse);
                db.SaveChanges();         
        }

        public void DeleteItem(int Id)
        {     
                var warehouse = db.Warehouses.Include(w => w.StoragePlace).SingleOrDefault(w => w.Id == Id);
                db.StoragePlaces.RemoveRange(warehouse.StoragePlace);
                db.Warehouses.Remove(warehouse);
                db.SaveChanges();        
        }

        public Warehouse GetItem(int Id)
        {         
                var warehouse = db.Warehouses.Include(w => w.StoragePlace.Select(s => s.Product)).SingleOrDefault(w => w.Id == Id);
                return warehouse;          
        }

        public Warehouse[] GetItems()
        {
                return db.Warehouses.ToArray();         
        }

        public void UpdateItem(Warehouse updatedWarehouse)
        {
                db.Warehouses.Attach(updatedWarehouse);
                var entry = db.Entry(updatedWarehouse);
                entry.State = EntityState.Modified;
                db.SaveChanges();
        }

        public StoragePlace[] GetOccupiedStoragePlaces(int id)
        {
                var warehouse = GetItem(id);
                return warehouse.StoragePlace.Where(s => s.Vacant == false).ToArray();            
        }

        // TODO: Kan vara onödig
        public Product GetProduct(int storagePlaceId)
        {
            var storagePlace = db.StoragePlaces.Include(s => s.Product).SingleOrDefault(p => p.Id == storagePlaceId);
            return storagePlace.Product;
        }
        
        
    }
}