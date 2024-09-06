using Api.Data.Repository;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Data.Context;
using Data.Repository;
using Domain.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static Api.Data.Test.Helpers.BaseHelper;

namespace Api.Data.Test.Transaction
{
    public class TransactionExecuteGetAll : BaseTestGet<TransactionEntity>, IClassFixture<DbTest>
    {
        private static readonly int RECORD_NUMBER = 35;

        public TransactionExecuteGetAll(DbTest dbTest) : base(dbTest) { }

        [Fact(DisplayName = "Get de Transações")]
        [Trait("GET", "TransactionEntity")]
        public async Task Eh_Possivel_Realizar_Get_Transacao()
        {
            using (var context = serviceProvider.GetService<SomniaContext>())
            {
                TransactionRepository _repositorio = new TransactionRepository(context);

                var accountEntity = await InsertAccont(context);
                var operationEntity = await InsertOperation(context);

                Random random = new Random();
                for (int i = 1; i <= RECORD_NUMBER; i++)
                {
                    TransactionEntity _transactionEntity = new TransactionEntity()
                    {
                        Value = random.Next(5000),
                        Observation = "Pago via pix",
                        Consolidated = SituationType.Nao,
                        Installment = random.Next(20),
                        TotalInstallments = 20,
                        Account = accountEntity,
                        AccountId = accountEntity.Id,
                        Operation = operationEntity,
                        OperationId = operationEntity.Id
                    };

                    await _repositorio.InsertAsync(_transactionEntity);
                }

                await RealizaGetPaginado(_repositorio);
            }
        }

        private async Task<CategoryEntity> InsertCategory(SomniaContext context, CategoryType type, string name)
        {
            CategoryRepository _repositorioCategory = new CategoryRepository(context);
            CategoryEntity _categoryEntity = new CategoryEntity()
            {
                Nome = name,
                Tipo = type,
                Status = StatusType.Ativo,
            };

            var _registroCriado = await _repositorioCategory.InsertAsync(_categoryEntity);
            Assert.NotNull(_registroCriado);
            Assert.True(_registroCriado.Id > 0);
            Assert.Equal(_categoryEntity.Nome, _registroCriado.Nome);

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
    }
}