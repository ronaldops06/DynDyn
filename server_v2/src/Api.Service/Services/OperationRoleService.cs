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
    public class OperationRoleService : BaseService, IOperationRoleService
    {
        private IOperationRoleRepository _repository;

        public OperationRoleService(IUserService userService,
                                IOperationRoleRepository repository,
                                ITrashService trashService,
                                IMapper mapper) : base(trashService, userService, mapper)
        {
            _repository = repository;
        }

        public async Task<OperationRoleModel> GetById(int id)
        {
            var user = await userService.GetLoggedUser();
            var entity = await _repository.SelectByIdAsync(user.Id, id);

            if (entity == null)
                throw new Exception("Papel de operação não encontrado.");

            return mapper.Map<OperationRoleModel>(entity);
        }
        
        public async Task<PageList<OperationRoleModel>> Get(PageParams pageParams)
        {
            var user = await userService.GetLoggedUser();
            var data = await _repository.SelectByParamAsync(user.Id, pageParams);
            var itens = mapper.Map<List<OperationRoleModel>>(data.Itens);

            return PageList<OperationRoleModel>.Create(pageParams, itens, data.Count);
        }

        public async Task<OperationRoleModel> Post(OperationRoleModel model)
        {
            var user = await userService.GetLoggedUser();
            var entityAux = await _repository.SelectByUkAsync(user.Id, model.Name);

            if (entityAux != null)
                throw new Exception("Papel de operação não disponível.");

            model.User = user;
            model.UserId = user.Id;
            var entity = mapper.Map<OperationRoleEntity>(model);
            _repository.UnchangedParentOperationRole(entity);
            entity = await _repository.InsertAsync(entity);

            model = mapper.Map<OperationRoleModel>(entity);

            return model;
        }

        public async Task<OperationRoleModel> Put(OperationRoleModel model)
        {
            var user = await userService.GetLoggedUser();
            var entityAux = await _repository.SelectByUkAsync(user.Id, model.Name);

            if (entityAux != null && model.Id != entityAux.Id)
                throw new Exception("Papel de operação não disponível.");

            entityAux = await _repository.SelectByIdAsync(user.Id, model.Id);

            if (entityAux == null)
                throw new Exception("Papel de operação não encontrado.");
            
            model.User = user;
            model.UserId = user.Id;
            
            var entity = mapper.Map<OperationRoleEntity>(model);
            _repository.UnchangedParentOperationRole(entity);
            entity = await _repository.UpdateAsync(entity);

            return mapper.Map<OperationRoleModel>(entity);
        }

        public async Task<bool> Delete(int id)
        {
            var user = await userService.GetLoggedUser();
            var entityAux = await _repository.SelectByIdAsync(user.Id, id);

            if (entityAux == null)
                throw new Exception("Papel de operação não encontrado.");

            var result = await _repository.DeleteAsync(id);
            if (result)
                await ProcessExcludeEntityAsync(EntitiesNames.OperationRole, id);

            return result;
        }
        
        public Task<OperationRoleModel> GenerateInitialByUser(UserModel user)
        {
            throw new NotImplementedException();
        }
    }
}