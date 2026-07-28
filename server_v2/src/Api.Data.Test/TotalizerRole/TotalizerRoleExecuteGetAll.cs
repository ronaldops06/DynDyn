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

namespace Api.Data.Test.TotalizerRole
{
    public class ExecuteGetAll : BaseTestGet<TotalizerRoleEntity>, IClassFixture<DbTest>
    {
        public ExecuteGetAll(DbTest dbTest) : base(dbTest) { }

        [Fact(DisplayName = "Get de papel de totalizador")]
        [Trait("GET", "TotalizerRoleEntity")]
        public async Task Eh_Possivel_Realizar_Get_Totalizer_Role()
        {
            using (var context = serviceProvider.GetService<SomniaContext>())
            {
                UserRepository userRepository = new UserRepository(context);
                var userCreated = await userRepository.InsertAsync(UserHelper.GetLoggedUserFake());
                Assert.NotNull(userCreated);
                Assert.True(userCreated.Id > 0);
                
                //Referêcias

                TotalizerRoleRepository _repositorio = new TotalizerRoleRepository(context);

                for (int i = 1; i <= RECORD_NUMBER; i++)
                {
                    TotalizerRoleEntity _entity = new TotalizerRoleEntity
                    {
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

                var totalizerRoleSelecionadas = await _repositorio.SelectByParamAsync(userCreated.Id, pageParams);
                Assert.NotNull(totalizerRoleSelecionadas);
                Assert.True(totalizerRoleSelecionadas.Itens.Count > 0);
                //Adicionar testes

                Thread.Sleep(1000);
                var lastSyncDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                for (int i = 1; i <= RECORD_NUMBER; i++)
                {
                    TotalizerRoleEntity _entity = new TotalizerRoleEntity
                    {
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
                    TotalizerRoleEntity _entity = await _repositorio.SelectByIdAsync(userCreated.Id, i);

                    await _repositorio.UpdateAsync(_entity);
                }

                await RealizaGetLasSyncDate(userCreated.Id, _repositorio, lastSyncDate, 10);
            }
        }
    }
}