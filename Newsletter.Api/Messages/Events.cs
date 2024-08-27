namespace Newsletter.Api.Messages;

public class SubscriberCreated
{
    public Guid SubscriberId { get; init; }

    public string Email { get; init; } = default!;
}

public class WelcomeEmailSent
{
    public Guid SubscriberId { get; init; }

    public string Email { get; init; } = default!;
}

public class FollowUpEmailSent
{
    public Guid SubscriberId { get; init; }

    public string Email { get; init; } = default!;
}

public class OnboardingCompleted
{
    public Guid SubscriberId { get; init; }

    public string Email { get; init; } = default!;
}