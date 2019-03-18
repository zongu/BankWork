
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

        [TestMethod]
        public void MoreThanDepositTests()
        {
            var account = Account.GenerateInstance("rdtest001");
            var insertResult = this.repo.Insert(account);
            Assert.IsTrue(insertResult);

            account.Deposit(1000);

            var updateResult = this.repo.Update(account);
            Assert.IsTrue(updateResult);

            var account1 = Account.GenerateInstance("rdtest002");
            var insertResult1 = this.repo.Insert(account1);
            Assert.IsTrue(insertResult1);

            account1.Deposit(500);

            var updateResult1 = this.repo.Update(account1);
            Assert.IsTrue(updateResult1);

            var queryResult = this.repo.MoreThanDeposit(1000);
            Assert.IsNotNull(queryResult);
            Assert.AreEqual(queryResult.Count(), 1);
            Assert.AreEqual(queryResult.FirstOrDefault().Name, "rdtest001");

            var queryResult1 = this.repo.MoreThanDeposit(500);
            Assert.IsNotNull(queryResult1);
            Assert.AreEqual(queryResult1.Count(), 2);
        }

        [TestMethod]
        public void MoreThanDrawalTests()
        {
            var account = Account.GenerateInstance("rdtest001");
            var insertResult = this.repo.Insert(account);
            Assert.IsTrue(insertResult);

            account.Deposit(1000);

            var updateResult = this.repo.Update(account);
            Assert.IsTrue(updateResult);
            account = this.repo.GetAccountByName(account.Name);
            Assert.IsNotNull(account);

            var account1 = Account.GenerateInstance("rdtest002");
            var insertResult1 = this.repo.Insert(account1);
            Assert.IsTrue(insertResult1);

            account1.Deposit(500);

            var updateResult1 = this.repo.Update(account1);
            Assert.IsTrue(updateResult1);
            account1 = this.repo.GetAccountByName(account1.Name);
            Assert.IsNotNull(account1);

            account.Deposit(-500);
            var drawalResult = this.repo.Update(account);
            Assert.IsTrue(drawalResult);
            account = this.repo.GetAccountByName(account.Name);
            Assert.IsNotNull(account);

            account1.Deposit(-100);
            var drawalResult1 = this.repo.Update(account1);
            Assert.IsTrue(drawalResult1);
            account1 = this.repo.GetAccountByName(account1.Name);
            Assert.IsNotNull(account1);

            var queryResult = this.repo.MoreThanDrawal(500);
            Assert.IsNotNull(queryResult);
            Assert.AreEqual(queryResult.Count(), 1);
            Assert.AreEqual(queryResult.FirstOrDefault().Name, "rdtest001");
            
            var queryResult1 = this.repo.MoreThanDrawal(100);
            Assert.IsNotNull(queryResult1);
            Assert.AreEqual(queryResult1.Count(), 2);
        }
    }
}
