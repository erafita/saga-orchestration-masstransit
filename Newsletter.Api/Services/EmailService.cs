using Newsletter.Api.Services.Abstractions;

namespace Newsletter.Api.Services;

public class EmailService : IEmailService
{
    public Task SendFollowUpMessageAsync(string email)
    {
        return Task.CompletedTask;
    }

    public Task SendWelcomeMessageAsync(string email)
    {
        return Task.CompletedTask;
    }
}