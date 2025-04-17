using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Errors;
using Store.APIs.Extensions;
using Store.Core.DTO.Auth;
using Store.Core.Entities.Identity;
using Store.Core.Services.Contract;

namespace Store.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountController(IUserService userService, 
            UserManager<AppUser> userManager, 
            ITokenService tokenService,
            IMapper mapper
            )
        {
            this.userService = userService;
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await userService.LoginAsync(loginDto);
            if (user is null) return Unauthorized(new ApiErrorResponse(401));

            return Ok(user);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<RegisterDto>> Register(RegisterDto registerDto)
        {
            var user = await userService.RegisterAsync(registerDto);
            if (user is null) return BadRequest(new ApiErrorResponse(400,"Invalid Register"));

            return Ok(user);
        }


        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser(RegisterDto registerDto)
        {
           var userEmail =  User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return BadRequest(new ApiErrorResponse(400));

            var user = await userManager.FindByEmailAsync(userEmail);

            if(user is null) return BadRequest(new ApiErrorResponse(400));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await tokenService.CreateTokenAsync(user,userManager)
            });

        }


        [HttpGet("Address")]
        public async Task<ActionResult<UserDto>> GetCurrentUserAddress(RegisterDto registerDto)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return BadRequest(new ApiErrorResponse(400));

            var user = await userManager.FindByEmailWithAdressAsync(User);

            if (user is null) return BadRequest(new ApiErrorResponse(400));

            return Ok(mapper.Map<AddressDto>(user.Address));

        }
    }
}
