using Api.Domain.Entities;
using Api.Domain.Enums;
using Data.Context;
using Data.Repository;
using Domain.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static Api.Data.Test.Helpers.CategoryHelper;
using static Api.Data.Test.Helpers.BaseHelper;
using System.Globalization;

namespace Api.Data.Test.Category
{
    public class CategoryExecuteGetAll : BaseTestGet<CategoryEntity>, IClassFixture<DbTest>
    {
        public CategoryExecuteGetAll(DbTest dbTest) : base(dbTest) { }

        [Fact(DisplayName = "Get de Categoria")]
        [Trait("GET", "CategoryEntity")]
        public async Task Eh_Possivel_Realizar_Get_Categoria()
        {
            using (var context = serviceProvider.GetService<SomniaContext>())
            {
                CategoryRepository _repositorio = new CategoryRepository(context);
                List<CategoryEntity> listCategoryDto = new List<CategoryEntity>();

                for (int i = 1; i <= RECORD_NUMBER; i++)
                {
                    CategoryEntity _entity = new CategoryEntity
                    {
                        Name = Faker.Name.FullName(),
                        Type = GetCategoryTypeRandom(),
                        Status = GetStatusTypeRandom(),
                    };

                    await _repositorio.InsertAsync(_entity);
                }

                await base.RealizaGetPaginado(_repositorio);

                // Testes aplicando filtros espefícicos
                PageParams pageParams = new PageParams
                {
                    PageSize = RECORD_NUMBER,
                    PageNumber = 1,
                    Tipo = (int)CategoryType.Conta
                };

                var categoriasSelecionadas = await _repositorio.SelectByParamAsync(pageParams);
                Assert.NotNull(categoriasSelecionadas);
                Assert.True(categoriasSelecionadas.Itens.Count > 0);
                Assert.True(categoriasSelecionadas.Itens.FindAll(x => x.Type == CategoryType.Conta).Count > 0);
                Assert.False(categoriasSelecionadas.Itens.FindAll(x => x.Type == CategoryType.Operação).Count > 0);

                pageParams.Tipo = (int)CategoryType.Operação;

                categoriasSelecionadas = await _repositorio.SelectByParamAsync(pageParams);
                Assert.NotNull(categoriasSelecionadas);
                Assert.True(categoriasSelecionadas.Itens.Count > 0);
                Assert.True(categoriasSelecionadas.Itens.FindAll(x => x.Type == CategoryType.Operação).Count > 0);
                Assert.False(categoriasSelecionadas.Itens.FindAll(x => x.Type == CategoryType.Conta).Count > 0);

                Thread.Sleep(1000);
                var lastSyncDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                for (int i = 1; i <= RECORD_NUMBER; i++)
                {
                    CategoryEntity _entity = new CategoryEntity
                    {
                        Name = Faker.Name.FullName(),
                        Type = GetCategoryTypeRandom(),
                        Status = GetStatusTypeRandom(),
                    };

                    await _repositorio.InsertAsync(_entity);
                }

                await RealizaGetLasSyncDate(_repositorio, lastSyncDate, 36);

                Thread.Sleep(1000);
                lastSyncDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                //O teste abaixo irá atualizar um número objetos para verificar se retorna corretamente
                for (int i = 10; i < (RECORD_NUMBER + 10); i++)
                {
                    CategoryEntity _entity = await _repositorio.SelectByIdAsync(i);

                    await _repositorio.UpdateAsync(_entity);
                }

                await RealizaGetLasSyncDate(_repositorio, lastSyncDate, 10);
            }
        }
    }
}