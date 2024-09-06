using Api.Application.V1.Controllers;
using Api.Domain.Dtos.Account;
using Api.Domain.Dtos.Category;
using Api.Domain.Dtos.Operation;
using Api.Domain.Dtos.Transaction;
using Api.Domain.Enums;
using Api.Domain.Models;
using Domain.Helpers;

namespace Api.Application.Test.Transaction
{
    public class BaseTestTransaction : BaseTestApplication
    {
        protected TransactionController Controller;

        protected TransactionModel TransactionModel;
        protected TransactionRequestDto TransactionRequestDto;
        protected PageParams PageParams;
        protected List<TransactionModel> ListTransactionModel = new List<TransactionModel>();

        protected BaseTestTransaction()
        {
            Random random = new Random();

            var accountModel = GenerateAccount(2, "Cach");
            var operationModel = GenerateOperation(1, "Compra de Monitor", OperationType.Debito);

            TransactionModel = new TransactionModel()
            {
                Id = 2,
                Value = random.Next(50000),
                Observation = "Pago via pix",
                Consolidated = SituationType.Nao,
                Installment = null,
                TotalInstallments = null,
                Account = accountModel,
                AccountId = accountModel.Id,
                Operation = operationModel,
                OperationId = operationModel.Id
            };

            ListTransactionModel.Add(TransactionModel);

            TransactionModel = new TransactionModel()
            {
                Id = 3,
                Value = random.Next(50000),
                Observation = "Pago via pix",
                Consolidated = SituationType.Nao,
                Installment = null,
                TotalInstallments = null,
                Account = accountModel,
                AccountId = accountModel.Id,
                Operation = operationModel,
                OperationId = operationModel.Id
            };

            ListTransactionModel.Add(TransactionModel);

            TransactionModel = new TransactionModel()
            {
                Id = 4,
                Value = random.Next(50000),
                Observation = "Pago via pix",
                Consolidated = SituationType.Nao,
                Installment = null,
                TotalInstallments = null,
                Account = accountModel,
                AccountId = accountModel.Id,
                Operation = operationModel,
                OperationId = operationModel.Id
            };

            ListTransactionModel.Add(TransactionModel);

            var categoryRequestDto = new CategoryRequestDto
            {
                Id = TransactionModel.Account.CategoryId
            };

            var accountRequestDto = new AccountRequestDto
            {
                Id = TransactionModel.AccountId,
                Category = categoryRequestDto
            };

            categoryRequestDto = new CategoryRequestDto
            {
                Id = TransactionModel.Operation.CategoryId
            };

            var operationRequestDto = new OperationRequestDto
            {
                Id = TransactionModel.OperationId,
                Category = categoryRequestDto
            };

            TransactionRequestDto = new TransactionRequestDto()
            {
                Value = TransactionModel.Value,
                Observation = TransactionModel.Observation,
                Consolidated = (TransactionModel.Consolidated == SituationType.Sim),
                Installment = TransactionModel.Installment,
                TotalInstallments = TransactionModel.TotalInstallments,
                Account = accountRequestDto,
                Operation = operationRequestDto
            };

            PageParams = new PageParams()
            {
                PageNumber = 1,
                PageSize = 3
            };
        }

        private CategoryModel GenerateCategory(CategoryType type, string name, int id)
        {
            return new CategoryModel()
            {
                Id = id,
                Nome = name,
                Tipo = type,
                Status = StatusType.Ativo,
            };
        }

        private AccountModel GenerateAccount(int id, string name)
        {
            var category = GenerateCategory(CategoryType.Conta, "Corrente", 1);

            AccountModel _parentAccountModel = new AccountModel()
            {
                Id = 1,
                Name = "Geral",
                Status = StatusType.Ativo,
                CategoryId = category.Id,
                Category = category
            };

            return new AccountModel()
            {
                Id = id,
                Name = name,
                Status = StatusType.Ativo,
                CategoryId = category.Id,
                Category = category,
                ParentAccountId = _parentAccountModel.Id,
                ParentAccount = _parentAccountModel
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
            };
        }
    }
}