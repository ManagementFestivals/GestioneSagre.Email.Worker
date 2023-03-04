namespace GestioneSagre.Email.Worker.BusinessLayer.Receivers;

public class EmailMessageReceiver : IMessageReceiver<EmailRequest>
{
    private readonly ILogger<EmailMessageReceiver> logger;
    private readonly IMediator mediator;
    private readonly ITransactionLogger transaction;

    public EmailMessageReceiver(ILogger<EmailMessageReceiver> logger, IMediator mediator, ITransactionLogger transaction)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.transaction = transaction;
    }

    public async Task ReceiveAsync(EmailRequest message, CancellationToken cancellationToken)
    {
        try
        {
            //Ricevo il messaggio dalla coda di RabbitMQ
            var newMessage = new MessageSaveRequest()
            {
                MessageId = message.MessageId,
                Recipient = message.RecipientEmail,
                Subject = message.Subject,
                Message = message.HtmlMessage,
                SenderCount = 0,
                Status = MailStatus.InProgress
            };

            // Verifico se il messaggio è già presente nel sistema
            var messageCount = await mediator.Send(new GetCountMessageQuery(newMessage.MessageId), cancellationToken);

            if (messageCount == 0)
            {
                // Salvo il messaggio sul database
                var SenderResult = await mediator.Send(new SaveMessageCommand(newMessage), cancellationToken);

                if (SenderResult == null)
                {
                    // Salvataggio dell'email su file di testo
                    await transaction.LogTransactionAsync(newMessage);
                }
            }

            // Chiamo il servizio api-email-sender di invio email
            var httpClient = new HttpClient();
            var result = await httpClient.PostAsJsonAsync("http://localhost:5164/api/home", message, cancellationToken);

            if (result.IsSuccessStatusCode)
            {
                // Aggiorno lo stato dell'email a SENT sul database
                await mediator.Send(new UpdateMessageCommand(message.MessageId));
            }
        }
        catch (Exception exc)
        {
            logger.LogError(exc, "Attempt to send email {email} via API failed !", message.MessageId);

            // Schedulazione nuovo tentativo in quanto è stato riscontrato un fallimento di quello precedente !
            await mediator.Send(new GetRetryMessageQuery(message), cancellationToken);
        }
    }
}