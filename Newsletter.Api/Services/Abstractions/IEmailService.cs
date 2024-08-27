namespace Newsletter.Api.Services.Abstractions;

public interface IEmailService
{
    Task SendWelcomeMessageAsync(string email);

    Task SendFollowUpMessageAsync(string email);
}