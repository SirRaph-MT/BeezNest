using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class UploadProductsViewModel
    {
        public UploadProduct UploadProduct { get; set; }
        public IEnumerable<UploadProduct> ListUploadProducts { get; set; }

        public IEnumerable<AskedQuestions> FAQs { get; set; }
      
    }
}
