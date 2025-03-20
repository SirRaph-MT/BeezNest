using Core.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BeezNest.Controllers
{
    public class BaseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.PendingOrdersCount = _context.Payments.Count(o => o.PaymentStatus == Core.Models.PaymentStatus.Pending);
            base.OnActionExecuting(context);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

