using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Operation.WhenRequestDelete
{
    public class ReturnDeleted : BaseTestOperation
    {
        [Fact(DisplayName = "É possível realizar o Delete")]
        public async Task Eh_Possivel_Invocar_Controller_Delete()
        {
            var serviceMock = new Mock<IOperationService>();

            serviceMock.Setup(m => m.Delete(It.IsAny<int>())).ReturnsAsync(true);

            Controller = new OperationController(serviceMock.Object, Mapper);

            var result = await Controller.Delete(1);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value;
            Assert.True((Boolean)resultValue);
        }
    }
}