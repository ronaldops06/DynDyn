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
using Service.Services;
using Domain.Types;

namespace Api.Service.Services
{
    public class PortfolioService : BaseService, IPortfolioService
    {
        private IPortfolioRepository _repository;
        
        public PortfolioService(IUserService userService,
                              IPortfolioRepository repository,
                              ITrashService trashService,
                              IMapper mapper) : base(trashService, userService, mapper)
        {
            _repository = repository;
        }

        public async Task<PortfolioModel> GetById(int id)
        {
            var user = await userService.GetLoggedUser();
            var entity = await _repository.SelectByIdAsync(user.Id, id);

            if (entity == null)
                throw new Exception("Conta não encontrada.");

            return mapper.Map<PortfolioModel>(entity);
        }

        public async Task<PageList<PortfolioModel>> Get(PageParams pageParams)
        {
            var user = await userService.GetLoggedUser();
            var data = await _repository.SelectByParamAsync(user.Id, pageParams);
            var itens = mapper.Map<List<PortfolioModel>>(data.Itens);

            return PageList<PortfolioModel>.Create(pageParams, itens, data.Count);
        }

        public async Task<PortfolioModel> Post(PortfolioModel model)
        {
            var user = await userService.GetLoggedUser();
            var categoryEntityAux = await _repository.SelectByUkAsync(user.Id, model.Name, model.Status);

            if (categoryEntityAux != null)
                throw new Exception("Conta não disponível.");

            model.User = user;
            model.UserId = user.Id;
            var accountEntity = mapper.Map<PortfolioEntity>(model);
            _repository.UnchangedParentAccount(accountEntity);
            accountEntity = await _repository.InsertAsync(accountEntity);

            model = mapper.Map<PortfolioModel>(accountEntity);

            return model;
        }

        public async Task<PortfolioModel> Put(PortfolioModel model)
        {
            var user = await userService.GetLoggedUser();
            var accountEntityAux = await _repository.SelectByUkAsync(user.Id, model.Name, model.Status);

            if (accountEntityAux != null && model.Id != accountEntityAux.Id)
                throw new Exception("Conta não disponível.");

            accountEntityAux = await _repository.SelectByIdAsync(user.Id, model.Id);

            if (accountEntityAux == null)
                throw new Exception("Conta não encontrada.");
            
            model.User = user;
            model.UserId = user.Id;
            var accountEntity = mapper.Map<PortfolioEntity>(model);
            _repository.UnchangedParentAccount(accountEntity);
            accountEntity = await _repository.UpdateAsync(accountEntity);

            return mapper.Map<PortfolioModel>(accountEntity);
        }
        public async Task<bool> Delete(int id)
        {
            var user = await userService.GetLoggedUser();
            var accountEntityAux = await _repository.SelectByIdAsync(user.Id, id);

            if (accountEntityAux == null)
                throw new Exception("Conta não encontrada.");

            var result = await _repository.DeleteAsync(id);
            if (result)
                await ProcessExcludeEntityAsync(EntitiesNames.Portfolio, id);

            return result;
        }
    }
}