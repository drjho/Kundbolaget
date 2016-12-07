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
        public void CreateItem(Warehouse newWarehouse)
        {
            using (var db = new StoreContext())
            {
                db.Warehouses.Add(newWarehouse);
                db.SaveChanges();
            }
        }

        public void DeleteItem(int Id)
        {
            using (var db = new StoreContext())
            {
                var warehouse = db.Warehouses.Include(w => w.StoragePlace).SingleOrDefault(w => w.Id == Id);
                db.StoragePlaces.RemoveRange(warehouse.StoragePlace);
                db.Warehouses.Remove(warehouse);
                db.SaveChanges();
            }
        }

        public Warehouse GetItem(int Id)
        {
            using (var db = new StoreContext())
            {
                return db.Warehouses.SingleOrDefault(w => w.Id == Id);
            }
        }

        public Warehouse[] GetItems()
        {
            using (var db = new StoreContext())
            {
                return db.Warehouses.ToArray();
            }
        }

        public void UpdateItem(Warehouse updatedWarehouse)
        {
            using (var db = new StoreContext())
            {
                db.Warehouses.Attach(updatedWarehouse);
                var entry = db.Entry(updatedWarehouse);
                entry.State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}