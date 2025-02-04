using Core.Db;
using Core.Models;
using Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BeezNest.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext db;

        public PaymentController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index(string? Id)
        {

            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }

            var data = db.ApplicationUsers
                .Where(u => u.Id == Id)
                .Select(u => new ApplicationUserViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    PhoneNumber = u.PhoneNumber,
                    Address = u.Address,
                    Email = u.Email,
                    DateCreated = u.DateCreated
                })
                .FirstOrDefault();

            if (data == null)
            {
                return NotFound();
            }

            return View(data);

        }

        [HttpGet]
        public JsonResult GetOrderDetails(int paymentId, bool isFromAdmin)
        {
            var payments = db.Payments
                .Where(x => x.Active)
                .Include(x => x.Client)
                .ToList();
            var stocks = new List<Stock>();

            if (!isFromAdmin)
            {
                var userPayments = payments
                    .Where(x => x.Active && x.Client?.Email == User.Identity.Name && x.Id == paymentId)
                    .FirstOrDefault();
                stocks = JsonConvert.DeserializeObject<List<Stock>>(userPayments.Stock);
            }
            else
            {
                var adminPayments = payments
                    .Where(x => x.Active && x.Id == paymentId)
                    .FirstOrDefault();
                stocks = JsonConvert.DeserializeObject<List<Stock>>(adminPayments.Stock);
            }

            return Json(new { isError = false, stock = stocks });
        }


        public IActionResult PaymentHistory()
        {
            var user = User.Identity.Name;
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var payments = db.Payments
                .Where(x => x.Active && x.Client.Email == user)
                .Select(s => new PaymentsViewModel
                {
                    Id = s.Id,
                    GrandTotal = s.GrandTotal,
                    User = s.Client.FirstName,
                    Stocks = JsonConvert.DeserializeObject<List<Stock>>(s.Stock),
                    PaymentDate = s.PaymentDate,
                    PaymentStatus = s.PaymentStatus,

                }).ToList();
            return View(payments);
        }



        [HttpPost]
        public JsonResult UserCheckOut(string stockDetails, IFormFile proofOfPayment)
        {
            if (string.IsNullOrEmpty(stockDetails))
            {
                return Json(new { isError = true, msg = "Error Occurred" });
            }

            var details = JsonConvert.DeserializeObject<PaymentsViewModel>(stockDetails);
            if (details == null)
            {
                return Json(new { isError = true, msg = "Error Occurred" });
            }

            var clientId = db.ApplicationUsers.FirstOrDefault(x => x.Email == details.ClientEmail)?.Id;
            if (string.IsNullOrEmpty(clientId))
            {
                return Json(new { isError = true, msg = "Error Occurred" });
            }

            // Handle file upload
            string proofOfPaymentPath = null;
            if (proofOfPayment != null && proofOfPayment.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + proofOfPayment.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    proofOfPayment.CopyTo(stream);
                }

                proofOfPaymentPath = $"/uploads/{uniqueFileName}";
            }

            var payment = new Payments
            {
                PaymentDate = DateTime.Now,
                Stock = details.StocksInString,
                GrandTotal = details.GrandTotal,
                ClientId = clientId,
                Active = true,
                ProofOfPaymentPath = proofOfPaymentPath,
                PaymentStatus = PaymentStatus.Pending
            };

            db.Add(payment);
            db.SaveChanges();

            // Get updated count of pending orders
            var pendingOrdersCount = db.Payments.Count(p => p.PaymentStatus == PaymentStatus.Pending);

            return Json(new { isError = false, msg = "Order Completed", orderCount = pendingOrdersCount });
        }


        [HttpGet]
        public IActionResult ConfirmOrderPayment(int Id)
        {
            if (Id != 0)
            {
                var order = db.Payments.FirstOrDefault(p => p.Active && p.PaymentStatus == PaymentStatus.Pending && p.Id == Id);

                if (order == null) return RedirectToAction("ClientOrders", "Orders");
                UpdateWarehouseInventory(order);
                order.PaymentStatus = PaymentStatus.Confirm;
                db.SaveChanges();
            }

            return RedirectToAction("ClientOrders", "Orders");
        }

        public void UpdateWarehouseInventory(Payments paymentDetails)
        {
            if (paymentDetails == null || string.IsNullOrEmpty(paymentDetails.Stock))
                return;

            var stocksFromPayment = JsonConvert.DeserializeObject<List<Stock>>(paymentDetails.Stock);
            if (stocksFromPayment == null || !stocksFromPayment.Any())
                return;

            var productNames = stocksFromPayment.Select(stock => stock.Name).Distinct().ToList();
            var productsInShop = db.UploadProducts.Where(p => productNames.Contains(p.ProductsModel)).ToList();

            foreach (var stock in stocksFromPayment)
            {
                if (stock == null)
                    continue;

                var product = productsInShop.FirstOrDefault(p => p.ProductsModel == stock.Name);
                if (product?.NumberOfItem > 0)
                {
                    var numberOfStockRemaining = product.NumberOfItem - stock.Quantity;
                    product.NumberOfItem = Convert.ToInt32(numberOfStockRemaining > 0 ? numberOfStockRemaining : 0);
                    db.UploadProducts.Update(product);
                }
            }

            db.SaveChanges();
        }


        [HttpGet]
        public IActionResult DeclineOrderPayment(int Id)
        {
            var order = db.Payments.FirstOrDefault(p => p.Active && p.PaymentStatus == PaymentStatus.Pending && p.Id == Id);

            if (order == null) return RedirectToAction("ClientOrders", "Orders");

            order.PaymentStatus = PaymentStatus.Decline;
            db.SaveChanges();

            return RedirectToAction("ClientOrders", "Orders");
        }

    }
}
