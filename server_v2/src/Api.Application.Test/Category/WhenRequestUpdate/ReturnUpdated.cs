using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Category;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static Api.Application.Test.Helpers.CategoryHelpers;
using static Api.Application.Test.Helpers.BaseHelper;

namespace Api.Application.Test.Category.WhenRequestUpdate
{
    public class ReturnUpdated : BaseTestApplication
    {
        private CategoryController _controller;

        [Fact(DisplayName = "É possível realizar o Update")]
        public async Task Eh_Possivel_Invocar_Controller_Update()
        {
            var serviceMock = new Mock<ICategoryService>();
            var nome = Faker.Name.FullName();
            var tipo = GetCategoryTypeRandom();
            var status = GetStatusTypeRandom();

            var categoryModel = new CategoryModel
            {
                Id = 1,
                Nome = nome,
                Tipo = tipo,
                Status = status,
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            serviceMock.Setup(m => m.Put(It.IsAny<CategoryModel>())).ReturnsAsync(categoryModel);

            _controller = new CategoryController(serviceMock.Object, Mapper);

            var categoryRequestDtoUpdate = new CategoryRequestDto
            {
                Id = 1,
                Nome = Faker.Name.FullName(),
                Tipo = (int)GetCategoryTypeRandom(),
                Status = (int)GetStatusTypeRandom()
            };

            var result = await _controller.Put(categoryRequestDtoUpdate);
            Assert.True(result is CreatedResult);

            var resultValue = ((CreatedResult)result).Value as CategoryResponseDto;
            Assert.NotNull(resultValue);
            Assert.Equal(categoryRequestDtoUpdate.Id, resultValue.Id);
            Assert.Equal(categoryRequestDtoUpdate.Nome, resultValue.Nome);
            Assert.Equal(categoryRequestDtoUpdate.Tipo, resultValue.Tipo);
            Assert.Equal(categoryRequestDtoUpdate.Status, resultValue.Status);
        }
    }
}
