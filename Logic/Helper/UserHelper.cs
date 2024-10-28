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
                if (applicationUserViewModel != null)
                {
                    var user = new ApplicationUser()
                    {
                        FirstName = applicationUserViewModel.FirstName,
                        LastName = applicationUserViewModel.LastName,
                        DateCreated = DateTime.Now,
                        PhoneNumber = applicationUserViewModel.PhoneNumber,
                        UserName = applicationUserViewModel.Email,
                        Email = applicationUserViewModel.Email,
                        Address = applicationUserViewModel.Address,
                    };
                    var result = await _userManager.CreateAsync(user, applicationUserViewModel.Password);
                    if (result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        if (!userRoles.Contains("User"))
                        {
                            var roleResult = await _userManager.AddToRoleAsync(user, "User");
                            if (!roleResult.Succeeded)
                            {
                                var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                                throw new Exception($"Role assignment failed: {roleErrors}");
                            }
                        }
                        await _userManager.AddToRoleAsync(user, applicationUserViewModel.Role);
                        return user;
                    }
                    return user;

                }
                return null;
            }
            catch (Exception myException)
            {

                throw myException;
            }

         
        }

    }
}
