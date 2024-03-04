using Application.V1.Controllers;
using Domain.Dtos.User;
using Domain.Helpers;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text;
using Xunit;

namespace Api.Application.Test.User.WhenRequestGetAll
{
    public class ReturnGetAll : BaseTestApplication
    {
        private UserController _controller;

        [Fact(DisplayName = "É possível realizar o GetAll")]
        public async Task Eh_Possivel_Invocar_Controller_GetAll()
        {
            var serviceMock = new Mock<IUserService>();

            var pageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 3
            };

            var listUserModel = new List<UserModel>
            {
                new UserModel{
                    Id = 1,
                    Name = Faker.Name.FullName(),
                    Login = Faker.Internet.Email(),
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow
                },
                 new UserModel{
                    Id = 2,
                    Name = Faker.Name.FullName(),
                    Login = Faker.Internet.Email(),
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow
                 },
                 new UserModel{
                    Id = 3,
                    Name = Faker.Name.FullName(),
                    Login = Faker.Internet.Email(),
                    DataCriacao = DateTime.UtcNow,
                    DataAlteracao = DateTime.UtcNow
                 }
            };

            var pageListUserModel = PageList<UserModel>.Create(pageParams, listUserModel, listUserModel.Count());

            serviceMock.Setup(m => m.Get(It.IsAny<PageParams>())).ReturnsAsync(pageListUserModel);

            _controller = new UserController(serviceMock.Object, Mapper);
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = await _controller.GetAll(pageParams);
            Assert.True(result is OkObjectResult);

            var resultValue = ((OkObjectResult)result).Value as List<UserResponseDto>;

            Assert.NotNull(resultValue);
            Assert.True(resultValue.Count() == 3);
        }
    }
}
