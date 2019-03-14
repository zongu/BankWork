
namespace BankWork.Domain.Repository
{
    using BankWork.Domain.Model;

    public interface IAccountRepository
    {
        Account GetAccountByName(string name);

        bool Insert(Account account);

        bool Update(Account account);
    }
}
