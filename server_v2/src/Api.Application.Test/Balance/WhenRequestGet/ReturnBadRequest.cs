using Api.Domain.Interfaces.Services;
using Application.V1.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Balance.WhenRequestGet
{
    public class ReturnBadRequest : BaseTestBalance
    {
        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<IBalanceService>();

            serviceMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(BalanceModel);

            Controller = new BalanceController(serviceMock.Object, Mapper);
            Controller.ModelState.AddModelError("Id", "Formato Inválido");

            var result = await Controller.Get(BalanceModel.Id);
            Assert.True(result is BadRequestObjectResult);
        }
    }
}