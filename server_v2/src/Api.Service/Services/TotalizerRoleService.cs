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
using Newtonsoft.Json;

namespace Api.Service.Services
{
    public class TotalizerRoleService : BaseService, ITotalizerRoleService
    {
        private ITotalizerRoleRepository _repository;

        public TotalizerRoleService(IUserService userService,
                                ITotalizerRoleRepository repository,
                                ITrashService trashService,
                                IMapper mapper) : base(trashService, userService, mapper)
        {
            _repository = repository;
        }

        public async Task<TotalizerRoleModel> GetById(int id)
        {
            var user = await userService.GetLoggedUser();
            var entity = await _repository.SelectByIdAsync(user.Id, id);

            if (entity == null)
                throw new Exception("papel de totalizador não encontrada.");

            return mapper.Map<TotalizerRoleModel>(entity);
        }

        public async Task<PageList<TotalizerRoleModel>> Get(PageParams pageParams)
        {
            var user = await userService.GetLoggedUser();
            var data = await _repository.SelectByParamAsync(user.Id, pageParams);
            var itens = mapper.Map<List<TotalizerRoleModel>>(data.Itens);

            return PageList<TotalizerRoleModel>.Create(pageParams, itens, data.Count);
        }

        public async Task<TotalizerRoleModel> Post(TotalizerRoleModel model)
        {
            var user = await userService.GetLoggedUser();
            var totalizerRoleEntityAux = await _repository.SelectByUkAsync(user.Id, model.Code, model.Type);

            if (totalizerRoleEntityAux != null)
                throw new Exception("papel de totalizador não disponível.");

            model.User = user;
            model.UserId = user.Id;
            var totalizerRoleEntity = mapper.Map<TotalizerRoleEntity>(model);
            _repository.UnchangedParentTotalizerRole(totalizerRoleEntity);
            totalizerRoleEntity = await _repository.InsertAsync(totalizerRoleEntity);
            
            model = mapper.Map<TotalizerRoleModel>(totalizerRoleEntity);
            Console.WriteLine("model: ");
            Console.WriteLine(JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
            return model;
        }

        public async Task<TotalizerRoleModel> Put(TotalizerRoleModel model)
        {
            var user = await userService.GetLoggedUser();
            var totalizerRoleEntityAux = await _repository.SelectByUkAsync(user.Id, model.Code, model.Type);

            if (totalizerRoleEntityAux != null && model.Id != totalizerRoleEntityAux.Id)
                throw new Exception("Papel de totalizador não disponível.");

            totalizerRoleEntityAux = await _repository.SelectByIdAsync(user.Id, model.Id);

            if (totalizerRoleEntityAux == null)
                throw new Exception("Papel de totalizador não encontrado.");
            
            model.User = user;
            model.UserId = user.Id;
            var totalizerRoleEntity = mapper.Map<TotalizerRoleEntity>(model);
            _repository.UnchangedParentTotalizerRole(totalizerRoleEntity);
            totalizerRoleEntity = await _repository.UpdateAsync(totalizerRoleEntity);

            return mapper.Map<TotalizerRoleModel>(totalizerRoleEntity);
        }

        public async Task<bool> Delete(int id)
        {
            var user = await userService.GetLoggedUser();
            var totalizerRoleEntityAux = await _repository.SelectByIdAsync(user.Id, id);

            if (totalizerRoleEntityAux == null)
                throw new Exception("Papel de totalizador não encontrado.");

            var result = await _repository.DeleteAsync(id);
            if (result)
                await ProcessExcludeEntityAsync(EntitiesNames.TotalizerRole, id);

            return result;
        }
        
        public Task<TotalizerRoleModel> GenerateInitialByUser(UserModel user)
        {
            throw new NotImplementedException();
        }
    }
}