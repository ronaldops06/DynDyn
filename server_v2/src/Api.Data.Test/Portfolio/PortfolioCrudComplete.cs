using Api.Data.Repository;
using Api.Data.Test.Helpers;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Data.Context;
using Data.Repository;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test.Portfolio
{
    public class PortfolioCrudComplete : IClassFixture<DbTest>
    {
        private ServiceProvider _serviceProvider;

        public PortfolioCrudComplete(DbTest dbTest)
        {
            _serviceProvider = dbTest.ServiceProvider;
        }

        private void AplicaTesteCampos(PortfolioEntity portfolioEntitySource, PortfolioEntity portfolioEntityDest)
        {
            Assert.NotNull(portfolioEntityDest);
            Assert.Equal(portfolioEntitySource.Name, portfolioEntityDest.Name);
            Assert.Equal(portfolioEntitySource.Status, portfolioEntityDest.Status);
            Assert.Equal(portfolioEntitySource.CategoryId, portfolioEntityDest.CategoryId);
            Assert.Equal(portfolioEntitySource.Category.Id, portfolioEntityDest.Category.Id);
            Assert.Equal(portfolioEntitySource.ParentPortfolioId, portfolioEntityDest.ParentPortfolioId);
            Assert.Equal(portfolioEntitySource.ParentPortfolio?.Id, portfolioEntityDest.ParentPortfolio?.Id);
            Assert.Equal(portfolioEntitySource.UserId, portfolioEntityDest.UserId);
            Assert.Equal(portfolioEntitySource.User.Id, portfolioEntityDest.User.Id);
        }

        [Fact(DisplayName = "CRUD de Conta")]
        [Trait("CRUD", "PortfolioEntity")]
        public async Task Eh_Possivel_Realizar_CRUD_Conta()
        {
            using (var context = _serviceProvider.GetService<SomniaContext>())
            {
                UserRepository userRepository = new UserRepository(context);
                var userCreated = await userRepository.InsertAsync(UserHelper.GetLoggedUserFake());
                Assert.NotNull(userCreated);
                Assert.True(userCreated.Id > 0);
                
                CategoryRepository _repositorioCategory = new CategoryRepository(context);
                
                CategoryEntity _categoryEntity = new CategoryEntity()
                {
                    Name = "Corrente",
                    Type = CategoryType.Conta,
                    Status = StatusType.Ativo,
                    UserId = userCreated.Id,
                    User = userCreated
                };

                var _categoryCreated = await _repositorioCategory.InsertAsync(_categoryEntity);
                Assert.NotNull(_categoryCreated);
                Assert.True(_categoryCreated.Id > 0);
                Assert.Equal(_categoryEntity.Name, _categoryCreated.Name);
                Assert.Equal(_categoryEntity.Type, _categoryCreated.Type);
                Assert.Equal(_categoryEntity.Status, _categoryCreated.Status);
                Assert.Equal(_categoryEntity.User.Id, _categoryCreated.User.Id);

                PortfolioRepository _repositorio = new PortfolioRepository(context);

                PortfolioEntity parentPortfolioEntity = new PortfolioEntity()
                {
                    Name = "Geral",
                    Status = StatusType.Ativo,
                    CategoryId = _categoryCreated.Id,
                    Category = _categoryCreated,
                    UserId = userCreated.Id,
                    User = userCreated
                };

                var _parentPortfolioCreated = await _repositorio.InsertAsync(parentPortfolioEntity);
                AplicaTesteCampos(parentPortfolioEntity, _parentPortfolioCreated);
                Assert.True(_parentPortfolioCreated.Id > 0);

                PortfolioEntity portfolioEntity = new PortfolioEntity()
                {
                    Name = "Cash",
                    Status = StatusType.Ativo,
                    CategoryId = _categoryCreated.Id,
                    Category = _categoryCreated,
                    ParentPortfolioId = parentPortfolioEntity.Id,
                    ParentPortfolio = parentPortfolioEntity,
                    UserId = userCreated.Id,
                    User = userCreated
                };

                var _registroCriado = await _repositorio.InsertAsync(portfolioEntity);
                AplicaTesteCampos(portfolioEntity, _registroCriado);
                Assert.True(_registroCriado.Id > 0);

                portfolioEntity.Status = StatusType.Inativo;

                var _registroAtualizado = await _repositorio.UpdateAsync(portfolioEntity);
                AplicaTesteCampos(portfolioEntity, _registroAtualizado);

                var _registroExiste = await _repositorio.ExistsAsync(_registroAtualizado.Id);
                Assert.True(_registroExiste);

                var _registroSelecionado = await _repositorio.SelectByIdAsync(userCreated.Id, _registroAtualizado.Id);
                AplicaTesteCampos(portfolioEntity, _registroSelecionado);

                var _todosRegistros = await _repositorio.SelectAsync(userCreated.Id);
                Assert.NotNull(_todosRegistros);
                Assert.True(_todosRegistros.Count() > 0);

                var _removeu = await _repositorio.DeleteAsync(_registroCriado.Id);
                Assert.True(_removeu);

                _registroCriado.Id = 0;
                await Assert.ThrowsAsync<Exception>(() => _repositorio.UpdateAsync(_registroCriado));
                await Assert.ThrowsAsync<Exception>(() => _repositorio.DeleteAsync(_registroCriado.Id));
            }
        }
    }
}