using BeezNest.Models;
using Core.Db;
using Core.Models;
using Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BeezNest.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;

        public UploadProduct uploadProduct { get; private set; }
        public object FAQs { get; private set; }

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Fetch products and their related data
            var products = _context.UploadProducts
                                   .Include(p => p.ProductImages)
                                   .OrderByDescending(p => p.DateSampled)
                                   //.Take(12)
                                   .ToList();

            var faqs = _context.AskedQuestions.ToList();

            var viewModel = new UploadProductsViewModel
            {
                UploadProduct = products.FirstOrDefault(),
                ListUploadProducts = products,
                FAQs = faqs
            };

            return View(viewModel);
        }



        public IActionResult ProductDetails(int productId)
        {
            var product = _context.UploadProducts
                .Include(p => p.ProductImages) 
                .Include(p => p.Ratings) 
                .FirstOrDefault(p => p.Id == productId);

           
            if (product == null)
            {
                return NotFound(); 
            }

            
            var relatedProducts = _context.UploadProducts
                .Where(p => p.Id != productId) 
                .Include(p => p.ProductImages)
                .Take(12) 
                .ToList();

          
            var averageRating = product.Ratings?.Any() == true
                ? product.Ratings.Average(r => r.RatingValue) 
                : 0.0; 

            var totalRatings = product.Ratings?.Count ?? 0; 

           
            var viewModel = new ProductDetailsViewModel
            {
                ProductDetails = product, 
                RelatedProducts = relatedProducts, 
                AverageRating = Math.Round(averageRating, 1), 
                TotalRatings = totalRatings,
                Ratings = product.Ratings 
            };

            return View(viewModel);
        }


        public IActionResult ProductRatingsAndReviews(int productId)
        {
            var product = _context.UploadProducts
                .Include(p => p.Ratings)
                .ThenInclude(r => r.Client) 
                .FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                return RedirectToAction("ProductNotFound");
            }

            ViewData["ProductName"] = product.ProductsModel ?? "Unnamed Product";
            ViewData["ProductId"] = product.Id;

            var raters = product.Ratings?.ToList() ?? new List<Rating>();
            return View(raters);
        }


        [HttpPost]
        public IActionResult Search(string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                
                return RedirectToAction("Index");
            }
            var filteredProducts = _context.UploadProducts
                 .Include(p => p.ProductImages)
                .AsEnumerable() 
                .Where(p => p.ProductsModel.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                || p.Specifications.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return View("SearchResults", filteredProducts);
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
