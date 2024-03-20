using Api.Domain.Entities;
using Api.Domain.Enums;
using Data.Context;
using Data.Repository;
using Domain.Helpers;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test.Category
{
    public class CategoryExecuteGetAll : BaseTestGet<CategoryEntity>, IClassFixture<DbTest>
    {
        public CategoryExecuteGetAll(DbTest dbTest) : base(dbTest) { }

        public CategoryType GetCategoryTypeRandom()
        {
            Array values = Enum.GetValues(typeof(CategoryType));

            Random random = new Random();
            return (CategoryType)values.GetValue(random.Next(values.Length));
        }

        public StatusType GetStatusTypeRandom()
        {
            Array values = Enum.GetValues(typeof(StatusType));

            Random random = new Random();
            return (StatusType)values.GetValue(random.Next(values.Length));
        }

        [Fact(DisplayName = "Get de Categoria")]
        [Trait("GET", "CategoryEntity")]
        public async Task Eh_Possivel_Realizar_Get_Categoria()
        {
            using (var context = serviceProvider.GetService<SomniaContext>())
            {
                CategoryRepository _repositorio = new CategoryRepository(context);
                List<CategoryEntity> listCategoryDto = new List<CategoryEntity>();

                for (int i = 1; i < 35; i++)
                {
                    CategoryEntity _entity = new CategoryEntity
                    {
                        Nome = Faker.Name.FullName(),
                        Tipo = GetCategoryTypeRandom(),
                        Status = GetStatusTypeRandom(),
                    };

                    await _repositorio.InsertAsync(_entity);
                }

                base.RealizaGetPaginado(_repositorio);

                PageParams pageParams = new PageParams
                {
                    PageSize = 35,
                    PageNumber = 1,
                    Tipo = (int)CategoryType.Conta
                };

                var categoriasSelecionadas = await _repositorio.SelectByParamAsync(pageParams);
                Assert.NotNull(categoriasSelecionadas);
                Assert.True(categoriasSelecionadas.Itens.Count > 0);
                Assert.True(categoriasSelecionadas.Itens.FindAll(x => x.Tipo == CategoryType.Conta).Count > 0);
                Assert.False(categoriasSelecionadas.Itens.FindAll(x => x.Tipo == CategoryType.Operação).Count > 0);

                pageParams.Tipo = (int)CategoryType.Operação;

                categoriasSelecionadas = await _repositorio.SelectByParamAsync(pageParams);
                Assert.NotNull(categoriasSelecionadas);
                Assert.True(categoriasSelecionadas.Itens.Count > 0);
                Assert.True(categoriasSelecionadas.Itens.FindAll(x => x.Tipo == CategoryType.Operação).Count > 0);
                Assert.False(categoriasSelecionadas.Itens.FindAll(x => x.Tipo == CategoryType.Conta).Count > 0);
            }
        }
    }
}