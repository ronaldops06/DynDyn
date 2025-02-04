using Api.Domain.Entities;
using Api.Domain.Interfaces.Services;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.Category
{
    public class WhenExecuteDelete : CategoryTest
    {
        [Fact(DisplayName = "É possível executar o método Delete.")]
        public async Task Eh_Possivel_Executar_Metodo_Delete()
        {
            var categoryEntity = Mapper.Map<CategoryEntity>(categoryModel);

            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(categoryEntity);
            RepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);
            ICategoryService service = new CategoryService(UserServiceMock.Object, RepositoryMock.Object, Mapper);

            var result = await service.Delete(categoryModel.Id);
            Assert.True(result);

            RepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);
            service = new CategoryService(UserServiceMock.Object, RepositoryMock.Object, Mapper);

            result = await service.Delete(99989);
            Assert.False(result);
        }
    }
}
