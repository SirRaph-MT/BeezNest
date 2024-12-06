using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IHelper
{
    public interface IAdminHelper
    {
        string GetUserId(string username);
        Cart CreateCart(int UploadProductId, string loggedInUser);
        Task<List<Cart>> GetAllCart(string email);
    }
}