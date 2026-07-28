using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Attribute.WhenRequestUpdate
{
    public class ReturnBadRequest : BaseTestAttribute
    {
        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_BadRequest()
        {
            var serviceMock = new Mock<IAttributeService>();

            serviceMock.Setup(m => m.Put(It.IsAny<AttributeModel>())).ReturnsAsync(AttributeModel);

            Controller = new AttributeController(serviceMock.Object, Mapper);
            //Aplicar validação de campos obrigatórios

            var result = await Controller.Put(AttributeRequestDto);
            Assert.True(result is BadRequestObjectResult);
            Assert.False(Controller.ModelState.IsValid);
        }
    }
}