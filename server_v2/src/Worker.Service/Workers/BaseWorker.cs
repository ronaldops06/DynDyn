using Worker.Service.Interfaces;

namespace Worker.Service.Workers;

public abstract class BaseWorker<T> : IWorkerTask
{
    protected readonly ILogger<T> logger;
    
    public BaseWorker(ILogger<T> logger)
    {
        this.logger = logger;
    }
    
    public abstract Task ExecuteAsync();
    
}