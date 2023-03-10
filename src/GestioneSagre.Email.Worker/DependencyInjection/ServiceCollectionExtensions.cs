namespace GestioneSagre.Email.Worker.DomainLayer.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceCollection(this IServiceCollection services, IConfiguration Configuration)
    {
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

        services
            .AddScoped<DbContext, EmailSenderDbContext>()
            .AddScoped(typeof(IUnitOfWork<,>), typeof(UnitOfWork<,>))
            .AddScoped(typeof(IDatabaseRepository<,>), typeof(DatabaseRepository<,>))
            .AddScoped(typeof(ICommandRepository<,>), typeof(CommandRepository<,>));

        services
            .AddTransient<IEmailMessages, EmailMessages>();

        services
            .Configure<KestrelServerOptions>(Configuration.GetSection("Kestrel"));

        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCountMessageHandler).Assembly));

        services.AddRabbitMq(settings =>
        {
            settings.ConnectionString = Configuration.GetConnectionString("RabbitMQ");
            settings.ExchangeName = Configuration.GetValue<string>("AppSettings:ApplicationName");
            settings.QueuePrefetchCount = Configuration.GetValue<ushort>("AppSettings:QueuePrefetchCount");
        }, queues =>
        {
            queues.Add<EmailRequest>();
        }).AddReceiver<EmailRequest, EmailMessageReceiver>();

        return services;
    }
}