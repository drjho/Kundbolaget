﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public class StoragePlace
    {

        public StoragePlace()
        {

        }
        /// <summary>
        /// Sets the storageplaceId by the following constructor
        /// </summary>
        public StoragePlace(int aNr, char side, int s, int sNr)
        {
            AisleNr = aNr;
            Side = side;
            Spot = s;
            ShelfNr = sNr;
        }

        public string StoragePlaceId
        {
            get
            {
                return $"{AisleNr} + {Side} + {Spot} + {ShelfNr}";
            }
        }

        /// <summary>
        /// Primary key.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// What number the aisle is on.
        /// </summary>
        // TODO : Kanske numreringen ska börja med 1.
        public int AisleNr { get; set; }

        /// <summary>
        /// What side of the aisle, A or B.
        /// </summary>
        public char Side { get; set; }
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
        public DateTime ArrivalDate { get; set; } = DateTime.Today;

        public bool Vacant { get; set; } = true;
    }
}