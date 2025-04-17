using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Store.Core.Entities.Identity;
using Store.Core.Services.Contract;
using Microsoft.IdentityModel.Tokens;

namespace Store.Service.Services.Tokens
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration config;

        public TokenService(IConfiguration config)
        {
            this.config = config;
        }
        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {
            //Header (Algorithm, type)
            // Payload (claims)
            // signature

            var authClaims = new List<Claim>()
            {

                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.DisplayName),
            };

            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SECRETKEY"]));

            var token = new JwtSecurityToken(
                issuer: config["Jwt:IssuerApi"],
                audience: config["Jwt:AudienceApi"],
                expires: DateTime.Now.AddDays(double.Parse(config["Jwt:DurationInDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey,SecurityAlgorithms.HmacSha256Signature )                
               );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
