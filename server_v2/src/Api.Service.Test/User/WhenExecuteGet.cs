using Domain.Dtos.User;
using Domain.Interfaces.Services.User;
using Moq;
using Xunit;

namespace Api.Service.Test.User
{
    public class WhenExecuteGet : UserTest
    {
        private IUserService _service;
        private Mock<IUserService> _serviceMock;

        [Fact(DisplayName = "É possível executar o método GET.")]
        public async Task Eh_Possivel_Executar_Metodo_Get()
        {
            _serviceMock = new Mock<IUserService>();
            _serviceMock.Setup(m => m.Get(pageParams)).ReturnsAsync(pageListResult);
            _service = _serviceMock.Object;

            var result = await _service.Get(pageParams);
            Assert.NotNull(result);
            Assert.Equal(pageListResult, result);

            /*_serviceMock = new Mock<IUserService>();
            _serviceMock.Setup(m => m.Get(It.IsAny<int>())).Returns(Task.FromResult((UserResultDto)null));
            _service = _serviceMock.Object;

            var _record = await _service.Get(pageParams);
            Assert.Null(_record);*/
        }
    }
}
