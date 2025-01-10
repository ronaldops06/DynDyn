using Api.Domain.Interfaces.Services;
using Application.V1.Controllers;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Balance.WhenRequestCreate
{
    public class ReturnBadRequest : BaseTestBalance
    {
        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_BadRequest()
        {
            var serviceMock = new Mock<IBalanceService>();

            serviceMock.Setup(m => m.Post(It.IsAny<BalanceModel>())).ReturnsAsync(BalanceModel);

            Controller = new BalanceController(serviceMock.Object, Mapper);
            Controller.ModelState.AddModelError("Value", "É um campo obrigatório");
            Controller.ModelState.AddModelError("BalanceDate", "É um campo obrigatório");

            Mock<IUrlHelper> url = new Mock<IUrlHelper>();
            url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");
            Controller.Url = url.Object;

            var result = await Controller.Post(BalanceRequestDto);
            Assert.True(result is BadRequestObjectResult);
        }
    }
}