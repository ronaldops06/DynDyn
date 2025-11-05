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
using Service.Services;
using Domain.Types;

namespace Api.Service.Services
{
    public class OperationService : BaseService, IOperationService
    {
        private IOperationRepository _repository;

        public OperationService(IUserService userService,
                                IOperationRepository repository,
                                ITrashService trashService,
                                IMapper mapper) : base(trashService, userService, mapper)
        {
            _repository = repository;
        }

        public async Task<OperationModel> GetById(int id)
        {
            var user = await userService.GetLoggedUser();
            var entity = await _repository.SelectByIdAsync(user.Id, id);

            if (entity == null)
                throw new Exception("Operação não encontrada.");

            return mapper.Map<OperationModel>(entity);
        }

        public async Task<PageList<OperationModel>> Get(PageParams pageParams)
        {
            var user = await userService.GetLoggedUser();
            var data = await _repository.SelectByParamAsync(user.Id, pageParams);
            var itens = mapper.Map<List<OperationModel>>(data.Itens);

            return PageList<OperationModel>.Create(pageParams, itens, data.Count);
        }

        public async Task<OperationModel> Post(OperationModel model)
        {
            var user = await userService.GetLoggedUser();
            var categoryEntityAux = await _repository.SelectByUkAsync(user.Id, model.Name, model.Type);

            if (categoryEntityAux != null)
                throw new Exception("Operação não disponível.");

            model.User = user;
            model.UserId = user.Id;
            var operationEntity = mapper.Map<OperationEntity>(model);
            _repository.UnchangedParentOperation(operationEntity);
            operationEntity = await _repository.InsertAsync(operationEntity);

            model = mapper.Map<OperationModel>(operationEntity);

            return model;
        }

        public async Task<OperationModel> Put(OperationModel model)
        {
            var user = await userService.GetLoggedUser();
            var operationEntityAux = await _repository.SelectByUkAsync(user.Id, model.Name, model.Type);

            if (operationEntityAux != null && model.Id != operationEntityAux.Id)
                throw new Exception("Operação não disponível.");

            operationEntityAux = await _repository.SelectByIdAsync(user.Id, model.Id);

            if (operationEntityAux == null)
                throw new Exception("Operação não encontrada.");
            
            model.User = user;
            model.UserId = user.Id;
            var operationEntity = mapper.Map<OperationEntity>(model);
            _repository.UnchangedParentOperation(operationEntity);
            operationEntity = await _repository.UpdateAsync(operationEntity);

            return mapper.Map<OperationModel>(operationEntity);
        }

        public async Task<bool> Delete(int id)
        {
            var user = await userService.GetLoggedUser();
            var operationEntityAux = await _repository.SelectByIdAsync(user.Id, id);

            if (operationEntityAux == null)
                throw new Exception("Operação não encontrada.");

            var result = await _repository.DeleteAsync(id);
            if (result)
                await ProcessExcludeEntityAsync(EntitiesNames.Operation, id);

            return result;
        }

        public async Task<OperationModel> GetByNameAndType(string name, OperationType type)
        {
            var user = await userService.GetLoggedUser();
            var entity = await _repository.SelectByUkAsync(user.Id, name, type);

            return mapper.Map<OperationModel>(entity);
        }

        public async Task<List<OperationModel>> GetByActiveAndRecurrent()
        {
            var user = await userService.GetLoggedUser();
            var entities = await _repository.SelectByActiveAndRecurrent(user.Id);

            return mapper.Map<List<OperationModel>>(entities);
        }
    }
}