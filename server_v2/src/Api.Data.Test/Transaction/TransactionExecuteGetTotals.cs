using Api.Data.Repository;
using Api.Data.Test.Helpers;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Data.Context;
using Data.Repository;
using Domain.Entities;
using Domain.Helpers;
using Faker;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test.Transaction
{
    public class TransactionExecuteGetTotals : IClassFixture<DbTest>
    {
        private UserEntity _user;
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
                UserRepository userRepository = new UserRepository(context);
                _user = await userRepository.InsertAsync(UserHelper.GetLoggedUserFake());
                Assert.NotNull(_user);
                Assert.True(_user.Id > 0);
                
                TransactionRepository _repositorio = new TransactionRepository(context);

                var portfolioEntity = await InsertPortfolio(context);
                var categoryOperationEntity = await InsertCategory(context, CategoryType.Operação, "Eletrônicos");

                var dataInicial = DateTime.Now;

                Thread.Sleep(1000);

                //Crédito = 665.80
                var operationEntity = await InsertOperation(context, categoryOperationEntity, OperationType.Credito);
                InsertTransaction(context, portfolioEntity, operationEntity, 15);

                operationEntity = await InsertOperation(context, categoryOperationEntity, OperationType.Credito);
                InsertTransaction(context, portfolioEntity, operationEntity, 150.8);

                operationEntity = await InsertOperation(context, categoryOperationEntity, OperationType.Credito);
                InsertTransaction(context, portfolioEntity, operationEntity, 500);

                //Aguarda alguns segundos para que a data seja alterada
                Thread.Sleep(1000);

                //Débito = 1586.90
                operationEntity = await InsertOperation(context, categoryOperationEntity, OperationType.Debito);
                InsertTransaction(context, portfolioEntity, operationEntity, 805);

                operationEntity = await InsertOperation(context, categoryOperationEntity, OperationType.Debito);
                InsertTransaction(context, portfolioEntity, operationEntity, 329.83);

                operationEntity = await InsertOperation(context, categoryOperationEntity, OperationType.Debito);
                InsertTransaction(context, portfolioEntity, operationEntity, 452.07);

                var dataFinal = DateTime.Now;

                //Aguarda mais alguns segundos para que a data seja alterada novamente e para que a última transação não seja listada
                Thread.Sleep(1000);

                operationEntity = await InsertOperation(context, categoryOperationEntity, OperationType.Debito);
                InsertTransaction(context, portfolioEntity, operationEntity, 300.00);

                PageParams pageParams = new PageParams
                {
                    DataCriacaoInicio = dataInicial,
                    DataCriacaoFim = dataFinal
                };

                var _registroSelecionado = await _repositorio.SelectTransactionsTotalsAsync(_user.Id, pageParams);
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

        private async Task<PortfolioEntity> InsertPortfolio(SomniaContext context)
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
            Assert.Equal(_registroCriado.User.Id, portfolioPortfolioEntity.User.Id);

            return _registroCriado;
        }

        private async Task<OperationEntity> InsertOperation(SomniaContext context, CategoryEntity categoryEntity, OperationType operationType)
        {
            OperationRepository _repositorio = new OperationRepository(context);

            OperationEntity _operationEntity = new OperationEntity()
            {
                Name = Name.FullName(),
                Recurrent = false,
                Type = operationType,
                Status = StatusType.Ativo,
                CategoryId = categoryEntity.Id,
                Category = categoryEntity,
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

        private async void InsertTransaction(SomniaContext context, PortfolioEntity portfolioEntity, OperationEntity operationEntity, double value)
        {
            TransactionRepository _repositorio = new TransactionRepository(context);

            TransactionEntity _transactionEntity = new TransactionEntity()
            {
                Value = value,
                Observation = "Pago via pix",
                Consolidated = SituationType.Nao,
                Installment = 0,
                TotalInstallments = 0,
                Portfolio = portfolioEntity,
                PortfolioId = portfolioEntity.Id,
                Operation = operationEntity,
                OperationId = operationEntity.Id,
                UserId = _user.Id,
                User = _user
            };

            var _registroCriado = await _repositorio.InsertAsync(_transactionEntity);
            Assert.NotNull(_registroCriado);
            Assert.True(_registroCriado.Id > 0);
            Assert.Equal(_registroCriado.Value, _transactionEntity.Value);
            Assert.Equal(_registroCriado.PortfolioId, _transactionEntity.PortfolioId);
            Assert.Equal(_registroCriado.OperationId, _transactionEntity.OperationId);
            Assert.Equal(_registroCriado.UserId, _transactionEntity.UserId);
        }
    }
}