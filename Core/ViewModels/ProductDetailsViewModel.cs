using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;


namespace Core.ViewModels
{
    public class ProductDetailsViewModel
    {
        public UploadProduct ProductDetails { get; set; } 
        public IEnumerable<UploadProduct> RelatedProducts { get; set; } 
    }

}
