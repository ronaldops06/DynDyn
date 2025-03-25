using System.Text.Json;
using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Portfolio;
using Api.Domain.Interfaces.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Portfolio.WhenRequestCreate
{
    public class ReturnCreated : BaseTestPortfolio
    {
        [Fact(DisplayName = "É possível realizar o Create")]
        public async Task Eh_Possivel_Invocar_Controller_Create()
        {
            var serviceMock = new Mock<IPortfolioService>();

            serviceMock.Setup(m => m.Post(It.IsAny<PortfolioModel>())).ReturnsAsync(PortfolioModel);

            Controller = new PortfolioController(serviceMock.Object, Mapper);

            Mock<IUrlHelper> url = new Mock<IUrlHelper>();
            url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");
            Controller.Url = url.Object;

            var result = await Controller.Post(PortfolioRequestDto);

            string jsonString = JsonSerializer.Serialize(result);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as PortfolioResponseDto;
            Assert.NotNull(resultValue);
            Assert.True(resultValue.Id > 0);
            Assert.Equal(PortfolioRequestDto.Name, resultValue.Name);
            Assert.Equal(PortfolioRequestDto.Status, resultValue.Status);
            Assert.Equal(PortfolioRequestDto.Category.Id, resultValue.Category.Id);
            Assert.Equal(PortfolioRequestDto.ParentPortfolio.Id, resultValue.ParentPortfolio.Id);
        }
    }
}