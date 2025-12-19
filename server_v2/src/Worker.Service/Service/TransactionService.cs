using Domain.Entities;
using Newtonsoft.Json;
using Worker.Service.Interfaces;

namespace Worker.Service.Service;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly INotificationRepository _notificationRepository;
    
    public TransactionService(ITransactionRepository transactionRepository, INotificationRepository notificationRepository)
    {
        _transactionRepository = transactionRepository;
        _notificationRepository = notificationRepository;
    }
    
    public async Task GenerateNotificationPayableTransactionsAsync()
    {
        var transactions = await _transactionRepository.SelectPayableTransactionsAsync();

        foreach (var transaction in transactions)
        {
            var notificationEntity = new NotificationEntity
            {
                DeviceToken = transaction.NotificationToken,
                Title = "Transação a Pagar",
                Message = $"A transação {transaction.OperationName} no valor de R$ {transaction.Value} vence hoje",
                Sent = false
            };

            await _notificationRepository.InsertAsync(notificationEntity);
        }
    }
}