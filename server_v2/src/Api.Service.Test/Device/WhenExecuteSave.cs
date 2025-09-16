using Api.Domain.Interfaces.Services;
using Api.Service.Services;
using Domain.Entities;
using Moq;
using Service.Services;
using Xunit;

namespace Api.Service.Test.Device;

public class WhenExecuteSave : DeviceTest
{
    [Fact(DisplayName = "É possível executar o método Save.")]
    public async Task Eh_Possivel_Executar_Metodo_Save()
    {
        var deviceEntity = Mapper.Map<DeviceEntity>(deviceModel);
        var deviceEntityResult = Mapper.Map<DeviceEntity>(deviceModelResult);
        var deviceEntityUpdateResult = Mapper.Map<DeviceEntity>(deviceModelUpdateResult);
        
        RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(It.IsAny<DeviceEntity>());
        RepositoryMock.Setup(m => m.InsertAsync(It.IsAny<DeviceEntity>())).ReturnsAsync(deviceEntityResult);
        RepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<DeviceEntity>())).ReturnsAsync(deviceEntityUpdateResult);
        IDeviceService service = new DeviceService(UserServiceMock.Object, RepositoryMock.Object, NotificationServiceMock.Object, Mapper);
        
        //Insert
        var result = await service.ExecuteSaveDevice(deviceModel);
        ApplyTest(deviceModel, result);
        
        //Update
        RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(deviceEntityResult);
        RepositoryMock.Setup(m => m.InsertAsync(It.IsAny<DeviceEntity>())).ReturnsAsync(deviceEntityResult);
        RepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<DeviceEntity>())).ReturnsAsync(deviceEntityUpdateResult);
        service = new DeviceService(UserServiceMock.Object, RepositoryMock.Object, NotificationServiceMock.Object, Mapper);
        
        result = await service.ExecuteSaveDevice(deviceModelUpdate);
        ApplyTest(deviceModelUpdate, result);
    }
}