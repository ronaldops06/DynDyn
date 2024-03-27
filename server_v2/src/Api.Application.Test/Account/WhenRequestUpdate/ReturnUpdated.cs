using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Account;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Account.WhenRequestUpdate
{
    public class ReturnUpdated : BaseTestAccount
    {
        [Fact(DisplayName = "É possível realizar o Update")]
        public async Task Eh_Possivel_Invocar_Controller_Update()
        {
            var serviceMock = new Mock<IAccountService>();

            serviceMock.Setup(m => m.Put(It.IsAny<AccountModel>())).ReturnsAsync(AccountModel);

            Controller = new AccountController(serviceMock.Object, Mapper);

            var result = await Controller.Put(AccountRequestDto);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as AccountResponseDto;
            Assert.NotNull(resultValue);
            Assert.Equal(AccountRequestDto.Id, resultValue.Id);
            Assert.Equal(AccountRequestDto.Name, resultValue.Name);
            Assert.Equal(AccountRequestDto.Status, resultValue.Status);
            Assert.Equal(AccountRequestDto.Category.Id, resultValue.Category.Id);
            Assert.Equal(AccountRequestDto.ParentAccount.Id, resultValue.ParentAccount.Id);
        }

    }
}