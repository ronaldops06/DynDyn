using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Domain.Interfaces.Services
{
    public interface INotificationService
    {
        Task SendMulticastMessageAsync(IEnumerable<string> targetTokens, string title, string body);
        
        Task SendMessageAsync(string targetToken, string title, string body);
    }
}