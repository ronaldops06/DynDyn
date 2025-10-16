using Domain.Entities;

namespace Worker.Service.Interfaces;

public interface INotificationRepository
{
    Task<List<NotificationEntity>> SelectPendingNotificationsAsync();
    Task<NotificationEntity> InsertAsync(NotificationEntity item);
    Task<NotificationEntity> UpdateAsync(NotificationEntity item);
}