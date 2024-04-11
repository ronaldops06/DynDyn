using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Operation;
using Api.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Operation.WhenRequestGet
{
    public class WhenRequestGet : BaseTestOperation
    {
        [Fact(DisplayName = "É possível realizar o GetAll")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<IOperationService>();

            serviceMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(OperationModel);

            Controller = new OperationController(serviceMock.Object, Mapper);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = await Controller.Get(OperationModel.Id);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as OperationResponseDto;
            Assert.NotNull(resultValue);
            Assert.True(resultValue.Id > 0);
            Assert.Equal(OperationRequestDto.Name, resultValue.Name);
            Assert.Equal(OperationRequestDto.Type, resultValue.Type);
            Assert.Equal(OperationRequestDto.Recurrent, resultValue.Recurrent);
            Assert.Equal(OperationRequestDto.Status, resultValue.Status);
            Assert.Equal(OperationRequestDto.Category.Id, resultValue.Category.Id);
        }
    }
}