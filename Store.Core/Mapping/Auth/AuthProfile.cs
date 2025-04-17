using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Store.Core.DTO.Auth;
using Store.Core.Entities.Identity;

namespace Store.Core.Mapping.Auth
{
    public class AuthProfile: Profile
    {

        public AuthProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
 