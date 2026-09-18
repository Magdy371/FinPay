namespace FinPay.Domain.Exceptions;

public sealed class InsufficientFundsException : Exception
{
    public InsufficientFundsException()
        : base("Insufficient funds.")
    {
    }
}