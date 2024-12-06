using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.IHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Helper
{
    public class AdminHelper : IAdminHelper
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;



        public AdminHelper(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
           
        }
        public string GetUserId(string username)
        {
            return _userManager.Users.Where(s => s.UserName == username)?.FirstOrDefaultAsync().Result.Id?.ToString();
        }

         public Cart CreateCart(int UploadProductId, string userId)
        {
            var sample = _context.UploadProducts.Where(s => s.Id == UploadProductId).FirstOrDefault();
            if (userId != null && UploadProductId > 0)
            {
                var cart = new Cart()
                {

                    Quantity = 1,
                    Active = true,
                    Deleted = false,
                    UserId = userId,
                    UploadProductId = UploadProductId,
                    
                };
                _context.Add(cart);
                _context.SaveChanges();


                return cart;

            }
            return null;

        }

        public async Task<List<Cart>> GetAllCart(string email)
        {
            var cartViewModel = new List<Cart>();
            var loggedInUser = _userManager.Users.Where(x => x.UserName == email).FirstOrDefault();
            var userCart = await _context.Carts.Where(x => x.UserId == loggedInUser.Id && x.Deleted == false && x.Active == true).Include(u => u.UploadProduct).Include(Carts => Carts.User).ToListAsync();
            var cart = userCart.Select(ct => new Cart
            {
                Id = ct.Id,
                Quantity = ct.Quantity,
                Price = ct.TotalPrice,
                UploadProduct = ct.UploadProduct,


            });
            if (userCart.Any())
            {
                return userCart;
            }
            return cartViewModel;
        }



    }
}
