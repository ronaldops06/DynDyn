using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Account.WhenRequestCreate
{
    public class ReturnBadRequest : BaseTestAccount
    {
        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_BadRequest()
        {
            var serviceMock = new Mock<IAccountService>();

            serviceMock.Setup(m => m.Post(It.IsAny<AccountModel>())).ReturnsAsync(AccountModel);

            Controller = new AccountController(serviceMock.Object, Mapper);
            Controller.ModelState.AddModelError("Nome", "É um campo obrigatório");

            Mock<IUrlHelper> url = new Mock<IUrlHelper>();
            url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");
            Controller.Url = url.Object;

            var result = await Controller.Post(AccountRequestDto);
            Assert.True(result is BadRequestObjectResult);
        }
    }
}