
namespace BankWork.Tests.Controllers
{
    using System.Net;
    using System.Net.Http;
    using BankWork.Controllers;
    using BankWork.Domain.Model;
    using BankWork.Domain.Repository;
    using BankWork.Domain.Service;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using static BankWork.Domain.Service.AccountDepositOOC;

    [TestClass]
    public class AccountControllerTests
    {
        [TestMethod]
        public void GetTest()
        {
            var mock = new Mock<IAccountRepository>();
            mock.Setup(p => p.GetAccountByName(It.IsAny<string>()))
                .Returns(Account.GenerateInstance("rdtest"))
                .Verifiable();

            var controller = new AccountController(mock.Object, null);
            var getResult = controller.Get("rdtest");
            Assert.IsNotNull(getResult);
            Assert.AreEqual(getResult.Name, "rdtest");
        }

        [TestMethod]
        public void PostTest()
        {
            var mock = new Mock<IAccountRepository>();
            mock.Setup(p => p.Insert(It.IsAny<Account>()))
                .Returns(true)
                .Verifiable();

            var controller = new AccountController(mock.Object, null);
            controller.Request = new HttpRequestMessage();
            var postResult = controller.Post(new AccountController.PostInput() { Name = "rdtest" });
            Assert.AreEqual(postResult.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public void PutTest()
        {
            var repoMock = new Mock<IAccountRepository>();
            repoMock.Setup(p => p.GetAccountByName(It.IsAny<string>()))
                .Returns(Account.GenerateInstance("rdtest"))
                .Verifiable();
            repoMock.Setup(p => p.Update(It.IsAny<Account>()))
                .Returns(true)
                .Verifiable();

            var cooMock = new Mock<IAccountDepositOOC>();
            cooMock.Setup(p => p.Deposit(It.IsAny<Account>(), It.IsAny<long>()))
                .Returns(DepositResult.Success)
                .Verifiable();

            var controller = new AccountController(repoMock.Object, cooMock.Object);
            controller.Request = new HttpRequestMessage();
            var putResult = controller.Put(new AccountController.PutInput() { Name = "rdtest", Points = 1000 });
            Assert.AreEqual(putResult.StatusCode, HttpStatusCode.OK);
        }
    }
}
