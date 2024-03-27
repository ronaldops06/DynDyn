using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Account;
using Api.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Account.WhenRequestGet
{
    public class WhenRequestGet : BaseTestAccount
    {
        [Fact(DisplayName = "É possível realizar o GetAll")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<IAccountService>();

            serviceMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(AccountModel);

            Controller = new AccountController(serviceMock.Object, Mapper);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = await Controller.Get(AccountModel.Id);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as AccountResponseDto;
            Assert.NotNull(resultValue);
            Assert.True(resultValue.Id > 0);
            Assert.Equal(AccountRequestDto.Name, resultValue.Name);
            Assert.Equal(AccountRequestDto.Status, resultValue.Status);
            Assert.Equal(AccountRequestDto.Category.Id, resultValue.Category.Id);
            Assert.Equal(AccountRequestDto.ParentAccount.Id, resultValue.ParentAccount.Id);
        }
    }
}