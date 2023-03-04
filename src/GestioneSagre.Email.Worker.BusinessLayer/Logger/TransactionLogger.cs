using Microsoft.Extensions.Hosting;

namespace GestioneSagre.Email.Worker.BusinessLayer.Logger;

internal class TransactionLogger : ITransactionLogger
{
    private readonly IHostEnvironment env;
    private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);

    public TransactionLogger(IHostEnvironment env)
    {
        this.env = env;
    }

    public async Task LogTransactionAsync(MessageSaveRequest inputModel)
    {
        var filePath = Path.Combine(env.ContentRootPath, "Logger", "emailWorker-transactions.txt");
        var content = $"\r\n{inputModel.MessageId}\t{inputModel.Recipient}\t{inputModel.Subject}\t{inputModel.Message}\t{inputModel.SenderCount}\t{inputModel.Status}";

        try
        {
            await semaphore.WaitAsync();
            await File.AppendAllTextAsync(filePath, content);
        }
        finally
        {
            semaphore.Release();
        }
    }
}