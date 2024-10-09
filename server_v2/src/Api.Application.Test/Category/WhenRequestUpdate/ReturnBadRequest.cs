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
    public class ReturnBadRequest : BaseTestApplication
    {
        private CategoryController _controller;

        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_BadRequest()
        {
            var serviceMock = new Mock<ICategoryService>();
            var nome = Faker.Name.FullName();
            var tipo = GetCategoryTypeRandom();
            var status = GetStatusTypeRandom();

            var categoryModel = new CategoryModel
            {
                Id = 1,
                Name = nome,
                Type = tipo,
                Status = status,
                DataAlteracao = DateTime.UtcNow,
                DataCriacao = DateTime.UtcNow
            };

            serviceMock.Setup(m => m.Put(It.IsAny<CategoryModel>())).ReturnsAsync(categoryModel);

            _controller = new CategoryController(serviceMock.Object, Mapper);
            _controller.ModelState.AddModelError("Email", "É um campo obrigatório");

            var categoryRequestDtoUpdate = new CategoryRequestDto
            {
                Id = 1,
                Name = Faker.Name.FullName(),
                Type = (int)GetCategoryTypeRandom(),
                Status = (int)GetStatusTypeRandom()
            };


            var result = await _controller.Put(categoryRequestDtoUpdate);
            Assert.True(result is BadRequestObjectResult);
            Assert.False(_controller.ModelState.IsValid);
        }
    }
}
