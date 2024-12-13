using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Logic.IHelper
{
   public interface IUserClaimsPrincipalFactory
    {
        Task<ApplicationUser> GenerateClaimsAsync(ApplicationUser user);

    }
}
