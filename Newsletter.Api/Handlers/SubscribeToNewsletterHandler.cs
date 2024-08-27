using MassTransit;
using Newsletter.Api.Messages;
using Newsletter.Api.Persistence;

namespace Newsletter.Api.Handlers;

public class SubscribeToNewsletterHandler(AppDbContext dbContext) : IConsumer<SubscribeToNewsletter>
{
    public async Task Consume(ConsumeContext<SubscribeToNewsletter> context)
    {
        var subscriber = dbContext.Subscribers.Add(new Subscriber
        {
            Id = Guid.NewGuid(),
            Email = context.Message.Email,
            SubscribeOnUtc = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync();

        await context.Publish(new SubscriberCreated
        {
            Email = context.Message.Email,
            SubscriberId = subscriber.Entity.Id
        });
    }
}