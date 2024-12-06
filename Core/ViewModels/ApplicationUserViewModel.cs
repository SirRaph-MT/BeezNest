using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class ApplicationUserViewModel
    {

        [Required(ErrorMessage = "Firstname is required.")]
        [StringLength(20, ErrorMessage = "Firstname must not exceed 20 characters.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Lastname is required.")]
        [StringLength(20, ErrorMessage = "Lastname must not exceed 20 characters.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string? Email { get; set; }
        public int? GenderId { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }
        public int? StateId { get; set; }
        public string? State { get; set; }
        public int? CityId { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string? City { get; set; }
        public bool Active { get; set; }
        public bool IsDeactivated { get; set; }
        public DateTime DateCreated { get; set; }
        public string? Role { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be 11 digits.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set; }

    }
}

