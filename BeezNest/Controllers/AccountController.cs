using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.Helper;
using Logic.IHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
            // tested committing
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
        public IActionResult AdminRegistration()
        {
            return View();
        }


        [HttpPost]
        public async Task<JsonResult> Register(string userDetails)
        {
            try
            {
                if (userDetails == null)
                {
                    return Json(new { isError = true, msg = "Invalid data." });
                }

                var info = JsonConvert.DeserializeObject<ApplicationUserViewModel>(userDetails);


                var checkForEmail = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == info.Email);
                if (checkForEmail != null)
                {
                    return Json(new { isError = true, msg = "Email already exists." });
                }

                // Create user
                var addUser = await _userHelper.CreateUserByAsync(info);
                if (addUser != null)
                {
                    return Json(new { isError = false, msg = "Registration successful!" });
                }
                else
                {
                    return Json(new { isError = true, msg = "Registration failed!" });
                }
            }
            catch (Exception myException)
            {
                return Json(new { isError = true, msg = "An error occurred during registration: " + myException.Message });
            }
        }



        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task <IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (model != null)
                {
                    var user = _userHelper.FindUserByEmail(model.Email);
                    if (user == null)
                    {
                        TempData["ErrorMessage"] = "Invalid Login Attempt.";
                    }
                    if (user != null)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, true, true).ConfigureAwait(false);

                        if (result.Succeeded)
                        {
                            var userRole = await _userManager.GetRolesAsync(user);
                            if (userRole != null)
                            {
                                user.Role = userRole.FirstOrDefault();
                            }
                            if (user.Role?.ToLower() == "admin")
                            {
                                return RedirectToAction("UploadedProducts", "Admin");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        TempData["ErrorMessage"] = "Invalid Login Attempt";
                        ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");

        }
    }
}
