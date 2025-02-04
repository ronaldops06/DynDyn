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

namespace Api.Service.Services
{
    public class OperationService : IOperationService
    {
        private IUserService _userService;
        private IOperationRepository _repository;
        private IMapper _mapper;
        public OperationService(IUserService userService,
                                IOperationRepository repository,
                                IMapper mapper)
        {
            _userService = userService;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationModel> GetById(int id)
        {
            var user = await _userService.GetLoggedUser();
            var entity = await _repository.SelectByIdAsync(user.Id, id);

            if (entity == null)
                throw new Exception("Operação não encontrada.");

            return _mapper.Map<OperationModel>(entity);
        }

        public async Task<PageList<OperationModel>> Get(PageParams pageParams)
        {
            var user = await _userService.GetLoggedUser();
            var data = await _repository.SelectByParamAsync(user.Id, pageParams);
            var itens = _mapper.Map<List<OperationModel>>(data.Itens);

            return PageList<OperationModel>.Create(pageParams, itens, data.Count);
        }

        public async Task<OperationModel> Post(OperationModel model)
        {
            var user = await _userService.GetLoggedUser();
            var categoryEntityAux = await _repository.SelectByUkAsync(user.Id, model.Name, model.Type);

            if (categoryEntityAux != null)
                throw new Exception("Operação não disponível.");

            model.User = user;
            model.UserId = user.Id;
            var operationEntity = _mapper.Map<OperationEntity>(model);
            _repository.UnchangedParentOperation(operationEntity);
            operationEntity = await _repository.InsertAsync(operationEntity);

            model = _mapper.Map<OperationModel>(operationEntity);

            return model;
        }

        public async Task<OperationModel> Put(OperationModel model)
        {
            var user = await _userService.GetLoggedUser();
            var operationEntityAux = await _repository.SelectByUkAsync(user.Id, model.Name, model.Type);

            if (operationEntityAux != null && model.Id != operationEntityAux.Id)
                throw new Exception("Operação não disponível.");

            operationEntityAux = await _repository.SelectByIdAsync(user.Id, model.Id);

            if (operationEntityAux == null)
                throw new Exception("Operação não encontrada.");
            
            model.User = user;
            model.UserId = user.Id;
            var operationEntity = _mapper.Map<OperationEntity>(model);
            _repository.UnchangedParentOperation(operationEntity);
            operationEntity = await _repository.UpdateAsync(operationEntity);

            return _mapper.Map<OperationModel>(operationEntity);
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _userService.GetLoggedUser();
            var operationEntityAux = await _repository.SelectByIdAsync(user.Id, id);

            if (operationEntityAux == null)
                throw new Exception("Operação não encontrada.");

            return await _repository.DeleteAsync(id);
        }

        public async Task<OperationModel> GetByNameAndType(string name, OperationType type)
        {
            var user = await _userService.GetLoggedUser();
            var entity = await _repository.SelectByUkAsync(user.Id, name, type);

            return _mapper.Map<OperationModel>(entity);
        }
    }
}