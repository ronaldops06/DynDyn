using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Portfolio.WhenRequestGet
{
    public class ReturnBadRequest : BaseTestPortfolio
    {
        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<IPortfolioService>();

            serviceMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(PortfolioModel);

            Controller = new PortfolioController(serviceMock.Object, Mapper);
            Controller.ModelState.AddModelError("Id", "Formato Inválido");

            var result = await Controller.Get(PortfolioModel.Id);
            Assert.True(result is BadRequestObjectResult);
        }
    }
}