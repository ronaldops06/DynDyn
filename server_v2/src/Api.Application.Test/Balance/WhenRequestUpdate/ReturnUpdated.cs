using Api.Domain.Dtos.Balance;
using Api.Domain.Interfaces.Services;
using Application.V1.Controllers;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Balance.WhenRequestUpdate
{
    public class ReturnUpdated : BaseTestBalance
    {
        [Fact(DisplayName = "É possível realizar o Update")]
        public async Task Eh_Possivel_Invocar_Controller_Update()
        {
            var serviceMock = new Mock<IBalanceService>();

            serviceMock.Setup(m => m.Put(It.IsAny<BalanceModel>())).ReturnsAsync(BalanceModel);

            Controller = new BalanceController(serviceMock.Object, Mapper);

            var result = await Controller.Put(BalanceRequestDto);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as BalanceResponseDto;
            ApplyTest(BalanceRequestDto, resultValue);
            Assert.Equal(BalanceRequestDto.Id, resultValue.Id);
        }

    }
}