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
        public void CreateItem(AlcoholLicense newItem)
        {
            using (var db = new StoreContext())
            {
                db.AlcoholLicense.Add(newItem);
                db.SaveChanges();
            }
        }

        public void DeleteItem(int id)
        {
            using (var db = new StoreContext())
            {
                var alcholLicense = db.AlcoholLicense.SingleOrDefault(a => a.Id == id);
                db.AlcoholLicense.Remove(alcholLicense);
                db.SaveChanges();
            }
        }

        public AlcoholLicense GetItem(int id)
        {
            using (var db = new StoreContext())
            {
                return db.AlcoholLicense.SingleOrDefault(a => a.Id == id);
            }
        }

        public AlcoholLicense[] GetItems()
        {
            using (var db = new StoreContext())
            {
                return db.AlcoholLicense.ToArray();
            }
        }

        public void UpdateItem(AlcoholLicense updatedItem)
        {
            using (var db = new StoreContext())
            {
                db.AlcoholLicense.Attach(updatedItem);
                var entry = db.Entry(updatedItem);
                entry.State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}