using FinPay.Domain.Common;
using FinPay.Domain.Enums;
namespace FinPay.Domain.Entities;
public class Wallet : AuditableEntity
{
    private readonly List<Account> _accounts = new();
    private Wallet() {}
    public Wallet(Guid userId, Currency currency)
    {
        if(userId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty.", nameof(userId));
        UserId = userId;
        Currency = currency;
        Status = WalletStatus.Active;
    }
    public Guid UserId { get; private set; }
    public Currency Currency { get; private set; }
    public WalletStatus Status { get; private set; }
    public User User { get; private set; } = null!;

    public IReadOnlyCollection<Account> Accounts => _accounts.AsReadOnly();

    public void Suspend()
    {
        Status = WalletStatus.Suspended;
        MarkUpdated();
    }
    public void Activate()
    {
        Status = WalletStatus.Active;
        MarkUpdated();
    }

    public void Freeze()
    {
        Status = WalletStatus.Frozen;
        MarkUpdated();
    }

    public void Close()
    {
        Status = WalletStatus.Closed;
        MarkUpdated();
    }

}