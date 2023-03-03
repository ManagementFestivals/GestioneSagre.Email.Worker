namespace GestioneSagre.Email.Worker.BusinessLayer.Query;

public class GetRetryMessageQuery : IRequest
{
    public Guid MessageId { get; set; }
    public string RecipientEmail { get; set; }
    public string Subject { get; set; }
    public string HtmlMessage { get; set; }

    public GetRetryMessageQuery(EmailRequest request)
    {
        MessageId = request.MessageId;
        RecipientEmail = request.RecipientEmail;
        Subject = request.Subject;
        HtmlMessage = request.HtmlMessage;
    }
}