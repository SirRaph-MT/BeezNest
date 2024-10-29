using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.Helper;
using Logic.IHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeezNest.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        private readonly IUserHelper _userHelper;
        private readonly IAdminHelper _adminHelper;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, IUserHelper userHelper, IAdminHelper adminHelper)
        {
            // tested comitting
            _userManager = userManager;
            _signInManager = signInManager;
            _userHelper = userHelper;
            _context = context;
            _adminHelper = adminHelper;

        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Register([FromBody] ApplicationUserViewModel applicationUserViewModel)
        {
            try
            {
                if (applicationUserViewModel == null)
                {
                    return Json(new { isError = true, msg = "Invalid data." });
                }
                var checkForEmail = _context.ApplicationUsers.FirstOrDefault(x => x.Email == applicationUserViewModel.Email);
                if (checkForEmail != null)
                {
                    return Json(new { isError = true, msg = "Email already exists." });
                }

                if (applicationUserViewModel != null)
                {
                    var addUser = await _userHelper.CreateUserByAsync(applicationUserViewModel);
                    if (addUser != null)
                    {
                        return Json(new { isError = false, msg = "Registration successful!" });
                    }

                }

                return Json(new { isError = true, msg = "An error occurred during registration." });

            }
            catch (Exception myException)
            {

                throw myException;
            }
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
