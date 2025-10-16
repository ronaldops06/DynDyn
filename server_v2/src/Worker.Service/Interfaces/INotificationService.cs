using Domain.Models;

namespace Worker.Service.Interfaces;

public interface INotificationService
{
    Task SendPendingNotificationsAsync();
}