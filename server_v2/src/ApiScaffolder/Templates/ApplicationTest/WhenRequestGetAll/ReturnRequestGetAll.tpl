using Api.Application.V1.Controllers;
using Api.Domain.Dtos.{{model}};
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.{{folder}}.{{model}}.WhenRequestGetAll
{
    public class ReturnRequestGetAll : BaseTest{{model}}
    {
        [Fact(DisplayName = "É possível realizar o GetAll")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<I{{model}}Service>();

            var pageList{{model}}Model = PageList<{{model}}Model>.Create(PageParams, List{{model}}Model, List{{model}}Model.Count());

            serviceMock.Setup(m => m.Get(It.IsAny<PageParams>())).ReturnsAsync(pageList{{model}}Model);

            Controller = new {{model}}Controller(serviceMock.Object, Mapper);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = await Controller.Get(PageParams);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as List<{{model}}ResponseDto>;

            Assert.NotNull(resultValue);
            Assert.True(resultValue.Count() == 3);
        }
    }
}