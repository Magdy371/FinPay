using FinPay.Domain.Common;
using FinPay.Domain.Enums;

namespace FinPay.Domain.Entities;

public class Notification : AuditableEntity
{
    private Notification()
    {
    }

    public Notification(
        Guid userId,
        NotificationType type,
        string title,
        string message)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID is required.");

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.");

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message is required.");

        UserId = userId;
        Type = type;
        Title = title;
        Message = message;
    }

    public Guid UserId { get; private set; }

    public NotificationType Type { get; private set; }

    public string Title { get; private set; } = null!;

    public string Message { get; private set; } = null!;

    public bool IsRead { get; private set; }

    public User User { get; private set; } = null!;

    public void MarkAsRead()
    {
        IsRead = true;
        MarkUpdated();
    }
}