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
using Domain.Types;
using Newtonsoft.Json;
using Service.Services;

namespace Api.Service.Services
{
    public class AttributeService : BaseService, IAttributeService
    {
        private IAttributeRepository _repository;

        public AttributeService(IUserService userService,
                                IAttributeRepository repository,
                                ITrashService trashService,
                                IMapper mapper) : base(trashService, userService, mapper)
        {
            _repository = repository;
        }

        public async Task<AttributeModel> GetById(int id)
        {
            var user = await userService.GetLoggedUser();
            var entity = await _repository.SelectByIdAsync(user.Id, id);

            if (entity == null)
                throw new Exception("Atributo não encontrado.");

            return mapper.Map<AttributeModel>(entity);
        }

        public async Task<PageList<AttributeModel>> Get(PageParams pageParams)
        {
            var user = await userService.GetLoggedUser();
            var data = await _repository.SelectByParamAsync(user.Id, pageParams);
            var itens = mapper.Map<List<AttributeModel>>(data.Itens);

            return PageList<AttributeModel>.Create(pageParams, itens, data.Count);
        }

        public async Task<AttributeModel> Post(AttributeModel model)
        {
            ValidateAttributeListOption(model);
            
            var user = await userService.GetLoggedUser();
            var attributeEntityAux = await _repository.SelectByUkAsync(user.Id, model.Name);

            if (attributeEntityAux != null)
                throw new Exception("Atributo não disponível.");

            model.User = user;
            model.UserId = user.Id;
            foreach (var option in model.Options)
            {
                option.User = null;
                option.UserId = user.Id;
            }
            
            var attributeEntity = mapper.Map<AttributeEntity>(model);

            _repository.UnchangedParentAttribute(attributeEntity);
            attributeEntity = await _repository.InsertAsync(attributeEntity);

            model = mapper.Map<AttributeModel>(attributeEntity);

            return model;
        }

        public async Task<AttributeModel> Put(AttributeModel model)
        {
            ValidateAttributeListOption(model);
            
            var user = await userService.GetLoggedUser();
            var attributeEntityAux = await _repository.SelectByUkAsync(user.Id, model.Name);

            if (attributeEntityAux != null && model.Id != attributeEntityAux.Id)
                throw new Exception("Atributo não disponível.");

            attributeEntityAux = await _repository.SelectByIdAsync(user.Id, model.Id);

            if (attributeEntityAux == null)
                throw new Exception("Atributo não encontrado.");
            
            model.User = user;
            model.UserId = user.Id;
            foreach (var option in model.Options)
            {
                option.User = null;
                option.UserId = user.Id;
            }
            
            var attributeEntity = mapper.Map<AttributeEntity>(model);
            _repository.UnchangedParentAttribute(attributeEntity);
            attributeEntity = await _repository.UpdateAsync(attributeEntity);

            return mapper.Map<AttributeModel>(attributeEntity);
        }
        
        public async Task<bool> Delete(int id)
        {
            var user = await userService.GetLoggedUser();
            var attributeEntityAux = await _repository.SelectByIdAsync(user.Id, id);

            if (attributeEntityAux == null)
                throw new Exception("Atributo não encontrado.");

            var result = await _repository.DeleteAsync(id);
            if (result)
                await ProcessExcludeEntityAsync(EntitiesNames.Attribute, id);

            return result;
        }

        private void ValidateAttributeListOption(AttributeModel model)
        {
            if (model.DataType == AttributeDataType.ListOptions){
                if (model.Options == null || model.Options.Count == 0)
                    throw new Exception("Não foi informado a(s) opção(oẽs) para o atributo do tipo 'lista de opção'.");

                if (model.Options?.FindAll(x => x.IsDefault && x.Status == StatusType.Ativo).Count > 1)
                    throw new Exception("O atributo do tipo 'lista de opções' só pode ter uma opção default ativa.");
            }
            
            if (model.DataType != AttributeDataType.ListOptions && (model.Options != null && model.Options.Count > 0))
                throw new Exception("Foi informado opção(oẽs) para um atributo que não é do tipo 'lista de opção'.");
        }
    }
}
