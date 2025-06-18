using Application.V1.Controllers;
using Domain.Dtos.User;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.TransientUser.WhenRequestCreate
{
    public class ReturnBadRequest : BaseTestApplication
    {
        private TransientUserController _controller;

        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_BadRequest()
        {
            var serviceMock = new Mock<ITransientUserService>();
            var name = Faker.Name.FullName();
            var login = Faker.Internet.Email();

            var userModel = new TransientUserModel
            {
                Id = 1,
                Name = name,
                Login = login,
                Password = Faker.Lorem.GetFirstWord(),
                VerificationCode = Faker.RandomNumber.Next(100000, 999999),
                ExpirationDate = DateTime.Now.AddMinutes(15),
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            serviceMock.Setup(m => m.Post(It.IsAny<TransientUserModel>())).ReturnsAsync(userModel);

            _controller = new TransientUserController(serviceMock.Object, Mapper);
            _controller.ModelState.AddModelError("Login", "É um campo obrigatório");

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
