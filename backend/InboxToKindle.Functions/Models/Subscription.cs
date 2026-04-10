namespace InboxToKindle.Functions.Models;

public sealed class Subscription
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string NewsletterName { get; set; } = string.Empty;
    public string SubscriptionEmail { get; set; } = string.Empty;
    public string KindleEmail { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
