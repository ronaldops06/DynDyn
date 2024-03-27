using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Account.WhenRequestGet
{
    public class ReturnBadRequest : BaseTestAccount
    {
        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<IAccountService>();

            serviceMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(AccountModel);

            Controller = new AccountController(serviceMock.Object, Mapper);
            Controller.ModelState.AddModelError("Id", "Formato Inválido");

            var result = await Controller.Get(AccountModel.Id);
            Assert.True(result is BadRequestObjectResult);
        }
    }
}