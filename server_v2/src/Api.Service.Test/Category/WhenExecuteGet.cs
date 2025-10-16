using Api.Domain.Entities;
using Api.Domain.Interfaces.Services;
using Api.Service.Services;
using Domain.Helpers;
using Domain.Models;
using Moq;
using Xunit;

namespace Api.Service.Test.Category
{
    public class WhenExecuteGet : CategoryTest
    {
        [Fact(DisplayName = "É possível executar o método GET.")]
        public async Task Eh_Possivel_Executar_Metodo_Get()
        {
            var categoryEntityResult = Mapper.Map<CategoryEntity>(categoryModelResult);
            var listCategoryEntity = Mapper.Map<List<CategoryEntity>>(listCategoryModelResult);

            var data = new Data<CategoryEntity>(listCategoryEntity.Count, listCategoryEntity);

            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(categoryEntityResult);
            RepositoryMock.Setup(m => m.SelectByParamAsync(It.IsAny<int>(), It.IsAny<PageParams>())).ReturnsAsync(data);
            ICategoryService service = new CategoryService(UserServiceMock.Object, RepositoryMock.Object, DeviceServiceMock.Object, Mapper);

            var resultById = await service.GetById(categoryModelResult.Id);
            ApplyTest(categoryModelResult, resultById);
            Assert.NotEqual(2, resultById.Id);

            var result = await service.Get(pageParams);
            Assert.NotNull(result);
            Assert.True(result.Count() == pageParams.PageSize);
        }
    }
}
