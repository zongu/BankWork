
namespace BankWork.Domain.Service
{
    using BankWork.Domain.Model;
    using BankWork.Domain.Repository;

    public class AccountDepositOOC : IAccountDepositOOC
    {
        private IAccountRepository repo;

        public AccountDepositOOC(IAccountRepository repo)
        {
            this.repo = repo;
        }

        public DepositResult Deposit(Account account, long points)
        {
            if (!account.Deposit(points))
            {
                return DepositResult.NotEnoughPoints;
            }

            while (!this.repo.Update(account))
            {
                account = this.repo.GetAccountByName(account.Name);
                if (!account.Deposit(points))
                {
                    return DepositResult.NotEnoughPoints;
                }
            }

            return DepositResult.Success;
        }
    }
}
