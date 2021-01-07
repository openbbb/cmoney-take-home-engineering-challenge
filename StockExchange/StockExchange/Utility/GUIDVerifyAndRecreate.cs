using Microsoft.Extensions.Logging;
using System;

namespace StockExchange.Utility
{
    public class GUIDVerifyAndRecreate
    {
        private readonly ILogger _logger;
        public GUIDVerifyAndRecreate(ILogger<GUIDVerifyAndRecreate> logger)
        {
            _logger = logger;
        }

        public Guid VerifyGUID(string candidate)
        {
            if (string.IsNullOrEmpty(candidate))
                return Guid.NewGuid();
            if (!Guid.TryParse(candidate, out Guid beParsed))
            {
                beParsed = Guid.NewGuid();
                _logger.LogInformation("[{0}] GUID Changes to {1}", candidate, beParsed);
            }
            return beParsed;
        }
    }
}
