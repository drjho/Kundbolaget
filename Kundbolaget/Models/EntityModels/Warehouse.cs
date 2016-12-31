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
            for (int row = 0; row < 11; row++)
            {
                foreach( Side s in Enum.GetValues(typeof(Side)))
                {
                    for (int place = 0; place < 22; place++)
                    {
                        for (int shelf = 0; shelf < 3; shelf++)
                        {
                            StoragePlace.Add(new StoragePlace(row, s, place, shelf));
                        }
                    }
                }
            }
        }
    }
}