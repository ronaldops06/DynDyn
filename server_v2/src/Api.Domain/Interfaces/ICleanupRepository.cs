using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICleanupRepository
    {
        int CleanupOrder { get; }
        Task<bool> DeleteAllByUserAsync(int userId);
    }
}