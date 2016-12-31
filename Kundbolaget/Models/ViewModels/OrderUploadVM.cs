using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.ViewModels
{
    public class OrderUploadVM
    {
        [Required, FileExtensions(Extensions = "json", ErrorMessage = "Add json file")]
        public HttpPostedFileBase File { get; set; }
    }
}