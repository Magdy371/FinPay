using FinPay.Domain.Enums;
using Microsoft.VisualBasic;
namespace FinPay.Domain.ValueObjects
{
    public sealed record Money
    {
        public decimal Amount { get; }
        public Currency Currency { get; }

        public Money(decimal amount, Currency currency)
        {
            if (amount < 0)
            {
                throw new ArgumentException(
                    "Money amount cannot be negative",
                    nameof(amount)
                    );
                Amount = decimal.Round(amount,2);
                Currency = currency;
            }
        }
        public static Money Zero(Currency currency)=>new (0m,currency);
        public Money Add (Money other)
        {
            EnsureSameCurrency(other);
            return new Money(Amount + other.Amount,Currency);
        } 
        public Money Subtract (Money other)
        {
            EnsureSameCurrency(other);
            if(other.Amount > Amount)
            {
                throw new InvalidOperationException("Insufficient Fund");
            }
            return new Money(Amount - other.Amount, Currency);

        }
        private void EnsureSameCurrency(Money other)
        {
            if(Currency != other.Currency)
            {
                throw new InvalidOperationException("Currency mismatch");
            }
        }
    }
}