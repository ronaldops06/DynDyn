using System.Globalization;
using Api.Data.Test.Helpers;
using Api.Domain.Entities;
using Data.Context;
using Data.Repository;
using Domain.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static Api.Data.Test.Helpers.BaseHelper;
using static Api.Data.Test.Helpers.AttributeHelper;

namespace Api.Data.Test.Attribute
{
    public class ExecuteGetAll : BaseTestGet<AttributeEntity>, IClassFixture<DbTest>
    {
        public ExecuteGetAll(DbTest dbTest) : base(dbTest) { }

        [Fact(DisplayName = "Get de Atributo")]
        [Trait("GET", "AttributeEntity")]
        public async Task Eh_Possivel_Realizar_Get_Atributo()
        {
            using (var context = serviceProvider.GetService<SomniaContext>())
            {
                UserRepository userRepository = new UserRepository(context);
                var userCreated = await userRepository.InsertAsync(UserHelper.GetLoggedUserFake());
                Assert.NotNull(userCreated);
                Assert.True(userCreated.Id > 0);
                
                //Referêcias

                AttributeRepository _repositorio = new AttributeRepository(context);

                for (int i = 1; i <= RECORD_NUMBER; i++)
                {
                    AttributeEntity _entity = new AttributeEntity
                    {
                        Name = Faker.Name.FullName(),
                        Description = Faker.Lorem.Sentence(),
                        Status = GetStatusTypeRandom(),
                        DataType = GetAttributeDataTypeRandom(),
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

                var attributeSelecionadas = await _repositorio.SelectByParamAsync(userCreated.Id, pageParams);
                Assert.NotNull(attributeSelecionadas);
                Assert.True(attributeSelecionadas.Itens.Count > 0);

                Thread.Sleep(1000);
                var lastSyncDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                for (int i = 1; i <= RECORD_NUMBER; i++)
                {
                    AttributeEntity _entity = new AttributeEntity
                    {
                        Name = Faker.Name.FullName(),
                        Description = Faker.Lorem.Sentence(),
                        Status = GetStatusTypeRandom(),
                        DataType = GetAttributeDataTypeRandom(),
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
                    AttributeEntity _entity = await _repositorio.SelectByIdAsync(userCreated.Id, i);

                    await _repositorio.UpdateAsync(_entity);
                }

                await RealizaGetLasSyncDate(userCreated.Id, _repositorio, lastSyncDate, 10);
            }
        }
    }
}