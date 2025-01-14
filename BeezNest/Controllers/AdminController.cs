using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.IHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
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

        [HttpPost]
        public IActionResult CreateDropdown(Dropdown dropdownView)
        {
            if (ModelState.IsValid)
            {
                _db.DropdownModels.Add(dropdownView);
                _db.SaveChanges();
               
            }
            return RedirectToAction("Index");
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
                uploadProductModel.Description = product.Description;
                uploadProductViewModels.Add(uploadProductModel);
            };
            return View(uploadProductViewModels);
        }

        //GET - Create/uploadproduct 
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
                    Description = item.Description,
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
                        var filePath = Path.Combine(uploadsFolderPath, fileName); 

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            image.CopyTo(fileStream); 
                        }

                        var imageUrl = $"/{uploadsFolder}/{fileName}";

                   
                        var productImage = new ProductImage
                        {
                            ImageUrl = imageUrl,
                            UploadProduct = product 
                        };

                        product.ProductImages.Add(productImage);
                    }
                }

                _db.UploadProducts.Add(product);
                _db.SaveChanges(); 

                return RedirectToAction("UploadedProducts");
            }

            return View(item); 
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
            data.DateSampled = DateTime.Now;
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

        [HttpGet]
        public IActionResult UploadedImages(int productId)
        {
           
            var product = _db.UploadProducts
                .Include(p => p.ProductImages) 
                .FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                return NotFound(); 
            }
 
            return View(product);

        }

        [HttpGet]
        public IActionResult GetStates()
        {
            var states = _db.DropdownModels
                .Where(d => d.Key == "State")
                .Select(d => new { d.Key, d.DropdownName })
                .ToList();

            return Json(states);
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            var cities = _db.DropdownModels
                .Where(d => d.Key == "City")
                .Select(d => new { d.Key, d.DropdownName })
                .ToList();

            return Json(cities);
        }



    }
}
