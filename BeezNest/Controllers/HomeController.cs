using BeezNest.Models;
using Core.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BeezNest.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Eagerly load ProductImages with UploadProducts
            var uploadProducts = _context.UploadProducts
                                         .Include(p => p.ProductImages)
                                         .OrderByDescending(x=>x.DateSampled)// Load related images
                                         .ToList();

            return View(uploadProducts);
        }

        public IActionResult ProductDetails(int productId)
        {
            // Find the product and include related images
            var product = _context.UploadProducts
                .Include(p => p.ProductImages) // Include related images
                .FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                return NotFound(); // Return 404 if product is not found
            }

            // Pass the product to the view
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
