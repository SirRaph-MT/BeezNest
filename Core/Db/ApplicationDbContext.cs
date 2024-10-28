using Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Db
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers{ get; set; }
        public DbSet<Dropdown> DropdownModels { get; set; }
        public DbSet<UploadProduct> UploadProducts { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }
    }
}
