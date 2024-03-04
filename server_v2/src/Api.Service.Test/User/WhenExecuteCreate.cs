using Domain.Interfaces.Services.User;
using Moq;
using Xunit;

namespace Api.Service.Test.User
{
    public class WhenExecuteCreate : UserTest
    {
        private IUserService _service;
        private Mock<IUserService> _serviceMock;

        [Fact(DisplayName = "É possível executar o método Create.")]
        public async Task Eh_Possivel_Executar_Metodo_Create()
        {
            _serviceMock = new Mock<IUserService>();
            _serviceMock.Setup(m => m.Post(userModel)).ReturnsAsync(userModelResult);
            _service = _serviceMock.Object;

            var result = await _service.Post(userModel);
            Assert.NotNull(result);
            Assert.Equal(UserLogin, result.Login);
            Assert.Equal(UserName, result.Name);
            Assert.Equal(UserRole, result.Role);
        }
    }
}
