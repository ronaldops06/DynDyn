using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Attribute.WhenRequestGet
{
    public class ReturnBadRequest : BaseTestAttribute
    {
        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<IAttributeService>();

            serviceMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(AttributeModel);

            Controller = new AttributeController(serviceMock.Object, Mapper);
            Controller.ModelState.AddModelError("Id", "Formato Inválido");

            var result = await Controller.Get(AttributeModel.Id);
            Assert.True(result is BadRequestObjectResult);
        }
    }
}