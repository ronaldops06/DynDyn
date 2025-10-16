using Api.Domain.Dtos.Device;
using Domain.Entities;
using Domain.Models;
using Xunit;

namespace Api.Application.Test.AutoMapper;

public class DeviceMapper : BaseTestApplication
{
    [Fact(DisplayName = "É possível mapear os modelos")]
    public void Eh_Possivel_Mapear_Os_Modelos()
    {
        var deviceRequestDto = new DeviceRequestDto
        {
            PhisicalDeviceId = "fdafda78d6f6as8",
            NotificationToken = "kjajjfa98fad87f89ayjkfnay7fd-jajfdd8a8f-fadjkjk"
        };
        
        //Dto -> Model
        var model = Mapper.Map<DeviceModel>(deviceRequestDto);
        Assert.Equal(deviceRequestDto.Id, model.Id);
        Assert.Equal(deviceRequestDto.PhisicalDeviceId, model.PhisicalDeviceId);
        Assert.Equal(deviceRequestDto.NotificationToken, model.NotificationToken);
        
        //Model -> Entity
        var entity = Mapper.Map<DeviceEntity>(model);
        Assert.Equal(model.Id, entity.Id);
        Assert.Equal(model.PhisicalDeviceId, entity.PhisicalDeviceId);
        Assert.Equal(model.NotificationToken, entity.NotificationToken);
        
    }
}