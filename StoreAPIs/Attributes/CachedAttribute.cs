using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Core.Services.Contract;

namespace Store.APIs.Attributes
{
    // IAsyncActionFilter 8abel ma y54 el controller y3mel check hal kan fe data fe el cache wla la
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int expireTime;

        public CachedAttribute(int expireTime)
        {
            this.expireTime = expireTime;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
           var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cacheResponse = await cacheService.GetCacheKeyAsync(cacheKey);

            if (!string.IsNullOrEmpty(cacheResponse)) 
            {
                var contentResult = new ContentResult()
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200,
                };

                context.Result = contentResult;

                return;
            }

           var excutedContext =  await next();

            if(excutedContext.Result is OkObjectResult response)
            {
               await cacheService.SetCacheKeyAsync(cacheKey,response.Value, TimeSpan.FromSeconds(expireTime));
            }
        }



        private string GenerateCacheKeyFromRequest(HttpRequest req)
        {
            var cacheKey = new StringBuilder();
            cacheKey.Append($"{req.Path}");

            foreach (var (key, value) in req.Query.OrderBy(X => X.Key))
            {
                cacheKey.Append($"|{key}-{value}");
            }

            return cacheKey.ToString();
        }
    }
}
