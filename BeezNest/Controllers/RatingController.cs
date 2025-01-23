using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.Helper;
using Logic.IHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BeezNest.Controllers
{
    public class RatingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserHelper _userHelper;

        public RatingController(ApplicationDbContext context, IUserHelper userHelper)
        {
            _userHelper = userHelper;
            _context = context;
        }
        public IActionResult RateProduct(int productId)
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var hasPurchased = _context.Payments
                .Any(p => p.ClientId == userId && p.Stock.Contains(productId.ToString())); 

            if (!hasPurchased)
            {
                return Unauthorized("You can only rate products you have purchased.");
            }

            var product = _context.UploadProducts.Find(productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            var model = new RatingViewModel
            {
                ProductId = productId,
                ProductName = product.ProductsModel,
            };

            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> SubmitRating([FromBody] RatingViewModel model)
        {
            if (model == null || model.Rating < 1 || model.Rating > 5)
            {
                return BadRequest(new { error = "Invalid rating value" });
            }
            var loggedInUsername =  User.Identity.Name;
            var clientFullDetails = await _userHelper.FindUserByUsername(loggedInUsername);
            
            var newRating = new Rating
            {
                ProductId = model.ProductId,
                RatingValue = model.Rating,
                ReviewComment = model.Review,
                ClientId = clientFullDetails.Id,
            };
          
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var hasPurchased = _context.Payments
                .Any(p => p.ClientId == userId && p.Stock.Contains(model.ToString()));

            if (!hasPurchased)
            {
                return Unauthorized("You can only rate products you have purchased.");
            }
            _context.Ratings.Add(newRating);
            _context.SaveChanges();

           
            var productRatings = _context.Ratings.Where(r => r.ProductId == model.ProductId);
            var averageRating = productRatings.Average(r => r.RatingValue);
            var totalRatings = productRatings.Count();

            
            return Ok(new { newAverageRating = averageRating, totalRatings = totalRatings });
        }




    }
}
