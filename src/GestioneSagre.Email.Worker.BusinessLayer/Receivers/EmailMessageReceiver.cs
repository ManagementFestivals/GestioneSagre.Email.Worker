using GestioneSagre.Messaging.Abstractions;
using GestioneSagre.SharedKernel.Models.Email;
using Microsoft.Extensions.Logging;

namespace GestioneSagre.Email.Worker.BusinessLayer.Receivers;

public class EmailMessageReceiver : IMessageReceiver<EmailRequest>
{
    private readonly ILogger<EmailMessageReceiver> logger;

    public EmailMessageReceiver(ILogger<EmailMessageReceiver> logger)
    {
        this.logger = logger;
    }

    public async Task ReceiveAsync(EmailRequest message, CancellationToken cancellationToken)
    {
        try
        {
            //Codice da usare in fase di test
            logger.LogInformation("Processing message {MessageId}...", message.MessageId);
            await Task.Delay(TimeSpan.FromSeconds(10 + Random.Shared.Next(10)));
            logger.LogInformation("End processing order {MessageId}", message.MessageId);

            //Ricevo il messaggio dalla coda di RabbitMQ
            //Salvo il messaggio sul database

            //Chiamo il servizio api-email-sender di invio email
            //var httpClient = new HttpClient();
            //var response = await httpClient.PostAsJsonAsync("http://localhost:5164/api/home", message, cancellationToken);
        }
        catch (Exception exc)
        {
            logger.LogError(exc, "Error during automatic process utility worker service for processing email {email}", message.MessageId);
        }
    }
}