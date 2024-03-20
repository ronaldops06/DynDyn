using Api.Domain.Interfaces.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.Category
{
    public class WhenExecuteGet : CategoryTest
    {
        private ICategoryService _service;
        private Mock<ICategoryService> _serviceMock;

        [Fact(DisplayName = "É possível executar o método GET.")]
        public async Task Eh_Possivel_Executar_Metodo_Get()
        {
            _serviceMock = new Mock<ICategoryService>();
            _serviceMock.Setup(m => m.GetById(CategoryId)).ReturnsAsync(categoryModelResult);
            _service = _serviceMock.Object;

            var resultById = await _service.GetById(CategoryId);
            Assert.NotNull(resultById);
            Assert.Equal(CategoryId, resultById.Id);
            Assert.Equal(CategoryNome, resultById.Nome);
            Assert.Equal(CategoryTipo, resultById.Tipo);
            Assert.Equal(CategoryStatus, resultById.Status);
            Assert.NotEqual(2, resultById.Id);

            _serviceMock = new Mock<ICategoryService>();
            _serviceMock.Setup(m => m.Get(pageParams)).ReturnsAsync(pageListResult);
            _service = _serviceMock.Object;

            var result = await _service.Get(pageParams);
            Assert.NotNull(result);
            Assert.Equal(pageListResult, result);
        }
    }
}
