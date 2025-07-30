using Domain.Models;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services.User
{
    public interface ILoginService
    {
        Task<TransientUserModel> GetLoginAsync(TransientUserModel userModel);

        Task<TransientUserModel> ExecuteChangePassword(string login, string password, string newPassword);

        string GenerateToken(TransientUserModel userModel);
    }
}
