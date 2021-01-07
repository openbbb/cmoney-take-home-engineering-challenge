using System.Threading.Tasks;

namespace StockExchange.Domain.Utilities.Clients
{
    public interface ITaiwanStockExchangeServerClient
    {
        Task<TaiwanStockExchangeResponse> GetStockExchangeAsync(TaiwanStockExchangeRequest request);
    }
}