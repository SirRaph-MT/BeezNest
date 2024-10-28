using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.IHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BeezNest.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AdminController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Dropdown> objList = _db.DropdownModels;
            return View(objList);
        }

        [HttpGet]
        public IActionResult CreateDropdown()
        {
          
            return View();
        }

        public IActionResult UploadedProduct()
        {
            
            var productList = _db.UploadProducts
                .Select(product => new
                {
                    Product = product,
                    Images = _db.ProductImages.Where(pi => pi.UploadProductId == product.Id).ToList()
                })
                .ToList();

            return View(productList);
        }

        [HttpGet]
        public IActionResult UploadedProducts()
        {
            var uploadProductViewModels = new List<UploadProductViewModel>();
            var products = _db.UploadProducts
                .OrderByDescending(x=>x.DateSampled)
                .ToList();
            if(products == null || !products.Any())
            {
                return View(uploadProductViewModels);
            }
            foreach (var product in products)
            {
                var uploadProductModel = new UploadProductViewModel();
                var images = _db.ProductImages
                    .Where(r => r.UploadProductId == product.Id)
                    .ToList();
                if (images != null && images.Count() > 0)
                {
                    uploadProductModel.UploadedImage = images.Select(x => x.ImageUrl).ToList().FirstOrDefault();
                }
                uploadProductModel.Id = product.Id;
                uploadProductModel.ProductsModel = product.ProductsModel;
                uploadProductModel.Specifications = product.Specifications;
                uploadProductModel.Price = product.Price;
                uploadProductModel.NumberOfItem = product.NumberOfItem;
                uploadProductModel.DateSampled = product.DateSampled;
                uploadProductModel.Colors = product.Colors;
               
                uploadProductViewModels.Add(uploadProductModel);
            };
            return View(uploadProductViewModels);
        }

        //GET - Create/uploadproducts 
        [HttpGet]
        public IActionResult CreateProduct()
        {

            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(UploadProductViewModel item)
        {
            if (ModelState.IsValid)
            {
                // Create new UploadProduct object
                var product = new UploadProduct
                {
                    ProductsModel = item.ProductsModel,
                    Specifications = item.Specifications,
                    Price = item.Price,
                    NumberOfItem = item.NumberOfItem,
                    DateSampled = DateTime.Now,
                    Colors = item.Colors,
                    ProductImages = new List<ProductImage>() 
                };

                // Handle multiple image uploads
                if (item.ProductImages != null && item.ProductImages.Any())
                {
                    var uploadsFolder = "StockPhotos";
                    var uploadsFolderPath = Path.Combine(webHostEnvironment.WebRootPath, uploadsFolder);

                    if (!Directory.Exists(uploadsFolderPath))
                    {
                        Directory.CreateDirectory(uploadsFolderPath);
                    }

                    foreach (var image in item.ProductImages)
                    {
                        string fileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                        var filePath = Path.Combine(uploadsFolderPath, fileName); // Full path to save the file

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            image.CopyTo(fileStream); // Save the file to disk
                        }

                        var imageUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{uploadsFolder}/{fileName}";

                        // Create and add ProductImage object to UploadProduct
                        var productImage = new ProductImage
                        {
                            ImageUrl = imageUrl, // Corrected: Save the image URL relative to the site
                            UploadProduct = product // Associate the image with the product
                        };

                        // Add the ProductImage to the ProductImages collection
                        product.ProductImages.Add(productImage);
                    }
                }


                // Save the product and the images
                _db.UploadProducts.Add(product);
                _db.SaveChanges(); // Save both the product and the associated images

                return RedirectToAction("UploadedProducts");
            }

            return View(item); // Reload the form if validation fails
        }

        //GET - EDITDROPDOWN
        [HttpGet]
        public IActionResult EditDropdown(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var data = _db.DropdownModels.Find(Id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        //POST - EDITDROPDOWN
        [HttpPost]
        public IActionResult EditDropdown(Dropdown data)
        {
            _db.DropdownModels.Update(data);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET - DELETDROPDOWN
        [HttpGet]
        public IActionResult DeleteDropdown(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var data = _db.DropdownModels.Find(Id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        //POST - DeleteDROPDOWN
        [HttpPost]
        public IActionResult DeletePost(int? Id)
        {
            var data = _db.DropdownModels.Find(Id);
            if (data == null)
            {
                return NotFound();
            }
            _db.DropdownModels.Remove(data);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET - EDITPRODUCT
        [HttpGet]
        public IActionResult EditProduct(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var data = _db.UploadProducts.Find(Id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }


        //POST - EDITPRODUCT
        [HttpPost]
        public IActionResult EditProduct(UploadProduct data)
        {
            _db.UploadProducts.Update(data);
            _db.SaveChanges();
            return RedirectToAction("UploadedProducts");
        }


        //GET - DELETPRODUCT
        [HttpGet]
        public IActionResult DeleteProduct(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var data = _db.UploadProducts.Find(Id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        //POST - DELETEPRODUCT
        [HttpPost]
        public IActionResult DeleteProducts(int? Id)
        {
            var data = _db.UploadProducts.Find(Id);
            if (data == null)
            {
                return NotFound();
            }
            _db.UploadProducts.Remove(data);
            _db.SaveChanges();
            return RedirectToAction("UploadedProducts");
        }

    }
}
