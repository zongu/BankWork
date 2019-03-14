
namespace BankWork.Persistent.Tests
{
    using System.Linq;
    using BankWork.Domain.Model;
    using BankWork.Domain.Repository;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MongoDB.Driver;

    [TestClass]
    public class MongoAccountRepositoryTests
    {
        private IAccountRepository repo;

        private const string mongoConn = @"mongodb://localhost:27017";

        [TestInitialize]
        public void Init()
        {
            var client = new MongoClient(mongoConn);
            var db = client.GetDatabase("BankWork");
            db.DropCollection("Account");

            this.repo = new MongoAccountRepository(client);
        }

        [TestMethod]
        public void InsertTests()
        {
            var insertResult = this.repo.Insert(Account.GenerateInstance("rdtest"));
            Assert.IsTrue(insertResult);
        }

        [TestMethod]
        public void GetAccountByNameTests()
        {
            var account = Account.GenerateInstance("rdtest");
            var insertResult = this.repo.Insert(account);
            Assert.IsTrue(insertResult);

            var getResult = this.repo.GetAccountByName("rdtest");
            Assert.IsNotNull(getResult);
            Assert.AreEqual(getResult.Name, account.Name);
        }

        [TestMethod]
        public void UpdateSuccessTests()
        {
            var account = Account.GenerateInstance("rdtest");
            var insertResult = this.repo.Insert(account);
            Assert.IsTrue(insertResult);

            account.Deposit(1000);

            var updateResult = this.repo.Update(account);
            Assert.IsTrue(updateResult);
        }

        [TestMethod]
        public void UpdateFailTests()
        {
            var account = Account.GenerateInstance("rdtest");
            var insertResult = this.repo.Insert(account);
            Assert.IsTrue(insertResult);

            account.Deposit(1000);
            account.SerialNo = 1;

            var updateResult = this.repo.Update(account);
            Assert.IsFalse(updateResult);
        }
    }
}
