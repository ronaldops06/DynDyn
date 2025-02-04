using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.Operation
{
    public class WhenExecuteUpdate : OperationTest
    {
        [Fact(DisplayName = "É possível executar o método Update.")]
        public async Task Eh_Possivel_Executar_Metodo_Update()
        {
            var operationEntityUpdateResult = Mapper.Map<OperationEntity>(operationModelUpdateResult);
            var operationEntityUpdate = Mapper.Map<OperationEntity>(operationModelUpdate);

            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<OperationType>())).ReturnsAsync(operationEntityUpdate);
            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(operationEntityUpdate);
            RepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<OperationEntity>())).ReturnsAsync(operationEntityUpdateResult);
            OperationService service = new OperationService(UserServiceMock.Object, RepositoryMock.Object, Mapper);

            var resultUpdate = await service.Put(operationModelUpdate);
            ApplyTest(operationModelUpdate, resultUpdate);
        }
    }
}