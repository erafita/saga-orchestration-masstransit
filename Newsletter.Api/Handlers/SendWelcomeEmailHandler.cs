using MassTransit;
using Newsletter.Api.Messages;
using Newsletter.Api.Services.Abstractions;

namespace Newsletter.Api.Handlers;

public class SendWelcomeEmailHandler(IEmailService emailService) : IConsumer<SendWelcomeEmail>
{
    public async Task Consume(ConsumeContext<SendWelcomeEmail> context)
    {
        await emailService.SendWelcomeMessageAsync(context.Message.Email);

        await context.Publish(new WelcomeEmailSent
        {
            Email = context.Message.Email,
            SubscriberId = context.Message.SubscriberId
        });
    }
}