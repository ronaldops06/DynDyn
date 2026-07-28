using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Attribute;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Attribute.WhenRequestUpdate
{
    public class ReturnUpdated : BaseTestAttribute
    {
        [Fact(DisplayName = "É possível realizar o Update")]
        public async Task Eh_Possivel_Invocar_Controller_Update()
        {
            var serviceMock = new Mock<IAttributeService>();

            serviceMock.Setup(m => m.Put(It.IsAny<AttributeModel>())).ReturnsAsync(AttributeModel);

            Controller = new AttributeController(serviceMock.Object, Mapper);

            var result = await Controller.Put(AttributeRequestDto);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as AttributeResponseDto;
            Assert.NotNull(resultValue);
            Assert.Equal(AttributeRequestDto.Id, resultValue.Id);
            Assert.Equal(AttributeRequestDto.Name, resultValue.Name);
            Assert.Equal(AttributeRequestDto.Description, resultValue.Description);
            Assert.Equal(AttributeRequestDto.DataType, resultValue.DataType);
            Assert.Equal(AttributeRequestDto.Status, resultValue.Status);
        }
    }
}