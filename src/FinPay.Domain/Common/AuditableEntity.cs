namespace FinPay.Domain.Common
{
    public class AuditableEntity : Entity
    {
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }

        protected AuditableEntity()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public void MarkUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}