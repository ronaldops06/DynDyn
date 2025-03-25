using System.Globalization;
using Api.Data.Repository;
using Api.Data.Test.Helpers;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Data.Context;
using Data.Repository;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test.Transaction
{
    public class TransactionExecuteGetAll : BaseTestGet<TransactionEntity>, IClassFixture<DbTest>
    {
        private UserEntity _user;
        
        public TransactionExecuteGetAll(DbTest dbTest) : base(dbTest) { }

        [Fact(DisplayName = "Get de Transações")]
        [Trait("GET", "TransactionEntity")]
        public async Task Eh_Possivel_Realizar_Get_Transacao()
        {
            using (var context = serviceProvider.GetService<SomniaContext>())
            {
                UserRepository userRepository = new UserRepository(context);
                _user = await userRepository.InsertAsync(UserHelper.GetLoggedUserFake());
                Assert.NotNull(_user);
                Assert.True(_user.Id > 0);
                
                TransactionRepository _repositorio = new TransactionRepository(context);

                var portfolioEntity = await InsertAccont(context);
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
                        Portfolio = portfolioEntity,
                        PortfolioId = portfolioEntity.Id,
                        Operation = operationEntity,
                        OperationId = operationEntity.Id,
                        UserId = _user.Id,
                        User = _user
                    };

                    await _repositorio.InsertAsync(_transactionEntity);
                }

                await RealizaGetPaginado(_user.Id, _repositorio);

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
                        Portfolio = portfolioEntity,
                        PortfolioId = portfolioEntity.Id,
                        Operation = operationEntity,
                        OperationId = operationEntity.Id,
                        UserId = _user.Id,
                        User = _user
                    };

                    await _repositorio.InsertAsync(_transactionEntity);
                }

                await RealizaGetLasSyncDate(_user.Id, _repositorio, lastSyncDate, 36);

                Thread.Sleep(1000);
                lastSyncDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                //O teste abaixo irá atualizar um número objetos para verificar se retorna corretamente
                for (int i = 10; i < (RECORD_NUMBER + 10); i++)
                {
                    TransactionEntity _entity = await _repositorio.SelectByIdAsync(_user.Id, i);

                    await _repositorio.UpdateAsync(_entity);
                }

                await RealizaGetLasSyncDate(_user.Id, _repositorio, lastSyncDate, 10);
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
                UserId = _user.Id,
                User = _user
            };

            var _registroCriado = await _repositorioCategory.InsertAsync(_categoryEntity);
            Assert.NotNull(_registroCriado);
            Assert.True(_registroCriado.Id > 0);
            Assert.Equal(_categoryEntity.Name, _registroCriado.Name);
            Assert.Equal(_categoryEntity.User.Id, _registroCriado.User.Id);

            return _registroCriado;
        }

        private async Task<PortfolioEntity> InsertAccont(SomniaContext context)
        {
            var _categoryCreated = await InsertCategory(context, CategoryType.Conta, "Corrente");

            PortfolioRepository _repositorio = new PortfolioRepository(context);

            PortfolioEntity parentPortfolioPortfolioEntity = new PortfolioEntity()
            {
                Name = "Geral",
                Status = StatusType.Ativo,
                CategoryId = _categoryCreated.Id,
                Category = _categoryCreated,
                UserId = _user.Id,
                User = _user
            };

            var _parentPortfolioCreated = await _repositorio.InsertAsync(parentPortfolioPortfolioEntity);
            Assert.NotNull(_parentPortfolioCreated);
            Assert.True(_parentPortfolioCreated.Id > 0);
            Assert.Equal(_parentPortfolioCreated.Name, parentPortfolioPortfolioEntity.Name);
            Assert.Equal(_parentPortfolioCreated.User.Id, parentPortfolioPortfolioEntity.User.Id);

            PortfolioEntity portfolioPortfolioEntity = new PortfolioEntity()
            {
                Name = "Cash",
                Status = StatusType.Ativo,
                CategoryId = _categoryCreated.Id,
                Category = _categoryCreated,
                ParentPortfolioId = parentPortfolioPortfolioEntity.Id,
                ParentPortfolio = parentPortfolioPortfolioEntity,
                UserId = _user.Id,
                User = _user
            };

            var _registroCriado = await _repositorio.InsertAsync(portfolioPortfolioEntity);
            Assert.True(_registroCriado.Id > 0);
            Assert.NotNull(_registroCriado);
            Assert.Equal(_registroCriado.Name, portfolioPortfolioEntity.Name);

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
                UserId = _user.Id,
                User = _user
            };

            var _registroCriado = await _repositorio.InsertAsync(_operationEntity);
            Assert.NotNull(_registroCriado);
            Assert.True(_registroCriado.Id > 0);
            Assert.Equal(_registroCriado.Name, _operationEntity.Name);
            Assert.Equal(_registroCriado.User.Id, _operationEntity.User.Id);

            return _registroCriado;
        }
    }
}