using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace StockExchange.Utility.Middleware
{
    public class SetupMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly GUIDVerifyAndRecreate _guidVerify;
        private const string GUID_HEADER = "x-guid";
        private const string TAG_HEADER = "x-str-tag";

        public SetupMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, GUIDVerifyAndRecreate guidVerify)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<SetupMiddleware>();
            _guidVerify = guidVerify;
        }

        public async Task Invoke(HttpContext context)
        {
            string strTag = $"{context.Request.Path.Value}_{context.Request.Method}";
            context.Request.Headers.TryGetValue("guid-id", out StringValues guid);
            Guid verifiedguid = _guidVerify.VerifyGUID(guid.ToString());
            context.Items.Add(GUID_HEADER, verifiedguid);
            context.Items.Add(TAG_HEADER, strTag);
            long start = DateTime.Now.Ticks;
            context.Response.Headers.Add("guid-id", verifiedguid.ToString());

            await _next(context);


            long costTime = (DateTime.Now.Ticks - start) / TimeSpan.TicksPerMillisecond;
            _logger.LogInformation($"[{verifiedguid}] {context.Items[TAG_HEADER]}  httpstatus : {context.Response.StatusCode} Total execution time :{costTime}");
        }
    }
}
