using StockExchange.Object.Tables;
using StockExchange.Repository.Interfaces;
using StockExchange.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace StockExchange.Repository.Repositories
{
    public class CMoneyRepository : ICMoneyRepository
    {
        private readonly CMDBContext _context;
        public CMoneyRepository(CMDBContext context)
        {
            _context = context;
        }

        public long Creates(List<Bwibbu> entities)
        {
            var result = 0;

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Bwibbu.AddRange(entities);

                    result = _context.SaveChanges();

                    dbContextTransaction.Commit();

                    return result;
                }
                catch (Exception ex)
                {
                    result = -1;
                    dbContextTransaction.Rollback();
                    throw ex;
                }
            }
        }

        public IEnumerable<Bwibbu> Find(Expression<Func<Bwibbu, bool>> expression)
        {
            return _context.Bwibbu.Where(expression);
        }
    }
}