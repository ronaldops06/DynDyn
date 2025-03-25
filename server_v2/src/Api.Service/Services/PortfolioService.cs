using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Interfaces.Services;
using Api.Domain.Repository;
using AutoMapper;
using Domain.Entities;
using Domain.Helpers;
using Domain.Interfaces.Services.User;
using Domain.Models;

namespace Api.Service.Services
{
    public class PortfolioService : IPortfolioService
    {
        private IUserService _userService;
        private IPortfolioRepository _repository;
        private IMapper _mapper;
        public PortfolioService(IUserService userService,
                              IPortfolioRepository repository,
                              IMapper mapper)
        {
            _userService = userService;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PortfolioModel> GetById(int id)
        {
            var user = await _userService.GetLoggedUser();
            var entity = await _repository.SelectByIdAsync(user.Id, id);

            if (entity == null)
                throw new Exception("Conta não encontrada.");

            return _mapper.Map<PortfolioModel>(entity);
        }

        public async Task<PageList<PortfolioModel>> Get(PageParams pageParams)
        {
            var user = await _userService.GetLoggedUser();
            var data = await _repository.SelectByParamAsync(user.Id, pageParams);
            var itens = _mapper.Map<List<PortfolioModel>>(data.Itens);

            return PageList<PortfolioModel>.Create(pageParams, itens, data.Count);
        }

        public async Task<PortfolioModel> Post(PortfolioModel model)
        {
            var user = await _userService.GetLoggedUser();
            var categoryEntityAux = await _repository.SelectByUkAsync(user.Id, model.Name, model.Status);

            if (categoryEntityAux != null)
                throw new Exception("Conta não disponível.");

            model.User = user;
            model.UserId = user.Id;
            var accountEntity = _mapper.Map<PortfolioEntity>(model);
            _repository.UnchangedParentAccount(accountEntity);
            accountEntity = await _repository.InsertAsync(accountEntity);

            model = _mapper.Map<PortfolioModel>(accountEntity);

            return model;
        }

        public async Task<PortfolioModel> Put(PortfolioModel model)
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
            var accountEntity = _mapper.Map<PortfolioEntity>(model);
            _repository.UnchangedParentAccount(accountEntity);
            accountEntity = await _repository.UpdateAsync(accountEntity);

            return _mapper.Map<PortfolioModel>(accountEntity);
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