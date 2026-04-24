using System.Text.Json;
using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Operation;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Operation.OperationRole.WhenRequestCreate
{
    public class ReturnCreated : BaseTestOperationRole
    {
        [Fact(DisplayName = "É possível realizar o Create")]
        public async Task Eh_Possivel_Invocar_Controller_Create()
        {
            var serviceMock = new Mock<IOperationRoleService>();

            serviceMock.Setup(m => m.Post(It.IsAny<OperationRoleModel>())).ReturnsAsync(operationRoleModel);

            Controller = new OperationRoleController(serviceMock.Object, Mapper);

            Mock<IUrlHelper> url = new Mock<IUrlHelper>();
            url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");
            Controller.Url = url.Object;

            var result = await Controller.Post(operationRoleRequestDto);

            string jsonString = JsonSerializer.Serialize(result);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as OperationRoleResponseDto;
            Assert.NotNull(resultValue);
            Assert.True(resultValue.Id > 0);
            Assert.Equal(operationRoleRequestDto.Name, resultValue.Name);
        }
    }
}