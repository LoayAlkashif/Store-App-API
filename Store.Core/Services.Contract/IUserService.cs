﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Core.DTO.Auth;

namespace Store.Core.Services.Contract
{
    public interface IUserService
    {
        Task<UserDto> LoginAsync(LoginDto loginDto);
        Task<UserDto> RegisterAsync(RegisterDto registerDto);


        Task<bool> CheckEmailExist(string email);
    }
}
