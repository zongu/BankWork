
namespace BankWork.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using BankWork.Domain.Model;
    using BankWork.Domain.Repository;

    public class AdvanceAccountController : ApiController
    {
        private IAccountRepository repo;

        public AdvanceAccountController(IAccountRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public IEnumerable<Account> Get([FromUri]GetInput input)
        {
            try
            {
                switch (input.Type)
                {
                    case GetInput.QueryType.Deposit:
                        return this.repo.MoreThanDeposit(input.Points);
                    case GetInput.QueryType.Drawal:
                        return this.repo.MoreThanDrawal(input.Points);
                    default:
                        throw new Exception("not exists querytype");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Account>();
            }
        }

        public class GetInput
        {
            public enum QueryType
            {
                Deposit,
                Drawal
            }

            public QueryType Type { get; set; }

            public long Points { get; set; }
        }
    }
}
