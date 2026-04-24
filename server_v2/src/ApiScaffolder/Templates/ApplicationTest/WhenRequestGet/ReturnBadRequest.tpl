using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.{{folder}}.{{model}}.WhenRequestGet
{
    public class ReturnBadRequest : BaseTest{{model}}
    {
        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<I{{model}}Service>();

            serviceMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync({{model}}Model);

            Controller = new {{model}}Controller(serviceMock.Object, Mapper);
            Controller.ModelState.AddModelError("Id", "Formato Inválido");

            var result = await Controller.Get({{model}}Model.Id);
            Assert.True(result is BadRequestObjectResult);
        }
    }
}