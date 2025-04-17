using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Store.Core.Entities.Identity;

namespace Store.Core.Services.Contract
{
    public interface ITokenService
    {

        Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager);
    }
}
