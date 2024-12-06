using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UploadProductId { get; set; }
        [ForeignKey("UploadProductId")]
        public virtual UploadProduct? UploadProduct { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

        [ForeignKey("ImageId")]
        public virtual ProductImage? ProductImage { get; set; }


        // Computed property to get the total price based on quantity and price
        public decimal TotalPrice => (Price ?? 0) * Quantity;
        public bool Deleted { get; set; }
        public bool Active { get; set; }
    }

}
