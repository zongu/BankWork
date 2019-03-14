
namespace BankWork.Domain.Repository
{
    using System.Collections.Generic;
    using BankWork.Domain.Model;

    public interface IAccountRepository
    {
        Account GetAccountByName(string name);

        bool Insert(Account account);

        IEnumerable<Account> MoreThanDeposit(long points);

        IEnumerable<Account> MoreThanDrawal(long points);

        bool Update(Account account);
    }
}
