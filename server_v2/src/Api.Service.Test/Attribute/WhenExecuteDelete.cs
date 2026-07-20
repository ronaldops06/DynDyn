using Api.Domain.Entities;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.Attribute
{
    public class WhenExecuteDelete : AttributeTest
    {
        [Fact(DisplayName = "É possível executar o método Delete.")]
        public async Task Eh_Possivel_Executar_Metodo_Delete()
        {
            var attributeEntity = Mapper.Map<AttributeEntity>(attributeModel);

            TrashServiceMock.Setup(m => m.Post(trashModel));
            
            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(attributeEntity);
            RepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);
            AttributeService service = new AttributeService(UserServiceMock.Object, RepositoryMock.Object, TrashServiceMock.Object, Mapper);

            var result = await service.Delete(attributeModel.Id);
            Assert.True(result);

            RepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);
            service = new AttributeService(UserServiceMock.Object, RepositoryMock.Object, TrashServiceMock.Object, Mapper);

            result = await service.Delete(99989);
            Assert.False(result);
        }
    }
}