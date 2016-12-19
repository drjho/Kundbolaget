using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public enum Side
    {
        Left, Right
    }

    public class StoragePlace
    {
        public string StoragePlaceId => $"Aisle:{AisleNr} Side:{Side.ToString()} Spot:{Spot} Shelf:{ShelfNr}";

        public int WarehouseId { get; set; }
        public virtual Warehouse Warehouse { get; set; }

        public int? ProductId { get; set; }
        public virtual Product Product { get; set; }

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
        /// What side of the aisle, Left or Right
        /// </summary>
        public Side Side { get; set; }
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
        /// <summary>
        /// Set to false when it is occupied.
        /// </summary>
        public bool Vacant { get; set; } = true;

        public int TotalAmount { get; set; }

        public int ReservedAmount { get; set; }

        public int AvailableAmount => TotalAmount - ReservedAmount;
    }
}