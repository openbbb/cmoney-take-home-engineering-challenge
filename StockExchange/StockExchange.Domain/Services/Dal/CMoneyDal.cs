using StockExchange.Object.Tables;
using StockExchange.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockExchange.Domain.Services.Dal
{
    public class CMoneyDal : ICMoneyDal
    {
        private readonly ICMoneyRepository _repo;

        public CMoneyDal(ICMoneyRepository repo)
        {
            _repo = repo;
        }

        public List<Bwibbu> GetSockExchange(int id)
        {
            return _repo.Find(x => x.Stockid == id).ToList();
        }

        public List<Bwibbu> GetSockExchange(DateTime date)
        {
            return _repo.Find(x => x.CreatedAt == date).ToList();
        }

        public List<Bwibbu> GetSockExchange(int id, DateTime startDate, DateTime endDate)
        {
            return _repo.Find(x => x.Stockid == id && x.CreatedAt >= startDate && x.CreatedAt <= endDate).ToList();
        }

        public bool InsertSockExchange(List<Bwibbu> entities)
        {
            var result = _repo.Creates(entities);

            return result > 0;
        }
    }
}