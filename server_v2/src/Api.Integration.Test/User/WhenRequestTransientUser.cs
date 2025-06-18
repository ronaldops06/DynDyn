using System.Net;
using Newtonsoft.Json;
using Domain.Dtos.User;
using Xunit;

namespace Api.Integration.Test.User;

public class WhenRequestTransientUser : BaseIntegration
{
    private class UserTransientBase
    {
        public string UserName { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
    }
    
    public static string GeneratePassword(int length)
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var result = new string(
            Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        return result;
    }

    [Fact(DisplayName = "CRUD do Usuário de transição")]
    public async Task Eh_Possivel_Realizar_Crud_Usuario()
    {
        //await AdicionarToken();
        var userBase = new UserTransientBase()
        {
            UserName = Faker.Name.FullName(),
            UserLogin = Faker.Internet.Email(),
            UserPassword = GeneratePassword(8)
        };

        var userRequestDto = new UserRequestDto
        {
            Role = "Admin"
        };

        //Required
        var response = await PostJsonAsync(userRequestDto, $"{HostApi}/TransientUser", Client);
        var postResult = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("Name é um campo obrigatório", postResult);
        Assert.Contains("Login é um campo obrigatório", postResult);
        Assert.Contains("Password é um campo obrigatório", postResult);

        //Post
        userRequestDto.Name = userBase.UserName;
        userRequestDto.Login = userBase.UserLogin;
        userRequestDto.Password = userBase.UserPassword;

        response = await PostJsonAsync(userRequestDto, $"{HostApi}/TransientUser", Client);
        postResult = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        //2º Post
        response = await PostJsonAsync(userRequestDto, $"{HostApi}/TransientUser", Client);
        postResult = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        //Post validação
        //Não é possível validar pois o VerificationCode só é gerado internamente e enviado por e-mail, não sendo exposto
    }
}