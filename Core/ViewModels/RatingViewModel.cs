using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{

    public class RatingViewModel
    {
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string? ProductName { get; set; }
        public string? Review { get; set; }

        [Range(1, 5, ErrorMessage = "Please select a rating between 1 and 5.")]
        public int RatingValue { get; set; }
        public string? ClientId { get; set; }
        public string? ReviewComment { get; set; }
    }


}
