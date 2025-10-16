using System.Threading.Tasks;

namespace Api.Domain.Interfaces.Services
{
    public interface ICleanupService
    {
        Task DeleteAllAsync();
    }
}