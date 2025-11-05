using System.Threading.Tasks;
using Api.Domain.Interfaces.Services;
using AutoMapper;
using Domain.Interfaces.Services.User;
using Domain.Models;

namespace Service.Services
{
    public class BaseService
    {
        private readonly ITrashService _trashService;
        protected readonly IUserService userService;
        protected readonly IMapper mapper;
        
        public BaseService(ITrashService trashService,
                           IUserService userService,
                           IMapper mapper)
        {
            _trashService = trashService;
            this.userService = userService;
            this.mapper = mapper;
        }

        protected async Task ProcessExcludeEntityAsync(string reference, int referenceId)
        {
            var trashModel = new TrashModel()
            {
                Reference = reference,
                ReferenceId = referenceId
            };

            await _trashService.Post(trashModel);
        }
    }
}