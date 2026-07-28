using System.Threading.Tasks;

namespace Api.Domain.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendAsync( string email, string name, string subject, string html);
    }
}