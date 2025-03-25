using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Portfolio;
using Api.Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Portfolio.WhenRequestUpdate
{
    public class ReturnUpdated : BaseTestPortfolio
    {
        [Fact(DisplayName = "É possível realizar o Update")]
        public async Task Eh_Possivel_Invocar_Controller_Update()
        {
            var serviceMock = new Mock<IPortfolioService>();

            serviceMock.Setup(m => m.Put(It.IsAny<PortfolioModel>())).ReturnsAsync(PortfolioModel);

            Controller = new PortfolioController(serviceMock.Object, Mapper);

            var result = await Controller.Put(PortfolioRequestDto);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as PortfolioResponseDto;
            Assert.NotNull(resultValue);
            Assert.Equal(PortfolioRequestDto.Id, resultValue.Id);
            Assert.Equal(PortfolioRequestDto.Name, resultValue.Name);
            Assert.Equal(PortfolioRequestDto.Status, resultValue.Status);
            Assert.Equal(PortfolioRequestDto.Category.Id, resultValue.Category.Id);
            Assert.Equal(PortfolioRequestDto.ParentPortfolio.Id, resultValue.ParentPortfolio.Id);
        }

    }
}