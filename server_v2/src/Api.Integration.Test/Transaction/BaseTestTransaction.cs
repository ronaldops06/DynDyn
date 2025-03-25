using System.Globalization;
using Api.Domain.Dtos.Category;
using Api.Domain.Dtos.Operation;
using Api.Domain.Dtos.Portfolio;
using Api.Domain.Dtos.Transaction;
using Api.Domain.Enums;
using Domain.Helpers;

namespace Api.Integration.Test.Transaction
{
    public class BaseTestTransaction : BaseIntegration
    {
        public class TransactionRequestBaseDto
        {
            public double? Value;
            public string Observation;
            public bool? Consolidated;
            public int? Installment;
            public int? TotalInstallments;
            public TransactionRequestDto ParentTransaction;
            public PortfolioRequestDto PortfolioAccount;
            public PortfolioRequestDto DestinationPortfolioAccount;
            public OperationRequestDto Operation;
        }

        protected TransactionRequestBaseDto TransactionRequestEmptyDto;
        protected CategoryRequestDto CategoryAccountRequestDto;
        protected CategoryRequestDto CategoryOperationRequestDto;
        protected OperationRequestDto OperationRequestDto;
        protected PortfolioRequestDto ParentPortfolioAccountRequestDto;
        protected PortfolioRequestDto PortfolioAccountRequestDto;
        protected TransactionRequestDto TransactionRequestDto;
        protected PageParams PageParams;

        protected BaseTestTransaction()
        {
            PageParams = new PageParams()
            {
                DataCriacaoInicio = DateTime.ParseExact("2024-09-01 23:59:59", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                DataCriacaoFim = DateTime.ParseExact("2024-09-30 23:59:59", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                PageNumber = 1,
                PageSize = 3
            };

            TransactionRequestDto = new TransactionRequestDto();
        }

        protected void GenerateRequestDto()
        {
            PortfolioAccountRequestDto = GenerateAccount(2, "Cach");
            OperationRequestDto = GenerateOperation(1, "Compra de Monitor", OperationType.Debito);

            Random random = new Random();

            TransactionRequestDto = new TransactionRequestDto()
            {
                Id = 1,
                Value = random.Next(5000),
                Observation = "Pago via pix",
                Consolidated = false,
                Installment = null,
                TotalInstallments = null,
                Portfolio = PortfolioAccountRequestDto,
                Operation = OperationRequestDto,
                DataCriacao = DateTime.ParseExact("2024-09-02 23:59:59", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            };
        }

        private CategoryRequestDto GenerateCategory(CategoryType type, string name, int id)
        {
            return new CategoryRequestDto()
            {
                Id = id,
                Name = name,
                Type = (int)type,
                Status = (int)StatusType.Ativo,
            };
        }

        private PortfolioRequestDto GenerateAccount(int id, string name)
        {
            CategoryAccountRequestDto = GenerateCategory(CategoryType.Conta, "Corrente", 2);

            ParentPortfolioAccountRequestDto = new PortfolioRequestDto()
            {
                Id = 1,
                Name = "Geral",
                Status = (int)StatusType.Ativo,
                Category = CategoryAccountRequestDto
            };

            return new PortfolioRequestDto()
            {
                Id = id,
                Name = name,
                Status = (int)StatusType.Ativo,
                Category = CategoryAccountRequestDto,
                ParentPortfolio = ParentPortfolioAccountRequestDto
            };
        }

        private OperationRequestDto GenerateOperation(int id, string name, OperationType type)
        {
            CategoryOperationRequestDto = GenerateCategory(CategoryType.Operação, "Eletrônicos", 3);

            return new OperationRequestDto()
            {
                Id = id,
                Name = name,
                Recurrent = false,
                Type = (int)type,
                Status = (int)StatusType.Ativo,
                Category = CategoryOperationRequestDto,
            };
        }
    }
}