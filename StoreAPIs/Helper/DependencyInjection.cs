using Microsoft.EntityFrameworkCore;
using Store.Core.Services.Contract;
using Store.Core;
using Store.Repository;
using Store.Repository.Data.Context;
using Store.Core.Mapping.Products;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Errors;
using Store.Core.Repositories.Contract;
using Store.Repository.Repositories;
using StackExchange.Redis;
using Store.Core.Mapping.Baskets;
using Store.Service.Services.Products;
using Store.Service.Services.Baskets;
using Store.Service.Services.Caches;
using Store.Repository.Identity.Contexts;
using Store.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Store.Service.Services.Tokens;
using Store.Service.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Store.Core.Mapping.Auth;
using Store.Core.Mapping.orders;
using Store.Service.Services.Orders;
using Store.Service.Services.Payments;

namespace Store.APIs.Helper
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddDependency(this IServiceCollection services, IConfiguration config)
        {
            services.AddBuiltInService();
            services.AddSwagerServices();
            services.AddDbContextService(config);
            services.AddUserDefinedService();
            services.AddAutoMapperService(config);
            services.AddInvalidModelStateResponseService();
            services.AddRedisService(config);
            services.AddIdentityService();
            services.AddAuthenticationService(config);

            return services;
        }

        private static IServiceCollection AddBuiltInService(this IServiceCollection services)
        {
            services.AddControllers();

            return services;
        }

        // swager
        private static IServiceCollection AddSwagerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }

        // Connect Database
        private static IServiceCollection AddDbContextService(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<StoreDbContext>(option =>
            {
                option.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<StoreIdentityDbContext>(option =>
            {
                option.UseSqlServer(config.GetConnectionString("IdentityConnection"));
            });


            return services;
        }
      
        private static IServiceCollection AddUserDefinedService(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<ICacheService, CacheServices>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();

            return services;
        }

        //AutoMapper
        private static IServiceCollection AddAutoMapperService(this IServiceCollection services, IConfiguration config)
        {
           services.AddAutoMapper(M => M.AddProfile(new ProductProfile(config)));
           services.AddAutoMapper(M => M.AddProfile(new BasketProfile()));
           services.AddAutoMapper(M => M.AddProfile(new AuthProfile()));
           services.AddAutoMapper(M => M.AddProfile(new OrderProfile(config)));

            return services;
        }

        // handle ModelState Response (handel validation)
        private static IServiceCollection AddInvalidModelStateResponseService(this IServiceCollection services)
        {
            // handel validation
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {

                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                            .SelectMany(p => p.Value.Errors)
                                            .Select(E => E.ErrorMessage)
                                            .ToArray();


                    var response = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    // return BadRequest mesh ynfa3 asta5demha hena 3l4an hya mwgoda gwa ControllerBase
                    return new BadRequestObjectResult(response);
                };
            });

            return services;
        }

        //Redis
        private static IServiceCollection AddRedisService(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connect = config.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connect);
            });


            return services;
        }

        //addIdentityService
        // allow dependency injection for all identity build-in service
        private static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();


            return services;
        }


        private static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // no3 el token mn no3 Bearer
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = config["Jwt:IssuerApi"],
                    ValidateAudience = true,
                    ValidAudience = config["Jwt:AudienceApi"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SECRETKEY"]))
                };
            });


            return services;
        }


    }
}
