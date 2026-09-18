using FinPay.Domain.Common;
using FinPay.Domain.Enums;
namespace FinPay.Domain.Entities;
public class Beneficiary : AuditableEntity
{
    public Guid UserId { get; private set; }

    public Guid BeneficiaryUserId { get; private set; }

    public string Name { get; private set; } = null!;

    public BeneficiaryStatus Status { get; private set; }

    public User User { get; private set; } = null!;

     public void Activate()
    {
        Status = BeneficiaryStatus.Active;
        MarkUpdated();
    }

    public void Block()
    {
        Status = BeneficiaryStatus.Blocked;
        MarkUpdated();
    }

    public void Remove()
    {
        Status = BeneficiaryStatus.Removed;
        MarkUpdated();
    }
}