using System.Globalization;
using Api.Data.Test.Helpers;
using Api.Domain.Entities;
using Data.Context;
using Data.Repository;
using Domain.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test.Operation
{
    public class OperationRoleExecuteGetAll : BaseTestGet<OperationRoleEntity>, IClassFixture<DbTest>
    {
        public OperationRoleExecuteGetAll(DbTest dbTest) : base(dbTest) { }

        [Fact(DisplayName = "Get de papeis de operação")]
        [Trait("GET", "OperationRoleEntity")]
        public async Task Eh_Possivel_Realizar_Get_Papel_Operacao()
        {
            using (var context = serviceProvider.GetService<SomniaContext>())
            {
                UserRepository userRepository = new UserRepository(context);
                var userCreated = await userRepository.InsertAsync(UserHelper.GetLoggedUserFake());
                Assert.NotNull(userCreated);
                Assert.True(userCreated.Id > 0);
                
                //Referêcias

                OperationRoleRepository _repositorio = new OperationRoleRepository(context);

                for (int i = 1; i <= RECORD_NUMBER; i++)
                {
                    OperationRoleEntity _entity = new OperationRoleEntity
                    {
                        Name = Faker.Name.FullName(),
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
                };

                var operationRoleSelecionadas = await _repositorio.SelectByParamAsync(userCreated.Id, pageParams);
                Assert.NotNull(operationRoleSelecionadas);
                Assert.True(operationRoleSelecionadas.Itens.Count > 0);
                //Adicionar testes

                Thread.Sleep(1000);
                var lastSyncDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                for (int i = 1; i <= RECORD_NUMBER; i++)
                {
                    OperationRoleEntity _entity = new OperationRoleEntity
                    {
                        Name = Faker.Name.FullName(),
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
                    OperationRoleEntity _entity = await _repositorio.SelectByIdAsync(userCreated.Id, i);

                    await _repositorio.UpdateAsync(_entity);
                }

                await RealizaGetLasSyncDate(userCreated.Id, _repositorio, lastSyncDate, 10);
            }
        }
    }
}