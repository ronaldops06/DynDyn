using System.Text.Json;
using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Operation;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Operation.WhenRequestCreate
{
    public class ReturnCreated : BaseTestOperation
    {
        [Fact(DisplayName = "É possível realizar o Create")]
        public async Task Eh_Possivel_Invocar_Controller_Create()
        {
            var serviceMock = new Mock<IOperationService>();

            serviceMock.Setup(m => m.Post(It.IsAny<OperationModel>())).ReturnsAsync(OperationModel);

            Controller = new OperationController(serviceMock.Object, Mapper);

            Mock<IUrlHelper> url = new Mock<IUrlHelper>();
            url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");
            Controller.Url = url.Object;

            var result = await Controller.Post(OperationRequestDto);

            string jsonString = JsonSerializer.Serialize(result);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as OperationResponseDto;
            Assert.NotNull(resultValue);
            Assert.True(resultValue.Id > 0);
            Assert.Equal(OperationRequestDto.Name, resultValue.Name);
            Assert.Equal(OperationRequestDto.Recurrent, resultValue.Recurrent);
            Assert.Equal(OperationRequestDto.Type, resultValue.Type);
            Assert.Equal(OperationRequestDto.Status, resultValue.Status);
            Assert.Equal(OperationRequestDto.Category.Id, resultValue.Category.Id);
        }
    }
}