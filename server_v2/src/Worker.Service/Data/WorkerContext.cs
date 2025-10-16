using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Worker.Service.Data;

public class WorkerContext : DbContext
{
    public WorkerContext(DbContextOptions<WorkerContext> options) : base(options) { }

    public DbSet<NotificationEntity> Notification { get; set; }
}