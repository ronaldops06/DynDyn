using Domain.Entities;
using Domain.Models;
using Xunit;

namespace Api.Service.Test.AutoMapper;

public class DeviceMapper : BaseTestService
{
    [Fact(DisplayName = "É possível mapear os modelos")]
    public void Eh_Possivel_Mapear_Os_Modelos()
    {
        DeviceModel model = new DeviceModel
        {
            PhisicalDeviceId = "fdafda78d6f6as8",
            NotificationToken = "kjajjfa98fad87f89ayjkfnay7fd-jajfdd8a8f-fadjkjk",
            User = UserModelFake,
            UserId = UserModelFake.Id
        };
        
        //Model -> Entity
        var entity = Mapper.Map<DeviceEntity>(model);
        Assert.Equal(entity.Id, model.Id);
        Assert.Equal(entity.PhisicalDeviceId, model.PhisicalDeviceId);
        Assert.Equal(entity.NotificationToken, model.NotificationToken);
        Assert.Equal(entity.UserId, model.UserId);
        Assert.Equal(entity.User.Id, model.User.Id);
        
        //Entity -> Model
        var deviceModel = Mapper.Map<DeviceModel>(entity);
        Assert.Equal(deviceModel.Id, entity.Id);
        Assert.Equal(deviceModel.PhisicalDeviceId, entity.PhisicalDeviceId);
        Assert.Equal(deviceModel.NotificationToken, entity.NotificationToken);
        Assert.Equal(deviceModel.UserId, entity.UserId);
        Assert.Equal(deviceModel.User.Id, entity.User.Id);
    }
    
    [Fact(DisplayName = "É possível mapear os modelos em lista")]
    public void Eh_Possivel_Mapear_Os_Modelos_Em_Lista()
    {
        var userEntity = Mapper.Map<UserEntity>(UserModelFake);
        var listEntity = new List<DeviceEntity>();

        for (int i = 1; i <= 5; i++)
        {
            var item = new DeviceEntity
            {
                Id = i,
                PhisicalDeviceId = "fdafda78d6f6as8",
                NotificationToken = "kjajjfa98fad87f89ayjkfnay7fd-jajfdd8a8f-fadjkjk",
                UserId = UserModelFake.Id,
                User = Mapper.Map<UserEntity>(UserModelFake)
            };
            
            listEntity.Add(item);
        }
        
        //List<Entity> -> List<Model>
        var listModel = Mapper.Map<List<DeviceModel>>(listEntity);

        Assert.True(listModel.Count() == listEntity.Count());

        for (int i = 0; i < listModel.Count(); i++)
        {
            Assert.Equal(listModel[i].Id, listEntity[i].Id);
            Assert.Equal(listModel[i].PhisicalDeviceId, listEntity[i].PhisicalDeviceId);
            Assert.Equal(listModel[i].NotificationToken, listEntity[i].NotificationToken);
            Assert.Equal(listModel[i].UserId, listEntity[i].UserId);
            Assert.Equal(listModel[i].User.Id, listEntity[i].User.Id);
        }
    }
}