namespace GestioneSagre.Email.Worker.BusinessLayer.Services;

public class EmailMessages : IEmailMessages
{
    private readonly IUnitOfWork<EmailMessage, int> unitOfWork;

    public EmailMessages(IUnitOfWork<EmailMessage, int> unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task CreateItemAsync(EmailMessage entity)
    {
        await unitOfWork.Command.CreateAsync(entity);
    }

    public async Task<int> GetCountEmailMessageAsync(Guid messageId)
    {
        var listEmail = await unitOfWork.ReadOnly.GetAllAsync();

        var counter = listEmail
            .Where(x => x.MessageId == messageId)
            .Count();

        return counter;
    }
}