using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        public int UploadProductId { get; set; }

        public string? ImageUrl { get; set; }

        [ForeignKey(nameof(UploadProductId))]
        public virtual UploadProduct? UploadProduct { get; set; }
    }
}
