using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.{{folder}}.{{model}}.WhenRequestGetAll
{
    public class ReturnBadRequest : BaseTest{{model}}
    {
        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<I{{model}}Service>();

            var pageList{{model}}Model = PageList<{{model}}Model>.Create(PageParams, List{{model}}Model, List{{model}}Model.Count());

            serviceMock.Setup(m => m.Get(It.IsAny<PageParams>())).ReturnsAsync(pageList{{model}}Model);

            Controller = new {{model}}Controller(serviceMock.Object, Mapper);
            Controller.ModelState.AddModelError("Id", "Formato Inválido");

            var result = await Controller.Get(PageParams);
            Assert.True(result is BadRequestObjectResult);
        }
    }
}