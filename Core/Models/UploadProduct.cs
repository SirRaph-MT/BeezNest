using System;
using System.Collections.Generic; // Added for ICollection
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class UploadProduct
    {
        [Key]
        public int Id { get; set; }

        public string? ProductsModel { get; set; }

        public string? Specifications { get; set; }

        public decimal? Price { get; set; } = 0.0m;

        public DateTime? DateSampled { get; set; }

        public int? NumberOfItem { get; set; }

        public virtual ICollection<ProductImage>? ProductImages { get; set; }
        public string? Colors { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Rating>? Ratings { get; set; }

    }
}
