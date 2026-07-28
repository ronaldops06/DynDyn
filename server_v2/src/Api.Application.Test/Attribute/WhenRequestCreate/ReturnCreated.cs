using System.Text.Json;
using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Attribute;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Attribute.WhenRequestCreate
{
    public class ReturnCreated : BaseTestAttribute
    {
        [Fact(DisplayName = "É possível realizar o Create")]
        public async Task Eh_Possivel_Invocar_Controller_Create()
        {
            var serviceMock = new Mock<IAttributeService>();

            serviceMock.Setup(m => m.Post(It.IsAny<AttributeModel>())).ReturnsAsync(AttributeModel);

            Controller = new AttributeController(serviceMock.Object, Mapper);

            Mock<IUrlHelper> url = new Mock<IUrlHelper>();
            url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns("http://localhost:5000");
            Controller.Url = url.Object;

            var result = await Controller.Post(AttributeRequestDto);

            string jsonString = JsonSerializer.Serialize(result);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as AttributeResponseDto;
            Assert.NotNull(resultValue);
            Assert.True(resultValue.Id > 0);
            Assert.Equal(AttributeRequestDto.Name, resultValue.Name);
            Assert.Equal(AttributeRequestDto.Description, resultValue.Description);
            Assert.Equal(AttributeRequestDto.DataType, resultValue.DataType);
            Assert.Equal(AttributeRequestDto.Status, resultValue.Status);
        }
    }
}