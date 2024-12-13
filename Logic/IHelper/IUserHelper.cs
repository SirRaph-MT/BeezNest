using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IHelper
{
    public interface IUserHelper
    {
        Task<ApplicationUser> CreateUserByAsync(ApplicationUserViewModel applicationUserViewModel);

        //ApplicationUser FindUserByEmailAsync(string email);
        //ApplicationUser FindUserByEmailAsync(string email);

        Task<ApplicationUser> FindUserByEmailAsync(string email);
    }
}
