using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Operation;
using Api.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Operation.OperationRole.WhenRequestGet
{
    public class ReturnRequestGet : BaseTestOperationRole
    {
        [Fact(DisplayName = "É possível realizar o GetAll")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<IOperationRoleService>();

            serviceMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(operationRoleModel);

            Controller = new OperationRoleController(serviceMock.Object, Mapper);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = await Controller.Get(operationRoleModel.Id);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as OperationRoleResponseDto;
            Assert.NotNull(resultValue);
            Assert.True(resultValue.Id > 0);
            Assert.Equal(operationRoleRequestDto.Name, resultValue.Name);
        }
    }
}