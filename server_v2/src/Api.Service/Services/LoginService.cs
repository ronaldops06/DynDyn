using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Domain.Repository;
using Domain.Security;
using Microsoft.IdentityModel.Tokens;

namespace Service.Services
{
    public class LoginService : ILoginService
    {
        private IUserRepository _repository;
        private SigningConfiguration _signingConfiguration;
        private IMapper _mapper;

        public LoginService(IUserRepository repository,
                            SigningConfiguration signingConfiguration,                            
                            IMapper mapper)
        {
            _repository = repository;
            _signingConfiguration = signingConfiguration;            
            _mapper = mapper;
        }

        public async Task<TransientUserModel> GetLoginAsync(TransientUserModel userModel)
        {
            var userEntity = await _repository.FindUsuarioByUsernamaAndPassword(userModel.Login, userModel.Password);
            if (userEntity == null)
                throw new Exception("Usuário ou senha inválidos");

            userModel = _mapper.Map<TransientUserModel>(userEntity);

            var token = GenerateToken(userModel);
            userModel.AccessToken = token;

             return userModel;
        }

        public string GenerateToken(TransientUserModel userModel)
        {
            var identity = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.SerialNumber, userModel.Id.ToString()),
                        new Claim(ClaimTypes.Name, userModel.Login.ToString())
                    });

            DateTimeOffset createDate = DateTimeOffset.UtcNow;
            DateTimeOffset expirationDate = createDate + TimeSpan.FromSeconds(Convert.ToInt32(Environment.GetEnvironmentVariable("Seconds")));
            var handler = new JwtSecurityTokenHandler();

            return CreateToken(identity, createDate, expirationDate, handler);
        }

        private string CreateToken(ClaimsIdentity identity, DateTimeOffset createDate, DateTimeOffset expirationDate, JwtSecurityTokenHandler handler)
        {
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = Environment.GetEnvironmentVariable("Issuer"),
                Audience = Environment.GetEnvironmentVariable("Audience"),
                SigningCredentials = _signingConfiguration.SigningCredentials,
                Subject = identity,
                IssuedAt = createDate.DateTime,
                Expires = expirationDate.DateTime,
            });

            var token = handler.WriteToken(securityToken);
            return token;
        }
    }
}
