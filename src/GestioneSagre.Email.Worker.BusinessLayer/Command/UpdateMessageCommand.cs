namespace GestioneSagre.Email.Worker.BusinessLayer.Command;

internal class UpdateMessageCommand : IRequest
{
    public Guid MessageId { get; set; }

    public UpdateMessageCommand(Guid messageId)
    {
        MessageId = messageId;
    }
}