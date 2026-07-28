using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Api.Domain.Repository;
using AutoMapper;
using Domain.Helpers;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Service.Services;
using Domain.Types;

namespace Api.Service.Services
{
    public class PortfolioAttributeService : BaseService, IPortfolioAttributeService
    {
        private IPortfolioAttributeRepository _repository;

        public PortfolioAttributeService(IUserService userService,
                                IPortfolioAttributeRepository repository,
                                ITrashService trashService,
                                IMapper mapper) : base(trashService, userService, mapper)
        {
            _repository = repository;
        }

        public async Task<PortfolioAttributeModel> GetById(int id)
        {
            var user = await userService.GetLoggedUser();
            var entity = await _repository.SelectByIdAsync(user.Id, id);

            if (entity == null)
                throw new Exception("Portfolio Attribute não encontrada.");

            return mapper.Map<PortfolioAttributeModel>(entity);
        }

        public async Task<PortfolioAttributeModel> GetByPortfolioAndAttributeAsync(int portfolioId, int attributeId)
        {
            var user = await userService.GetLoggedUser();
            var entity = await _repository.SelectByPortfolioAndAttributeAsync(portfolioId, attributeId);

            if (entity == null)
                throw new Exception("Portfolio Attribute não encontrada.");

            return mapper.Map<PortfolioAttributeModel>(entity);
        }

        public async Task<IEnumerable<PortfolioAttributeModel>> GetByPortfolioAsync(int portfolioId)
        {
            var user = await userService.GetLoggedUser();
            var entities = await _repository.SelectByPortfolioAsync(portfolioId);

            return mapper.Map<IEnumerable<PortfolioAttributeModel>>(entities);
        }

        public async Task<PageList<PortfolioAttributeModel>> Get(PageParams pageParams)
        {
            var user = await userService.GetLoggedUser();
            var data = await _repository.SelectByParamAsync(user.Id, pageParams);
            var itens = mapper.Map<List<PortfolioAttributeModel>>(data.Itens);

            return PageList<PortfolioAttributeModel>.Create(pageParams, itens, data.Count);
        }

        public async Task<PortfolioAttributeModel> Post(PortfolioAttributeModel model)
        {
            var user = await userService.GetLoggedUser();
            var existingEntity = await _repository.SelectByPortfolioAndAttributeAsync(model.PortfolioId, model.AttributeId);

            if (existingEntity != null)
                throw new Exception("Já existe um atributo com essa combinação de Portfolio e Attribute.");

            var paEntity = mapper.Map<PortfolioAttributeEntity>(model);
            _repository.UnchangedParentPortfolioAttribute(paEntity);
            paEntity = await _repository.InsertAsync(paEntity);

            model = mapper.Map<PortfolioAttributeModel>(paEntity);

            return model;
        }

        public async Task<PortfolioAttributeModel> Put(PortfolioAttributeModel model)
        {
            var user = await userService.GetLoggedUser();
            var paEntityAux = await _repository.SelectByIdAsync(user.Id, model.Id);

            if (paEntityAux == null)
                throw new Exception("Portfolio Attribute não encontrada.");

            var existingEntity = await _repository.SelectByPortfolioAndAttributeAsync(model.PortfolioId, model.AttributeId);
            if (existingEntity != null && existingEntity.Id != model.Id)
                throw new Exception("Já existe um atributo com essa combinação de Portfolio e Attribute.");
            
            var paEntity = mapper.Map<PortfolioAttributeEntity>(model);
            _repository.UnchangedParentPortfolioAttribute(paEntity);
            paEntity = await _repository.UpdateAsync(paEntity);

            return mapper.Map<PortfolioAttributeModel>(paEntity);
        }

        public async Task<bool> Delete(int id)
        {
            var user = await userService.GetLoggedUser();
            var paEntityAux = await _repository.SelectByIdAsync(user.Id, id);

            if (paEntityAux == null)
                throw new Exception("Portfolio Attribute não encontrada.");

            var result = await _repository.DeleteAsync(id);
            if (result)
                await ProcessExcludeEntityAsync(EntitiesNames.PortfolioAttribute, id);

            return result;
        }
    }
}