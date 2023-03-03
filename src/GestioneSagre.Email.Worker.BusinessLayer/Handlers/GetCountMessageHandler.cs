using GestioneSagre.Email.Worker.BusinessLayer.Query;

namespace GestioneSagre.Email.Worker.BusinessLayer.Handlers;

public class GetCountMessageHandler : IRequestHandler<GetCountMessageQuery, int>
{
    private readonly ILogger<GetCountMessageHandler> logger;
    private readonly IEmailMessages emailMessages;

    public GetCountMessageHandler(ILogger<GetCountMessageHandler> logger, IEmailMessages emailMessages)
    {
        this.logger = logger;
        this.emailMessages = emailMessages;
    }

    public async Task<int> Handle(GetCountMessageQuery query, CancellationToken cancellationToken)
    {
        var counter = await emailMessages.GetCountEmailMessageAsync(query.MessageId);

        return counter;
    }
}