using System.Globalization;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Worker.Service.Interfaces;

namespace Worker.Service.Data.Repository;

public class NotificationRepository : INotificationRepository
{
    private readonly WorkerContext _context;
    private DbSet<NotificationEntity> _dataset;
    
    public NotificationRepository(WorkerContext dbContext)
    {
        _context = dbContext;
        _dataset = dbContext.Set<NotificationEntity>();
    }

    public async Task<List<NotificationEntity>> SelectPendingNotificationsAsync()
    {
        IQueryable<NotificationEntity> query = _context.Notification;
        
        query = query.Where(x => !x.Sent);
                
        query = query.AsNoTracking().OrderBy(a => a.Id);
        return await query.ToListAsync();
    }
    
    public async Task<NotificationEntity> InsertAsync(NotificationEntity item)
    {
        try
        {
            item.DataCriacao = item.DataCriacao ?? DateTime.UtcNow;
            item.DataAlteracao = item.DataCriacao ?? DateTime.UtcNow;

            _dataset.Add(item);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var message = ex.Message;
                
            if (ex.InnerException != null)
                message = $"Inner Exception: {ex.InnerException.Message}";
            
            throw new Exception(message);
        }

        return item;
    }
    
    public async Task<NotificationEntity> UpdateAsync(NotificationEntity item)
    {
        try
        {
            var result = await _dataset.SingleOrDefaultAsync(x => x.Id.Equals(item.Id));
            if (result == null)
                throw new Exception("No data found");
            
            _context.Entry(result).CurrentValues.SetValues(item);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return item;
    }
}