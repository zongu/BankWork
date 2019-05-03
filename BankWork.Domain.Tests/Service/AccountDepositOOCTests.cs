
namespace BankWork.Domain.Tests.Service
{
    using BankWork.Domain.Model;
    using BankWork.Domain.Repository;
    using BankWork.Domain.Service;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AccountDepositOOCTests
    {
        [TestMethod]
        public void OOCDepositTests()
        {
            var account = Account.GenerateInstance("rdtest");
            var mock = new Mock<IAccountRepository>();
            mock.Setup(p => p.Update(It.IsAny<Account>()))
                .Returns(true)
                .Verifiable();

            var ooc = new AccountDepositOOC(mock.Object);
            var depositResult = ooc.Deposit(account, 123456);
            Assert.AreEqual(depositResult, DepositResult.Success);
        }
    }
}
