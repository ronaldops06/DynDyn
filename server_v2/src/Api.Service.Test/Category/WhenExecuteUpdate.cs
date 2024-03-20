using Api.Domain.Interfaces.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.Category
{
    public class WhenExecuteUpdate : CategoryTest
    {
        private ICategoryService _service;
        private Mock<ICategoryService> _serviceMock;

        [Fact(DisplayName = "É possível executar o método Update.")]
        public async Task Eh_Possivel_Executar_Metodo_Update()
        {
            _serviceMock = new Mock<ICategoryService>();
            _serviceMock.Setup(m => m.Post(categoryModel)).ReturnsAsync(categoryModelResult);
            _service = _serviceMock.Object;

            var result = await _service.Post(categoryModel);
            Assert.NotNull(result);
            Assert.Equal(CategoryNome, result.Nome);
            Assert.Equal(CategoryTipo, result.Tipo);
            Assert.Equal(CategoryStatus, result.Status);

            _serviceMock = new Mock<ICategoryService>();
            _serviceMock.Setup(m => m.Put(categoryModelUpdate)).ReturnsAsync(categoryModelUpdateResult);
            _service = _serviceMock.Object;

            var resultUpdate = await _service.Put(categoryModelUpdate);
            Assert.NotNull(resultUpdate);
            Assert.Equal(CategoryNomeUpdated, resultUpdate.Nome);
            Assert.Equal(CategoryTipoUpdated, resultUpdate.Tipo);
            Assert.Equal(CategoryStatusUpdated, resultUpdate.Status);
        }
    }
}
