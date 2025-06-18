using System.Text.Json;
using Application.V1.Controllers;
using Domain.Dtos.User;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.TransientUser.WhenRequestUserValidate;

public class ReturnCreated : BaseTestApplication
{
    private TransientUserController _controller;

    [Fact(DisplayName = "É possível realizar o Create")]
    public async Task Eh_Possivel_Invocar_Controller_Create()
    {
        var serviceMock = new Mock<ITransientUserService>();
        var name = Faker.Name.FullName();
        var login = Faker.Internet.Email();
        var password = GeneratePassword(8);
        var verificationCode = Faker.RandomNumber.Next(100000, 999999);

        var userModel = new TransientUserModel
        {
            Id = 1,
            Name = name,
            Login = login,
            Password = password,
            VerificationCode = verificationCode,
            AccessToken = "jfdkjfa7fdafd76f54fajndfdffddfa909dfkk.fd6d54dfh",
            ExpirationDate = DateTime.Now.AddMinutes(15),
            DataAlteracao = DateTime.UtcNow,
            DataCriacao = DateTime.UtcNow
        };

        serviceMock.Setup(m => m.ExecuteVerificationCode(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(userModel);

        _controller = new TransientUserController(serviceMock.Object, Mapper);

        Mock<IUrlHelper> url = new Mock<IUrlHelper>();
        url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");
        _controller.Url = url.Object;

        var validationUserDto = new ValidationUserDto
        {
            Login = login,
            VerificationCode = verificationCode,
        };

        var result = await _controller.Post(validationUserDto);

        string jsonString = JsonSerializer.Serialize(result);
        Assert.True(result is CreatedResult);

        var resultValue = ((CreatedResult)result).Value as LoginResponseDto;
        Assert.NotNull(resultValue);
        Assert.Equal(userModel.Name, resultValue.Name);
        Assert.Equal(userModel.Login, resultValue.Login);
        Assert.Equal(userModel.AccessToken, resultValue.AccessToken);
    }
}