
namespace BankWork.Domain.Tests.Model
{
    using BankWork.Domain.Model;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void DepositSuccessTest()
        {
            var account = Account.GenerateInstance("rdtest");
            var depositSuccessResult = account.Deposit(1000);
            Assert.IsTrue(depositSuccessResult);
        }

        [TestMethod]
        public void DepositFailTest()
        {
            var account = Account.GenerateInstance("rdtest");
            var depositFaikResult = account.Deposit(-1000);
            Assert.IsFalse(depositFaikResult);
        }
    }
}
