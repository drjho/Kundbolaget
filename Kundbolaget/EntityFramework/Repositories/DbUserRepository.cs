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
    public class DbUserRepository : IGenericRepository<User>
    {
        public void CreateItem(User newUser)
        {
            using (var db = new StoreContext())
            {
                db.Users.Add(newUser);
                db.SaveChanges();
            }
        }

        public void DeleteItem(int userId)
        {
            using (var db = new StoreContext())
            {
                var user = db.Users.SingleOrDefault(u => u.Id == userId);
                db.Users.Remove(user);
                db.SaveChanges();
            }
        }

        public User GetItem(int userId)
        {
            using (var db = new StoreContext())
            {
                return db.Users.Include(u => u.Users).SingleOrDefault(u => u.Id == userId);

            }
        }

        public User[] GetItems()
        {
            using (var db = new StoreContext())
            {
                return db.Users.Include(u => u.Users).ToArray();
            }
        }

        public void UpdateItem(User updateUser)
        {
            using (var db = new StoreContext())
            {
                db.Users.Attach(updateUser);
                var entry = db.Entry(updateUser);
                entry.State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}