using Kundbolaget.EntityFramework.Context;
using Kundbolaget.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kundbolaget.EntityFramework.Repositories
{
    public class DbAlcoholLicenseRepository : IGenericRepository<AlcoholLicense>
    {
        StoreContext db;

        public DbAlcoholLicenseRepository()
        {
            db = new StoreContext();
        }

        public DbAlcoholLicenseRepository(StoreContext fakeContext)
        {
            db = fakeContext;
        }

        public void CreateItem(AlcoholLicense newItem)
        {           
                db.AlcoholLicense.Add(newItem);
                db.SaveChanges();      
        }

        public void DeleteItem(int id)
        {
                var alcholLicense = db.AlcoholLicense.SingleOrDefault(a => a.Id == id);
                db.AlcoholLicense.Remove(alcholLicense);
                db.SaveChanges();
        }

        public AlcoholLicense GetItem(int id)
        {
                return db.AlcoholLicense.SingleOrDefault(a => a.Id == id);
        }

        public AlcoholLicense[] GetItems()
        {
                return db.AlcoholLicense.ToArray();
        }

        public void UpdateItem(AlcoholLicense updatedItem)
        {
                db.AlcoholLicense.Attach(updatedItem);
                var entry = db.Entry(updatedItem);
                entry.State = EntityState.Modified;
                db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}