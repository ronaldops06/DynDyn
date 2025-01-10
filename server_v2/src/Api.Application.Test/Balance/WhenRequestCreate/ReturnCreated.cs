using System.Text.Json;
using Api.Domain.Dtos.Balance;
using Api.Domain.Interfaces.Services;
using Application.V1.Controllers;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Balance.WhenRequestCreate
{
    public class ReturnCreated : BaseTestBalance
    {
        [Fact(DisplayName = "É possível realizar o Create")]
        public async Task Eh_Possivel_Invocar_Controller_Create()
        {
            var serviceMock = new Mock<IBalanceService>();

            serviceMock.Setup(m => m.Post(It.IsAny<BalanceModel>())).ReturnsAsync(BalanceModel);

            Controller = new BalanceController(serviceMock.Object, Mapper);

            Mock<IUrlHelper> url = new Mock<IUrlHelper>();
            url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");
            Controller.Url = url.Object;

            var result = await Controller.Post(BalanceRequestDto);

            string jsonString = JsonSerializer.Serialize(result);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as BalanceResponseDto;
            ApplyTest(BalanceRequestDto, resultValue);
            Assert.True(resultValue.Id > 0);
        }
    }
}