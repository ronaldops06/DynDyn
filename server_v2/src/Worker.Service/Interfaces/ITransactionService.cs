namespace Worker.Service.Interfaces;

public interface ITransactionService
{
    Task GenerateNotificationPayableTransactionsAsync();
}