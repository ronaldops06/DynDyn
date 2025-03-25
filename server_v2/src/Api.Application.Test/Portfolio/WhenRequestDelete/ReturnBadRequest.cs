using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Portfolio.WhenRequestDelete
{
    public class ReturnBadRequest : BaseTestPortfolio
    {
        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_BadRequest()
        {
            var serviceMock = new Mock<IPortfolioService>();

            serviceMock.Setup(m => m.Delete(It.IsAny<int>())).ReturnsAsync(true);

            Controller = new PortfolioController(serviceMock.Object, Mapper);
            Controller.ModelState.AddModelError("Id", "Formato Inválido");


            var result = await Controller.Delete(default(int));
            Assert.True(result is BadRequestObjectResult);
            Assert.False(Controller.ModelState.IsValid);
        }
    }
}