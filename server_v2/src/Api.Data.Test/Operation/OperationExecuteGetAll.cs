using System.Globalization;
using Api.Data.Repository;
using Api.Data.Test.Helpers;
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
        public OperationExecuteGetAll(DbTest dbTest) : base(dbTest) { }

        [Fact(DisplayName = "Get de Operação")]
        [Trait("GET", "OperationEntity")]
        public async Task Eh_Possivel_Realizar_Get_Operacao()
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
                    Name = "Animais Estimação",
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
                        UserId = userCreated.Id,
                        User = userCreated
                    };

                    await _repositorio.InsertAsync(_entity);
                }

                await RealizaGetPaginado(userCreated.Id, _repositorio);

                // Testes aplicando filtros espefícicos
                PageParams pageParams = new PageParams
                {
                    PageSize = RECORD_NUMBER,
                    PageNumber = 1,
                    Tipo = (int)OperationType.Credito
                };

                var operacoesSelecionadas = await _repositorio.SelectByParamAsync(userCreated.Id, pageParams);
                Assert.NotNull(operacoesSelecionadas);
                Assert.True(operacoesSelecionadas.Itens.Count > 0);
                Assert.True(operacoesSelecionadas.Itens.FindAll(x => x.Type == OperationType.Credito).Count > 0);
                Assert.False(operacoesSelecionadas.Itens.FindAll(x => x.Type == OperationType.Debito).Count > 0);
                Assert.False(operacoesSelecionadas.Itens.FindAll(x => x.Type == OperationType.Transferencia).Count > 0);

                pageParams.Tipo = (int)OperationType.Debito;

                operacoesSelecionadas = await _repositorio.SelectByParamAsync(userCreated.Id, pageParams);
                Assert.NotNull(operacoesSelecionadas);
                Assert.True(operacoesSelecionadas.Itens.Count > 0);
                Assert.True(operacoesSelecionadas.Itens.FindAll(x => x.Type == OperationType.Debito).Count > 0);
                Assert.False(operacoesSelecionadas.Itens.FindAll(x => x.Type == OperationType.Credito).Count > 0);
                Assert.False(operacoesSelecionadas.Itens.FindAll(x => x.Type == OperationType.Transferencia).Count > 0);

                pageParams.Tipo = (int)OperationType.Transferencia;

                operacoesSelecionadas = await _repositorio.SelectByParamAsync(userCreated.Id, pageParams);
                Assert.NotNull(operacoesSelecionadas);
                Assert.True(operacoesSelecionadas.Itens.Count > 0);
                Assert.True(operacoesSelecionadas.Itens.FindAll(x => x.Type == OperationType.Transferencia).Count > 0);
                Assert.False(operacoesSelecionadas.Itens.FindAll(x => x.Type == OperationType.Debito).Count > 0);
                Assert.False(operacoesSelecionadas.Itens.FindAll(x => x.Type == OperationType.Credito).Count > 0);

                Thread.Sleep(1000);
                var lastSyncDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

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
                        UserId = userCreated.Id,
                        User = userCreated
                    };

                    await _repositorio.InsertAsync(_entity);
                }

                await RealizaGetLasSyncDate(userCreated.Id, _repositorio, lastSyncDate, 36);

                Thread.Sleep(1000);
                lastSyncDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                //O teste abaixo irá atualizar um número objetos para verificar se retorna corretamente
                for (int i = 10; i < (RECORD_NUMBER + 10); i++)
                {
                    OperationEntity _entity = await _repositorio.SelectByIdAsync(userCreated.Id, i);

                    await _repositorio.UpdateAsync(_entity);
                }

                await RealizaGetLasSyncDate(userCreated.Id, _repositorio, lastSyncDate, 10);
            }
        }
    }
}