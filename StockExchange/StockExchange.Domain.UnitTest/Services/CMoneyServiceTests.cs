using Moq;
using NUnit.Framework;
using StockExchange.Domain.Services;
using StockExchange.Domain.Services.Dal;
using StockExchange.Domain.Utilities.Clients;
using StockExchange.Object.Tables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockExchange.Domain.UnitTest.Services
{
    [TestFixture]
    public class CMoneyServiceTests
    {
        private Mock<ICMoneyDal> _dal;
        private Mock<ITaiwanStockExchangeServerClient> _client;
        private CMoneyProcess _process;

        [SetUp]
        public void SetUp()
        {
            _dal = new Mock<ICMoneyDal>();
            _client = new Mock<ITaiwanStockExchangeServerClient>();

            _process = new CMoneyProcess(_dal.Object, _client.Object);
        }

        [Test]
        public async Task Test()
        {
            var response = new TaiwanStockExchangeResponse() { stat = "ERROR" };
            _client.Setup(x => x.GetStockExchangeAsync(It.IsAny<TaiwanStockExchangeRequest>())).Returns(Task.FromResult(response));

            var result = await _process.InsertStockExchange(new Object.Services.StockExchangeInput() { });

            Assert.That(result.IsSuccess, Is.EqualTo(false));
            Assert.That(result.ErrorMessage, Is.EqualTo("API 異常"));
        }

        [Test]
        public async Task Insert_success_test()
        {
            var response = new TaiwanStockExchangeResponse() { stat = "OK", data = new List<List<string>>() { } };
            _client.Setup(x => x.GetStockExchangeAsync(It.IsAny<TaiwanStockExchangeRequest>())).Returns(Task.FromResult(response));


            _dal.Setup(x => x.InsertSockExchange(It.IsAny<List<Bwibbu>>())).Returns(false);


            var result = await _process.InsertStockExchange(new Object.Services.StockExchangeInput() { });


            Assert.That(result.IsSuccess, Is.EqualTo(false));
            Assert.That(result.ErrorMessage, Is.EqualTo("新增資料異常"));

        }
    }
}