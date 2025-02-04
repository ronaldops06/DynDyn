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

namespace Api.Service.Services
{
    public class TransactionService : ITransactionService
    {
        private IUserService _userService;
        private ITransactionRepository _repository;
        private IOperationService _operationService;
        private readonly IMapper _mapper;
        public TransactionService(IUserService userService,
                                  ITransactionRepository repository,
                                  IOperationService operationService,
                                  IMapper mapper)
        {
            _userService = userService;
            _repository = repository;
            _operationService = operationService;
            _mapper = mapper;
        }

        public async Task<TransactionModel> GetById(int id)
        {
            var user = await _userService.GetLoggedUser();
            var entity = await _repository.SelectByIdAsync(user.Id, id);

            if (entity == null)
                throw new Exception("Transação não encontrada.");

            return _mapper.Map<TransactionModel>(entity);
        }

        public async Task<TransactionTotalModel> GetTotais(PageParams pageParams)
        {
            var user = await _userService.GetLoggedUser();
            var transactionTotals = await _repository.SelectTransactionsTotalsAsync(user.Id, pageParams);
            return _mapper.Map<TransactionTotalModel>(transactionTotals);
        }

        public async Task<PageList<TransactionModel>> Get(PageParams pageParams)
        {
            var user = await _userService.GetLoggedUser();
            var data = await _repository.SelectByParamAsync(user.Id, pageParams);
            var itens = _mapper.Map<List<TransactionModel>>(data.Itens);

            return PageList<TransactionModel>.Create(pageParams, itens, data.Count);
        }

        public async Task<TransactionModel> Post(TransactionModel model)
        {
            var user = await _userService.GetLoggedUser();
            model.User = user;
            model.UserId = user.Id;
            TransferValidate(model);

            await OperacaoNotExists(model);

            var transactionEntity = _mapper.Map<TransactionEntity>(model);

            _repository.UnchangedParentTransaction(transactionEntity);
            transactionEntity = await _repository.InsertAsync(transactionEntity);

            model = _mapper.Map<TransactionModel>(transactionEntity);

            ExecuteInstallments(user, model);

            return model;
        }

        public async Task<TransactionModel> Put(TransactionModel model)
        {
            var user = await _userService.GetLoggedUser();
            var transactionEntityAux = await _repository.SelectByIdAsync(user.Id, model.Id);

            if (transactionEntityAux == null)
                throw new Exception("Transação não encontrada.");

            model.User = user;
            model.UserId = user.Id;
            TransferValidate(model);

            await OperacaoNotExists(model);
            
            var transactionEntity = _mapper.Map<TransactionEntity>(model);
            _repository.UnchangedParentTransaction(transactionEntity);
            transactionEntity = await _repository.UpdateAsync(transactionEntity);

            return _mapper.Map<TransactionModel>(transactionEntity);
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _userService.GetLoggedUser();
            var transactionEntityAux = await _repository.SelectByIdAsync(user.Id, id);

            if (transactionEntityAux == null)
                throw new Exception("Transação não encontrada.");

            var result = await _repository.DeleteAsync(id);

            if (result)
                ExecuteDeleteInstallments(user.Id, transactionEntityAux);

            return result;
        }

        private void TransferValidate(TransactionModel transactionModel)
        {
            if (transactionModel.Operation.Type == OperationType.Transferencia)
            {
                ExistsDestinationAccount(transactionModel.DestinationAccountId);

                IsDifferentAccounts(transactionModel.AccountId, transactionModel.DestinationAccountId);

                NotExistsInstallments(transactionModel.TotalInstallments);
            }
        }

        private void ExistsDestinationAccount(int? destinationAccountId)
        {
            if (destinationAccountId == null || destinationAccountId == 0)
                throw new Exception("A conta de destino deve ser informada.");
        }

        private void IsDifferentAccounts(int accountId, int? destinationAccountId)
        {
            if (accountId == destinationAccountId)
                throw new Exception("A conta de destino deve ser diferente da conta origem.");
        }

        private void NotExistsInstallments(int? totalInstallments)
        {
            if (totalInstallments != null && totalInstallments > 1)
                throw new Exception("Transações de transferência não devem ter parcelas informadas.");
        }

        private async Task OperacaoNotExists(TransactionModel transactionModel)
        {
            // Pode estar sendo inserido uma transação com uma operação que ainda não existe, neste caso cria uma nova operação para a transação.
            if (transactionModel.Operation.Id == 0)
                await ExistsOperationByNameAndType(transactionModel);
        }

        private async Task ExistsOperationByNameAndType(TransactionModel transactionModel)
        {
            var operationEntity = _mapper.Map<OperationEntity>(transactionModel.Operation);

            // Verifica se não existe uma operação com o mesmo nome e tipo, se já existir irá somente vincular à que já existe.
            var operacaoAux = await _operationService.GetByNameAndType(operationEntity.Name, operationEntity.Type);

            if (operacaoAux != null && operacaoAux.Id != 0)
            {
                transactionModel.OperationId = operacaoAux.Id;
                transactionModel.Operation = operacaoAux;
            }
            else
            {
                operacaoAux = await _operationService.Post(transactionModel.Operation);
                transactionModel.OperationId = operacaoAux.Id;
                transactionModel.Operation = operacaoAux;
            }
        }

        private async void ExecuteInstallments(UserModel userModel, TransactionModel transactionModel)
        {
            if (transactionModel.TotalInstallments > 1)
            {
                for (var i = 2; i <= transactionModel.TotalInstallments; i++)
                {
                    TransactionModel transacaoParcelaModel = (TransactionModel)transactionModel.Clone();
                    transacaoParcelaModel.Id = 0;
                    transacaoParcelaModel.Installment = i;
                    transacaoParcelaModel.DataCriacao = transactionModel.DataCriacao?.AddMonths(i - 1);
                    transacaoParcelaModel.DataAlteracao = transactionModel.DataAlteracao?.AddMonths(i - 1);
                    transacaoParcelaModel.Operation = null;
                    transacaoParcelaModel.ParentTransactionId = transactionModel.Id;
                    transacaoParcelaModel.ParentTransaction = transactionModel;
                    transacaoParcelaModel.User = userModel;

                    var transactionParcelaEntity = _mapper.Map<TransactionEntity>(transacaoParcelaModel);

                    _repository.UnchangedParentTransaction(transactionParcelaEntity);
                    transactionParcelaEntity = await _repository.InsertAsync(transactionParcelaEntity);
                }

                transactionModel.Installment = 1;

                var transactionEntity = _mapper.Map<TransactionEntity>(transactionModel);
                _repository.UnchangedParentTransaction(transactionEntity);
                transactionEntity = await _repository.UpdateAsync(transactionEntity);

                transactionModel = _mapper.Map<TransactionModel>(transactionEntity);
            }
        }

        private async void ExecuteDeleteInstallments(int userId, TransactionEntity transactionEntityAux)
        {
            if (transactionEntityAux.ParentTransactionId != null)
            {
                var transactions = await _repository.SelectTransactionByParentTransactionIdAsync(userId, transactionEntityAux.ParentTransactionId ?? 0);
                foreach (var transactionEntity in transactions)
                {
                    await _repository.DeleteAsync(transactionEntity.Id);
                }
            }
        }
    }
}