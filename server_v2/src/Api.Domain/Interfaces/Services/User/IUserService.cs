using Domain.Helpers;
using Domain.Models;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services.User
{
    public interface IUserService
    {
        Task<UserModel> GetUsuarioByUsernameAndPassword(UserModel user);

        Task<UserModel> GetUsuarioByLogin(string login);

        Task<PageList<UserModel>> Get(PageParams pageParams);

        Task<UserModel> Post(UserModel user);

        Task<UserModel> Put(UserModel user);

        Task<bool> Delete(int id);
    }
}
