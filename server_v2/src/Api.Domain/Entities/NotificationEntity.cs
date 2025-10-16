namespace Domain.Entities
{
    public class NotificationEntity : BaseEntity
    {
        public string DeviceToken { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool Sent { get; set; }
    }
}