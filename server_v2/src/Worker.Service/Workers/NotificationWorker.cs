using Worker.Service.Interfaces;

namespace Worker.Service.Workers;

public class NotificationWorker : BaseWorker<NotificationWorker>
{
    private readonly INotificationService _service;
    
    public NotificationWorker(ILogger<NotificationWorker> logger, INotificationService service) : base(logger)
    {
        _service = service;
    }

    public override async Task ExecuteAsync()
    {
        try
        {
            logger.LogInformation("Executando o serviço de notificações");
            await _service.SendPendingNotificationsAsync();
        }
        catch(Exception ex)
        {
            logger.LogError($"Erro no serviço de envio de notificações. Erro: {ex.Message}");
        }
    }
}