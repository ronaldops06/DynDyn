using System.Threading;
using System.Threading.Tasks;

namespace Worker.Service.Interfaces
{
    public interface IWorkerTask
    {
        Task ExecuteAsync();
    }
}