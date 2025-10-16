using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.Interfaces.Services;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.Category
{
    public class WhenExecuteUpdate : CategoryTest
    {
        [Fact(DisplayName = "É possível executar o método Update.")]
        public async Task Eh_Possivel_Executar_Metodo_Update()
        {
            var categoryEntityUpdateResult = Mapper.Map<CategoryEntity>(categoryModelUpdateResult);
            var categoryEntityUpdate = Mapper.Map<CategoryEntity>(categoryModelUpdate);

            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(), It.IsAny<CategoryType>(), It.IsAny<string>())).ReturnsAsync(categoryEntityUpdate);
            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(categoryEntityUpdate);
            RepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<CategoryEntity>())).ReturnsAsync(categoryEntityUpdateResult);
            ICategoryService service = new CategoryService(UserServiceMock.Object, RepositoryMock.Object, DeviceServiceMock.Object, Mapper);

            var resultUpdate = await service.Put(categoryModelUpdate);
            ApplyTest(categoryModelUpdate, resultUpdate);
        }
    }
}
