
namespace BankWork.Tests.Controllers
{
    using System.Net;
    using System.Net.Http;
    using BankWork.Controllers;
    using BankWork.Domain.Model;
    using BankWork.Domain.Repository;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

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

            var controller = new AccountController(mock.Object);
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

            var controller = new AccountController(mock.Object);
            controller.Request = new HttpRequestMessage();
            var postResult = controller.Post(new AccountController.PostInput() { Name = "rdtest" });
            Assert.AreEqual(postResult.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public void PutTest()
        {
            var mock = new Mock<IAccountRepository>();
            mock.Setup(p => p.GetAccountByName(It.IsAny<string>()))
                .Returns(Account.GenerateInstance("rdtest"))
                .Verifiable();
            mock.Setup(p => p.Update(It.IsAny<Account>()))
                .Returns(true)
                .Verifiable();

            var controller = new AccountController(mock.Object);
            controller.Request = new HttpRequestMessage();
            var putResult = controller.Put(new AccountController.PutInput() { Name = "rdtest", Points = 1000 });
            Assert.AreEqual(putResult.StatusCode, HttpStatusCode.OK);
        }
    }
}
