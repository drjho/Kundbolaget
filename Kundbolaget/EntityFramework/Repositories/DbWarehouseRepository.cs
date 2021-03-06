﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Kundbolaget.EntityFramework.Context;
using Kundbolaget.Models.EntityModels;

namespace Kundbolaget.EntityFramework.Repositories
{
    public class DbWarehouseRepository : IGenericRepository<Warehouse>, IDisposable
    {
        private StoreContext db;

        public DbWarehouseRepository()
        {
            db  = new StoreContext();
        }

        public DbWarehouseRepository(StoreContext context)
        {
            db = context;
        }
        public void CreateItem(Warehouse newWarehouse)
        {
            db.Warehouses.Add(newWarehouse);
            db.SaveChanges();
        }

        public void DeleteItem(int id)
        {
            var warehouse = db.Warehouses.Include(w => w.StoragePlaces).SingleOrDefault(w => w.Id == id);
            db.StoragePlaces.RemoveRange(warehouse.StoragePlaces);
            db.Warehouses.Remove(warehouse);
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public Warehouse GetItem(int Id)
        {
            var warehouse = db.Warehouses.Include(w => w.StoragePlaces.Select(s => s.Product)).SingleOrDefault(w => w.Id == Id);
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


    }
}