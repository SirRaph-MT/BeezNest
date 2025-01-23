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
        public List<UploadProduct> RelatedProducts { get; set; } 
        public double AverageRating { get; set; } 
        public int TotalRatings { get; set; } 
        public ICollection<Rating> Ratings { get; set; } 
        
    }

}
