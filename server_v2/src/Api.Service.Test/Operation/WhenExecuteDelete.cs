using Api.Domain.Entities;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.Operation
{
    public class WhenExecuteDelete : OperationTest
    {
        [Fact(DisplayName = "É possível executar o método Delete.")]
        public async Task Eh_Possivel_Executar_Metodo_Delete()
        {
            var operationEntity = Mapper.Map<OperationEntity>(operationModel);

            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>())).ReturnsAsync(operationEntity);
            RepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);
            OperationService service = new OperationService(RepositoryMock.Object, Mapper);

            var result = await service.Delete(operationModel.Id);
            Assert.True(result);

            RepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);
            service = new OperationService(RepositoryMock.Object, Mapper);

            result = await service.Delete(99989);
            Assert.False(result);
        }
    }
}