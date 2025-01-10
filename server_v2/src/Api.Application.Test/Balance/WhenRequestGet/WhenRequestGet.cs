using Api.Domain.Dtos.Balance;
using Api.Domain.Interfaces.Services;
using Application.V1.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Balance.WhenRequestGet
{
    public class WhenRequestGet : BaseTestBalance
    {
        [Fact(DisplayName = "É possível realizar o GetAll")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<IBalanceService>();

            serviceMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(BalanceModel);

            Controller = new BalanceController(serviceMock.Object, Mapper);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = await Controller.Get(BalanceModel.Id);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as BalanceResponseDto;
            ApplyTest(BalanceRequestDto, resultValue);
            Assert.True(resultValue.Id > 0);
        }
    }
}