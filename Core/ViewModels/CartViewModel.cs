using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class CartViewModel
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

        public decimal TotalPrice => (Price ?? 0) * Quantity;
        public bool Deleted { get; set; }
        public bool Active { get; set; }
    }
}
