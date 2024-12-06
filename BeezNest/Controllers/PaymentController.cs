using Core.Db;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace BeezNest.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext db;

        public PaymentController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }  
        public IActionResult PaymentHistory()
        {
            var user = User.Identity.Name;
           if(user == null)
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
                    PaymentDate = s.PaymentDate.Value.ToString("dd-MM-YYYY"),
                    PaymentStatus = s.PaymentStatus,
                    
                }).ToList();
            return View(payments);
        }

        //[HttpPost]
        //public JsonResult UserCheckOut(string stockDetails)
        //{
        //    if (string.IsNullOrEmpty(stockDetails))
        //    {
        //        return Json(new { isError = true, msg = "Error Occurred" });
        //    }
        //    var details = JsonConvert.DeserializeObject<PaymentsViewModel>(stockDetails);
        //    if (details== null)
        //    {
        //        return Json(new { isError = true, msg = "Error Occurred" });
        //    }
        //    var clientId = db.ApplicationUsers.FirstOrDefault(x=> x.Email == details.ClientEmail)?.Id;
        //    if (string.IsNullOrEmpty(clientId))
        //    {
        //        return Json(new { isError = true, msg = "Error Occurred" });
        //    }
        //    var payment = new Payments
        //    {
        //        PaymentDate = DateTime.Now,
        //        Stock = details.StocksInString,
        //        GrandTotal = details.GrandTotal,
        //        ClientId = clientId,
        //        Active  = true
        //    };
        //    db.Add(payment);
        //    db.SaveChanges();
        //    return Json(new { isError = false, msg = "Order Completed" });
        //}

        //[HttpPost]
        //public JsonResult UserCheckOut(string stockDetails, IFormFile proofOfPayment)
        //{
        //    if (string.IsNullOrEmpty(stockDetails))
        //    {
        //        return Json(new { isError = true, msg = "Error Occurred" });
        //    }

        //    var details = JsonConvert.DeserializeObject<PaymentsViewModel>(stockDetails);
        //    if (details == null)
        //    {
        //        return Json(new { isError = true, msg = "Error Occurred" });
        //    }

        //    var clientId = db.ApplicationUsers.FirstOrDefault(x => x.Email == details.ClientEmail)?.Id;
        //    if (string.IsNullOrEmpty(clientId))
        //    {
        //        return Json(new { isError = true, msg = "Error Occurred" });
        //    }

        //    // Handle file upload
        //    string proofOfPaymentPath = null;
        //    if (proofOfPayment != null && proofOfPayment.Length > 0)
        //    {
        //        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        //        if (!Directory.Exists(uploadsFolder))
        //        {
        //            Directory.CreateDirectory(uploadsFolder);
        //        }

        //        var uniqueFileName = Guid.NewGuid().ToString() + "_" + proofOfPayment.FileName;
        //        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            proofOfPayment.CopyTo(stream);
        //        }

        //        proofOfPaymentPath = $"/uploads/{uniqueFileName}";
        //    }

        //    var payment = new Payments
        //    {
        //        PaymentDate = DateTime.Now,
        //        Stock = details.StocksInString,
        //        GrandTotal = details.GrandTotal,
        //        ClientId = clientId,
        //        Active = true,
        //        ProofOfPaymentPath = proofOfPaymentPath, // Save the file path
        //        PaymentStatus = PaymentStatus.Pending
        //    };

        //    db.Add(payment);
        //    db.SaveChanges();

        //    return Json(new { isError = false, msg = "Order Completed" });
        //}

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
                ProofOfPaymentPath = proofOfPaymentPath, // Save the file path
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
            var order = db.Payments.FirstOrDefault(p => p.Active && p.PaymentStatus == PaymentStatus.Pending && p.Id == Id);

            if (order == null) return RedirectToAction("ClientOrders", "Orders");

            order.PaymentStatus = PaymentStatus.Confirm;
            db.SaveChanges(); 

            return RedirectToAction("ClientOrders", "Orders");
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
