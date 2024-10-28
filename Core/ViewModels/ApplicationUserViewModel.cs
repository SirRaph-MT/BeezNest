using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class ApplicationUserViewModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public int? GenderId { get; set; }
        public string? Address { get; set; }
        public int? StateId { get; set; }
        public string? State { get; set; }
        public int? CityId { get; set; }
        public string? City { get; set; }
        public bool Active { get; set; }
        public bool IsDeactivated { get; set; }
        public DateTime DateCreated { get; set; }
        public string Role { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }

    }
}
