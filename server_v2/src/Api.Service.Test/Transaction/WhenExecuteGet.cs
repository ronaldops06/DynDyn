using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Service.Services;
using Domain.Helpers;
using Domain.Models;
using Moq;
using Xunit;

namespace Api.Service.Test.Transaction
{
    public class WhenExecuteGet : TransactionTest
    {
        [Fact(DisplayName = "É possível executar o método GET.")]
        public async Task Eh_Possivel_Executar_Metodo_Get()
        {
            var transactionEntityResult = Mapper.Map<TransactionEntity>(transactionModelResult);
            var listTransactionEntity = Mapper.Map<List<TransactionEntity>>(listTransactionModelResult);

            var data = new Data<TransactionEntity>(listTransactionEntity.Count, listTransactionEntity);

            RepositoryMock.Setup(m => m.SelectByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(transactionEntityResult);
            RepositoryMock.Setup(m => m.SelectByParamAsync(It.IsAny<int>(), It.IsAny<PageParams>())).ReturnsAsync(data);
            RepositoryMock.Setup(m => m.SelectTransactionsTotalsAsync(It.IsAny<int>(), It.IsAny<PageParams>())).ReturnsAsync(transactionTotals);
            TransactionService service = new TransactionService(UserServiceMock.Object, RepositoryMock.Object, OperationServiceMock.Object, Mapper);

            var resultById = await service.GetById(transactionModelResult.Id);
            ApplyTest(transactionModelResult, resultById);
            Assert.NotEqual(1, resultById.Id);

            var result = await service.Get(pageParams);
            Assert.NotNull(result);
            Assert.True(result.Count() == pageParams.PageSize);

            var resultTotals = await service.GetTotais(pageParams);
            Assert.NotNull(resultTotals);
            Assert.Equal(500.45, resultTotals.Debit);
            Assert.Equal(1000.90, resultTotals.Credit);
            Assert.Equal(250, resultTotals.Transfer);
        }
    }
}