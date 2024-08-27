using MassTransit;
using Newsletter.Api.Messages;

namespace Newsletter.Api.Sagas;

public class NewsletterOnboardingSaga : MassTransitStateMachine<NewsletterOnboardingSagaData>
{
    public NewsletterOnboardingSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => SubscriberCreated, e => e.CorrelateById(m => m.Message.SubscriberId));
        Event(() => WelcomeEmailSent, e => e.CorrelateById(m => m.Message.SubscriberId));
        Event(() => FollowUpEmailSent, e => e.CorrelateById(m => m.Message.SubscriberId));

        Initially(
            When(SubscriberCreated)
                .Then(context =>
                {
                    context.Saga.Email = context.Message.Email;
                    context.Saga.SubscriberId = context.Message.SubscriberId;
                })
                .TransitionTo(Welcoming)
                .Publish(context => new SendWelcomeEmail(context.Message.SubscriberId, context.Message.Email)));

        During(Welcoming,
            When(WelcomeEmailSent)
                .Then(context => context.Saga.WelcomeEmailSent = true)
                .TransitionTo(FollowingUp)
                .Publish(context => new SendFollowUpEmail(context.Message.SubscriberId, context.Message.Email)));

        During(FollowingUp,
            When(FollowUpEmailSent)
                .Then(context =>
                {
                    context.Saga.FollowUpEmailSent = true;
                    context.Saga.OnboardingCompleted = true;
                })
                .TransitionTo(Onboarding)
                .Publish(context => new OnboardingCompleted
                {
                    SubscriberId = context.Message.SubscriberId,
                    Email = context.Message.Email
                })
                .Finalize());
    }

    public State Welcoming { get; set; } = default!;
    public State FollowingUp { get; set; } = default!;
    public State Onboarding { get; set; } = default!;

    public Event<SubscriberCreated> SubscriberCreated { get; set; } = default!;
    public Event<WelcomeEmailSent> WelcomeEmailSent { get; set; } = default!;
    public Event<FollowUpEmailSent> FollowUpEmailSent { get; set; } = default!;
}