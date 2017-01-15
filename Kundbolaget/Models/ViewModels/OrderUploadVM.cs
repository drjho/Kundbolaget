using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kundbolaget.Models.ViewModels
{
    public class OrderUploadVM
    {
        [Required, FileExtensions(Extensions = "json", ErrorMessage = "Filen måste vara av Json-format")]
        public HttpPostedFileBase File { get; set; }
    }
}