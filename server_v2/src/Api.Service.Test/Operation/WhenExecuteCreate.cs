using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.Operation
{
    public class WhenExecuteCreate : OperationTest
    {
        [Fact(DisplayName = "É possível executar o método Create.")]
        public async Task Eh_Possivel_Executar_Metodo_Create()
        {
            var operationEntityResult = Mapper.Map<OperationEntity>(operationModelResult);
            var operationEntity = Mapper.Map<OperationEntity>(operationModel);

            RepositoryMock.Setup(m => m.SelectByUkAsync(It.IsAny<string>(), It.IsAny<OperationType>())).ReturnsAsync(It.IsAny<OperationEntity>());
            RepositoryMock.Setup(m => m.InsertAsync(It.IsAny<OperationEntity>())).ReturnsAsync(operationEntityResult);
            OperationService service = new OperationService(RepositoryMock.Object, Mapper);

            var result = await service.Post(operationModel);
            ApplyTest(operationModel, result);
        }
    }
}