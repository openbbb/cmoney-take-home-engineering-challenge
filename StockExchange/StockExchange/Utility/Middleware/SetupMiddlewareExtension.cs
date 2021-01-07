using Microsoft.AspNetCore.Builder;

namespace StockExchange.Utility.Middleware
{
    public static class SetupMiddlewareExtension
    {
        public static void ConfigureSetupMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<SetupMiddleware>();
        }
    }
}
