namespace GestioneSagre.Email.Worker.BusinessLayer.Handlers;

public class SaveMessageHandler : IRequestHandler<SaveMessageCommand, EmailMessage>
{
    private readonly ILogger<SaveMessageHandler> logger;
    private readonly IEmailMessages emailMessages;

    public SaveMessageHandler(ILogger<SaveMessageHandler> logger, IEmailMessages emailMessages)
    {
        this.logger = logger;
        this.emailMessages = emailMessages;
    }

    public async Task<EmailMessage> Handle(SaveMessageCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var newMessage = new EmailMessage()
            {
                MessageId = command.MessageId,
                Recipient = command.Recipient,
                Subject = command.Subject,
                Message = command.Message,
                SenderCount = command.SenderCount,
                Status = command.Status
            };

            await emailMessages.CreateItemAsync(newMessage);

            var response = new EmailMessage()
            {
                Id = newMessage.Id,
                MessageId = newMessage.MessageId,
                Recipient = command.Recipient,
                Subject = command.Subject,
                Message = command.Message,
                SenderCount = command.SenderCount,
                Status = command.Status
            };

            return response;
        }
        catch (Exception exc)
        {
            logger.LogError(exc, "Error during automatic process utility worker service for processing email {email}", command.MessageId);
            throw;
        }
    }
}