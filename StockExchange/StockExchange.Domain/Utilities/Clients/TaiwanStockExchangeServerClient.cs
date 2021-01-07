using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace StockExchange.Domain.Utilities.Clients
{
    public class TaiwanStockExchangeServerClient : ITaiwanStockExchangeServerClient
    {
        private readonly string _clientServer;
        private readonly IHttpRestfulClient _client;

        public TaiwanStockExchangeServerClient(IHttpRestfulClient client)
        {
            _clientServer = "https://www.twse.com.tw/exchangeReport/BWIBBU_d";
            _client = client;
        }

        public async Task<TaiwanStockExchangeResponse> GetStockExchangeAsync(TaiwanStockExchangeRequest request)
        {
            string searchTemp = $"?response={request.response}&date={request.date}&selectType=ALL";

            var response = await _client.SendRequestAsync(_clientServer, searchTemp, null, HttpMethod.Get);

            return JsonConvert.DeserializeObject<TaiwanStockExchangeResponse>(response.HttpBody);
        }
    }

    public class TaiwanStockExchangeRequest
    {
        public string response { get; set; }
        public string date { get; set; }
    }

    public class TaiwanStockExchangeResponse
    {
        public string stat { get; set; }
        public string title { get; set; }
        public List<string> fields { get; set; }
        public List<List<string>> data { get; set; }
    }
}