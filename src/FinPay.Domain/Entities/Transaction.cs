using FinPay.Domain.Common;
using FinPay.Domain.Enums;
namespace FinPay.Domain.Entities;

public class Transaction : AuditableEntity
{
    private readonly List<LedgerEntry> _ledgerEntries = new();
    private Transaction()
    {
    }
    public Transaction(
        string reference,
        TransactionType type,
        decimal amount,
        Currency currency,
        Guid? fromAccountId,
        Guid? toAccountId)
    {
        if (string.IsNullOrWhiteSpace(reference))
            throw new ArgumentException("Reference is required.");

        if (amount <= 0)
            throw new ArgumentException(
                "Amount must be greater than zero.");

        Reference = reference;
        Type = type;
        Amount = amount;
        Currency = currency;
        FromAccountId = fromAccountId;
        ToAccountId = toAccountId;
        Status = TransactionStatus.Pending;
    }
    public string Reference { get; private set; } = null!;
    public TransactionType Type { get; private set; }
    public TransactionStatus Status { get; private set; }
    public Currency Currency { get; private set; }
    public decimal Amount { get; private set; }
    public Guid? FromAccountId { get; private set; }
    public Guid? ToAccountId { get; private set; }
    public Account? FromAccount { get; private set; } = null!;
    public Account? ToAccount { get; private set; } = null!;

    public IReadOnlyCollection<LedgerEntry> LedgerEntries =>
        _ledgerEntries.AsReadOnly();

    public void AddLedgerEntry(LedgerEntry entry)
    {
        _ledgerEntries.Add(entry);
    }

    public void MarkProcessing()
    {
        Status = TransactionStatus.Processing;
        MarkUpdated();
    }

    public void Complete()
    {
        Status = TransactionStatus.Completed;
        MarkUpdated();
    }

    public void Fail()
    {
        Status = TransactionStatus.Failed;
        MarkUpdated();
    }

    public void Reverse()
    {
        Status = TransactionStatus.Reversed;
        MarkUpdated();
    }

    public void Cancel()
    {
        Status = TransactionStatus.Cancelled;
        MarkUpdated();
    }


}