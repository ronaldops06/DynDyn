using System.Globalization;
using Api.Data.Repository;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Data.Context;
using Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test.Transaction
{
    public class TransactionExecuteGetAll : BaseTestGet<TransactionEntity>, IClassFixture<DbTest>
    {
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

                Thread.Sleep(1000);
                var lastSyncDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

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

                await RealizaGetLasSyncDate(_repositorio, lastSyncDate, 36);

                Thread.Sleep(1000);
                lastSyncDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                //O teste abaixo irá atualizar um número objetos para verificar se retorna corretamente
                for (int i = 10; i < (RECORD_NUMBER + 10); i++)
                {
                    TransactionEntity _entity = await _repositorio.SelectByIdAsync(i);

                    await _repositorio.UpdateAsync(_entity);
                }

                await RealizaGetLasSyncDate(_repositorio, lastSyncDate, 10);
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
    }
}