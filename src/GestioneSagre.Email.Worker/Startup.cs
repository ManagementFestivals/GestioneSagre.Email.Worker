using GestioneSagre.Email.Worker.BusinessLayer.Receivers;
using GestioneSagre.Email.Worker.DataAccessLayer;
using GestioneSagre.Messaging.RabbitMq;
using GestioneSagre.SharedKernel.Models.Email;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

namespace GestioneSagre.Email.Worker;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();

        services.Configure<KestrelServerOptions>(Configuration.GetSection("Kestrel"));

        services.AddRabbitMq(settings =>
        {
            settings.ConnectionString = Configuration.GetConnectionString("RabbitMQ");
            settings.ExchangeName = Configuration.GetValue<string>("AppSettings:ApplicationName");
            settings.QueuePrefetchCount = Configuration.GetValue<ushort>("AppSettings:QueuePrefetchCount");
        }, queues =>
        {
            queues.Add<EmailRequest>();
        })
        .AddReceiver<EmailRequest, EmailMessageReceiver>();

        services.AddDbContextPool<EmailSenderDbContext>(optionsBuilder =>
        {
            var connectionString = Configuration.GetSection("ConnectionStrings").GetValue<string>("Default");

            optionsBuilder.UseSqlServer(connectionString, options =>
            {
                // Abilito il connection resiliency per gestire le connessioni perse
                // Info su: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                options.EnableRetryOnFailure(3);
            });
        });
    }

    public void Configure(WebApplication app)
    {
        IWebHostEnvironment env = app.Environment;

        app.UseRouting();
    }
}