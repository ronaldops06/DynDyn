using Domain.Dtos.User;
using Domain.Interfaces.Services.User;
using Domain.Models;
using Moq;
using Xunit;

namespace Api.Service.Test.Login
{
    public class WhenExecuteFindByLogin
    {
        private ILoginService _service;
        private Mock<ILoginService> _serviceMock;

        [Fact(DisplayName = "É possível executar o método FindByLogin.")]
        public async Task Eh_Possivel_Executar_Metodo_FindByLogin()
        {
            var email = Faker.Internet.Email();
            var password = Faker.Name.FullName();

            var userModelReturn = new UserModel
            {
                Id = 1,
                Login = email,
                Name = password,
                Password = Faker.Lorem.GetFirstWord(),
                Role = ""                
            };

            var userModel = new UserModel
            {
                Login = email,
                Password = password
            };

            _serviceMock = new Mock<ILoginService>();
            _serviceMock.Setup(m => m.GetLoginAsync(userModel)).ReturnsAsync(userModelReturn);
            _service = _serviceMock.Object;

            var result = await _service.GetLoginAsync(userModel);
            Assert.NotNull(result);
        }
    }
}
