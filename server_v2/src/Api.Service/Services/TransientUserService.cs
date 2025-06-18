using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Domain.Repository;

namespace Service.Services
{
    public class TransientUserService : ITransientUserService
    {
        private static readonly int EXPIRATION_DATE_FIFTEEN_MINUTES = 15;
        private static readonly string EMAIL_FROM = "no-reply@sagemoney.com.br";
        private ITransientUserRepository _repository;
        private IUserService _userService;
        private ILoginService _loginService;
        private readonly IMapper _mapper;

        public TransientUserService(ITransientUserRepository repository,
                                  IUserService userService,
                                  ILoginService loginService,
                                  IMapper mapper)
        {
            _repository = repository;
            _userService = userService;
            _loginService = loginService;
            _mapper = mapper;
        }
        
        public async Task<TransientUserModel> ExecuteVerificationCode(string login, int verificationCode)
        {
            var userEntityAux = await _repository.SelectUsuarioByLogin(login);
            
            if (userEntityAux == null)
                throw new Exception("Usuário não encontrado para validação, reinicie o cadastro.");
            
            if (userEntityAux.VerificationCode != verificationCode)
                throw new Exception("O código não corresponde ao código de verificação.");

            if (userEntityAux.ExpirationDate < DateTime.Now)
                throw new Exception("Código de validação expirado, reinicie o cadastro.");
            
            var userModel = _mapper.Map<UserModel>(userEntityAux);
            userModel = await _userService.Post(userModel);
            
            var transientUserModel = _mapper.Map<TransientUserModel>(userModel);
            transientUserModel.AccessToken = _loginService.GenerateToken(transientUserModel);

            var result = await Delete(userEntityAux.Id);

            return transientUserModel;
        }

        public async Task<TransientUserModel> Post(TransientUserModel userModel)
        {
            var userEntityAux = await _userService.GetUsuarioByLogin(userModel.Login);

            if (userEntityAux != null)
                throw new Exception("Usuário não disponível.");

            userModel.VerificationCode = new Random().Next(100000, 999999);
            userModel.ExpirationDate = DateTime.Now.AddMinutes(EXPIRATION_DATE_FIFTEEN_MINUTES);
            
            var userEntity = _mapper.Map<TransientUserEntity>(userModel);

            var transientUserEntityAux = await _repository.SelectUsuarioByLogin(userModel.Login);

            if (transientUserEntityAux == null)
            {
                userEntity = await _repository.InsertAsync(userEntity);
            }
            else
            {
                userEntity.Id = transientUserEntityAux.Id;
                userEntity = await _repository.UpdateAsync(userEntity);
            }
            
            userModel = _mapper.Map<TransientUserModel>(userEntity);

            await SendMailVerification(userModel.Login, userModel.VerificationCode);

            return userModel;
        }

        public async Task<TransientUserModel> Put(TransientUserModel userModel)
        {
            var userEntity = _mapper.Map<TransientUserEntity>(userModel);
            userEntity = await _repository.UpdateAsync(userEntity);

            return _mapper.Map<TransientUserModel>(userEntity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _repository.DeleteAsync(id);
        }
        
        private async Task SendMailVerification(string emailTo, int? verificationCode)
        {
            var smtpUser = Environment.GetEnvironmentVariable("SMTP_USER");
            var smtpPass =  Environment.GetEnvironmentVariable("SMTP_PASS");

            try
            {
                var client = new SmtpClient("email-smtp.sa-east-1.amazonaws.com", 587)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = true
                };
                
                var assembly = Assembly.GetExecutingAssembly();
                using var stream = assembly.GetManifestResourceStream("Service.Templates.VerificationEmail.html");
                using var reader = new StreamReader(stream!);
                string htmlTemplate = reader.ReadToEnd();
                string htmlContent = htmlTemplate.Replace("{{CODE}}", verificationCode.ToString());
                
                var message = new MailMessage();
                message.IsBodyHtml = true;
                message.From = new MailAddress(EMAIL_FROM);
                message.To.Add(emailTo);
                message.Subject = "Código de verificação - SageMoney";
                message.Body = htmlContent;
                
                client.Send(message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao enviar e-mail: {ex.Message}");
            }
        }
    }
}