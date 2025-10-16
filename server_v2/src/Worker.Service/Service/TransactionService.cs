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
            var body = new
            {
                OperationName = transaction.OperationName,
                Value = transaction.Value
            };
                
            var notificationEntity = new NotificationEntity
            {
                DeviceToken = transaction.NotificationToken,
                Title = "Payable Transactions",
                Message = JsonConvert.ToString(JsonConvert.SerializeObject(body)),
                Sent = false
            };

            await _notificationRepository.InsertAsync(notificationEntity);
        }
    }
}