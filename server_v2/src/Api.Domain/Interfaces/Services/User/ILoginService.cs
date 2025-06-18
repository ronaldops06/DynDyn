using Domain.Models;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services.User
{
    public interface ILoginService
    {
        Task<TransientUserModel> GetLoginAsync(TransientUserModel userModel);

        string GenerateToken(TransientUserModel userModel);
    }
}
