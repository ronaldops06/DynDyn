using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces.Services.User
{
    public interface ITransientUserService
    {
        Task<TransientUserModel> ExecuteVerificationCode(string login, int verificationCode);
        Task<TransientUserModel> Post(TransientUserModel user);
        Task<TransientUserModel> PostLoginPasswordRecovery(string login);
        Task<TransientUserModel> ExecuteVerificationCodeChangePassword(string login, int verificationCode);
        Task<TransientUserModel> ExecutePasswordRecreation(string login, string verificationToken, string password);
        Task<TransientUserModel> Put(TransientUserModel user);  

        Task<bool> Delete(int id);
    }
}