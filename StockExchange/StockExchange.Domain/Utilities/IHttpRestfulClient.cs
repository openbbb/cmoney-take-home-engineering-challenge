using System.Net.Http;
using System.Threading.Tasks;

namespace StockExchange.Domain.Utilities
{
    public interface IHttpRestfulClient
    {
        Task<ClientResponse> SendRequestAsync(string serverUrl, string apiUrl, object requestObj, HttpMethod httpMethod);
    }
}