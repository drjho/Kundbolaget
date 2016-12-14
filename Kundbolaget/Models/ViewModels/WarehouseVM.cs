using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kundbolaget.Models.EntityModels;

namespace Kundbolaget.Models.ViewModels
{
    public class WarehouseVM // WareHouseVM
    {
        public int Id; //Warehouse ID
        public string Name; // Warehouse name
        public string City; // Warehouse city

        public List<StoragePlace> Places { get; set; }

        public WarehouseVM()
        {
            Places = new List<StoragePlace>();
        }
        
    }
}