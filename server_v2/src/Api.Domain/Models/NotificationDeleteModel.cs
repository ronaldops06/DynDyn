using Domain.Interfaces;

namespace Domain.Models
{
    public class NotificationDeleteModel : IBodyNotificationModel
    {
        public string Reference { get; set; }
        public int Id { get; set; }
    }
}