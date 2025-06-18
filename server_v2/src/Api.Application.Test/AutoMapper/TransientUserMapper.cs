using Domain.Dtos.User;
using Domain.Models;
using Xunit;

namespace Api.Application.Test.AutoMapper;

public class TransientUserMapper : BaseTestApplication
{
    [Fact(DisplayName = "É possível mapear os modelos")]
    public void Eh_Possivel_Mapear_Os_Modelos()
    {
        var userRequestDto = new UserRequestDto
        {
            Name = Faker.Name.FullName(),
            Login = Faker.Internet.Email(),
            Password = Faker.Lorem.GetFirstWord(),
            Role = "Admin"
        };
        
        var transientUserModel = Mapper.Map<TransientUserModel>(userRequestDto);
        Assert.Equal(transientUserModel.Name, userRequestDto.Name);
        Assert.Equal(transientUserModel.Login, userRequestDto.Login);
        Assert.Equal(transientUserModel.Password, userRequestDto.Password);
        
        var userResponseDto = Mapper.Map<LoginResponseDto>(transientUserModel);
        Assert.Equal(userResponseDto.Name, transientUserModel.Name);
        Assert.Equal(userResponseDto.Login, transientUserModel.Login);
        Assert.Equal(userResponseDto.AccessToken, transientUserModel.AccessToken);
    }
}