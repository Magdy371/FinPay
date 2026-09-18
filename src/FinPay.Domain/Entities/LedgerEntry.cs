using FinPay.Domain.Common;
using FinPay.Domain.Enums;

namespace FinPay.Domain.Entities;

public class LedgerEntry : Entity
{
    private LedgerEntry()
    {
    }

    public LedgerEntry(
        Guid transactionId,
        Guid accountId,
        LedgerEntryType type,
        decimal amount,
        Currency currency)
    {
        if (transactionId == Guid.Empty)
            throw new ArgumentException(
                "Transaction ID is required.");

        if (accountId == Guid.Empty)
            throw new ArgumentException(
                "Account ID is required.");

        if (amount <= 0)
            throw new ArgumentException(
                "Amount must be greater than zero.");

        TransactionId = transactionId;
        AccountId = accountId;
        Type = type;
        Amount = amount;
        Currency = currency;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid TransactionId { get; private set; }

    public Guid AccountId { get; private set; }

    public LedgerEntryType Type { get; private set; }

    public decimal Amount { get; private set; }

    public Currency Currency { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public Transaction Transaction { get; private set; } = null!;

    public Account Account { get; private set; } = null!;
}