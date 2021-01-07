using Microsoft.AspNetCore.Builder;

namespace StockExchange.Utility.Middleware
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
