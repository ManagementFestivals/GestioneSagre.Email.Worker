namespace GestioneSagre.Email.Worker.BusinessLayer.Query;

public class GetCountMessageQuery : IRequest<int>
{
    public Guid MessageId { get; set; }

    public GetCountMessageQuery(Guid messageId)
    {
        MessageId = messageId;
    }
}