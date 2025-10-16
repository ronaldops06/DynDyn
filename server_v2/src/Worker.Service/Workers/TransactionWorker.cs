using Worker.Service.Interfaces;

namespace Worker.Service.Workers;

public class TransactionWorker : BaseWorker<TransactionWorker>
{
    private readonly ITransactionService _service;
    
    public TransactionWorker(ILogger<TransactionWorker> logger, ITransactionService service) : base(logger)
    {
        _service = service;
    }

    public override async Task ExecuteAsync()
    {
        try
        {
            logger.LogInformation($"[{DateTime.Now}] Executando o serviço de transações a serem pagas.");
            await _service.GenerateNotificationPayableTransactionsAsync();
        }
        catch(Exception ex)
        {
            logger.LogError($"Erro no serviço de transações a serem pagas. Erro: {ex.Message}");
        }
    }
}