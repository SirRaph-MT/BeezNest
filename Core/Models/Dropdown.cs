using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Dropdown
    {

        [Key]
        public int Id { get; set; }

        [DisplayName("Dropdown Name")]
        public string? DropdownName { get; set; }

        public string? Key { get; set; }

        public bool? Active { get; set; }

        public DateTime? DateCreated { get; set; }

    }
}
