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
    public class {{model}}Service : BaseService, I{{model}}Service
    {
        private I{{model}}Repository _repository;

        public {{model}}Service(IUserService userService,
                                I{{model}}Repository repository,
                                ITrashService trashService,
                                IMapper mapper) : base(trashService, userService, mapper)
        {
            _repository = repository;
        }

        public async Task<{{model}}Model> GetById(int id)
        {
            var user = await userService.GetLoggedUser();
            var entity = await _repository.SelectByIdAsync(user.Id, id);

            if (entity == null)
                throw new Exception("{{name}} não encontrada.");

            return mapper.Map<{{model}}Model>(entity);
        }

        public async Task<PageList<{{model}}Model>> Get(PageParams pageParams)
        {
            var user = await userService.GetLoggedUser();
            var data = await _repository.SelectByParamAsync(user.Id, pageParams);
            var itens = mapper.Map<List<{{model}}Model>>(data.Itens);

            return PageList<{{model}}Model>.Create(pageParams, itens, data.Count);
        }

        public async Task<{{model}}Model> Post({{model}}Model model)
        {
            var user = await userService.GetLoggedUser();
            var {{alias}}EntityAux = await _repository.SelectByUkAsync(user.Id);

            if ({{alias}}EntityAux != null)
                throw new Exception("{{name}} não disponível.");

            model.User = user;
            model.UserId = user.Id;
            var {{alias}}Entity = mapper.Map<{{model}}Entity>(model);
            _repository.UnchangedParent{{model}}({{alias}}Entity);
            {{alias}}Entity = await _repository.InsertAsync({{alias}}Entity);

            model = mapper.Map<{{model}}Model>({{alias}}Entity);

            return model;
        }

        public async Task<{{model}}Model> Put({{model}}Model model)
        {
            var user = await userService.GetLoggedUser();
            var {{alias}}EntityAux = await _repository.SelectByUkAsync(user.Id);

            if ({{alias}}EntityAux != null && model.Id != {{alias}}EntityAux.Id)
                throw new Exception("{{name}} não disponível.");

            {{alias}}EntityAux = await _repository.SelectByIdAsync(user.Id, model.Id);

            if ({{alias}}EntityAux == null)
                throw new Exception("{{name}} não encontrada.");
            
            model.User = user;
            model.UserId = user.Id;
            var {{alias}}Entity = mapper.Map<{{model}}Entity>(model);
            _repository.UnchangedParent{{model}}({{alias}}Entity);
            {{alias}}Entity = await _repository.UpdateAsync({{alias}}Entity);

            return mapper.Map<{{model}}Model>({{alias}}Entity);
        }

        public async Task<bool> Delete(int id)
        {
            var user = await userService.GetLoggedUser();
            var {{alias}}EntityAux = await _repository.SelectByIdAsync(user.Id, id);

            if ({{alias}}EntityAux == null)
                throw new Exception("{{name}} não encontrada.");

            var result = await _repository.DeleteAsync(id);
            if (result)
                await ProcessExcludeEntityAsync(EntitiesNames.{{model}}, id);

            return result;
        }
    }
}