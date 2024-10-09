using System.Data;
using Api.Data.Repository;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Data.Context;
using Data.Repository;
using Domain.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test.Transaction
{
    public class TransactionExecuteGetTotals : IClassFixture<DbTest>
    {
        private ServiceProvider _serviceProvider;

        public TransactionExecuteGetTotals(DbTest dbTest)
        {
            _serviceProvider = dbTest.ServiceProvider;
        }

        [Fact(DisplayName = "Get dos valores totais das Transações")]
        [Trait("GET", "TransactionEntity")]
        public async Task Eh_Possivel_Realizar_Get_Transacao()
        {
            using (var context = _serviceProvider.GetService<SomniaContext>())
            {
                TransactionRepository _repositorio = new TransactionRepository(context);

                var accountEntity = await InsertAccont(context);
                var categoryOperationEntity = await InsertCategory(context, CategoryType.Operação, "Eletrônicos");

                var dataInicial = DateTime.Now;

                Thread.Sleep(1000);

                //Crédito = 665.80
                var operationEntity = await InsertOperation(context, categoryOperationEntity, OperationType.Credito);
                InsertTransaction(context, accountEntity, operationEntity, 15);

                operationEntity = await InsertOperation(context, categoryOperationEntity, OperationType.Credito);
                InsertTransaction(context, accountEntity, operationEntity, 150.8);

                operationEntity = await InsertOperation(context, categoryOperationEntity, OperationType.Credito);
                InsertTransaction(context, accountEntity, operationEntity, 500);

                //Aguarda alguns segundos para que a data seja alterada
                Thread.Sleep(1000);

                //Débito = 1586.90
                operationEntity = await InsertOperation(context, categoryOperationEntity, OperationType.Debito);
                InsertTransaction(context, accountEntity, operationEntity, 805);

                operationEntity = await InsertOperation(context, categoryOperationEntity, OperationType.Debito);
                InsertTransaction(context, accountEntity, operationEntity, 329.83);

                operationEntity = await InsertOperation(context, categoryOperationEntity, OperationType.Debito);
                InsertTransaction(context, accountEntity, operationEntity, 452.07);

                var dataFinal = DateTime.Now;

                //Aguarda mais alguns segundos para que a data seja alterada novamente e para que a última transação não seja listada
                Thread.Sleep(1000);

                operationEntity = await InsertOperation(context, categoryOperationEntity, OperationType.Debito);
                InsertTransaction(context, accountEntity, operationEntity, 300.00);

                PageParams pageParams = new PageParams
                {
                    DataCriacaoInicio = dataInicial,
                    DataCriacaoFim = dataFinal
                };

                var _registroSelecionado = await _repositorio.SelectTransactionsTotalsAsync(pageParams);
                Assert.NotNull(_registroSelecionado);
                Assert.Equal(1586.90, _registroSelecionado.GetValueOrDefault(OperationType.Debito));
                Assert.Equal(665.80, _registroSelecionado.GetValueOrDefault(OperationType.Credito));
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

        private async Task<OperationEntity> InsertOperation(SomniaContext context, CategoryEntity categoryEntity, OperationType operationType)
        {
            OperationRepository _repositorio = new OperationRepository(context);

            OperationEntity _operationEntity = new OperationEntity()
            {
                Name = Faker.Name.FullName(),
                Recurrent = false,
                Type = operationType,
                Status = StatusType.Ativo,
                CategoryId = categoryEntity.Id,
                Category = categoryEntity,
            };

            var _registroCriado = await _repositorio.InsertAsync(_operationEntity);
            Assert.NotNull(_registroCriado);
            Assert.True(_registroCriado.Id > 0);
            Assert.Equal(_registroCriado.Name, _operationEntity.Name);

            return _registroCriado;
        }

        private async void InsertTransaction(SomniaContext context, AccountEntity accountEntity, OperationEntity operationEntity, double value)
        {
            TransactionRepository _repositorio = new TransactionRepository(context);

            TransactionEntity _transactionEntity = new TransactionEntity()
            {
                Value = value,
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
            Assert.NotNull(_registroCriado);
            Assert.True(_registroCriado.Id > 0);
            Assert.Equal(_registroCriado.Value, _transactionEntity.Value);
            Assert.Equal(_registroCriado.AccountId, _transactionEntity.AccountId);
            Assert.Equal(_registroCriado.OperationId, _transactionEntity.OperationId);
        }
    }
}