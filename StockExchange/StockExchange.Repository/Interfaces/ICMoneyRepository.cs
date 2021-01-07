using StockExchange.Object.Tables;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StockExchange.Repository.Interfaces
{
    public interface ICMoneyRepository
    {
        IEnumerable<Bwibbu> Find(Expression<Func<Bwibbu, bool>> expression);
        long Creates(List<Bwibbu> entities);
    }
}