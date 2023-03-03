namespace GestioneSagre.Email.Worker.BusinessLayer.Services;

public interface IEmailMessages
{
    Task CreateItemAsync(EmailMessage entity);
    Task<int> GetCountEmailMessageAsync(Guid messageId);
    Task RetrySendEmailAsync(EmailMessage request);
    Task UpdateEmailMessageAsync(Guid messageId);
}