using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Portfolio;
using Api.Domain.Interfaces.Services;
using Domain.Helpers;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Portfolio.WhenRequestGetAll
{
    public class ReturnRequestGetAll : BaseTestPortfolio
    {
        [Fact(DisplayName = "É possível realizar o GetAll")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<IPortfolioService>();

            var pageListPortfolioModel = PageList<PortfolioModel>.Create(PageParams, ListPortfolioModel, ListPortfolioModel.Count());

            serviceMock.Setup(m => m.Get(It.IsAny<PageParams>())).ReturnsAsync(pageListPortfolioModel);

            Controller = new PortfolioController(serviceMock.Object, Mapper);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = await Controller.Get(PageParams);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as List<PortfolioResponseDto>;

            Assert.NotNull(resultValue);
            Assert.True(resultValue.Count() == 3);
        }
    }
}