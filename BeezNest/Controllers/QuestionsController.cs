using Core.Db;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeezNest.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public QuestionsController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
          
        }

        public IActionResult Index()
        {
            IEnumerable<AskedQuestions> objList = _db.AskedQuestions;
            return View(objList);
        }
        public IActionResult CreateQuestions()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateQuestions(AskedQuestions dropdownView)
        {
            if (ModelState.IsValid)
            {
                _db.AskedQuestions.Add(dropdownView);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
