using FinPay.Domain.Common;
using FinPay.Domain.Enums;

namespace FinPay.Domain.Entities;

public class Card : AuditableEntity
{
    private Card()
    {
    }

    public Card(
        Guid userId,
        Guid walletId,
        string lastFourDigits)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID is required.");

        if (walletId == Guid.Empty)
            throw new ArgumentException("Wallet ID is required.");

        if (string.IsNullOrWhiteSpace(lastFourDigits))
            throw new ArgumentException(
                "Last four digits are required.");

        UserId = userId;
        WalletId = walletId;
        LastFourDigits = lastFourDigits;
        Status = CardStatus.Active;
    }

    public Guid UserId { get; private set; }

    public Guid WalletId { get; private set; }

    public string LastFourDigits { get; private set; } = null!;

    public CardStatus Status { get; private set; }

    public User User { get; private set; } = null!;

    public Wallet Wallet { get; private set; } = null!;

    public void Block()
    {
        Status = CardStatus.Blocked;
        MarkUpdated();
    }

    public void Activate()
    {
        Status = CardStatus.Active;
        MarkUpdated();
    }

    public void Cancel()
    {
        Status = CardStatus.Cancelled;
        MarkUpdated();
    }
}