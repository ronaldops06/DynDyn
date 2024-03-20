using Domain.Entities;
using Domain.Helpers;
using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> InsertAsync(T item);
        Task<T> UpdateAsync(T item);
        Task<bool> DeleteAsync(int id);
        Task<T> SelectByIdAsync(int id);
        Task<IEnumerable<T>> SelectAsync();
        Task<Data<T>> SelectByParamAsync(PageParams pageParams);
        Task<bool> ExistsAsync(int id);
    }
}
