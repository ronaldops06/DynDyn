using Api.Data.Repository;
using Api.Domain.Repository;
using Data.Context;
using Data.Repository;
using Domain.Interfaces;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CrossCutting.DependencyInjection
{
    public class ConfigureRepository
    {
        public static void ConfigureDependenciesRepository(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<ICategoryRepository, CategoryRepository>();
            serviceCollection.AddScoped<IAccountRepository, AccountRepository>();
            serviceCollection.AddScoped<IOperationRepository, OperationRepository>();

            if (Environment.GetEnvironmentVariable("DATABASE").ToLower() == "POSTGRES".ToLower())
                serviceCollection.AddDbContext<SomniaContext>(
                    options => options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION"))
                );
        }
    }
}
