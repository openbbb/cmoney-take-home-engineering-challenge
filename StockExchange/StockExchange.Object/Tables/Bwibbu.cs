using System;

namespace StockExchange.Object.Tables
{
    public partial class Bwibbu
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