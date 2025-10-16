namespace Worker.Service.Workers.Tasks;

public class Teste
{
    private ILogger _logger;

    public Teste(ILogger<Teste> logger)
    {
        _logger = logger;
    }
    
    public void Execute()
    {
        _logger.LogInformation("Executando teste");
        Console.WriteLine("teste executando");
    }
}