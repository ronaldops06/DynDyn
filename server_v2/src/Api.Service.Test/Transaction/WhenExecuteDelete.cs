using Api.Domain.Entities;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.Transaction
{
    public class WhenExecuteDelete : TransactionTest
    {
        [Fact(DisplayName = "É possível executar o método Delete.")]
        public async Task Eh_Possivel_Executar_Metodo_Delete()
        {
            var transactionEntity = Mapper.Map<TransactionEntity>(transactionModel);
            
            TrashServiceMock.Setup(m => m.Post(trashModel));
            
            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(transactionEntity);
            RepositoryMock.Setup(m => m.SelectTransactionByParentTransactionIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(It.IsAny<IEnumerable<TransactionEntity>>());
            RepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);
            TransactionService service = new TransactionService(UserServiceMock.Object, RepositoryMock.Object, OperationServiceMock.Object, TrashServiceMock.Object, Mapper);

            var result = await service.Delete(transactionModel.Id);
            Assert.True(result);

            RepositoryMock.Setup(m => m.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);
            service = new TransactionService(UserServiceMock.Object, RepositoryMock.Object, OperationServiceMock.Object, TrashServiceMock.Object, Mapper);

            result = await service.Delete(99989);
            Assert.False(result);
        }
    }
}