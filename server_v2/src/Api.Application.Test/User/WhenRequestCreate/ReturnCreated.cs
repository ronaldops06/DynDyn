using Application.V1.Controllers;
using AutoMapper;
using Domain.Dtos.User;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Api.Application.Test.User.WhenRequestCreate
{
    public class ReturnCreated : BaseTestApplication
    {
        private UserController _controller;

        [Fact(DisplayName = "É possível realizar o Create")]
        public async Task Eh_Possivel_Invocar_Controller_Create()
        {
            var serviceMock = new Mock<IUserService>();
            var name = Faker.Name.FullName();
            var login = Faker.Internet.Email();
            var password = GeneratePassword(8);

            var userModel = new UserModel
            {
                Id = 1,
                Name = name,
                Login = login,
                Password = password,
                Role = "",
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            serviceMock.Setup(m => m.Post(It.IsAny<UserModel>())).ReturnsAsync(userModel);

            _controller = new UserController(serviceMock.Object, Mapper);

            Mock<IUrlHelper> url = new Mock<IUrlHelper>();
            url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");
            _controller.Url = url.Object;

            var userRequestDto = new UserRequestDto
            {
                Name = name,
                Login = login,
                Password = password,
                Role = ""
            };

            var result = await _controller.Post(userRequestDto);

            string jsonString = JsonSerializer.Serialize(result);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as UserResponseDto;
            Assert.NotNull(resultValue);
            Assert.Equal(userRequestDto.Name, resultValue.Name);
            Assert.Equal(userRequestDto.Login, resultValue.Login);
        }
    }
}
