namespace GestioneSagre.Email.Worker.BusinessLayer.Command;

public class SaveMessageCommand : IRequest<EmailMessage>
{
    public Guid MessageId { get; set; }
    public string Recipient { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public int SenderCount { get; set; }
    public MailStatus Status { get; set; }

    public SaveMessageCommand(MessageSaveRequest inputModel)
    {
        MessageId = inputModel.MessageId;
        Recipient = inputModel.Recipient;
        Subject = inputModel.Subject;
        Message = inputModel.Message;
        SenderCount = inputModel.SenderCount;
        Status = inputModel.Status;
    }
}