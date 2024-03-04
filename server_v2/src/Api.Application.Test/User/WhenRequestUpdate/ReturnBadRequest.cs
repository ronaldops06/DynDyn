using Application.V1.Controllers;
using Domain.Dtos.User;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.User.WhenRequestUpdate
{
    public class ReturnBadRequest : BaseTestApplication
    {
        private UserController _controller;

        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_BadRequest()
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
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow
            };

            serviceMock.Setup(m => m.Put(It.IsAny<UserModel>())).ReturnsAsync(userModel);

            _controller = new UserController(serviceMock.Object, Mapper);
            _controller.ModelState.AddModelError("Email", "É um campo obrigatório");

            var userRequestDtoUpdate = new UserRequestDto
            {
                Id = 1,
                Name = name,
                Login = login,
                Password = GeneratePassword(8)
            };

            var result = await _controller.Put(userRequestDtoUpdate);
            Assert.True(result is BadRequestObjectResult);
            Assert.False(_controller.ModelState.IsValid);
        }
    }
}
