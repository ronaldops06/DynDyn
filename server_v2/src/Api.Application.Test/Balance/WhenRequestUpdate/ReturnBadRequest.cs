using Api.Domain.Interfaces.Services;
using Application.V1.Controllers;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Balance.WhenRequestUpdate
{
    public class ReturnBadRequest : BaseTestBalance
    {
        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_BadRequest()
        {
            var serviceMock = new Mock<IBalanceService>();

            serviceMock.Setup(m => m.Put(It.IsAny<BalanceModel>())).ReturnsAsync(BalanceModel);

            Controller = new BalanceController(serviceMock.Object, Mapper);
            Controller.ModelState.AddModelError("Value", "É um campo obrigatório");

            var result = await Controller.Put(BalanceRequestDto);
            Assert.True(result is BadRequestObjectResult);
            Assert.False(Controller.ModelState.IsValid);
        }
    }
}