    using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class ApplicationUser: IdentityUser
    {
        public ApplicationUser() 
        { 
            Active = true;
            DateCreated = DateTime.Now;
        }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
       
        [NotMapped]
        public string? Role { get; set; }

    }

   
}
