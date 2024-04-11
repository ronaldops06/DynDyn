using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Operation;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Operation.WhenRequestUpdate
{
    public class ReturnUpdated : BaseTestOperation
    {
        [Fact(DisplayName = "É possível realizar o Update")]
        public async Task Eh_Possivel_Invocar_Controller_Update()
        {
            var serviceMock = new Mock<IOperationService>();

            serviceMock.Setup(m => m.Put(It.IsAny<OperationModel>())).ReturnsAsync(OperationModel);

            Controller = new OperationController(serviceMock.Object, Mapper);

            var result = await Controller.Put(OperationRequestDto);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as OperationResponseDto;
            Assert.NotNull(resultValue);
            Assert.Equal(OperationRequestDto.Id, resultValue.Id);
            Assert.Equal(OperationRequestDto.Name, resultValue.Name);
            Assert.Equal(OperationRequestDto.Type, resultValue.Type);
            Assert.Equal(OperationRequestDto.Recurrent, resultValue.Recurrent);
            Assert.Equal(OperationRequestDto.Status, resultValue.Status);
            Assert.Equal(OperationRequestDto.Category.Id, resultValue.Category.Id);
        }

    }
}