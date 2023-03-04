namespace GestioneSagre.Email.Worker.BusinessLayer.Logger;

public interface ITransactionLogger
{
    Task LogTransactionAsync(MessageSaveRequest inputModel);
}