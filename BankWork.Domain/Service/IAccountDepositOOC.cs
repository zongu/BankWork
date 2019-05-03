
namespace BankWork.Domain.Service
{
    using BankWork.Domain.Model;

    public interface IAccountDepositOOC
    {
        DepositResult Deposit(Account account, long points);
    }
}
