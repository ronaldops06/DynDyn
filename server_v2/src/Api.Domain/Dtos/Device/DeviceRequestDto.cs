using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Dtos.Device
{
    public class DeviceRequestDto : BaseDto
    {
        /// <summary>
        /// Token do broker de push notification
        /// </summary>
        public string NotificationToken { get; set; }
        
        /// <summary>
        /// Identificador do dispositivo físico
        /// </summary>
        [Required(ErrorMessage = "{0} é um campo obrigatório")]
        public string PhisicalDeviceId { get; set; }
    }
}