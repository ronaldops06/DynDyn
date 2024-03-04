using Domain.Interfaces.Services.User;
using Moq;
using Xunit;

namespace Api.Service.Test.User
{
    public class WhenExecuteDelete : UserTest
    {
        private IUserService _service;
        private Mock<IUserService> _serviceMock;

        [Fact(DisplayName = "É possível executar o método Delete.")]
        public async Task Eh_Possivel_Executar_Metodo_Delete()
        {
            _serviceMock = new Mock<IUserService>();
            _serviceMock.Setup(m => m.Delete(It.IsAny<int>())).ReturnsAsync(true);
            _service = _serviceMock.Object;

            var result = await _service.Delete(UserId);
            Assert.True(result);

            _serviceMock = new Mock<IUserService>();
            _serviceMock.Setup(m => m.Delete(It.IsAny<int>())).ReturnsAsync(false);
            _service = _serviceMock.Object;

            result = await _service.Delete(99989);
            Assert.False(result);
        }
    }
}
