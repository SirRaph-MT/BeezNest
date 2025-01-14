using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class AskedQuestions
    {
        [Key]
        public int Id { get; set; } = 0;    
        public string Questions { get; set; } = string.Empty;   
        public string Answers { get; set; } = string.Empty;
    }
}
