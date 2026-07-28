using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Service.Services;
using Domain.Helpers;
using Domain.Models;
using Moq;
using Xunit;

namespace Api.Service.Test.Attribute
{
    public class WhenExecuteGet : AttributeTest
    {
        [Fact(DisplayName = "É possível executar o método GET.")]
        public async Task Eh_Possivel_Executar_Metodo_Get()
        {
            var attributeEntityResult = Mapper.Map<AttributeEntity>(attributeModelResult);
            var listAttributeEntity = Mapper.Map<List<AttributeEntity>>(listAttributeModelResult);

            var data = new Data<AttributeEntity>(listAttributeEntity.Count, listAttributeEntity);

            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(attributeEntityResult);
            RepositoryMock.Setup(m => m.SelectByParamAsync(It.IsAny<int>(), It.IsAny<PageParams>())).ReturnsAsync(data);
            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(attributeEntityResult);
            AttributeService service = new AttributeService(UserServiceMock.Object, RepositoryMock.Object, TrashServiceMock.Object, Mapper);

            var resultById = await service.GetById(attributeModelResult.Id);
            ApplyTest(attributeModelResult, resultById);
            Assert.NotEqual(1, resultById.Id);

            var result = await service.Get(pageParams);
            Assert.NotNull(result);
            Assert.True(result.Count() == pageParams.PageSize);

            //Métodos específicos
        }
    }
}