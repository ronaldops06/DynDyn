using AutoMapper;
using Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILoginService _loginService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository repository,
                           IMapper mapper,
                           ILoginService loginService,
                           IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _mapper = mapper;
            _loginService = loginService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserModel> GetLoggedUser()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.SerialNumber)?.Value);
            var entity = await _repository.SelectByIdAsync(userId);
            
            return _mapper.Map<UserModel>(entity);
        }

        public async Task<UserModel> GetUsuarioByUsernameAndPassword(UserModel user)
        {
            var entity = await _repository.FindUsuarioByUsernamaAndPassword(user.Login, user.Password);
            return _mapper.Map<UserModel>(entity);
        }

        public async Task<UserModel> GetUsuarioByLogin(string login)
        {
            var entity = await _repository.FindUsuarioByLogin(login);
            return _mapper.Map<UserModel>(entity);
        }

        public async Task<PageList<UserModel>> Get(PageParams pageParams)
        {
            var data = await _repository.SelectByParamAsync(pageParams);
            var itens = _mapper.Map<List<UserModel>>(data.Itens);

            return PageList<UserModel>.Create(pageParams, itens, data.Count);
        }

        public async Task<UserModel> Post(UserModel userModel)
        {
            var userEntityAux = await _repository.FindUsuarioByLogin(userModel.Login);

            if (userEntityAux != null)
                throw new Exception("Usuário não disponível.");

            var userEntity = _mapper.Map<UserEntity>(userModel);
            userEntity = await _repository.InsertAsync(userEntity);

            userModel = _mapper.Map<UserModel>(userEntity);
            userModel.AccessToken = _loginService.GenerateToken(userModel);

            return userModel;
        }

        public async Task<UserModel> Put(UserModel userModel)
        {
            var userEntity = _mapper.Map<UserEntity>(userModel);
            userEntity = await _repository.UpdateAsync(userEntity);

            return _mapper.Map<UserModel>(userEntity);
        }

        public async Task<bool> Delete(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
