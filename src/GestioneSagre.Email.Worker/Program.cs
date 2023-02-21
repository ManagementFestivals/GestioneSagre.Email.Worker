using GestioneSagre.Email.Worker.BusinessLayer.Receivers;
using GestioneSagre.Email.Worker.DataAccessLayer;
using GestioneSagre.Messaging.RabbitMq;
using GestioneSagre.SharedKernel.Models.Email;
using Microsoft.EntityFrameworkCore;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServices)
    .Build();

await host.RunAsync();

void ConfigureServices(HostBuilderContext hostingContext, IServiceCollection services)
{
    var configuration = hostingContext.Configuration;

    services.AddRabbitMq(settings =>
    {
        settings.ConnectionString = configuration.GetConnectionString("RabbitMQ");
        settings.ExchangeName = configuration.GetValue<string>("AppSettings:ApplicationName");
        settings.QueuePrefetchCount = configuration.GetValue<ushort>("AppSettings:QueuePrefetchCount");
    }, queues =>
    {
        queues.Add<EmailRequest>();
    })
    .AddReceiver<EmailRequest, EmailMessageReceiver>();

    services.AddDbContextPool<EmailSenderDbContext>(optionsBuilder =>
    {
        var connectionString = configuration.GetSection("ConnectionStrings").GetValue<string>("Default");

        optionsBuilder.UseSqlServer(connectionString, options =>
        {
            // Abilito il connection resiliency per gestire le connessioni perse
            // Info su: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
            options.EnableRetryOnFailure(3);
        });
    });
}