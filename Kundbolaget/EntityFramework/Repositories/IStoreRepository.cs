using Kundbolaget.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kundbolaget.EntityFramework.Repositories
{
    interface IStoreRepository
    {
        OrderProduct[] GetProducts();
        OrderProduct GetProduct(int id);
        void CreateProduct(OrderProduct newProduct);
        void UpdateProduct(OrderProduct updatedProduct);
        void DeleteProduct(int id);
    }
}
