namespace GestioneSagre.Email.Worker.BusinessLayer.Services;

public class EmailMessages : IEmailMessages
{
    private readonly IUnitOfWork<EmailMessage, int> unitOfWork;
    private readonly IMessageSender messageSender;

    public EmailMessages(IUnitOfWork<EmailMessage, int> unitOfWork, IMessageSender messageSender)
    {
        this.unitOfWork = unitOfWork;
        this.messageSender = messageSender;
    }

    public async Task CreateItemAsync(EmailMessage entity)
    {
        await unitOfWork.Command.CreateAsync(entity);
    }

    public async Task<int> GetCountEmailMessageAsync(Guid messageId)
    {
        var listEmail = await GetAllEmailAsync();
        var counter = listEmail
            .Where(x => x.MessageId == messageId)
            .Count();

        return counter;
    }

    public async Task RetrySendEmailAsync(EmailMessage request)
    {
        var statusCounter = request.SenderCount;

        if (statusCounter >= 10)
        {
            // Se contatore uguale o maggiore a 10, imposto lo stato dell'email a DELETED e non aggiungo la mail alla coda di RabbitMQ
            request.Status = MailStatus.Deleted;

            await unitOfWork.Command.UpdateAsync(request);
        }
        else
        {
            // Se contatore inferiore a 10, incremento il contatore +1 e non aggiorno lo stato sul database
            request.SenderCount++;

            await unitOfWork.Command.UpdateAsync(request);

            await messageSender.PublishAsync(request);
        }
    }

    public async Task UpdateEmailMessageAsync(Guid messageId)
    {
        var listMessage = await GetAllEmailAsync();

        var message = listMessage
            .Where(x => x.MessageId == messageId)
            .FirstOrDefault();

        message.Status = MailStatus.Sent;

        await unitOfWork.Command.UpdateAsync(message);
    }

    private async Task<List<EmailMessage>> GetAllEmailAsync()
    {
        var listEmail = await unitOfWork.ReadOnly.GetAllAsync();

        return listEmail;
    }
}