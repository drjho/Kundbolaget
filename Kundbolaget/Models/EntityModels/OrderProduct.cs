using System.ComponentModel.DataAnnotations;

namespace Kundbolaget.Models.EntityModels
{
    public class OrderProduct
    {
        public int Id { get; set; }

        [Display(Name = "Produkt id")]
        public int? ProductId { get; set; }
        public virtual Product Product { get; set; }


        [Display(Name = "Beställt antal")]
        public int OrderedAmount { get; set; }

        [Display(Name = "Levererat antal")]
        public int DeliveredAmount { get; set; }

        [Display(Name = "Mottaget antal")]
        public int AcceptedAmount { get; set; }

        [Display(Name = "Kommentar")]
        public string Comment { get; set; }

        public int? OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}