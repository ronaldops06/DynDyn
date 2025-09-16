using System.Text.Json;
using Api.Domain.Dtos.Device;
using Api.Domain.Interfaces.Services;
using Application.V1.Controllers;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Device;

public class WhenRequestSave : BaseTestApplication
{
    [Fact(DisplayName = "É possível realizar o Save")]
    public async Task Eh_Possivel_Invocar_Controller_Save()
    {
        var serviceMock = new Mock<IDeviceService>();

        var deviceModel = new DeviceModel
        {
            Id = 1,
            PhisicalDeviceId = "123456",
            NotificationToken = "jafkjdjfdfad8s7f8a7fafnaf3n3n3jjd",
            DataAlteracao = DateTime.UtcNow,
            DataCriacao = DateTime.UtcNow
        };
        
        serviceMock.Setup(m => m.ExecuteSaveDevice(It.IsAny<DeviceModel>())).ReturnsAsync(deviceModel);
        
        var controller = new DeviceController(serviceMock.Object, Mapper);
        
        Mock<IUrlHelper> url = new Mock<IUrlHelper>();
        url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");
        controller.Url = url.Object;
        
        var deviceRequestDto = new DeviceRequestDto()
        {
            PhisicalDeviceId = "123456",
            NotificationToken = "jafkjdjfdfad8s7f8a7fafnaf3n3n3jjd"
        };
        
        var result = await controller.Post(deviceRequestDto);
        
        string jsonString = JsonSerializer.Serialize(result);
        Assert.True(result is CreatedResult);
        
        var resultValue = ((CreatedResult)result).Value as DeviceResponseDto;
        Assert.NotNull(resultValue);
        Assert.True(resultValue.Id > 0);
        Assert.Equal(deviceRequestDto.PhisicalDeviceId, resultValue.PhisicalDeviceId);
        Assert.Equal(deviceRequestDto.NotificationToken, resultValue.NotificationToken);
    }
}