using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.Interfaces.Services;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.Category
{
    public class WhenExecuteCreate : CategoryTest
    {

        [Fact(DisplayName = "É possível executar o método Create.")]
        public async Task Eh_Possivel_Executar_Metodo_Create()
        {
            var categoryEntityResult = Mapper.Map<CategoryEntity>(categoryModelResult);
            var categoryEntity = Mapper.Map<CategoryEntity>(categoryModel);

            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(), It.IsAny<CategoryType>(), It.IsAny<string>())).ReturnsAsync(It.IsAny<CategoryEntity>());
            RepositoryMock.Setup(m => m.InsertAsync(It.IsAny<CategoryEntity>())).ReturnsAsync(categoryEntityResult);
            ICategoryService service = new CategoryService(UserServiceMock.Object, RepositoryMock.Object, TrashServiceMock.Object, Mapper);

            var result = await service.Post(categoryModel);
            ApplyTest(categoryModel, result);
        }
    }
}
