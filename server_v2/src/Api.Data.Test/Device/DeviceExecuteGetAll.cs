using System.Globalization;
using Api.Data.Test.Helpers;
using Data.Context;
using Data.Repository;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test.Device;

public class DeviceExecuteGetAll : BaseTestGet<DeviceEntity>, IClassFixture<DbTest>
{
    public DeviceExecuteGetAll(DbTest dbTest) : base(dbTest) { }

    private void AplicaTesteCampos(DeviceEntity deviceEntitySource, DeviceEntity deviceEntityDest)
    {
        Assert.NotNull(deviceEntityDest);
        Assert.Equal(deviceEntitySource.PhisicalDeviceId, deviceEntityDest.PhisicalDeviceId);
        Assert.Equal(deviceEntitySource.NotificationToken, deviceEntityDest.NotificationToken);
    }
    
    [Fact(DisplayName = "Get de Dispositivo")]
    [Trait("GET", "DeviceEntity")]
    public async Task Eh_Possivel_Realizar_Get_Dispositivo()
    {
        using (var context = serviceProvider.GetService<SomniaContext>())
        {
            UserRepository userRepository = new UserRepository(context);
            var userCreated = await userRepository.InsertAsync(UserHelper.GetLoggedUserFake());
            Assert.NotNull(userCreated);
            Assert.True(userCreated.Id > 0);
            
            DeviceRepository _repositorio = new DeviceRepository(context);

            DeviceEntity deviceEntity = new DeviceEntity
            {
                PhisicalDeviceId = "f89s8f87jk3j4",
                NotificationToken = Faker.Identification.UKNationalInsuranceNumber(),
                UserId= userCreated.Id,
                User = userCreated
            };
            
            var _registroCriado = await _repositorio.InsertAsync(deviceEntity);
            AplicaTesteCampos(deviceEntity, _registroCriado);
            Assert.True(_registroCriado.Id > 0);
            
            for (int i = 1; i < RECORD_NUMBER; i++)
            {
                DeviceEntity _entity = new DeviceEntity
                {
                    PhisicalDeviceId = $"f8d7d9ddf{i}",
                    NotificationToken = Faker.Lorem.Words(RECORD_NUMBER).ElementAt(i-1) + i,
                    UserId = userCreated.Id,
                    User = userCreated
                };

                await _repositorio.InsertAsync(_entity);
            }

            await RealizaGetPaginado(userCreated.Id, _repositorio);
            
            Thread.Sleep(1000);
            var lastSyncDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            Thread.Sleep(1000);
            
            for (int i = 1; i <= RECORD_NUMBER; i++)
            {
                DeviceEntity _entity = new DeviceEntity
                {
                    PhisicalDeviceId = $"f8d9d8fsalbn4{i}",
                    NotificationToken = Faker.Lorem.Words(RECORD_NUMBER).ElementAt(i-1),
                    UserId = userCreated.Id,
                    User = userCreated
                };

                await _repositorio.InsertAsync(_entity);
            }

            await RealizaGetLasSyncDate(userCreated.Id, _repositorio, lastSyncDate, 36);
            
            Thread.Sleep(1000);
            lastSyncDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            Thread.Sleep(1000);
            
            //O teste abaixo irá atualizar um número objetos para verificar se retorna corretamente
            for (int i = 10; i < (RECORD_NUMBER + 10); i++)
            {
                DeviceEntity _entity = await _repositorio.SelectByIdAsync(userCreated.Id, i);

                await _repositorio.UpdateAsync(_entity);
            }

            await RealizaGetLasSyncDate(userCreated.Id, _repositorio, lastSyncDate, 10);
        }
    }
}