using Core.Db;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace BeezNest.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult ClientOrders()
        {
            var user = _context.ApplicationUsers;
            if (user == null)
            {

            }

            var payments = _context.Payments
                
                .Where(x => x.Active)
                .Include(x => x.Client)
                .Select(s => new PaymentsViewModel
                {
                    Id = s.Id,
                    GrandTotal = s.GrandTotal,
                    User = s.Client.FirstName,
                    ProofOfPaymentPath = s.ProofOfPaymentPath,
                    //Stocks = JsonConvert.DeserializeObject<List<Stock>>(s.Stock),
                    Stocks = string.IsNullOrEmpty(s.Stock)
                                ? new List<Stock>()
                                : JsonConvert.DeserializeObject<List<Stock>>(s.Stock),
                    PaymentDate = s.PaymentDate.Value.ToString("dd-MM-YY"),
                    PaymentStatus = s.PaymentStatus
                }).ToList();
            return View(payments);

        
        }

        //var clientOrders = _context.Payments.Include(p => p.Client)
        //                    .Where(x => x.Active)
        //                .OrderByDescending(x => x.PaymentDate)
        //                .ToList();

        //return View(clientOrders);
 

        public IActionResult Orders()
        {
            return View();
        }
    }
}
