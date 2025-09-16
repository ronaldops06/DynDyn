namespace Domain.Models
{
    public class DeviceModel : BaseModel
    {
        /// <summary>
        /// Token do broker de push notification
        /// </summary>
        public string NotificationToken { get; set; }
        
        /// <summary>
        /// Identificador do dispositivo físico
        /// </summary>
        public string PhisicalDeviceId { get; set; }
        
        /// <summary>
        /// Identificador do usuário base.
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// Usuário base
        /// </summary>
        public UserModel User { get; set; }
    }
}