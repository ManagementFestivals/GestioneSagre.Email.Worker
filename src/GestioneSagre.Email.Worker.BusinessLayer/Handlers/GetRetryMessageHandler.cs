using GestioneSagre.Email.Worker.BusinessLayer.Query;

namespace GestioneSagre.Email.Worker.BusinessLayer.Handlers;

public class GetRetryMessageHandler : IRequestHandler<GetRetryMessageQuery>
{
    private readonly ILogger<GetCountMessageHandler> logger;
    private readonly IEmailMessages emailMessages;

    public GetRetryMessageHandler(ILogger<GetCountMessageHandler> logger, IEmailMessages emailMessages)
    {
        this.logger = logger;
        this.emailMessages = emailMessages;
    }

    public async Task Handle(GetRetryMessageQuery query, CancellationToken cancellationToken)
    {
        var message = new EmailMessage()
        {
            MessageId = query.MessageId,
            Recipient = query.RecipientEmail,
            Subject = query.Subject,
            Message = query.HtmlMessage
        };

        await emailMessages.RetrySendEmailAsync(message);
    }
}