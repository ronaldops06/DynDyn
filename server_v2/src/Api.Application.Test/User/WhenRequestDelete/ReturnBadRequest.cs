using Application.V1.Controllers;
using Domain.Interfaces.Services.User;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.User.WhenRequestDelete
{
    public class ReturnBadRequest : BaseTestApplication
    {
        private UserController _controller;

        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_BadRequest()
        {
            var serviceMock = new Mock<IUserService>();

            serviceMock.Setup(m => m.Delete(It.IsAny<int>())).ReturnsAsync(true);

            _controller = new UserController(serviceMock.Object, Mapper);
            _controller.ModelState.AddModelError("Id", "Formato Inválido");


            var result = await _controller.Delete(default(int));
            Assert.True(result is BadRequestObjectResult);
            Assert.False(_controller.ModelState.IsValid);
        }
    }
}
