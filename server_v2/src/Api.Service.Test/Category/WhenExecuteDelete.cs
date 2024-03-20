using Api.Domain.Interfaces.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.Category
{
    public class WhenExecuteDelete : CategoryTest
    {
        private ICategoryService _service;
        private Mock<ICategoryService> _serviceMock;

        [Fact(DisplayName = "É possível executar o método Delete.")]
        public async Task Eh_Possivel_Executar_Metodo_Delete()
        {
            _serviceMock = new Mock<ICategoryService>();
            _serviceMock.Setup(m => m.Delete(It.IsAny<int>())).ReturnsAsync(true);
            _service = _serviceMock.Object;

            var result = await _service.Delete(CategoryId);
            Assert.True(result);

            _serviceMock = new Mock<ICategoryService>();
            _serviceMock.Setup(m => m.Delete(It.IsAny<int>())).ReturnsAsync(false);
            _service = _serviceMock.Object;

            result = await _service.Delete(99989);
            Assert.False(result);
        }
    }
}
