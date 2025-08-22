using Api.Domain.Interfaces.Services;
using Api.Service.Services;
using Domain.Interfaces.Services.User;
using Microsoft.Extensions.DependencyInjection;
using Service.Services;

namespace CrossCutting.DependencyInjection
{
    public class ConfigureService
    {
        public static void ConfigureDependenciesService(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<ITransientUserService, TransientUserService>();
            serviceCollection.AddTransient<ILoginService, LoginService>();
            serviceCollection.AddTransient<ICategoryService, CategoryService>();
            serviceCollection.AddTransient<IPortfolioService, PortfolioService>();
            serviceCollection.AddTransient<IOperationService, OperationService>();
            serviceCollection.AddTransient<ITransactionService, TransactionService>();
            serviceCollection.AddTransient<IBalanceService, BalanceService>();
            
            serviceCollection.AddTransient<ICleanupService, CleanupService>();
        }
    }
}
