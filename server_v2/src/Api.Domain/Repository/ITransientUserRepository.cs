using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repository
{
    public interface ITransientUserRepository
    {
        Task<TransientUserEntity> InsertAsync(TransientUserEntity item);
        Task<TransientUserEntity> UpdateAsync(TransientUserEntity item);
        Task<bool> DeleteAsync(int id);
        Task<TransientUserEntity> SelectByIdAsync(int id);
        Task<TransientUserEntity> SelectUsuarioByLogin(string login);
    }
}