using System.Net;
using Api.Domain.Dtos.Device;
using Newtonsoft.Json;
using Xunit;

namespace Api.Integration.Test.Device;

public class WhenRequestDevice : BaseIntegration
{
    [Fact(DisplayName = "Save de Device")]
    public async Task Eh_Possivel_Realizar_Save()
    {
        await AdicionarToken();
        
        var deviceRequestDto = new DeviceRequestDto()
        {
            PhisicalDeviceId = null,
            NotificationToken = "jafkjdjfdfad8s7f8a7fafnaf3n3n3jjd"
        };
        
        //Required
        var response = await PostJsonAsync(deviceRequestDto, $"{HostApi}/Device/RegisterDevice", Client);
        var postResult = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("PhisicalDeviceId é um campo obrigatório", postResult);

        deviceRequestDto.PhisicalDeviceId = "fadfa5f46adf84";
        
        //Post
        response = await PostJsonAsync(deviceRequestDto, $"{HostApi}/Device/RegisterDevice", Client);
        postResult = await response.Content.ReadAsStringAsync();
        var registroPost = JsonConvert.DeserializeObject<DeviceResponseDto>(postResult);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(registroPost);
        Assert.True(registroPost.Id > 0);
        Assert.Equal(deviceRequestDto.PhisicalDeviceId, registroPost.PhisicalDeviceId);
        Assert.Equal(deviceRequestDto.NotificationToken, registroPost.NotificationToken);
        
        //Post - Mesmo Device
        deviceRequestDto.NotificationToken = "fjadjfjafjadsjfadlkjfkdjaskf48787f4d5a4";
        
        response = await PostJsonAsync(deviceRequestDto, $"{HostApi}/Device/RegisterDevice", Client);
        postResult = await response.Content.ReadAsStringAsync();
        Console.WriteLine(postResult);
        var registroPostMesmoDevice = JsonConvert.DeserializeObject<DeviceResponseDto>(postResult);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(registroPostMesmoDevice);
        Assert.Equal(registroPost.Id, registroPostMesmoDevice.Id);
        Assert.Equal(deviceRequestDto.PhisicalDeviceId, registroPostMesmoDevice.PhisicalDeviceId);
        Assert.Equal(deviceRequestDto.NotificationToken, registroPostMesmoDevice.NotificationToken);
        
        //Post - Novo Device
        deviceRequestDto.NotificationToken = "uudfuaf97974561231321321dfadaf44";
        deviceRequestDto.PhisicalDeviceId = "77889911";
        
        response = await PostJsonAsync(deviceRequestDto, $"{HostApi}/Device/RegisterDevice", Client);
        postResult = await response.Content.ReadAsStringAsync();
        var registroPostNovoDevice = JsonConvert.DeserializeObject<DeviceResponseDto>(postResult);
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(registroPostNovoDevice);
        Assert.NotEqual(registroPostMesmoDevice.Id, registroPostNovoDevice.Id);
        Assert.Equal(deviceRequestDto.PhisicalDeviceId, registroPostNovoDevice.PhisicalDeviceId);
        Assert.Equal(deviceRequestDto.NotificationToken, registroPostNovoDevice.NotificationToken);
        
        //Post - Mesmo Device e User Diferente
        await AdicionarTokenUsuarioAdicional();
        
        response = await PostJsonAsync(deviceRequestDto, $"{HostApi}/Device/RegisterDevice", Client);
        postResult = await response.Content.ReadAsStringAsync();
        var registroPostNovoUser = JsonConvert.DeserializeObject<DeviceResponseDto>(postResult);
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(registroPostNovoDevice);
        Assert.NotEqual(registroPost.Id, registroPostNovoUser.Id);
        Assert.NotEqual(registroPostMesmoDevice.Id, registroPostNovoUser.Id);
        Assert.NotEqual(registroPostNovoDevice.Id, registroPostNovoUser.Id);
    }
}