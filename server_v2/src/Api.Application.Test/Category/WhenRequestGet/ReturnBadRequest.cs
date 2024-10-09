using Api.Application.V1.Controllers;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static Api.Application.Test.Helpers.CategoryHelpers;
using static Api.Application.Test.Helpers.BaseHelper;

namespace Api.Application.Test.Category.WhenRequestGet
{
    public class ReturnBadRequest : BaseTestApplication
    {
        private CategoryController _controller;

        [Fact(DisplayName = "É possível realizar o BadRequest")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            int categoryId = 1;

            var categoryModel = new CategoryModel
            {
                Id = categoryId,
                Name = Faker.Name.FullName(),
                Type = GetCategoryTypeRandom(),
                Status = GetStatusTypeRandom(),
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow
            };

            var serviceMock = new Mock<ICategoryService>();

            serviceMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(categoryModel);

            _controller = new CategoryController(serviceMock.Object, Mapper);
            _controller.ModelState.AddModelError("Id", "Formato Inválido");

            var result = await _controller.Get(categoryId);
            Assert.True(result is BadRequestObjectResult);
        }
    }
}
