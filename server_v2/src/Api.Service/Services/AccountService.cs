using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Api.Domain.Repository;
using AutoMapper;
using Domain.Helpers;
using Domain.Interfaces.Services.User;

namespace Api.Service.Services
{
    public class AccountService : IAccountService
    {
        private IUserService _userService;
        private IAccountRepository _repository;
        private IMapper _mapper;
        public AccountService(IUserService userService,
                              IAccountRepository repository,
                              IMapper mapper)
        {
            _userService = userService;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AccountModel> GetById(int id)
        {
            var user = await _userService.GetLoggedUser();
            var entity = await _repository.SelectByIdAsync(user.Id, id);

            if (entity == null)
                throw new Exception("Conta não encontrada.");

            return _mapper.Map<AccountModel>(entity);
        }

        public async Task<PageList<AccountModel>> Get(PageParams pageParams)
        {
            var user = await _userService.GetLoggedUser();
            var data = await _repository.SelectByParamAsync(user.Id, pageParams);
            var itens = _mapper.Map<List<AccountModel>>(data.Itens);

            return PageList<AccountModel>.Create(pageParams, itens, data.Count);
        }

        public async Task<AccountModel> Post(AccountModel model)
        {
            var user = await _userService.GetLoggedUser();
            var categoryEntityAux = await _repository.SelectByUkAsync(user.Id, model.Name, model.Status);

            if (categoryEntityAux != null)
                throw new Exception("Conta não disponível.");

            model.User = user;
            model.UserId = user.Id;
            var accountEntity = _mapper.Map<AccountEntity>(model);
            _repository.UnchangedParentAccount(accountEntity);
            accountEntity = await _repository.InsertAsync(accountEntity);

            model = _mapper.Map<AccountModel>(accountEntity);

            return model;
        }

        public async Task<AccountModel> Put(AccountModel model)
        {
            var user = await _userService.GetLoggedUser();
            var accountEntityAux = await _repository.SelectByUkAsync(user.Id, model.Name, model.Status);

            if (accountEntityAux != null && model.Id != accountEntityAux.Id)
                throw new Exception("Conta não disponível.");

            accountEntityAux = await _repository.SelectByIdAsync(user.Id, model.Id);

            if (accountEntityAux == null)
                throw new Exception("Conta não encontrada.");
            
            model.User = user;
            model.UserId = user.Id;
            var accountEntity = _mapper.Map<AccountEntity>(model);
            _repository.UnchangedParentAccount(accountEntity);
            accountEntity = await _repository.UpdateAsync(accountEntity);

            return _mapper.Map<AccountModel>(accountEntity);
        }
        public async Task<bool> Delete(int id)
        {
            var user = await _userService.GetLoggedUser();
            var accountEntityAux = await _repository.SelectByIdAsync(user.Id, id);

            if (accountEntityAux == null)
                throw new Exception("Conta não encontrada.");

            return await _repository.DeleteAsync(id);
        }
    }
}