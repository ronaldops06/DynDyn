using Api.Domain.Dtos.Balance;
using Api.Domain.Interfaces.Services;
using Application.V1.Controllers;
using Domain.Helpers;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Balance.WhenRequestGetAll
{
    public class ReturnRequestGetAll : BaseTestBalance
    {
        [Fact(DisplayName = "É possível realizar o GetAll")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<IBalanceService>();

            var pageListBalanceModel = PageList<BalanceModel>.Create(PageParams, ListBalanceModel, ListBalanceModel.Count());

            serviceMock.Setup(m => m.Get(It.IsAny<PageParams>())).ReturnsAsync(pageListBalanceModel);

            Controller = new BalanceController(serviceMock.Object, Mapper);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = await Controller.Get(PageParams);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as List<BalanceResponseDto>;

            Assert.NotNull(resultValue);
            Assert.True(resultValue.Count() == 3);
        }
    }
}