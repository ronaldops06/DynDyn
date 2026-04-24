using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Operation.OperationRole.WhenRequestGet
{
    public class ReturnBadRequest : BaseTestOperationRole
    {
        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<IOperationRoleService>();

            serviceMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(operationRoleModel);

            Controller = new OperationRoleController(serviceMock.Object, Mapper);
            Controller.ModelState.AddModelError("Id", "Formato Inválido");

            var result = await Controller.Get(operationRoleModel.Id);
            Assert.True(result is BadRequestObjectResult);
        }
    }
}