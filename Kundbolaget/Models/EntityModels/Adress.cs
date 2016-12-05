using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public class Adress
    {
        public int Id { get; set; }

        /// <summary>
        /// Type: 0 = Faktura, 1 = Leverans, 2 = Besök.
        /// </summary>
        public int TypeId { get; set; }

        public virtual Customer Customer { get; set; }

        public string StreetName { get; set; }

        public int Number { get; set; }

        public string PostalCode { get; set; }

        public string Area { get; set; }


    }
}