using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.Models;
using Api.Service.Services;
using Moq;
using Xunit;

namespace Api.Service.Test.Transaction
{
    public class WhenExecuteCreate : TransactionTest
    {
        [Fact(DisplayName = "É possível executar o método Create.")]
        public async Task Eh_Possivel_Executar_Metodo_Create()
        {
            await TesteBasicoGeral();

            await TesteComParcelas();

            await TesteTransferencia();
        }

        private async Task TesteBasicoGeral()
        {
            var transactionEntityResult = Mapper.Map<TransactionEntity>(transactionModelResult);

            OperationServiceMock.Setup(m => m.GetByNameAndType(It.IsAny<string>(), It.IsAny<OperationType>())).ReturnsAsync(It.IsAny<OperationModel>());
            OperationServiceMock.Setup(m => m.Post(It.IsAny<OperationModel>())).ReturnsAsync(operationModel);

            RepositoryMock.Setup(m => m.InsertAsync(It.IsAny<TransactionEntity>())).ReturnsAsync(transactionEntityResult);
            TransactionService service = new TransactionService(UserServiceMock.Object, RepositoryMock.Object, OperationServiceMock.Object, TrashServiceMock.Object, Mapper);

            var result = await service.Post(transactionModel);
            ApplyTest(transactionModel, result);
        }

        private async Task TesteComParcelas()
        {
            var transactionEntityResult = Mapper.Map<TransactionEntity>(installmentTransactionModelResult);

            RepositoryMock.Setup(m => m.InsertAsync(It.IsAny<TransactionEntity>())).ReturnsAsync(transactionEntityResult);
            var service = new TransactionService(UserServiceMock.Object, RepositoryMock.Object, OperationServiceMock.Object, TrashServiceMock.Object, Mapper);

            transactionModel.TotalInstallments = installmentTransactionModelResult.TotalInstallments;
            transactionModel.Installment = installmentTransactionModelResult.Installment;

            var result = await service.Post(transactionModel);
            ApplyTest(installmentTransactionModelResult, result);
        }

        private async Task TesteTransferencia()
        {
            var transactionEntityResult = Mapper.Map<TransactionEntity>(transferTransactionModelResult);

            RepositoryMock.Setup(m => m.InsertAsync(It.IsAny<TransactionEntity>())).ReturnsAsync(transactionEntityResult);
            var service = new TransactionService(UserServiceMock.Object, RepositoryMock.Object, OperationServiceMock.Object, TrashServiceMock.Object, Mapper);

            transactionModel.DestinationPortfolio = DestinationPortfolioModel;
            transactionModel.DestinationPortfolioId = DestinationPortfolioModel.Id;
            transactionModel.Operation = transferOperationModel;
            transactionModel.OperationId = transferOperationModel.Id;
            transactionModel.TotalInstallments = null;
            var result = await service.Post(transactionModel);
            ApplyTest(transferTransactionModelResult, result);
        }
    }
}