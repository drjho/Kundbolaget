using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Kundbolaget.EntityFramework.Context;
using Kundbolaget.Models.EntityModels;

namespace Kundbolaget.EntityFramework.Repositories
{
    public class DbInvoiceDepository : IGenericRepository<Invoice>, IDisposable
    {
        private StoreContext db;
        public DbInvoiceDepository(StoreContext context)
        {
            db = context;
        }
        public Invoice GetItem(int id)
        {
            return db.Invoices.SingleOrDefault(i => i.Id == id);
        }

        public Invoice[] GetItems()
        {
            return db.Invoices.ToArray();
        }

        public void CreateItem(Invoice newItem)
        {
            db.Invoices.Add(newItem);
            db.SaveChanges();
        }

        public void UpdateItem(Invoice updatedItem)
        {
            db.Invoices.Attach(updatedItem);
            var entry = db.Entry(updatedItem);
            entry.State = EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteItem(int id)
        {
            var invoice = db.Invoices.SingleOrDefault(i => i.Id == id);
            db.Invoices.Remove(invoice);
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}