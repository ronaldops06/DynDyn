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
            serviceCollection.AddTransient<ILoginService, LoginService>();
            serviceCollection.AddTransient<ICategoryService, CategoryService>();
            serviceCollection.AddTransient<IAccountService, AccountService>();
            serviceCollection.AddTransient<IOperationService, OperationService>();
            serviceCollection.AddTransient<ITransactionService, TransactionService>();
        }
    }
}
