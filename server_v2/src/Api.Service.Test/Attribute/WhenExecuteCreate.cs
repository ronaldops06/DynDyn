using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.Attribute
{
    public class WhenExecuteCreate : AttributeTest
    {
        [Fact(DisplayName = "É possível executar o método Create.")]
        public async Task Eh_Possivel_Executar_Metodo_Create()
        {
            var attributeEntityResult = Mapper.Map<AttributeEntity>(attributeModelResult);
            var attributeEntity = Mapper.Map<AttributeEntity>(attributeModel);

            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(It.IsAny<AttributeEntity>());
            RepositoryMock.Setup(m => m.InsertAsync(It.IsAny<AttributeEntity>())).ReturnsAsync(attributeEntityResult);
            AttributeService service = new AttributeService(UserServiceMock.Object, RepositoryMock.Object, TrashServiceMock.Object, Mapper);

            var result = await service.Post(attributeModel);
            ApplyTest(attributeModel, result);
        }
    }
}