using System.Text.Json;
using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Account;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Account.WhenRequestCreate
{
    public class ReturnCreated : BaseTestAccount
    {
        [Fact(DisplayName = "É possível realizar o Create")]
        public async Task Eh_Possivel_Invocar_Controller_Create()
        {
            var serviceMock = new Mock<IAccountService>();

            serviceMock.Setup(m => m.Post(It.IsAny<AccountModel>())).ReturnsAsync(AccountModel);

            Controller = new AccountController(serviceMock.Object, Mapper);

            Mock<IUrlHelper> url = new Mock<IUrlHelper>();
            url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");
            Controller.Url = url.Object;

            var result = await Controller.Post(AccountRequestDto);

            string jsonString = JsonSerializer.Serialize(result);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as AccountResponseDto;
            Assert.NotNull(resultValue);
            Assert.True(resultValue.Id > 0);
            Assert.Equal(AccountRequestDto.Name, resultValue.Name);
            Assert.Equal(AccountRequestDto.Status, resultValue.Status);
            Assert.Equal(AccountRequestDto.Category.Id, resultValue.Category.Id);
            Assert.Equal(AccountRequestDto.ParentAccount.Id, resultValue.ParentAccount.Id);
        }
    }
}