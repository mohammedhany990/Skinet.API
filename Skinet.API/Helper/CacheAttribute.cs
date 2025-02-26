using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Skinet.Service.Abstracts;

namespace Skinet.API.Helper
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeInSeconds;

        public CacheAttribute(int timeInSeconds)
        {
            _timeInSeconds = timeInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var responseCacheService = context.HttpContext
                .RequestServices
                .GetRequiredService<IResponseCacheService>();

            var cacheKey = GenerateCacheKey(context.HttpContext.Request);

            var response = await responseCacheService.GetCachedResponseAsync(cacheKey);
            if (!string.IsNullOrEmpty(response))
            {
                var contentResult = new ContentResult()
                {
                    Content = response,
                    StatusCode = 200,
                    ContentType = "application/json"
                };
                context.Result = contentResult;
                return;
            }

            var actionExecutedContext = await next.Invoke();
            if (actionExecutedContext.Result is ObjectResult result && result.Value is not null)
            {
                await responseCacheService
                    .CacheResponseAsync(cacheKey, result, TimeSpan.FromSeconds(_timeInSeconds));
            }

        }

        private string GenerateCacheKey(HttpRequest httpRequest)
        {
            var result = new StringBuilder();

            result.Append(httpRequest.Path);

            foreach (var (key, value) in httpRequest.Query.OrderBy(o=>o.Key))
            {
                result.Append($"|{key}-{value}");
            }

            return result.ToString();
        }
    }
}
