using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public class Warehouse
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public Adress Adress { get; set; }

        public string City { get; set; }
        public string Country { get; set; }
        public int ZipCode { get; set; }
        public virtual List<StoragePlace> StoragePlace { get; set; }

        public Warehouse()
        {
            CreatePlaces(1452);
        }

        void CreatePlaces(int places)
        {
            StoragePlace = new List<StoragePlace>();
            //for (int i = 0; i < places; i++)
            //{

            //}
        }
    }
}