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
            // Fetch the main product details
            var product = _context.UploadProducts
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                return NotFound();
            }

            
            var relatedProducts = _context.UploadProducts
                .Include(p => p.ProductImages)
                .Where(p => p.Id != productId) 
                .Take(12) 
                .ToList();

            // Create and populate the view model
            var viewModel = new ProductDetailsViewModel
            {
                ProductDetails = product,
                RelatedProducts = relatedProducts
            };

            // Pass the view model to the view
            return View(viewModel);
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
