using Api.Domain.Enums;
using Api.Domain.Interfaces.Services;
using Api.Domain.Models;
using Api.Domain.Repository;
using Domain.Helpers;
using Domain.Models;
using Moq;
using Xunit;

namespace Api.Service.Test.Transaction
{
    public class TransactionTest : BaseTestService
    {
        private static readonly int RECORD_NUMBER = 10;
        
        protected Mock<IDeviceService> DeviceServiceMock = new Mock<IDeviceService>();
        protected Mock<IOperationService> OperationServiceMock = new Mock<IOperationService>();
        protected Mock<ITransactionRepository> RepositoryMock = new Mock<ITransactionRepository>();
        protected List<TransactionModel> listTransactionModel = new List<TransactionModel>();
        protected List<TransactionModel> listTransactionModelResult = new List<TransactionModel>();
        protected Dictionary<OperationType, double> transactionTotals = new Dictionary<OperationType, double>();
        protected OperationModel operationModel;
        protected OperationModel transferOperationModel;
        protected PortfolioModel DestinationPortfolioModel;
        protected TransactionModel transactionModel;
        protected TransactionModel transactionModelResult;
        protected TransactionModel installmentTransactionModelResult;
        protected TransactionModel transferTransactionModelResult;
        protected TransactionModel transactionModelUpdate;
        protected TransactionModel transactionModelUpdateResult;
        protected PageParams pageParams;
        protected NotificationModel notificationModel;

        public TransactionTest()
        {
            pageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 5,
            };

            Random random = new Random();
            var accountModel = GenerateAccount(2, "Cach");
            DestinationPortfolioModel = GenerateAccount(3, "Bradesco");
            operationModel = GenerateOperation(1, "Compra de Monitor", OperationType.Debito);
            transferOperationModel = GenerateOperation(2, "Transferência entre contas", OperationType.Transferencia);

            for (int i = 1; i <= RECORD_NUMBER; i++)
            {
                TransactionModel model = new TransactionModel()
                {
                    Id = i,
                    Value = random.Next(5000),
                    Observation = "Pago via pix",
                    Consolidated = SituationType.Nao,
                    Installment = null,
                    TotalInstallments = null,
                    Portfolio = accountModel,
                    PortfolioId = accountModel.Id,
                    Operation = operationModel,
                    OperationId = operationModel.Id,
                    User = UserModelFake,
                    UserId = UserModelFake.Id
                };

                listTransactionModel.Add(model);
            }

            listTransactionModelResult = listTransactionModel.Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
                                                             .Take(pageParams.PageSize)
                                                             .ToList();


            transactionModel = new TransactionModel()
            {
                Id = 2,
                Value = random.Next(5000),
                Observation = "Pago via pix",
                Consolidated = SituationType.Nao,
                Installment = null,
                TotalInstallments = null,
                Portfolio = accountModel,
                PortfolioId = accountModel.Id,
                Operation = operationModel,
                OperationId = operationModel.Id,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            transactionModelResult = new TransactionModel()
            {
                Id = transactionModel.Id,
                Value = transactionModel.Value,
                Observation = transactionModel.Observation,
                Consolidated = transactionModel.Consolidated,
                Installment = transactionModel.Installment,
                TotalInstallments = transactionModel.TotalInstallments,
                Portfolio = transactionModel.Portfolio,
                PortfolioId = transactionModel.PortfolioId,
                Operation = transactionModel.Operation,
                OperationId = transactionModel.OperationId,
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            installmentTransactionModelResult = new TransactionModel()
            {
                Id = transactionModel.Id,
                Value = transactionModel.Value,
                Observation = transactionModel.Observation,
                Consolidated = transactionModel.Consolidated,
                Installment = 1,
                TotalInstallments = 3,
                Portfolio = transactionModel.Portfolio,
                PortfolioId = transactionModel.PortfolioId,
                Operation = transactionModel.Operation,
                OperationId = transactionModel.OperationId,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            transferTransactionModelResult = new TransactionModel()
            {
                Id = transactionModel.Id,
                Value = transactionModel.Value,
                Observation = transactionModel.Observation,
                Consolidated = transactionModel.Consolidated,
                Installment = transactionModel.TotalInstallments,
                TotalInstallments = transactionModel.TotalInstallments,
                Portfolio = transactionModel.Portfolio,
                PortfolioId = transactionModel.PortfolioId,
                DestinationPortfolio = DestinationPortfolioModel,
                DestinationPortfolioId = DestinationPortfolioModel.Id,
                Operation = transactionModel.Operation,
                OperationId = transactionModel.OperationId,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            transactionModelUpdate = new TransactionModel()
            {
                Id = transactionModel.Id,
                Value = random.Next(5000),
                Observation = "Valor alterado para compensar perda",
                Consolidated = SituationType.Sim,
                Installment = transactionModel.Installment,
                TotalInstallments = transactionModel.TotalInstallments,
                Portfolio = transactionModel.Portfolio,
                PortfolioId = transactionModel.PortfolioId,
                Operation = transactionModel.Operation,
                OperationId = transactionModel.OperationId,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            transactionModelUpdateResult = new TransactionModel()
            {
                Id = transactionModelUpdate.Id,
                Value = transactionModelUpdate.Value,
                Observation = transactionModelUpdate.Observation,
                Consolidated = transactionModelUpdate.Consolidated,
                Installment = transactionModelUpdate.Installment,
                TotalInstallments = transactionModelUpdate.TotalInstallments,
                Portfolio = transactionModelUpdate.Portfolio,
                PortfolioId = transactionModelUpdate.PortfolioId,
                Operation = transactionModelUpdate.Operation,
                OperationId = transactionModelUpdate.OperationId,
                DataCriacao = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            transactionTotals.Add(OperationType.Credito, 1000.90);
            transactionTotals.Add(OperationType.Debito, 500.45);
            transactionTotals.Add(OperationType.Transferencia, 250);
            
            notificationModel = new NotificationModel
            {
                Title = "Exclude Entity",
                Body = new
                {
                    Operation = "DELETE",
                    Reference = "transaction",
                    Id = transactionModel.Id
                }
            };
        }

        private CategoryModel GenerateCategory(CategoryType type, string name, int id)
        {
            return new CategoryModel()
            {
                Id = id,
                Name = name,
                Type = type,
                Status = StatusType.Ativo,
            };
        }

        private PortfolioModel GenerateAccount(int id, string name)
        {
            var category = GenerateCategory(CategoryType.Conta, "Corrente", 1);

            PortfolioModel parentPortfolioAccountModel = new PortfolioModel()
            {
                Id = 1,
                Name = "Geral",
                Status = StatusType.Ativo,
                CategoryId = category.Id,
                Category = category,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            return new PortfolioModel()
            {
                Id = id,
                Name = name,
                Status = StatusType.Ativo,
                CategoryId = category.Id,
                Category = category,
                ParentPortfolioId = parentPortfolioAccountModel.Id,
                ParentPortfolio = parentPortfolioAccountModel,User = UserModelFake,
                UserId = UserModelFake.Id
            };
        }

        private OperationModel GenerateOperation(int id, string name, OperationType type)
        {
            var category = GenerateCategory(CategoryType.Operação, "Eletrônicos", 2);

            return new OperationModel()
            {
                Id = id,
                Name = name,
                Recurrent = false,
                Type = type,
                Status = StatusType.Ativo,
                CategoryId = category.Id,
                Category = category,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };
        }

        protected void ApplyTest(TransactionModel transactionModelSource, TransactionModel transactionModelDest)
        {
            Assert.NotNull(transactionModelDest);
            Assert.NotNull(transactionModelDest.User);
            Assert.True(transactionModelDest.UserId > 0);
            Assert.Equal(transactionModelSource.Id, transactionModelDest.Id);
            Assert.Equal(transactionModelSource.Value, transactionModelDest.Value);
            Assert.Equal(transactionModelSource.Observation, transactionModelDest.Observation);
            Assert.Equal(transactionModelSource.Consolidated, transactionModelDest.Consolidated);
            Assert.Equal(transactionModelSource.Installment, transactionModelDest.Installment);
            Assert.Equal(transactionModelSource.TotalInstallments, transactionModelDest.TotalInstallments);
            Assert.Equal(transactionModelSource.PortfolioId, transactionModelDest.PortfolioId);
            Assert.Equal(transactionModelSource.Portfolio.Id, transactionModelDest.Portfolio.Id);
            Assert.Equal(transactionModelSource.DestinationPortfolioId, transactionModelDest.DestinationPortfolioId);
            Assert.Equal(transactionModelSource.DestinationPortfolio?.Id, transactionModelDest.DestinationPortfolio?.Id);
            Assert.Equal(transactionModelSource.OperationId, transactionModelDest.OperationId);
            Assert.Equal(transactionModelSource.Operation.Id, transactionModelDest.Operation.Id);
            Assert.Equal(transactionModelSource.UserId, transactionModelDest.UserId);
            Assert.Equal(transactionModelSource.User.Id, transactionModelDest.User.Id);
        }
    }
}