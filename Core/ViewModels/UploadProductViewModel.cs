using Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic; // Added for ICollection
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.ViewModels
{
    public class UploadProductViewModel
    {
        public int Id { get; set; }
        public string? ProductsModel { get; set; }

        public string? Specifications { get; set; }

        public decimal? Price { get; set; } = 0.0m;

        public DateTime? DateSampled { get; set; }

        public int? NumberOfItem { get; set; }

        public string? Colors { get; set; }
        public List<IFormFile> ProductImages { get; set; }
        public string? UploadedImage { get; set; }
   
    }


}
