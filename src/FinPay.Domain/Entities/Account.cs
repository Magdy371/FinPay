using FinPay.Domain.Common;
using FinPay.Domain.Enums;
namespace FinPay.Domain.Entities;
using FinPay.Domain.Exceptions;
public class Account : AuditableEntity
{
     private Account()
    {
    }

    public Account(
        Guid walletId,
        Currency currency,
        AccountType type)
    {
        if (walletId == Guid.Empty)
            throw new ArgumentException("Wallet ID is required.");

        WalletId = walletId;
        Currency = currency;
        Type = type;
        Balance = 0m;
    }

    public Guid WalletId { get; private set; }

    public Currency Currency { get; private set; }

    public AccountType Type { get; private set; }

    public decimal Balance { get; private set; }
    public Wallet Wallet { get; private set; } = null!;

    public void ApplyDebit(decimal amount)
    {
        if(amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
        }
        if (Balance < amount)
        {
            throw new InsufficientFundsException();
        }
        Balance -= amount;
        MarkUpdated();
    }
    public void ApplyCredit(decimal amount)
    {
        if(amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
        }
        Balance += amount;
        MarkUpdated();
    }

}