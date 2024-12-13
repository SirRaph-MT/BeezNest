using Core.Db;
using Core.Models;
using Core.ViewModels;
using Logic.Helper;
using Logic.IHelper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;



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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (model == null)
                {
                    TempData["ErrorMessage"] = "Please provide your login details.";
                    return View(model);
                }

                var user = await _userHelper.FindUserByEmailAsync(model.Email); // Ensure this is asynchronous if supported.
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Invalid email or password.";
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, true, true).ConfigureAwait(false);

                if (result.Succeeded)
                {
                    var userRole = await _userManager.GetRolesAsync(user);
                    user.Role = userRole.FirstOrDefault();

                    if (user.Role?.ToLower() == "admin")
                    {
                        return RedirectToAction("UploadedProducts", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                // Handle specific failure cases for better feedback
                if (result.IsLockedOut)
                {
                    TempData["ErrorMessage"] = "Your account is locked. Please try again later.";
                }
                else if (result.IsNotAllowed)
                {
                    TempData["ErrorMessage"] = "Your account is not enabled. Please contact support.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid email or password.";
                }

                return View(model);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                TempData["ErrorMessage"] = "An error occurred while processing your request. Please try again.";
                return View(model);
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    try
        //    {
        //        if (model == null)
        //        {
        //            TempData["ErrorMessage"] = "Please provide your login details.";
        //            return View(model);
        //        }

        //        // Find the user by email
        //        var user = await _userHelper.FindUserByEmailAsync(model.Email);
        //        if (user == null)
        //        {
        //            TempData["ErrorMessage"] = "Invalid email or password.";
        //            return View(model);
        //        }

        //        // Attempt password sign-in
        //        var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, true, true).ConfigureAwait(false);

        //        if (result.Succeeded)
        //        {
        //            // Add custom claims (e.g., FirstName) to the user's claims identity
        //            await AddCustomClaims(user);

        //            // Determine user role for redirection
        //            var userRole = await _userManager.GetRolesAsync(user);
        //            user.Role = userRole.FirstOrDefault();

        //            if (user.Role?.ToLower() == "admin")
        //            {
        //                return RedirectToAction("UploadedProducts", "Admin");
        //            }
        //            else
        //            {
        //                return RedirectToAction("Index", "Home");
        //            }
        //        }

        //        // Handle specific failure cases for better feedback
        //        if (result.IsLockedOut)
        //        {
        //            TempData["ErrorMessage"] = "Your account is locked. Please try again later.";
        //        }
        //        else if (result.IsNotAllowed)
        //        {
        //            TempData["ErrorMessage"] = "Your account is not enabled. Please contact support.";
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = "Invalid email or password.";
        //        }

        //        return View(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception if needed
        //        TempData["ErrorMessage"] = "An error occurred while processing your request. Please try again.";
        //        return View(model);
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    try
        //    {
        //        if (model == null)
        //        {
        //            TempData["ErrorMessage"] = "Please provide your login details.";
        //            return View(model);
        //        }

        //        // Find the user by email
        //        var user = await _userHelper.FindUserByEmailAsync(model.Email);
        //        if (user == null)
        //        {
        //            TempData["ErrorMessage"] = "Invalid email or password.";
        //            return View(model);
        //        }

        //        // Attempt password sign-in
        //        var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, true, true).ConfigureAwait(false);

        //        if (result.Succeeded)
        //        {
        //            // Add custom claims (e.g., FirstName) to the user's claims identity
        //            await AddCustomClaims(user);

        //            // Determine user role for redirection
        //            var userRole = await _userManager.GetRolesAsync(user);
        //            user.Role = userRole.FirstOrDefault();

        //            if (user.Role?.ToLower() == "admin")
        //            {
        //                return RedirectToAction("UploadedProducts", "Admin");
        //            }
        //            else
        //            {
        //                return RedirectToAction("Index", "Home");
        //            }
        //        }

        //        // Handle specific failure cases for better feedback
        //        if (result.IsLockedOut)
        //        {
        //            TempData["ErrorMessage"] = "Your account is locked. Please try again later.";
        //        }
        //        else if (result.IsNotAllowed)
        //        {
        //            TempData["ErrorMessage"] = "Your account is not enabled. Please contact support.";
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = "Invalid email or password.";
        //        }

        //        return View(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception if needed
        //        TempData["ErrorMessage"] = "An error occurred while processing your request. Please try again.";
        //        return View(model);
        //    }
        //}


        private async Task AddCustomClaims(ApplicationUser user)
        {
            // Retrieve the current claims principal
            var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
            var identity = claimsPrincipal.Identity as ClaimsIdentity;

            if (identity != null)
            {
                // Check if the claim already exists
                if (!identity.HasClaim(c => c.Type == "FirstName"))
                {
                    identity.AddClaim(new Claim("FirstName", user.FirstName));
                }
            }

            // Sign in the user with updated claims
            await HttpContext.SignOutAsync(); // Sign out the previous session
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        }







        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");

        }
    }
}
