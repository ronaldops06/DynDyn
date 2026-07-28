using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.Attribute
{
    public class WhenExecuteUpdate : AttributeTest
    {
        [Fact(DisplayName = "É possível executar o método Update.")]
        public async Task Eh_Possivel_Executar_Metodo_Update()
        {
            var attributeEntityUpdateResult = Mapper.Map<AttributeEntity>(attributeModelUpdateResult);
            var attributeEntityUpdate = Mapper.Map<AttributeEntity>(attributeModelUpdate);

            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(attributeEntityUpdate);
            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(attributeEntityUpdate);
            RepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<AttributeEntity>())).ReturnsAsync(attributeEntityUpdateResult);
            AttributeService service = new AttributeService(UserServiceMock.Object, RepositoryMock.Object, TrashServiceMock.Object, Mapper);

            var resultUpdate = await service.Put(attributeModelUpdate);
            ApplyTest(attributeModelUpdate, resultUpdate);
        }
    }
}