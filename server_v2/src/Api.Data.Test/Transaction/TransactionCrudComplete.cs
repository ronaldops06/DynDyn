using Api.Data.Repository;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Data.Context;
using Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test.Transaction
{
    public class TransactionCrudComplete : IClassFixture<DbTest>
    {
        private ServiceProvider _serviceProvider;

        public TransactionCrudComplete(DbTest dbTest)
        {
            _serviceProvider = dbTest.ServiceProvider;
        }

        [Fact(DisplayName = "CRUD de Transação")]
        [Trait("CRUD", "TransactionEntity")]
        public async Task Eh_Possivel_Realizar_CRUD_Transacao()
        {
            using (var context = _serviceProvider.GetService<SomniaContext>())
            {
                TransactionRepository _repositorio = new TransactionRepository(context);

                var accountEntity = await InsertAccont(context);
                var operationEntity = await InsertOperation(context);

                TransactionEntity _transactionEntity = new TransactionEntity()
                {
                    Value = 150,
                    Observation = "Pago via pix",
                    Consolidated = SituationType.Nao,
                    Installment = 0,
                    TotalInstallments = 0,
                    Account = accountEntity,
                    AccountId = accountEntity.Id,
                    Operation = operationEntity,
                    OperationId = operationEntity.Id
                };

                var _registroCriado = await _repositorio.InsertAsync(_transactionEntity);
                AplicaTesteCampos(_transactionEntity, _registroCriado);
                Assert.True(_registroCriado.Id > 0);

                _transactionEntity.Value = 205.50;
                _transactionEntity.Observation = "Pago em cach";
                _transactionEntity.Consolidated = SituationType.Sim;

                var _registroAtualizado = await _repositorio.UpdateAsync(_transactionEntity);
                AplicaTesteCampos(_transactionEntity, _registroAtualizado);

                var _registroExiste = await _repositorio.ExistsAsync(_registroAtualizado.Id);
                Assert.True(_registroExiste);

                var _registroSelecionado = await _repositorio.SelectByIdAsync(_registroAtualizado.Id);
                AplicaTesteCampos(_transactionEntity, _registroSelecionado);

                var _todosRegistros = await _repositorio.SelectAsync();
                Assert.NotNull(_todosRegistros);
                Assert.True(_todosRegistros.Count() > 0);

                var _removeu = await _repositorio.DeleteAsync(_registroCriado.Id);
                Assert.True(_removeu);

                _registroCriado.Id = 0;
                await Assert.ThrowsAsync<Exception>(() => _repositorio.UpdateAsync(_registroCriado));
                await Assert.ThrowsAsync<Exception>(() => _repositorio.DeleteAsync(_registroCriado.Id));
            }
        }

        private async Task<CategoryEntity> InsertCategory(SomniaContext context, CategoryType type, string name)
        {
            CategoryRepository _repositorioCategory = new CategoryRepository(context);
            CategoryEntity _categoryEntity = new CategoryEntity()
            {
                Name = name,
                Type = type,
                Status = StatusType.Ativo,
            };

            var _registroCriado = await _repositorioCategory.InsertAsync(_categoryEntity);
            Assert.NotNull(_registroCriado);
            Assert.True(_registroCriado.Id > 0);
            Assert.Equal(_categoryEntity.Name, _registroCriado.Name);

            return _registroCriado;
        }

        private async Task<AccountEntity> InsertAccont(SomniaContext context)
        {
            var _categoryCreated = await InsertCategory(context, CategoryType.Conta, "Corrente");

            AccountRepository _repositorio = new AccountRepository(context);

            AccountEntity _parentAccountEntity = new AccountEntity()
            {
                Name = "Geral",
                Status = StatusType.Ativo,
                CategoryId = _categoryCreated.Id,
                Category = _categoryCreated
            };

            var _parentAccountCreated = await _repositorio.InsertAsync(_parentAccountEntity);
            Assert.NotNull(_parentAccountCreated);
            Assert.True(_parentAccountCreated.Id > 0);
            Assert.Equal(_parentAccountCreated.Name, _parentAccountEntity.Name);

            AccountEntity _accountEntity = new AccountEntity()
            {
                Name = "Cash",
                Status = StatusType.Ativo,
                CategoryId = _categoryCreated.Id,
                Category = _categoryCreated,
                ParentAccountId = _parentAccountEntity.Id,
                ParentAccount = _parentAccountEntity
            };

            var _registroCriado = await _repositorio.InsertAsync(_accountEntity);
            Assert.True(_registroCriado.Id > 0);
            Assert.NotNull(_registroCriado);
            Assert.Equal(_registroCriado.Name, _accountEntity.Name);

            return _registroCriado;
        }

        private async Task<OperationEntity> InsertOperation(SomniaContext context)
        {
            var _categoryCreated = await InsertCategory(context, CategoryType.Operação, "Eletrônicos");

            OperationRepository _repositorio = new OperationRepository(context);

            OperationEntity _operationEntity = new OperationEntity()
            {
                Name = "Compra Monitor",
                Recurrent = false,
                Type = OperationType.Debito,
                Status = StatusType.Ativo,
                CategoryId = _categoryCreated.Id,
                Category = _categoryCreated,
            };

            var _registroCriado = await _repositorio.InsertAsync(_operationEntity);
            Assert.NotNull(_registroCriado);
            Assert.True(_registroCriado.Id > 0);
            Assert.Equal(_registroCriado.Name, _operationEntity.Name);

            return _registroCriado;
        }

        private void AplicaTesteCampos(TransactionEntity transactionEntitySource, TransactionEntity transactionEntityDest)
        {
            Assert.NotNull(transactionEntityDest);
            Assert.Equal(transactionEntitySource.Value, transactionEntityDest.Value);
            Assert.Equal(transactionEntitySource.Observation, transactionEntityDest.Observation);
            Assert.Equal(transactionEntitySource.Consolidated, transactionEntityDest.Consolidated);
            Assert.Equal(transactionEntitySource.Installment, transactionEntityDest.Installment);
            Assert.Equal(transactionEntitySource.TotalInstallments, transactionEntityDest.TotalInstallments);
            Assert.Equal(transactionEntitySource.AccountId, transactionEntityDest.AccountId);
            Assert.Equal(transactionEntitySource.Account.Id, transactionEntityDest.Account.Id);
            Assert.Equal(transactionEntitySource.OperationId, transactionEntityDest.OperationId);
            Assert.Equal(transactionEntitySource.Operation.Id, transactionEntityDest.Operation.Id);
            Assert.Equal(transactionEntitySource.ParentTransactionId, transactionEntityDest.ParentTransactionId);
            Assert.Equal(transactionEntitySource.ParentTransaction?.Id, transactionEntityDest.ParentTransaction?.Id);
        }
    }
}