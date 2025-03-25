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
using static Api.Data.Test.Helpers.BaseHelper;

namespace Api.Data.Test.Portfolio
{
    public class PortfolioExecuteGetAll : BaseTestGet<PortfolioEntity>, IClassFixture<DbTest>
    {
        public PortfolioExecuteGetAll(DbTest dbTest) : base(dbTest) { }

        [Fact(DisplayName = "Get de Conta")]
        [Trait("GET", "PortfolioEntity")]
        public async Task Eh_Possivel_Realizar_Get_Conta()
        {
            using (var context = serviceProvider.GetService<SomniaContext>())
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
                Assert.NotNull(_parentPortfolioCreated);
                Assert.Equal(parentPortfolioEntity.Name, _parentPortfolioCreated.Name);
                Assert.Equal(parentPortfolioEntity.Status, _parentPortfolioCreated.Status);
                Assert.Equal(parentPortfolioEntity.CategoryId, _parentPortfolioCreated.CategoryId);
                Assert.Equal(parentPortfolioEntity.Category.Id, _parentPortfolioCreated.Category.Id);
                Assert.Equal(parentPortfolioEntity.ParentPortfolioId, _parentPortfolioCreated.ParentPortfolioId);
                Assert.Equal(parentPortfolioEntity.ParentPortfolio?.Id, _parentPortfolioCreated.ParentPortfolio?.Id);
                Assert.Equal(parentPortfolioEntity.UserId, _parentPortfolioCreated.UserId);
                Assert.Equal(parentPortfolioEntity.User.Id, _parentPortfolioCreated.User.Id);
                Assert.True(_parentPortfolioCreated.Id > 0);

                PortfolioRepository portfolioRepository = new PortfolioRepository(context);

                for (int i = 1; i < RECORD_NUMBER; i++)
                {
                    PortfolioEntity _entity = new PortfolioEntity
                    {
                        Name = Faker.Name.FullName(),
                        Status = GetStatusTypeRandom(),
                        CategoryId = _categoryCreated.Id,
                        Category = _categoryCreated,
                        ParentPortfolioId = parentPortfolioEntity.Id,
                        ParentPortfolio = parentPortfolioEntity,
                        UserId = userCreated.Id,
                        User = userCreated
                    };

                    await portfolioRepository.InsertAsync(_entity);
                }

                await RealizaGetPaginado(userCreated.Id, portfolioRepository);

                Thread.Sleep(1000);
                var lastSyncDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                Thread.Sleep(1000);

                for (int i = 1; i <= RECORD_NUMBER; i++)
                {
                    PortfolioEntity _entity = new PortfolioEntity
                    {
                        Name = Faker.Name.FullName(),
                        Status = GetStatusTypeRandom(),
                        CategoryId = _categoryCreated.Id,
                        Category = _categoryCreated,
                        ParentPortfolioId = parentPortfolioEntity.Id,
                        ParentPortfolio = parentPortfolioEntity,
                        UserId = userCreated.Id,
                        User = userCreated
                    };

                    await portfolioRepository.InsertAsync(_entity);
                }

                await RealizaGetLasSyncDate(userCreated.Id, portfolioRepository, lastSyncDate, 36);

                Thread.Sleep(1000);
                lastSyncDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                Thread.Sleep(1000);

                //O teste abaixo irá atualizar um número objetos para verificar se retorna corretamente
                for (int i = 10; i < (RECORD_NUMBER + 10); i++)
                {
                    PortfolioEntity _entity = await portfolioRepository.SelectByIdAsync(userCreated.Id, i);

                    await portfolioRepository.UpdateAsync(_entity);
                }

                await RealizaGetLasSyncDate(userCreated.Id, portfolioRepository, lastSyncDate, 10);
            }
        }
    }
}