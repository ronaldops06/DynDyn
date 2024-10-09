using Api.Data.Repository;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Data.Context;
using Data.Repository;
using Domain.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static Api.Data.Test.Helpers.BaseHelper;
using static Api.Data.Test.Helpers.OperationHelper;

namespace Api.Data.Test.Operation
{
    public class OperationExecuteGetAll : BaseTestGet<OperationEntity>, IClassFixture<DbTest>
    {
        private static readonly int RECORD_NUMBER = 35;

        public OperationExecuteGetAll(DbTest dbTest) : base(dbTest) { }

        [Fact(DisplayName = "Get de Operação")]
        [Trait("GET", "OperationEntity")]
        public async Task Eh_Possivel_Realizar_Get_Operacao()
        {
            using (var context = serviceProvider.GetService<SomniaContext>())
            {
                CategoryRepository _repositorioCategory = new CategoryRepository(context);
                CategoryEntity _categoryEntity = new CategoryEntity()
                {
                    Name = "Animais Estimação",
                    Type = CategoryType.Conta,
                    Status = StatusType.Ativo,
                };

                var _categoryCreated = await _repositorioCategory.InsertAsync(_categoryEntity);
                Assert.NotNull(_categoryCreated);
                Assert.True(_categoryCreated.Id > 0);
                Assert.Equal(_categoryEntity.Name, _categoryCreated.Name);
                Assert.Equal(_categoryEntity.Type, _categoryCreated.Type);
                Assert.Equal(_categoryEntity.Status, _categoryCreated.Status);

                OperationRepository _repositorio = new OperationRepository(context);

                for (int i = 1; i <= RECORD_NUMBER; i++)
                {
                    OperationEntity _entity = new OperationEntity
                    {
                        Name = Faker.Name.FullName(),
                        Recurrent = false,
                        Type = GetOperationTypeRandom(),
                        Status = GetStatusTypeRandom(),
                        CategoryId = _categoryCreated.Id,
                        Category = _categoryCreated,
                    };

                    await _repositorio.InsertAsync(_entity);
                }

                await RealizaGetPaginado(_repositorio);

                // Testes aplicando filtros espefícicos
                PageParams pageParams = new PageParams
                {
                    PageSize = RECORD_NUMBER,
                    PageNumber = 1,
                    Tipo = (int)OperationType.Credito
                };

                var operacoesSelecionadas = await _repositorio.SelectByParamAsync(pageParams);
                Assert.NotNull(operacoesSelecionadas);
                Assert.True(operacoesSelecionadas.Itens.Count > 0);
                Assert.True(operacoesSelecionadas.Itens.FindAll(x => x.Type == OperationType.Credito).Count > 0);
                Assert.False(operacoesSelecionadas.Itens.FindAll(x => x.Type == OperationType.Debito).Count > 0);
                Assert.False(operacoesSelecionadas.Itens.FindAll(x => x.Type == OperationType.Transferencia).Count > 0);

                pageParams.Tipo = (int)OperationType.Debito;

                operacoesSelecionadas = await _repositorio.SelectByParamAsync(pageParams);
                Assert.NotNull(operacoesSelecionadas);
                Assert.True(operacoesSelecionadas.Itens.Count > 0);
                Assert.True(operacoesSelecionadas.Itens.FindAll(x => x.Type == OperationType.Debito).Count > 0);
                Assert.False(operacoesSelecionadas.Itens.FindAll(x => x.Type == OperationType.Credito).Count > 0);
                Assert.False(operacoesSelecionadas.Itens.FindAll(x => x.Type == OperationType.Transferencia).Count > 0);

                pageParams.Tipo = (int)OperationType.Transferencia;

                operacoesSelecionadas = await _repositorio.SelectByParamAsync(pageParams);
                Assert.NotNull(operacoesSelecionadas);
                Assert.True(operacoesSelecionadas.Itens.Count > 0);
                Assert.True(operacoesSelecionadas.Itens.FindAll(x => x.Type == OperationType.Transferencia).Count > 0);
                Assert.False(operacoesSelecionadas.Itens.FindAll(x => x.Type == OperationType.Debito).Count > 0);
                Assert.False(operacoesSelecionadas.Itens.FindAll(x => x.Type == OperationType.Credito).Count > 0);
            }
        }
    }
}