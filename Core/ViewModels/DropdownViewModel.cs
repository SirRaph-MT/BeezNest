using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class DropdownViewModel
    {
        public int Id { get; set; }
        public string? DropdownName { get; set; }
        public string? Key { get; set; }
        public bool? Active { get; set; }
        public DateTime? DateCreated { get; set; }

        // New property for storing the selected dropdown ID
        public int? SelectedDropdownId { get; set; } // Add this line
    }

}
