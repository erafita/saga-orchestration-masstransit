namespace Newsletter.Api.Persistence;

public class Subscriber
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public DateTime SubscribeOnUtc { get; set; }
}