using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.{{folder}}.{{model}}.WhenRequestCreate
{
    public class ReturnBadRequest : BaseTest{{model}}
    {
        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_BadRequest()
        {
            var serviceMock = new Mock<I{{model}}Service>();

            serviceMock.Setup(m => m.Post(It.IsAny<{{model}}Model>())).ReturnsAsync({{model}}Model);

            Controller = new {{model}}Controller(serviceMock.Object, Mapper);
            //Aplicar testes de campos obrigatórios

            Mock<IUrlHelper> url = new Mock<IUrlHelper>();
            url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");
            Controller.Url = url.Object;

            var result = await Controller.Post({{model}}RequestDto);
            Assert.True(result is BadRequestObjectResult);
        }
    }
}