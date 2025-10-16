using System.Data;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Worker.Service.Data;
using Worker.Service.Data.Repository;
using Worker.Service.Interfaces;
using Worker.Service.Service;
using Worker.Service.Workers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
        
        // Registra a conexão do PostgreSQL para o EF
        services.AddDbContext<WorkerContext>(options => options.UseNpgsql(connectionString));
        // Registra a conexão pura do PostgreSQL como IDbConnection
        services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(connectionString));
        
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(options => options.UseNpgsqlConnection(connectionString)));

        services.AddHangfireServer();
        
        services.AddScoped<INotificationService, FcmNotificationService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
    })/*.ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseUrls("http://192.168.0.8:5000");
        webBuilder.Configure(app =>
        {   
            app.UseRouting();
            // Dashboard em /hangfire
            app.UseHttpsRedirection();
            app.UseHangfireDashboard("/hangfire");
        });
    })*/
    .Build();

JobStorage.Current = host.Services.GetRequiredService<JobStorage>();

RecurringJob.AddOrUpdate<TransactionWorker>(
    "job-transaction",
    s => s.ExecuteAsync(),
    "0 */12 * * *",
    TimeZoneInfo.Local);

RecurringJob.AddOrUpdate<NotificationWorker>(
    "job-notification",
    s => s.ExecuteAsync(),
    "*/5 * * * *",
    TimeZoneInfo.Local);

await host.RunAsync();

