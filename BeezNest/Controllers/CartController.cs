using Microsoft.AspNetCore.Mvc;
using Core.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Core.Db;
using Logic.IHelper;
using Microsoft.AspNetCore.Identity;

namespace BeezNest.Controllers
{
    public class CartController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ICartHelper cartHelper;
        private readonly IUserHelper _userHelper;
        private readonly IAdminHelper _adminHelper;
        public CartController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, IUserHelper userHelper, IAdminHelper adminHelper)
        {
           
            _userManager = userManager;
            _signInManager = signInManager;
            _userHelper = userHelper;
            _context = context;
            _adminHelper = adminHelper;

        }

        public IActionResult ViewCart()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Checkout([FromBody] List<Cart> cartItems)
        //{
        //    if (cartItems == null || !cartItems.Any())
        //    {
        //        return BadRequest(new { message = "Cart is empty" });
        //    }

        //    foreach (var item in cartItems)
        //    {
        //        var orderItem = new order 

        //        {
        //            ProductId = item.UploadProductId,
        //            Price = item.Price,
        //            Quantity = item.Quantity,
        //            UserId = item.UserId,
        //            OrderDate = DateTime.Now 
        //        };

        //        _context.Carts.Add(orderItem); 
        //    }

        //    // Save changes to the database
        //    await _context.SaveChangesAsync();


           

        //    return Ok(new { message = "Checkout successful!" });
        //}

    }

}