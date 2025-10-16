using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.Models;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.Transaction
{
    public class WhenExecuteUpdate : TransactionTest
    {
        [Fact(DisplayName = "É possível executar o método Update.")]
        public async Task Eh_Possivel_Executar_Metodo_Update()
        {
            var transactionEntityUpdateResult = Mapper.Map<TransactionEntity>(transactionModelUpdateResult);
            var transactionEntityUpdate = Mapper.Map<TransactionEntity>(transactionModelUpdate);

            OperationServiceMock.Setup(m => m.GetByNameAndType(It.IsAny<string>(), It.IsAny<OperationType>())).ReturnsAsync(It.IsAny<OperationModel>());
            OperationServiceMock.Setup(m => m.Post(It.IsAny<OperationModel>())).ReturnsAsync(operationModel);

            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(transactionEntityUpdate);
            RepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<TransactionEntity>())).ReturnsAsync(transactionEntityUpdateResult);
            TransactionService service = new TransactionService(UserServiceMock.Object, RepositoryMock.Object, OperationServiceMock.Object, DeviceServiceMock.Object, Mapper);

            var resultUpdate = await service.Put(transactionModelUpdate);
            ApplyTest(transactionModelUpdate, resultUpdate);
        }
    }
}