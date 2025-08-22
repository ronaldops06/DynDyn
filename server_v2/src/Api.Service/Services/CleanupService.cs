using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Domain.Interfaces.Services;
using Domain.Interfaces;
using Domain.Interfaces.Services.User;

namespace Service.Services
{
    public class CleanupService : ICleanupService
    {
        private IUserService _userService;
        private readonly IEnumerable<ICleanupRepository> _repositories;
        
        public CleanupService(IUserService userService, IEnumerable<ICleanupRepository> repositories)
        {
            _userService = userService;
            _repositories = repositories;
        }
        
        public async Task DeleteAllAsync()
        {
            var user = await _userService.GetLoggedUser();
            
            foreach (var repository in _repositories.OrderByDescending(x => x.CleanupOrder))
                await repository.DeleteAllByUserAsync(user.Id);
        }
    }
}