using Api.Domain.Interfaces.Services;
using Domain.Models;
using Domain.Repository;
using Moq;
using Xunit;

namespace Api.Service.Test.Device;

public class DeviceTest : BaseTestService
{
    protected Mock<IDeviceRepository> RepositoryMock = new Mock<IDeviceRepository>();
    protected Mock<INotificationService> NotificationServiceMock = new Mock<INotificationService>();
    protected DeviceModel deviceModel;
    protected DeviceModel deviceModelResult;
    protected DeviceModel deviceModelUpdate;
    protected DeviceModel deviceModelUpdateResult;
    
    protected DeviceTest()
    {
        deviceModel = new DeviceModel
        {
            Id = 1,
            PhisicalDeviceId = "fdafda78d6f6as8",
            NotificationToken = "kjajjfa98fad87f89ayjkfnay7fd-jajfdd8a8f-fadjkjk",
            User = UserModelFake,
            UserId = UserModelFake.Id
        };
        
        deviceModelResult = new DeviceModel
        {
            Id = deviceModel.Id,
            PhisicalDeviceId = deviceModel.PhisicalDeviceId,
            NotificationToken = deviceModel.NotificationToken,
            User = deviceModel.User,
            UserId = deviceModel.UserId,
            DataCriacao = DateTime.UtcNow,
            DataAlteracao = DateTime.UtcNow,
        };
        
        deviceModelUpdate = new DeviceModel
        {
            Id = 1,
            PhisicalDeviceId = "fdafda78d6f6as8",
            NotificationToken = "kjajjfa98fad87f89ayjkfnay7fd-jddfdadfdfda",
            User = UserModelFake,
            UserId = UserModelFake.Id
        };

        deviceModelUpdateResult = new DeviceModel
        {
            Id = deviceModelUpdate.Id,
            PhisicalDeviceId = deviceModelUpdate.PhisicalDeviceId,
            NotificationToken = deviceModelUpdate.NotificationToken,
            User = deviceModelUpdate.User,
            UserId = deviceModelUpdate.UserId,
            DataCriacao = DateTime.UtcNow,
            DataAlteracao = DateTime.UtcNow,
        };
    }
    
    protected void ApplyTest(DeviceModel deviceModelSource, DeviceModel deviceModelDest)
    {
        Assert.NotNull(deviceModelDest);
        Assert.NotNull(deviceModelDest.User);
        Assert.True(deviceModelDest.UserId > 0);
        Assert.Equal(deviceModelSource.Id, deviceModelDest.Id);
        Assert.Equal(deviceModelSource.PhisicalDeviceId, deviceModelDest.PhisicalDeviceId);
        Assert.Equal(deviceModelSource.NotificationToken, deviceModelDest.NotificationToken);
        Assert.Equal(deviceModelSource.UserId, deviceModelDest.UserId);
        Assert.Equal(deviceModelSource.User.Id, deviceModelDest.User.Id);
    }
}