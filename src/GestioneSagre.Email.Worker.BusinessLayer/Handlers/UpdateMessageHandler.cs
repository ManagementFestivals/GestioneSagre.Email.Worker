namespace GestioneSagre.Email.Worker.BusinessLayer.Handlers;

internal class UpdateMessageHandler : IRequestHandler<UpdateMessageCommand>
{
    private readonly ILogger<UpdateMessageHandler> logger;
    private readonly IEmailMessages emailMessages;

    public UpdateMessageHandler(ILogger<UpdateMessageHandler> logger, IEmailMessages emailMessages)
    {
        this.logger = logger;
        this.emailMessages = emailMessages;
    }

    public async Task Handle(UpdateMessageCommand command, CancellationToken cancellationToken)
    {
        await emailMessages.UpdateEmailMessageAsync(command.MessageId);
    }
}