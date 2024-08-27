using MassTransit;

namespace Newsletter.Api.Sagas;

public class NewsletterOnboardingSagaData : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = default!;

    public Guid SubscriberId { get; set; }
    public string Email { get; set; } = default!;
    public bool WelcomeEmailSent { get; set; }
    public bool FollowUpEmailSent { get; set; }
    public bool OnboardingCompleted { get; set; }
}