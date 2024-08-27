using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newsletter.Api.Messages;
using Newsletter.Api.Persistence;
using Newsletter.Api.Sagas;
using Newsletter.Api.Services;
using Newsletter.Api.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddMassTransit(configuration =>
{
    configuration.SetKebabCaseEndpointNameFormatter();

    configuration.AddConsumers(typeof(Program).Assembly);

    configuration.AddSagaStateMachine<NewsletterOnboardingSaga, NewsletterOnboardingSagaData>()
        .EntityFrameworkRepository(options =>
        {
            options.ExistingDbContext<AppDbContext>();
            options.UsePostgres();
        });

    configuration.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri(builder.Configuration.GetConnectionString("RabbitMq")!), host =>
        {
            host.Username("guest");
            host.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });

    configuration.AddEntityFrameworkOutbox<AppDbContext>(options =>
    {
        options.UseBusOutbox();
        options.UsePostgres();
    });

    configuration.AddConfigureEndpointsCallback((context, name, cfg) =>
    {
        cfg.UseEntityFrameworkOutbox<AppDbContext>(context);
    });
});

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

app.MapPost("/newsletter", async ([FromBody] string email, IBus bus) =>
{
    await bus.Publish(new SubscribeToNewsletter(email));

    return Results.Accepted();
});

app.Run();