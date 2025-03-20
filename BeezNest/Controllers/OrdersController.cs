using Core.Db;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace BeezNest.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<OrderNotificationHub> _hubContext;

        public OrdersController(IHubContext<OrderNotificationHub> hubContext, ApplicationDbContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }


        public IActionResult ClientOrders()
        {
            PaymentDetailViewModels model = new PaymentDetailViewModels();
            int pendingPayments = _context.Payments.Count(p => p.Active && p.PaymentStatus == PaymentStatus.Pending);
            var payments = _context.Payments
                
                .Where(x => x.Active)
                .Include(x => x.Client)
                .Select(s => new PaymentsViewModel
                {
                    Id = s.Id,
                    GrandTotal = s.GrandTotal,
                    User = s.Client.FirstName,
                    Address = s.Client.Address,
                    PhoneNumber = s.Client.PhoneNumber,
                    ProofOfPaymentPath = s.ProofOfPaymentPath,
                    Stocks = string.IsNullOrEmpty(s.Stock)
                                ? new List<Stock>()
                                : JsonConvert.DeserializeObject<List<Stock>>(s.Stock),
                    PaymentDate = s.PaymentDate,
                    PaymentStatus = s.PaymentStatus,
                    
                })
                .OrderByDescending(x => x.PaymentDate)
                .ToList();
            model.PaymentList = payments;
            model.PendingOrdersCount = payments.Count;
            ViewBag.PendingOrdersCount = pendingPayments;
            model.PendingOrdersCount = pendingPayments;
            return View(model);

        
        }

        public IActionResult Orders()
        {
            return View();
        }
    }
}
