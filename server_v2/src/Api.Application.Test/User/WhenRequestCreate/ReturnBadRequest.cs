using Application.V1.Controllers;
using Domain.Dtos.User;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.User.WhenRequestCreate
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

            var userModel = new UserModel
            {
                Id = 1,
                Name = name,
                Login = login,
                Role = "",
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            serviceMock.Setup(m => m.Post(It.IsAny<UserModel>())).ReturnsAsync(userModel);

            _controller = new UserController(serviceMock.Object, Mapper);
            _controller.ModelState.AddModelError("Name", "É um campo obrigatório");

            Mock<IUrlHelper> url = new Mock<IUrlHelper>();
            url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");
            _controller.Url = url.Object;

            var userRequestDto = new UserRequestDto
            {
                Name = name,
                Login = login,
            };

            var result = await _controller.Post(userRequestDto);
            Assert.True(result is BadRequestObjectResult);
        }
    }
}
