using Kundbolaget.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kundbolaget.EntityFramework.Repositories
{
    interface IGenericRepository<T>
    {
        T GetItem(int id);
        T[] GetItems();
        void CreateItem(T newItem);
        void UpdateItem(T updatedItem);
        void DeleteItem(int id);       
    }
}
