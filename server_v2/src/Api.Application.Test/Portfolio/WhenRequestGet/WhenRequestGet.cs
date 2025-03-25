using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Portfolio;
using Api.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Portfolio.WhenRequestGet
{
    public class WhenRequestGet : BaseTestPortfolio
    {
        [Fact(DisplayName = "É possível realizar o GetAll")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<IPortfolioService>();

            serviceMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(PortfolioModel);

            Controller = new PortfolioController(serviceMock.Object, Mapper);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = await Controller.Get(PortfolioModel.Id);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as PortfolioResponseDto;
            Assert.NotNull(resultValue);
            Assert.True(resultValue.Id > 0);
            Assert.Equal(PortfolioRequestDto.Name, resultValue.Name);
            Assert.Equal(PortfolioRequestDto.Status, resultValue.Status);
            Assert.Equal(PortfolioRequestDto.Category.Id, resultValue.Category.Id);
            Assert.Equal(PortfolioRequestDto.ParentPortfolio.Id, resultValue.ParentPortfolio.Id);
        }
    }
}