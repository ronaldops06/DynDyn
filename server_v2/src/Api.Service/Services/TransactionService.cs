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
using Service.Types;

namespace Api.Service.Services
{
    public class TransactionService : BaseService, ITransactionService
    {
        private ITransactionRepository _repository;
        private IOperationService _operationService;

        public TransactionService(IUserService userService,
            ITransactionRepository repository,
            IOperationService operationService,
            IDeviceService deviceService,
            IMapper mapper) : base(deviceService, userService, mapper)
        {
            _repository = repository;
            _operationService = operationService;
        }

        public async Task<TransactionModel> GetById(int id)
        {
            var user = await userService.GetLoggedUser();
            var entity = await _repository.SelectByIdAsync(user.Id, id);

            if (entity == null)
                throw new Exception("Transação não encontrada.");

            return mapper.Map<TransactionModel>(entity);
        }

        public async Task<TransactionTotalModel> GetTotais(PageParams pageParams)
        {
            var user = await userService.GetLoggedUser();
            var transactionTotals = await _repository.SelectTransactionsTotalsAsync(user.Id, pageParams);
            return mapper.Map<TransactionTotalModel>(transactionTotals);
        }

        public async Task<PageList<TransactionModel>> Get(PageParams pageParams)
        {
            var user = await userService.GetLoggedUser();
            var data = await _repository.SelectByParamAsync(user.Id, pageParams);
            var itens = mapper.Map<List<TransactionModel>>(data.Itens);

            return PageList<TransactionModel>.Create(pageParams, itens, data.Count);
        }

        public async Task<TransactionModel> Post(TransactionModel model)
        {
            var user = await userService.GetLoggedUser();
            model.User = user;
            model.UserId = user.Id;
            TransferValidate(model);

            await OperacaoNotExists(model);

            var transactionEntity = mapper.Map<TransactionEntity>(model);

            _repository.UnchangedParentTransaction(transactionEntity);
            transactionEntity = await _repository.InsertAsync(transactionEntity);

            model = mapper.Map<TransactionModel>(transactionEntity);
            
            return model;
        }

        public async Task<TransactionModel> Put(TransactionModel model)
        {
            var user = await userService.GetLoggedUser();
            var transactionEntityAux = await _repository.SelectByIdAsync(user.Id, model.Id);

            if (transactionEntityAux == null)
                throw new Exception("Transação não encontrada.");

            model.User = user;
            model.UserId = user.Id;
            TransferValidate(model);

            await OperacaoNotExists(model);

            var transactionEntity = mapper.Map<TransactionEntity>(model);
            _repository.UnchangedParentTransaction(transactionEntity);
            transactionEntity = await _repository.UpdateAsync(transactionEntity);

            return mapper.Map<TransactionModel>(transactionEntity);
        }

        public async Task<bool> Delete(int id)
        {
            var user = await userService.GetLoggedUser();
            var transactionEntityAux = await _repository.SelectByIdAsync(user.Id, id);

            if (transactionEntityAux == null)
                throw new Exception("Transação não encontrada.");

            var result = await _repository.DeleteAsync(id);
            if (result)
                await ProcessExcludeEntityAsync(EntitiesNames.Transaction, id);
            
            return result;
        }
        
        public async Task GenerateRecurringAndInstallmentPayments(DateTime? baseDate)
        {
            var startDayOfMonth = 5;
            var periodPreviousMonth = DateHelper.CalculatePeriod(baseDate, startDayOfMonth, -1);
            var periodCurrentMonth = DateHelper.CalculatePeriod(baseDate, startDayOfMonth, 0);
            var user = await userService.GetLoggedUser();

            await GenerateRecurring(user, periodPreviousMonth, periodCurrentMonth);

            await GenerateInstallmentPayments(user, periodPreviousMonth, periodCurrentMonth);
        }

        private async Task GenerateRecurring(UserModel user, Period periodPreviousMonth, Period periodCurrentMonth)
        {
            var operationModels = await _operationService.GetByActiveAndRecurrent();
            foreach (var operation in operationModels)
            {
                var transactionPreviousMonth =
                    await _repository.SelectByOperationAndPeriodAsync(user.Id, operation.Id, periodPreviousMonth);

                var transactionCurrentMonth =
                    await _repository.SelectByOperationAndPeriodAsync(user.Id, operation.Id, periodCurrentMonth);

                //Se já existir a transação para a operação no mês atual ou se não existe no mês anterior para usar como base pula o registro 
                if (transactionCurrentMonth != null || transactionPreviousMonth == null)
                    continue;
                
                await IncludeTransactionBasedPrevious(transactionPreviousMonth);
            }
        }

        private async Task GenerateInstallmentPayments(UserModel user, Period periodPreviousMonth, Period periodCurrentMonth)
        {
            var transactionsEntities = await _repository.SelectByPendingInstallments(user.Id, periodPreviousMonth);
            foreach (var transaction in transactionsEntities)
            {
                var transactionCurrentMonth =
                    await _repository.SelectByOperationAndPeriodAsync(user.Id, transaction.OperationId, periodCurrentMonth);

                if (transactionCurrentMonth != null)
                    continue;
                
                await IncludeTransactionBasedPrevious(transaction, true);
            }
        }

        private async Task IncludeTransactionBasedPrevious(TransactionEntity transaction, bool incrementInstallment = false)
        {
            var transactionEntity = (TransactionEntity)transaction.Clone();
            transactionEntity.DataCriacao = transactionEntity.DataCriacao?.AddMonths(1);
            transactionEntity.DataAlteracao = null;
            transactionEntity.Id = 0;
            transactionEntity.Portfolio = null;
            transactionEntity.User = null;
            transactionEntity.Consolidated = SituationType.Nao;
            
            if (incrementInstallment)
                transactionEntity.Installment++;

            _repository.UnchangedParentTransaction(transactionEntity);
            transactionEntity = await _repository.InsertAsync(transactionEntity);
        }
        
        private void TransferValidate(TransactionModel transactionModel)
        {
            if (transactionModel.Operation.Type == OperationType.Transferencia)
            {
                ExistsDestinationAccount(transactionModel.DestinationPortfolioId);

                IsDifferentAccounts(transactionModel.PortfolioId, transactionModel.DestinationPortfolioId);

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
            var operationEntity = mapper.Map<OperationEntity>(transactionModel.Operation);

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
    }
}