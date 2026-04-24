using Api.Application.V1.Controllers;
using Api.Domain.Dtos.{{model}};
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.{{folder}}.{{model}}.WhenRequestUpdate
{
    public class ReturnUpdated : BaseTest{{model}}
    {
        [Fact(DisplayName = "É possível realizar o Update")]
        public async Task Eh_Possivel_Invocar_Controller_Update()
        {
            var serviceMock = new Mock<I{{model}}Service>();

            serviceMock.Setup(m => m.Put(It.IsAny<{{model}}Model>())).ReturnsAsync({{model}}Model);

            Controller = new {{model}}Controller(serviceMock.Object, Mapper);

            var result = await Controller.Put({{model}}RequestDto);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as {{model}}ResponseDto;
            Assert.NotNull(resultValue);
            Assert.Equal({{model}}RequestDto.Id, resultValue.Id);
            //
        }

    }
}