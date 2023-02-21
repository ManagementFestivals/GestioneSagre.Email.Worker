using GestioneSagre.SharedKernel.Enums;

namespace GestioneSagre.Email.Worker.DataAccessLayer.Entities;

public class EmailMessage
{
    public int Id { get; set; }
    public Guid MessageId { get; set; }
    public string Recipient { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public int SenderCount { get; set; }
    public MailStatus Status { get; set; }

    public void ChangeStatus(MailStatus status)
    {
        Status = status;
    }

    public void ChangeSenderCount(int senderCount)
    {
        SenderCount = senderCount;
    }
}