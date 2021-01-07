using Microsoft.AspNetCore.Mvc;
using StockExchange.API.Models.Objects;
using StockExchange.Domain.Services;
using StockExchange.Object.Services;
using StockExchange.Utility.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.API.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class StockExchangeController : ControllerBase
    {
        private readonly ICMoneyProcess _process;
        public StockExchangeController(ICMoneyProcess process)
        {
            _process = process;
        }

        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(BasicResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<BasicResponse> Post([FromBody] StockExchangeRequest request)
        {
            var result = await _process.InsertStockExchange(new StockExchangeInput() { Date = request.Date });
            if (result.IsSuccess)
                return new BasicResponse() { HttpCode = "00", HttpMessage = "" };

            else
                return new BasicResponse() { HttpCode = "98", HttpMessage = result.ErrorMessage };
        }

        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(StockExchangeResponse), 200)]
        [ProducesResponseType(404)]
        public StockExchangeResponse Get([FromQuery] StockExchangeRequest request)
        {
            var result = _process.QueryStockExchange(new StockExchangeInput() { StockId = request.StockId, Date = request.Date, StartDate = request.StartDate, EndDate = request.EndDate });
            if (result.IsSuccess)
                return new StockExchangeResponse() { HttpCode = "00", StockExchanges = ConvertStockExchange(result.StockExchanges) };

            else
                return new StockExchangeResponse() { HttpCode = "98", HttpMessage = result.ErrorMessage };
        }

        private List<Models.Objects.StockExchange> ConvertStockExchange(List<Object.Services.StockExchange> stocks)
        {
            return stocks.Select(x => new Models.Objects.StockExchange()
            {
                Stockid = x.Stockid,
                CreatedAt = x.CreatedAt,
                Name = x.Name,
                Yield = x.Yield,
                Yieldyear = x.Yieldyear,
                Pb = x.Pb,
                Pe = x.Pe,
                Reportyear = x.Reportyear
            }).ToList();
        }
    }
}
