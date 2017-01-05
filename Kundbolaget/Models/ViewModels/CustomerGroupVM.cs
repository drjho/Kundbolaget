using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kundbolaget.Models.ViewModels
{
    public class CustomerGroupVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public int? PriceListId { get; set; }
        public int? CustomerGroupId { get; set; }

        //public List<SelectListItem> CustomerGroupList { get; set; }
    }
}