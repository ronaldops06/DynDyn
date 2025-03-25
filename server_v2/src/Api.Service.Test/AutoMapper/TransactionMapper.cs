using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Entities;
using Domain.Models;
using Xunit;

namespace Api.Service.Test.AutoMapper
{
    public class TransactionMapper : BaseTestService
    {
        [Fact(DisplayName = "É possível mapear os modelos")]
        public void Eh_Possivel_Mapear_Os_Modelos()
        {
            var portfolioModel = GeneratePortfolio(2, "Cach");
            var operationModel = GenerateOperation(1, "Compra de Monitor", OperationType.Debito);

            var model = new TransactionModel()
            {
                Id = 2,
                Value = 500.54,
                Observation = "Pago via pix",
                Consolidated = SituationType.Nao,
                Installment = null,
                TotalInstallments = null,
                Portfolio = portfolioModel,
                PortfolioId = portfolioModel.Id,
                Operation = operationModel,
                OperationId = operationModel.Id,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };

            //Model -> Entity
            var entity = Mapper.Map<TransactionEntity>(model);
            Assert.Equal(entity.Id, model.Id);
            Assert.Equal(entity.Value, model.Value);
            Assert.Equal(entity.Observation, model.Observation);
            Assert.Equal(entity.Consolidated, model.Consolidated);
            Assert.Equal(entity.Installment, model.Installment);
            Assert.Equal(entity.TotalInstallments, model.TotalInstallments);
            Assert.Equal(entity.PortfolioId, model.PortfolioId);
            Assert.Equal(entity.Portfolio.Id, model.Portfolio.Id);
            Assert.Equal(entity.OperationId, model.OperationId);
            Assert.Equal(entity.Operation.Id, model.Operation.Id);
            Assert.Equal(entity.ParentTransactionId, model.ParentTransactionId);
            Assert.Equal(entity.ParentTransaction?.Id, model.ParentTransaction?.Id);
            Assert.Equal(entity.UserId, model.UserId);
            Assert.Equal(entity.User.Id, model.User.Id);

            //Entity -> Model
            var transactionModel = Mapper.Map<TransactionModel>(entity);
            Assert.Equal(transactionModel.Id, entity.Id);
            Assert.Equal(transactionModel.Value, entity.Value);
            Assert.Equal(transactionModel.Observation, entity.Observation);
            Assert.Equal(transactionModel.Consolidated, entity.Consolidated);
            Assert.Equal(transactionModel.Installment, entity.Installment);
            Assert.Equal(transactionModel.TotalInstallments, entity.TotalInstallments);
            Assert.Equal(transactionModel.PortfolioId, entity.PortfolioId);
            Assert.Equal(transactionModel.Portfolio.Id, entity.Portfolio.Id);
            Assert.Equal(transactionModel.OperationId, entity.OperationId);
            Assert.Equal(transactionModel.Operation.Id, entity.Operation.Id);
            Assert.Equal(transactionModel.ParentTransactionId, entity.ParentTransactionId);
            Assert.Equal(transactionModel.ParentTransaction?.Id, entity.ParentTransaction?.Id);
            Assert.Equal(transactionModel.UserId, entity.UserId);
            Assert.Equal(transactionModel.User.Id, entity.User.Id);
        }

        [Fact(DisplayName = "É possível mapear os modelos em lista")]
        public void Eh_Possivel_Mapear_Os_Modelos_Em_Lista()
        {
            var userEntity = Mapper.Map<UserEntity>(UserModelFake);
            var portfolioModel = GeneratePortfolio(2, "Cach");
            var portfolioEntity = Mapper.Map<PortfolioEntity>(portfolioModel);
            var operationModel = GenerateOperation(1, "Compra de Monitor", OperationType.Debito);
            var operationEntity = Mapper.Map<OperationEntity>(operationModel);

            var listEntity = new List<TransactionEntity>();
            for (int i = 1; i <= 5; i++)
            {
                var item = new TransactionEntity()
                {
                    Id = 2,
                    Value = 500.54,
                    Observation = "Pago via pix",
                    Consolidated = SituationType.Nao,
                    Installment = null,
                    TotalInstallments = null,
                    Portfolio = portfolioEntity,
                    PortfolioId = portfolioEntity.Id,
                    Operation = operationEntity,
                    OperationId = operationEntity.Id,
                    User = userEntity,
                    UserId = userEntity.Id
                };

                listEntity.Add(item);
            }

            //List<Entity> -> List<Model>
            var listModel = Mapper.Map<List<TransactionModel>>(listEntity);

            Assert.True(listModel.Count() == listEntity.Count());

            for (int i = 0; i < listModel.Count(); i++)
            {
                Assert.Equal(listModel[i].Id, listEntity[i].Id);
                Assert.Equal(listModel[i].Value, listEntity[i].Value);
                Assert.Equal(listModel[i].Observation, listEntity[i].Observation);
                Assert.Equal(listModel[i].Consolidated, listEntity[i].Consolidated);
                Assert.Equal(listModel[i].Installment, listEntity[i].Installment);
                Assert.Equal(listModel[i].TotalInstallments, listEntity[i].TotalInstallments);
                Assert.Equal(listModel[i].PortfolioId, listEntity[i].PortfolioId);
                Assert.Equal(listModel[i].Portfolio.Id, listEntity[i].Portfolio.Id);
                Assert.Equal(listModel[i].OperationId, listEntity[i].OperationId);
                Assert.Equal(listModel[i].Operation.Id, listEntity[i].Operation.Id);
                Assert.Equal(listModel[i].ParentTransactionId, listEntity[i].ParentTransactionId);
                Assert.Equal(listModel[i].ParentTransaction?.Id, listEntity[i].ParentTransaction?.Id);
                Assert.Equal(listModel[i].DataCriacao, listEntity[i].DataCriacao);
                Assert.Equal(listModel[i].DataAlteracao, listEntity[i].DataAlteracao);
                Assert.Equal(listModel[i].UserId, listEntity[i].UserId);
                Assert.Equal(listModel[i].User.Id, listEntity[i].User.Id);
            }
        }

        private CategoryModel GenerateCategory(CategoryType type, string name, int id)
        {
            return new CategoryModel()
            {
                Id = id,
                Name = name,
                Type = type,
                Status = StatusType.Ativo,
                User = UserModelFake,
                UserId = UserModelFake.Id
            };
        }

        private PortfolioModel GeneratePortfolio(int id, string name)
        {
            var category = GenerateCategory(CategoryType.Conta, "Corrente", 1);

            PortfolioModel parentPortfolioPortfolioModel = new PortfolioModel()
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
                ParentPortfolioId = parentPortfolioPortfolioModel.Id,
                ParentPortfolio = parentPortfolioPortfolioModel,
                User = UserModelFake,
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
    }
}