using StockExchange.Object.Tables;
using System;
using System.Collections.Generic;

namespace StockExchange.Domain.Services.Dal
{
    public interface ICMoneyDal
    {
        List<Bwibbu> GetSockExchange(int id);
        List<Bwibbu> GetSockExchange(DateTime date);
        List<Bwibbu> GetSockExchange(int id, DateTime startDate, DateTime endDate);

        bool InsertSockExchange(List<Bwibbu> entities);
    }
}