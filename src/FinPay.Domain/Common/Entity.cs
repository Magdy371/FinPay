namespace FinPay.Domain.Common
{
    public abstract class Entity
    {
        //We use Guid instead of sequential integers for public identifiers.
        public Guid Id { get; protected set; } = Guid.NewGuid();
    }
}