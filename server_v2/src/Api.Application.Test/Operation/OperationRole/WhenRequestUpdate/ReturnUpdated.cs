using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Operation;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Operation.OperationRole.WhenRequestUpdate
{
    public class ReturnUpdated : BaseTestOperationRole
    {
        [Fact(DisplayName = "É possível realizar o Update")]
        public async Task Eh_Possivel_Invocar_Controller_Update()
        {
            var serviceMock = new Mock<IOperationRoleService>();

            serviceMock.Setup(m => m.Put(It.IsAny<OperationRoleModel>())).ReturnsAsync(operationRoleModel);

            Controller = new OperationRoleController(serviceMock.Object, Mapper);

            var result = await Controller.Put(operationRoleRequestDto);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as OperationRoleResponseDto;
            Assert.NotNull(resultValue);
            Assert.Equal(operationRoleRequestDto.Id, resultValue.Id);
            Assert.Equal(operationRoleRequestDto.Name, resultValue.Name);
        }

    }
}