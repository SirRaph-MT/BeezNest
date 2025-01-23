using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; } 

        [Required]
        public string? ClientId { get; set; } 

        [Range(1, 5)]
        public int RatingValue { get; set; } 

        public string? ReviewComment { get; set; } 

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey(nameof(ProductId))]
        public virtual UploadProduct Product { get; set; }

        [ForeignKey(nameof(ClientId))]
        public virtual ApplicationUser Client { get; set; }
    }

}
