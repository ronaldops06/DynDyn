using Application.V1.Controllers;
using Domain.Interfaces.Services.User;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.User.WhenRequestDelete
{
    public class ReturnDeleted : BaseTestApplication
    {
        private UserController _controller;

        [Fact(DisplayName = "É possível realizar o Delete")]
        public async Task Eh_Possivel_Invocar_Controller_Delete()
        {
            var serviceMock = new Mock<IUserService>();

            serviceMock.Setup(m => m.Delete(It.IsAny<int>())).ReturnsAsync(true);

            _controller = new UserController(serviceMock.Object, Mapper);

            var result = await _controller.Delete(1);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value;
            Assert.True((Boolean)resultValue);
        }
    }
}
