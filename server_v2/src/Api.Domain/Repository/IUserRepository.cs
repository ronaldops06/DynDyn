using Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces;
using Domain.Models;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        Task<UserEntity> FindUsuarioByUsernamaAndPassword(string login, string password);

        Task<UserEntity> FindUsuarioByLogin(string login);

        Task<Data<UserEntity>> FindAllUsuariosAsync(PageParams pageParams);
    }
}
