using System.Threading.Tasks;
using Api.Domain.Interfaces.Services;
using AutoMapper;
using Domain.Interfaces.Services.User;
using Domain.Models;

namespace Service.Services
{
    public class BaseService
    {
        private readonly IDeviceService _deviceService;
        protected readonly IUserService userService;
        protected readonly IMapper mapper;
        
        public BaseService(IDeviceService deviceService,
                           IUserService userService,
                           IMapper mapper)
        {
            _deviceService = deviceService;
            this.userService = userService;
            this.mapper = mapper;
        }

        protected async Task ProcessExcludeEntityAsync(string reference, int referenceId)
        {
            var notification = new NotificationModel
            {
                Title = "Excluded Entity",
                Body = new
                {
                    Operation = "DELETE",
                    Reference = reference,
                    Id = referenceId
                }
            };

            await _deviceService.SendNotificationByUser(notification);
        }
    }
}