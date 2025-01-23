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


        [HttpGet]
        public IActionResult EditQuestions(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var data = _db.AskedQuestions.Find(Id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

      
        [HttpPost]
        public IActionResult EditQuestions(AskedQuestions data)
        {
            _db.AskedQuestions.Update(data);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET - DELETQUESTION
        [HttpGet]
        public IActionResult DeleteQuestions(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var data = _db.AskedQuestions.Find(Id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        //POST - DeleteQUESTION
        [HttpPost]
        public IActionResult DeleteQuestion(int? Id)
        {
            var data = _db.AskedQuestions.Find(Id);
            if (data == null)
            {
                return NotFound();
            }
            _db.AskedQuestions.Remove(data);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
