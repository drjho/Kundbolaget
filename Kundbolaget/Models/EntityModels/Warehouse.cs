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

        [Display(Name = "Namn")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Ort")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Land")]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Postnummer")]
        public int ZipCode { get; set; }

        public virtual List<StoragePlace> StoragePlace { get; set; }

        public Warehouse()
        {
            CreatePlaces(1452);
        }

        void CreatePlaces(int places)
        {
            StoragePlace = new List<StoragePlace>();
            for (int aisle = 0; aisle < 11; aisle++)
            {
                foreach (Side side in Enum.GetValues(typeof(Side)))
                {
                    for (int spot = 0; spot < 22; spot++)
                    {
                        for (int shelf = 0; shelf < 3; shelf++)
                        {
                            StoragePlace.Add(new StoragePlace
                            {
                                AisleNr = aisle,
                                Side = side,
                                Spot = spot,
                                ShelfNr = shelf
                            });

                        }
                    }
                }
            }
        }
    }
}