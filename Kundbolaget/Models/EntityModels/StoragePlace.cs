using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public class StoragePlace
    {
        /// <summary>
        /// Sets the storageplaceId by the following constructor
        /// </summary>
        public StoragePlace()
        {
            StoragePlaceId = $"{AisleNr} + {Part} + {Spot} + {ShelfNr}";
        }

        public string StoragePlaceId { get; private set; }
        /// <summary>
        /// What number the aisle is on.
        /// </summary>
        public int AisleNr { get; set; }

        /// <summary>
        /// What side of the aisle, A or B.
        /// </summary>
        public char Part { get; set; }
        /// <summary>
        /// The spot number on the aisle.
        /// </summary>
        public int Spot { get; set; }
        /// <summary>
        /// Shelf number on the spot (1-3).
        /// </summary>
        public int ShelfNr { get; set; }
        /// <summary>
        /// The arrival time of the product.
        /// </summary>
        public DateTime ArrivalDate { get; set; }

        
    }
}