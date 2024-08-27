using MassTransit;
using Newsletter.Api.Messages;
using Newsletter.Api.Services.Abstractions;

namespace Newsletter.Api.Handlers;

public class SendFollowUpEmailHandler(IEmailService emailService) : IConsumer<SendFollowUpEmail>
{
    public async Task Consume(ConsumeContext<SendFollowUpEmail> context)
    {
        await emailService.SendFollowUpMessageAsync(context.Message.Email);

        await context.Publish(new FollowUpEmailSent
        {
            Email = context.Message.Email,
            SubscriberId = context.Message.SubscriberId
        });
    }
}