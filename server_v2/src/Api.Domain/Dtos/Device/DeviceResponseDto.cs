namespace Api.Domain.Dtos.Device
{
    /// <summary>
    /// Objeto de transferência de dados para o retorno de dispositivo nas requisições.
    /// </summary>
    public class DeviceResponseDto : BaseDto
    {
        /// <summary>
        /// Token do broker de push notification
        /// </summary>
        public string NotificationToken { get; set; }
        
        /// <summary>
        /// Identificador do dispositivo físico
        /// </summary>
        public string PhisicalDeviceId { get; set; }
    }
}