using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.EntityModels
{
    public enum ConsumerPackage
    {
        Flaska, Burk, Box
    }

    public enum StoragePackage
    {
        Back, Kartong, Flak
    }

    public enum ProductGroup
    {
        Cider, Rödvin, Vitvin, Starksprit, Öl, Mousserande, Alkoholfritt
    }

    public enum ProductStatus
    {
        Normal, Beställningsvara, Avvaktar, Problem, Utsålt, Utgått 
    }

    public class Product
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Benämning")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Konsumentförpackning")]
        public ConsumerPackage ConsumerPackage { get; set; }
        [Required]
        [Display(Name = "Volym[cl]")]
        public int Volume { get; set; }
        [Required]
        [Display(Name = "Lagerförpackning")]
        public StoragePackage StoragePackage { get; set; }
        [Required]
        [Display(Name = "%-volym")]
        public float Alcohol { get; set; }
        [Required]
        [Display(Name = "KonsFörp/LagerFörp")]
        public int ConsumerPerStorage { get; set; }
        [Required]
        [Display(Name = "Produktgrupp")]
        public ProductGroup ProductGroup { get; set; }
        [Required]
        [Display(Name = "Bokföringskod")]
        public int AuditCode { get; set; }
        [Required]
        [Display(Name = "Momskod")]
        public int VatCode { get; set; }

        [Display(Name = "Säljstatus", ShortName = "Status")]
        public ProductStatus ProductStatus { get; set; } = ProductStatus.Normal;

        public virtual List<StoragePlace> StoragePlaces { get; set; }

        [Display(Name = "Produktorderid")]
        public string ProductOrderId => Id.ToString().PadLeft(6, '0');

        public string ShortDescription => $"{Name} | {Volume} cl | {Alcohol} %";

    }
}