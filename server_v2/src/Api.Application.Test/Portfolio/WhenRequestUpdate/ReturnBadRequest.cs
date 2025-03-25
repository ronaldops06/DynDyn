using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Portfolio.WhenRequestUpdate
{
    public class ReturnBadRequest : BaseTestPortfolio
    {
        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_BadRequest()
        {
            var serviceMock = new Mock<IPortfolioService>();

            serviceMock.Setup(m => m.Put(It.IsAny<PortfolioModel>())).ReturnsAsync(PortfolioModel);

            Controller = new PortfolioController(serviceMock.Object, Mapper);
            Controller.ModelState.AddModelError("Name", "É um campo obrigatório");

            var result = await Controller.Put(PortfolioRequestDto);
            Assert.True(result is BadRequestObjectResult);
            Assert.False(Controller.ModelState.IsValid);
        }
    }
}