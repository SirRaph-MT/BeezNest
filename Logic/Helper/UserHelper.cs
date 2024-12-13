using Core.Models;
using Core.ViewModels;
using Logic.IHelper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Helper
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserHelper(UserManager<ApplicationUser> userManager) 
        {
            _userManager = userManager;

        }


        public async Task<ApplicationUser> CreateUserByAsync(ApplicationUserViewModel applicationUserViewModel)
        {
            try
            {
                if (applicationUserViewModel == null)
                {
                    throw new ArgumentNullException(nameof(applicationUserViewModel), "User data is required.");
                }

            
                var user = new ApplicationUser
                {
                    FirstName = applicationUserViewModel.FirstName,
                    LastName = applicationUserViewModel.LastName,
                    DateCreated = DateTime.Now,
                    PhoneNumber = applicationUserViewModel.PhoneNumber,
                    UserName = applicationUserViewModel.Email,                  
                    Email = applicationUserViewModel.Email,                  
                    Address = applicationUserViewModel.Address,
                    Role = applicationUserViewModel.Role,
                };

               
                var result = await _userManager.CreateAsync(user, applicationUserViewModel.Password).ConfigureAwait(false);
                if (result.Succeeded)
                {
                   await _userManager.AddToRoleAsync(user, applicationUserViewModel.Role).ConfigureAwait(false);
                    return user;
                 
                }


                return null;
            }
            catch (Exception ex)
            {
              
                throw new Exception($"An error occurred during user creation: {ex.Message}", ex);
            }
        }



        //public ApplicationUser FindUserByEmailAsync(string email)
        //{
        //    var user = _userManager.Users.Where(x => x.Email == email).FirstOrDefault();
        //    return user;
        //}


        public async Task<ApplicationUser> FindUserByEmailAsync(string email)
        {
            // Use the UserManager's built-in asynchronous method
            return await _userManager.FindByEmailAsync(email);
        }

    }
}
