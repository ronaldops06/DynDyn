using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Operation.WhenRequestUpdate
{
    public class ReturnBadRequest : BaseTestOperation
    {
        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_BadRequest()
        {
            var serviceMock = new Mock<IOperationService>();

            serviceMock.Setup(m => m.Put(It.IsAny<OperationModel>())).ReturnsAsync(OperationModel);

            Controller = new OperationController(serviceMock.Object, Mapper);
            Controller.ModelState.AddModelError("Name", "É um campo obrigatório");

            var result = await Controller.Put(OperationRequestDto);
            Assert.True(result is BadRequestObjectResult);
            Assert.False(Controller.ModelState.IsValid);
        }
    }
}