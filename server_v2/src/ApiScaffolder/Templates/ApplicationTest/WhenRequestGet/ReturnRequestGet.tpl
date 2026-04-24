using Api.Application.V1.Controllers;
using Api.Domain.Dtos.{{model}};
using Api.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.{{folder}}.{{model}}.WhenRequestGet
{
    public class ReturnRequestGet : BaseTest{{model}}
    {
        [Fact(DisplayName = "É possível realizar o GetAll")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<I{{model}}Service>();

            serviceMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync({{model}}Model);

            Controller = new {{model}}Controller(serviceMock.Object, Mapper);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = await Controller.Get({{model}}Model.Id);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as {{model}}ResponseDto;
            Assert.NotNull(resultValue);
            Assert.True(resultValue.Id > 0);
           //
        }
    }
}