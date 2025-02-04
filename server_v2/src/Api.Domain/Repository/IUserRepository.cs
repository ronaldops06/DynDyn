using System.Collections.Generic;
using Domain.Entities;
using Domain.Interfaces;
using System.Threading.Tasks;
using Domain.Helpers;
using Domain.Models;

namespace Domain.Repository
{
    public interface IUserRepository
    {
        Task<UserEntity> InsertAsync(UserEntity item);
        Task<UserEntity> UpdateAsync(UserEntity item);
        Task<bool> DeleteAsync(int id);
        Task<UserEntity> SelectByIdAsync(int id);
        Task<IEnumerable<UserEntity>> SelectAsync();
        Task<Data<UserEntity>> SelectByParamAsync(PageParams pageParams);
        
        Task<UserEntity> FindUsuarioByUsernamaAndPassword(string login, string password);

        Task<UserEntity> FindUsuarioByLogin(string login);
    }
}
