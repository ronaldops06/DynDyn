using Api.Data.Repository;
using Api.Domain.Repository;
using Data.Context;
using Data.Repository;
using Domain.Interfaces;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;

namespace CrossCutting.DependencyInjection
{
    public class ConfigureRepository
    {
        public static void ConfigureDependenciesRepository(IServiceCollection serviceCollection)
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var logger = serviceProvider.GetRequiredService<ILogger<ConfigureRepository>>();
            
            try
            {
                logger.LogInformation("Configuring repository");
                
                serviceCollection.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
                serviceCollection.AddScoped<IUserRepository, UserRepository>();
                serviceCollection.AddScoped<ITransientUserRepository, TransientUserRepository>();
                serviceCollection.AddScoped<ICategoryRepository, CategoryRepository>();
                serviceCollection.AddScoped<IPortfolioRepository, PortfolioRepository>();
                serviceCollection.AddScoped<IOperationRepository, OperationRepository>();
                serviceCollection.AddScoped<ITransactionRepository, TransactionRepository>();
                serviceCollection.AddScoped<IBalanceRepository, BalanceRepository>();
                
                serviceCollection.AddScoped<ICleanupRepository, CategoryRepository>();
                serviceCollection.AddScoped<ICleanupRepository, PortfolioRepository>();
                serviceCollection.AddScoped<ICleanupRepository, OperationRepository>();
                serviceCollection.AddScoped<ICleanupRepository, TransactionRepository>();
                serviceCollection.AddScoped<ICleanupRepository, BalanceRepository>();
                serviceCollection.AddScoped<ICleanupRepository, UserRepository>();
                serviceCollection.AddScoped<ICleanupRepository, BalanceRepository>();
                
                logger.LogInformation("Dependencias injetadas");
                logger.LogInformation("Configurando database");
                
                if (Environment.GetEnvironmentVariable("DATABASE").ToLower() == "POSTGRES".ToLower())
                    serviceCollection.AddDbContext<SomniaContext>(
                        options => options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION"))
                    );
                logger.LogInformation("Database configurado");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
