using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Kund")]
        public string Name { get; set; }

        [Display(Name = "Kundorder id")]
        public string CustomerOrderId { get { return Id.ToString().PadLeft(6, '0'); } }

        [Display(Name = "Koncern struktur")]
        public string CorporateStucture { get; set; }

        [Display(Name = "Kreditgräns")]
        public int CreditLimit { get; set; }

        [Display(Name = "Leveranstid")]
        public int DaysToDelievery { get; set; }

        [Display(Name = "Kundbokföringskod")]
        public int CustomerAuditCode { get; set; }

        [Display(Name = "Organisationsnummer")]
        public string OrganisationNumber { get; set; }

        [Display(Name = "Kundgruppsid")]
        public int CustomerGroupId { get; set; }

        [Display(Name = "Kundgrupp")]
        public virtual CustomerGroup CustomerGroup { get; set; }
    }
}