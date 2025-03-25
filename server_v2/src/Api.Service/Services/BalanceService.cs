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

namespace Service.Services
{
    public class BalanceService : IBalanceService
    {
        private IUserService _userService;
        private IBalanceRepository _repository;
        private IMapper _mapper;
        
        public BalanceService(IUserService userService, IBalanceRepository repository, IMapper mapper)
        {
            _userService = userService;
            _repository = repository;
            _mapper = mapper;
        }
        
        public async Task<BalanceModel> GetById(int id)
        {
            var user = await _userService.GetLoggedUser();
            var entity = await _repository.SelectByIdAsync(user.Id, id);

            if (entity == null)
                throw new Exception("Saldo não encontrado.");

            return _mapper.Map<BalanceModel>(entity);
        }
        
        public async Task<PageList<BalanceModel>> Get(PageParams pageParams)
        {
            var user = await _userService.GetLoggedUser();
            var data = await _repository.SelectByParamAsync(user.Id, pageParams);
            var itens = _mapper.Map<List<BalanceModel>>(data.Itens);

            return PageList<BalanceModel>.Create(pageParams, itens, data.Count);
        }

        public async Task<BalanceModel> Post(BalanceModel model)
        {
            var user = await _userService.GetLoggedUser();
            var balanceEntityAux = await _repository.SelectByUkAsync(user.Id, model.PortfolioId, model.Month, model.Year);

            if (balanceEntityAux != null)
                throw new Exception("Saldo não disponível.");

            model.User = user;
            model.UserId = user.Id;
            var balanceEntity = _mapper.Map<BalanceEntity>(model);
            _repository.UnchangedParentBalance(balanceEntity);
            balanceEntity = await _repository.InsertAsync(balanceEntity);

            model = _mapper.Map<BalanceModel>(balanceEntity);

            return model;
        }

        public async Task<BalanceModel> Put(BalanceModel model)
        {
            var user = await _userService.GetLoggedUser();
            var balanceEntityAux = await _repository.SelectByUkAsync(user.Id, model.PortfolioId, model.Month, model.Year);

            if (balanceEntityAux != null && model.Id != balanceEntityAux.Id)
                throw new Exception("Saldo não disponível.");

            balanceEntityAux = await _repository.SelectByIdAsync(user.Id, model.Id);

            if (balanceEntityAux == null)
                throw new Exception("Saldo não encontrado.");

            model.User = user;
            model.UserId = user.Id;
            var balanceEntity = _mapper.Map<BalanceEntity>(model);
            _repository.UnchangedParentBalance(balanceEntity);
            balanceEntity = await _repository.UpdateAsync(balanceEntity);

            return _mapper.Map<BalanceModel>(balanceEntity);
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _userService.GetLoggedUser();
            var balanceEntityAux = await _repository.SelectByIdAsync(user.Id, id);

            if (balanceEntityAux == null)
                throw new Exception("Saldo não encontrado.");

            return await _repository.DeleteAsync(id);
        }

    }
}