using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Services;
using AutoMapper;
using Domain.Helpers;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Domain.Repository;
using Domain.Types;

namespace Service.Services
{
    public class BalanceService : BaseService, IBalanceService
    {
        private readonly IBalanceRepository _repository;
        
        public BalanceService(IUserService userService,
                              IBalanceRepository repository,
                              ITrashService trashService,
                              IMapper mapper) : base(trashService, userService, mapper)
        {
            _repository = repository;
        }
        
        public async Task<BalanceModel> GetById(int id)
        {
            var user = await userService.GetLoggedUser();
            var entity = await _repository.SelectByIdAsync(user.Id, id);

            if (entity == null)
                throw new Exception("Saldo não encontrado.");

            return mapper.Map<BalanceModel>(entity);
        }
        
        public async Task<PageList<BalanceModel>> Get(PageParams pageParams)
        {
            var user = await userService.GetLoggedUser();
            var data = await _repository.SelectByParamAsync(user.Id, pageParams);
            var itens = mapper.Map<List<BalanceModel>>(data.Itens);

            return PageList<BalanceModel>.Create(pageParams, itens, data.Count);
        }

        public async Task<BalanceModel> Post(BalanceModel model)
        {
            var user = await userService.GetLoggedUser();
            var balanceEntityAux = await _repository.SelectByUkAsync(user.Id, model.PortfolioId, model.Month, model.Year);

            if (balanceEntityAux != null)
                throw new Exception($"Saldo para o mês {model.Month + 1} e ano {model.Year}, conta (ID) {model.PortfolioId} não disponível.");

            model.User = user;
            model.UserId = user.Id;
            var balanceEntity = mapper.Map<BalanceEntity>(model);
            _repository.UnchangedParentBalance(balanceEntity);
            balanceEntity = await _repository.InsertAsync(balanceEntity);

            model = mapper.Map<BalanceModel>(balanceEntity);

            return model;
        }

        public async Task<BalanceModel> Put(BalanceModel model)
        {
            var user = await userService.GetLoggedUser();
            var balanceEntityAux = await _repository.SelectByUkAsync(user.Id, model.PortfolioId, model.Month, model.Year);

            if (balanceEntityAux != null && model.Id != balanceEntityAux.Id)
                throw new Exception($"Saldo para o mês {model.Month + 1} e ano {model.Year}, conta (ID) {model.PortfolioId} não disponível.");

            balanceEntityAux = await _repository.SelectByIdAsync(user.Id, model.Id);

            if (balanceEntityAux == null)
                throw new Exception($"Saldo para o mês {model.Month + 1} e ano {model.Year}, conta (ID) {model.PortfolioId} não encontrado.");

            model.User = user;
            model.UserId = user.Id;
            var balanceEntity = mapper.Map<BalanceEntity>(model);
            _repository.UnchangedParentBalance(balanceEntity);
            balanceEntity = await _repository.UpdateAsync(balanceEntity);

            return mapper.Map<BalanceModel>(balanceEntity);
        }

        public async Task<bool> Delete(int id)
        {
            var user = await userService.GetLoggedUser();
            var balanceEntityAux = await _repository.SelectByIdAsync(user.Id, id);

            if (balanceEntityAux == null)
                throw new Exception("Saldo não encontrado.");

            var result = await _repository.DeleteAsync(id);
            if (result)
                await ProcessExcludeEntityAsync(EntitiesNames.Balance, id);

            return result;
        }
    }
}