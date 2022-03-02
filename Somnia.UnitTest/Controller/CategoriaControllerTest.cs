using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Somnia.API.Data;
using Somnia.API.Helpers;
using Somnia.API.Models;
using Somnia.API.V1.Controller;
using Somnia.API.V1.Dtos;
using Somnia.API.V1.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Somnia.UnitTest.Controller
{
    public class CategoriaControllerTest
    {
        [Fact]
        public async void GetAllCategories()
        {
            //Arrange
            //Repository
            var mockRepo = new Mock<Repository>();

            var categorias = new List<Categoria>(){
                new Categoria() { ID=1, Nome = "Schedule1" },
                new Categoria() { ID=2, Nome = "Schedule2" },
                new Categoria() { ID=3, Nome = "Schedule3" }
            }.ToArray();

            mockRepo.Setup(m => m.GetAllCategorias()).Returns(value: categorias);

            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new SomniaProfile());
            });

            var mapper = mockMapper.CreateMapper();

            CategoriaController controller = new CategoriaController(repository: mockRepo.Object, mapper: mapper);

            //Act
            PageParams pageParams = new PageParams();
            var result = await controller.Get(pageParams);
            Console.WriteLine(result);
            //Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(result);

            var model = okResult.Value as IEnumerable<CategoriaDTO>;
            if (model.Count() > 0)
            {
                Assert.NotNull(model);

                var expected = model?.FirstOrDefault().Nome;
                var actual = categorias?.FirstOrDefault().Nome;

                Assert.Equal(expected: expected, actual: actual);
            }
        }
    }
}
