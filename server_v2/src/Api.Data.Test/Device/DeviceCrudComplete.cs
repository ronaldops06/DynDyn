using Api.Data.Test.Helpers;
using Data.Context;
using Data.Repository;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test.Device;

public class DeviceCrudComplete : IClassFixture<DbTest>
{
    private ServiceProvider _serviceProvider;

    public DeviceCrudComplete(DbTest dbTest)
    {
        _serviceProvider = dbTest.ServiceProvider;
    }
    
    private void AplicaTesteCampos(DeviceEntity deviceEntitySource, DeviceEntity deviceEntityDest)
    {
        Assert.NotNull(deviceEntityDest);
        Assert.Equal(deviceEntitySource.PhisicalDeviceId, deviceEntityDest.PhisicalDeviceId);
        Assert.Equal(deviceEntitySource.NotificationToken, deviceEntityDest.NotificationToken);
    }

    [Fact(DisplayName = "CRUD de dispositivo (excepions)")]
    [Trait("CRUD", "DeviceEntity")]
    public async Task Eh_Lancado_Excecao_Campos_Nao_Informados()
    {
        using (var context = _serviceProvider.GetService<SomniaContext>())
        {
            UserRepository userRepository = new UserRepository(context);
            var userCreated = await userRepository.InsertAsync(UserHelper.GetLoggedUserFake());
            Assert.NotNull(userCreated);
            Assert.True(userCreated.Id > 0);
            
            DeviceRepository _repositorio = new DeviceRepository(context);

            DeviceEntity deviceEntity = new DeviceEntity
            {
                PhisicalDeviceId = Faker.Lorem.GetFirstWord(),
                NotificationToken = Faker.Identification.UKNationalInsuranceNumber(),
            };
            
            Func<Task> act = () => _repositorio.InsertAsync(deviceEntity);
            var exception = Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Inner Exception: SQLite Error 19: 'FOREIGN KEY constraint failed'.", exception.Result.Message);

            deviceEntity.UserId = userCreated.Id;
            deviceEntity.User = userCreated;
            deviceEntity.PhisicalDeviceId = null;
            act = () => _repositorio.InsertAsync(deviceEntity);
            exception = Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Inner Exception: SQLite Error 19: 'NOT NULL constraint failed: Device.PhisicalDeviceId'.", exception.Result.Message);
        }
    }

    [Fact(DisplayName = "CRUD de dispositivo")]
    [Trait("CRUD", "DeviceEntity")]
    public async Task Eh_Possivel_Realizar_CRUD_Device()
    {
        using (var context = _serviceProvider.GetService<SomniaContext>())
        {
            UserRepository userRepository = new UserRepository(context);
            var userCreated = await userRepository.InsertAsync(UserHelper.GetLoggedUserFake());
            Assert.NotNull(userCreated);
            Assert.True(userCreated.Id > 0);
            
            DeviceRepository _repositorio = new DeviceRepository(context);

            DeviceEntity deviceEntity = new DeviceEntity
            {
                PhisicalDeviceId = Faker.Lorem.GetFirstWord(),
                NotificationToken = Faker.Identification.UKNationalInsuranceNumber(),
                UserId = userCreated.Id,
                User = userCreated
            };
            
            var _registroCriado = await _repositorio.InsertAsync(deviceEntity);
            AplicaTesteCampos(deviceEntity, _registroCriado);
            Assert.True(_registroCriado.Id > 0);

            deviceEntity.PhisicalDeviceId = Faker.Lorem.Words(3).Last();
            
            var _registroAtualizado = await _repositorio.UpdateAsync(deviceEntity);
            AplicaTesteCampos(deviceEntity, _registroAtualizado);
            
            var _registroExiste = await _repositorio.ExistsAsync(_registroAtualizado.Id);
            Assert.True(_registroExiste);

            var _registroSelecionado = await _repositorio.SelectByIdAsync(userCreated.Id, _registroAtualizado.Id);
            AplicaTesteCampos(deviceEntity, _registroSelecionado);

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