using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Store.Core.Entities.Identity;

namespace Store.Repository.Identity.DataSeed
{
    public static class StoreIdentityDbContextSeed
    {

        public async static Task SeedAppUserAsync(UserManager<AppUser> userManager)
        {

            if(userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    Email = "loayalkashif@gmail.com",
                    DisplayName = "Loay Alkashif",
                    UserName = "Loay.Alkashif",
                    Address = new Address()
                    {
                        FName = "Loay",
                        LName = "Alkashif",
                        City = "Zag",
                        Country = "Egypt",

                    }
                };

                await userManager.CreateAsync(user, "Lo@y12");
            }

        }
    }
}
