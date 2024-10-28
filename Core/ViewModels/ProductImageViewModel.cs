using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Migrations;

namespace Core.ViewModels
{
    public class ProductImageViewModel
    {
        public int Id { get; set; }

        public string? ImageUrl { get; set; }

        public int UploadProductId { get; set; }
        public string? ProductsModel { get; set; }

        public string? Specifications { get; set; }

        public decimal? Price { get; set; } = 0.0m;

        public DateTime? DateSampled { get; set; }

        public int? NumberOfItem { get; set; }

        // Navigation property to store multiple images
        public virtual ICollection<ProductImage> ProductImages { get; set; }

        // New property to store product color options
        public string? Colors { get; set; }
    }
}
