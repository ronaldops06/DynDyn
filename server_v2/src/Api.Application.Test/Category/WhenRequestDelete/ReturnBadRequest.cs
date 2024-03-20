using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Category.WhenRequestDelete
{
    public class ReturnBadRequest : BaseTestApplication
    {
        private CategoryController _controller;

        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_BadRequest()
        {
            var serviceMock = new Mock<ICategoryService>();

            serviceMock.Setup(m => m.Delete(It.IsAny<int>())).ReturnsAsync(true);

            _controller = new CategoryController(serviceMock.Object, Mapper);
            _controller.ModelState.AddModelError("Id", "Formato Inválido");


            var result = await _controller.Delete(default(int));
            Assert.True(result is BadRequestObjectResult);
            Assert.False(_controller.ModelState.IsValid);
        }
    }
}
