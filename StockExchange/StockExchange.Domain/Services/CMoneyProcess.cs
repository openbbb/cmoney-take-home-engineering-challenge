using StockExchange.Domain.Services.Dal;
using StockExchange.Domain.Utilities.Clients;
using StockExchange.Object;
using StockExchange.Object.Services;
using StockExchange.Object.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.Domain.Services
{
    public class CMoneyProcess : ICMoneyProcess
    {
        private readonly ICMoneyDal _dal;
        private readonly ITaiwanStockExchangeServerClient _client;
        public CMoneyProcess(ICMoneyDal dal, ITaiwanStockExchangeServerClient client)
        {
            _dal = dal;
            _client = client;
        }


        public async Task<CommandOutput> InsertStockExchange(StockExchangeInput input)
        {
            var date = Convert.ToDateTime(input.Date).Date;
            var stockExchanges = await _client.GetStockExchangeAsync(new TaiwanStockExchangeRequest() { response = "json", date = date.ToString("yyyyMMdd") });
            if (stockExchanges.stat != "OK")
                return new CommandOutput() { IsSuccess = false, ErrorMessage = "API 異常" };

            var datas = ConvertStockExchange(stockExchanges.data, date);
            var insertResult = _dal.InsertSockExchange(datas);
            if (!insertResult)
                return new CommandOutput() { IsSuccess = false, ErrorMessage = "新增資料異常" };

            return new CommandOutput() { IsSuccess = true, ErrorMessage = "" };
        }

        private List<Bwibbu> ConvertStockExchange(List<List<string>> datas, DateTime date)
        {
            var result = new List<Bwibbu>();

            foreach (var data in datas)
            {
                if (data.Count == 7)
                {
                    result.Add(new Bwibbu()
                    {
                        Stockid = int.TryParse(data[0], out int id) ? id : default(int),
                        CreatedAt = date,
                        Name = data[1],
                        Yield = double.TryParse(data[2], out double y) ? y : default(double?),
                        Yieldyear = int.TryParse(data[3], out int yyear) ? yyear : default(int?),
                        Pe = double.TryParse(data[4], out double pe) ? pe : default(double?),
                        Pb = double.TryParse(data[5], out double pb) ? pb : default(double?),
                        Reportyear = data[6]
                    });
                }
            }

            return result;
        }

        public StockExchangeOutput QueryStockExchange(StockExchangeInput input)
        {
            var format = IsVaildFormat(input);
            if (!format.IsVaild)
                return new StockExchangeOutput() { IsSuccess = false, ErrorMessage = "格式錯誤" };

            var result = new List<Bwibbu>();
            switch (format.SearchType)
            {
                case 1:
                    result = _dal.GetSockExchange(input.StockId);
                    break;
                case 2:
                    var date = Convert.ToDateTime(input.Date).Date;

                    result = _dal.GetSockExchange(date).OrderByDescending(x => x.Pe).ToList();
                    break;
                case 3:
                    var startDate = Convert.ToDateTime(input.StartDate).Date;
                    var endDate = Convert.ToDateTime(input.EndDate).Date;

                    result = _dal.GetSockExchange(input.StockId, startDate, endDate).OrderByDescending(x => x.Yield).ThenByDescending(x => x.Pe).ToList();
                    break;
            }

            if (result.Count == 0)
                return new StockExchangeOutput() { IsSuccess = false, ErrorMessage = "無資料" };

            return new StockExchangeOutput() { IsSuccess = true, StockExchanges = ConvertStockExchange(result) };
        }

        private List<Object.Services.StockExchange> ConvertStockExchange(List<Bwibbu> entities)
        {
            return entities.Select(x => new Object.Services.StockExchange()
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
        private StockExchangeQueryInput IsVaildFormat(StockExchangeInput input)
        {
            var result = new StockExchangeQueryInput() { IsVaild = false };
            // 依照 代碼搜尋
            if (input.StockId != default(int) && string.IsNullOrEmpty(input.Date) && string.IsNullOrEmpty(input.StartDate) && string.IsNullOrEmpty(input.EndDate))
            {
                result.IsVaild = true;
                result.SearchType = 1;
            }

            // 依照 特定日期搜尋
            if (input.StockId == default(int) && !string.IsNullOrEmpty(input.Date) && string.IsNullOrEmpty(input.StartDate) && string.IsNullOrEmpty(input.EndDate))
            {
                result.IsVaild = true;
                result.SearchType = 2;
            }

            // 依照 代碼 日期範圍搜尋
            if (input.StockId != default(int) && string.IsNullOrEmpty(input.Date) && !string.IsNullOrEmpty(input.StartDate) && !string.IsNullOrEmpty(input.EndDate))
            {
                if (string.Compare(input.StartDate, input.EndDate) <= 0)
                {
                    result.IsVaild = true;
                    result.SearchType = 3;
                }
            }

            return result;
        }
    }

    public class StockExchangeQueryInput
    {
        public bool IsVaild { get; set; }
        public int SearchType { get; set; }
    }
}