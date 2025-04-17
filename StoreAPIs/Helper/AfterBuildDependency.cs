using Store.APIs.Middlewares;
using Store.Repository.Data.Context;
using Store.Repository.Data;
using StoreAPIs;
using Microsoft.EntityFrameworkCore;
using Store.Repository.Identity.Contexts;
using Microsoft.AspNetCore.Identity;
using Store.Core.Entities.Identity;
using Store.Repository.Identity.DataSeed;

namespace Store.APIs.Helper
{
    public static class AfterBuildDependency
    {
        public static async Task<WebApplication> ConfigurationMiddlewareAsync(this WebApplication app)
        {
            // a3mel apply migrations lw mkansh m3molaha update-database bykoond ba3d el build w 8abl el run
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<StoreDbContext>(); // ask clr create object storeDbContext
            var identityContext = services.GetRequiredService<StoreIdentityDbContext>(); // ask clr create object StoreIdentityDbContext
            var userManager = services.GetRequiredService<UserManager<AppUser>>(); // ask clr create object StoreIdentityDbContext
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await context.Database.MigrateAsync(); // update-database

                await StoreDbContextSeed.SeedAsync(context);

                await identityContext.Database.MigrateAsync(); // update-database
                await StoreIdentityDbContextSeed.SeedAppUserAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();

                logger.LogError(ex, "There are problems during apply migrations");
            }
            //

            // middleware
            app.UseMiddleware<ExceptionMiddleware>(); // configure user-define middleware

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithRedirects("/error/{0}");

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
