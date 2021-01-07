using StockExchange.Utility.Objects;
using System;
using System.Collections.Generic;

namespace StockExchange.API.Models.Objects
{
    public class StockExchangeRequest
    {
        public int StockId { get; set; }
        public string Date { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    public class StockExchangeResponse : BasicResponse
    {
        public List<StockExchange> StockExchanges { get; set; }
    }
    public class StockExchange
    {
        public int Stockid { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public double? Yield { get; set; }
        public int? Yieldyear { get; set; }
        public double? Pe { get; set; }
        public double? Pb { get; set; }
        public string Reportyear { get; set; }
    }
}
