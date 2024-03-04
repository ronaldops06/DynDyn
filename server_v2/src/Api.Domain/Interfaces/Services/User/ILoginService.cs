using Domain.Models;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services.User
{
    public interface ILoginService
    {
        Task<UserModel> GetLoginAsync(UserModel userModel);

        string GenerateToken(UserModel userModel);
    }
}
