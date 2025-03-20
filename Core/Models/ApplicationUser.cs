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
		public int DropdownId { get; set; }
        [ForeignKey(nameof(DropdownId))]
		public virtual Dropdown? Dropdown { get; set; }
       
        [NotMapped]
        public string? Role { get; set; }

        [NotMapped]
        public string? FullName => $"{FirstName} {LastName}";

    }

   
}
