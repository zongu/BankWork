
namespace BankWork.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Autofac;
    using BankWork.Domain.Model;
    using BankWork.Domain.Repository;
    using BankWork.Domain.Service;

    public class AccountController : ApiController
    {
        private IAccountRepository repo;

        private AccountDepositOOC ooc;

        public AccountController(IAccountRepository repo)
        {
            this.repo = repo;
            this.ooc = AccountDepositOOC.GenerateInstance(repo);
        }

        [HttpGet]
        public Account Get(string name)
        {
            try
            {
                var queryResult = this.repo.GetAccountByName(name);
                return queryResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        [HttpPost]
        public HttpResponseMessage Post(PostInput input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.Name))
                {
                    throw new Exception("name is empty");
                }

                var insertResult = this.repo.Insert(Account.GenerateInstance(input.Name));
                if (!insertResult)
                {
                    throw new Exception($"the name:{input.Name} insert to repo fail");
                }

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPut]
        public HttpResponseMessage Put(PutInput input)
        {
            try
            {
                var account = this.repo.GetAccountByName(input.Name);
                if(account == null)
                {
                    throw new Exception($"cat not find account name:{input.Name}");
                }

                var depositResult = this.ooc.Deposit(account, input.Points);
                if(depositResult == AccountDepositOOC.DepositResult.NotEnoughPoints)
                {
                    throw new Exception($"{nameof(AccountDepositOOC.DepositResult.NotEnoughPoints)}");
                }

                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public class PostInput
        {
            public string Name { get; set; }
        }

        public class PutInput
        {
            public string Name { get; set; }

            public long Points { get; set; }
        }
    }
}
