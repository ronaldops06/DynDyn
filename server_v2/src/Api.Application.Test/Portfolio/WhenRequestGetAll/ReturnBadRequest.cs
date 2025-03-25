using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Domain.Helpers;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Portfolio.WhenRequestGetAll
{
    public class ReturnBadRequest : BaseTestPortfolio
    {
        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<IPortfolioService>();

            var pageListPortfolioModel = PageList<PortfolioModel>.Create(PageParams, ListPortfolioModel, ListPortfolioModel.Count());

            serviceMock.Setup(m => m.Get(It.IsAny<PageParams>())).ReturnsAsync(pageListPortfolioModel);

            Controller = new PortfolioController(serviceMock.Object, Mapper);
            Controller.ModelState.AddModelError("Id", "Formato Inválido");

            var result = await Controller.Get(PageParams);
            Assert.True(result is BadRequestObjectResult);
        }
    }
}