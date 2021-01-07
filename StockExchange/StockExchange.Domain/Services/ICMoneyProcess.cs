using StockExchange.Object;
using StockExchange.Object.Services;
using System.Threading.Tasks;

namespace StockExchange.Domain.Services
{
    public interface ICMoneyProcess
    {
        StockExchangeOutput QueryStockExchange(StockExchangeInput input);
        Task<CommandOutput> InsertStockExchange(StockExchangeInput input);
    }
}