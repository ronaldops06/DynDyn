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
    public class TransactionCrudComplete : IClassFixture<DbTest>
    {
        private UserEntity _user;
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
                UserRepository userRepository = new UserRepository(context);
                _user = await userRepository.InsertAsync(UserHelper.GetLoggedUserFake());
                Assert.NotNull(_user);
                Assert.True(_user.Id > 0);
                
                TransactionRepository _repositorio = new TransactionRepository(context);

                var portfolioEntity = await InsertAccont(context);
                var operationEntity = await InsertOperation(context);

                TransactionEntity _transactionEntity = new TransactionEntity()
                {
                    Value = 150,
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
                AplicaTesteCampos(_transactionEntity, _registroCriado);
                Assert.True(_registroCriado.Id > 0);

                _transactionEntity.Value = 205.50;
                _transactionEntity.Observation = "Pago em cach";
                _transactionEntity.Consolidated = SituationType.Sim;

                var _registroAtualizado = await _repositorio.UpdateAsync(_transactionEntity);
                AplicaTesteCampos(_transactionEntity, _registroAtualizado);

                var _registroExiste = await _repositorio.ExistsAsync(_registroAtualizado.Id);
                Assert.True(_registroExiste);

                var _registroSelecionado = await _repositorio.SelectByIdAsync(_user.Id, _registroAtualizado.Id);
                AplicaTesteCampos(_transactionEntity, _registroSelecionado);

                var _todosRegistros = await _repositorio.SelectAsync(_user.Id);
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
            Assert.Equal(_registroCriado.User.Id, portfolioPortfolioEntity.User.Id);

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

        private void AplicaTesteCampos(TransactionEntity transactionEntitySource, TransactionEntity transactionEntityDest)
        {
            Assert.NotNull(transactionEntityDest);
            Assert.Equal(transactionEntitySource.Value, transactionEntityDest.Value);
            Assert.Equal(transactionEntitySource.Observation, transactionEntityDest.Observation);
            Assert.Equal(transactionEntitySource.Consolidated, transactionEntityDest.Consolidated);
            Assert.Equal(transactionEntitySource.Installment, transactionEntityDest.Installment);
            Assert.Equal(transactionEntitySource.TotalInstallments, transactionEntityDest.TotalInstallments);
            Assert.Equal(transactionEntitySource.PortfolioId, transactionEntityDest.PortfolioId);
            Assert.Equal(transactionEntitySource.Portfolio.Id, transactionEntityDest.Portfolio.Id);
            Assert.Equal(transactionEntitySource.OperationId, transactionEntityDest.OperationId);
            Assert.Equal(transactionEntitySource.Operation.Id, transactionEntityDest.Operation.Id);
            Assert.Equal(transactionEntitySource.ParentTransactionId, transactionEntityDest.ParentTransactionId);
            Assert.Equal(transactionEntitySource.ParentTransaction?.Id, transactionEntityDest.ParentTransaction?.Id);
            Assert.Equal(transactionEntitySource.UserId, transactionEntityDest.UserId);
            Assert.Equal(transactionEntitySource.User.Id, transactionEntityDest.User.Id);
        }
    }
}