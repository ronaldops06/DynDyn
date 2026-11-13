                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               using System.Globalization;
using Api.Domain.Dtos.Attribute;
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
            OperationRequestDto = GenerateOperation(2, "Compra de Monitor", OperationType.Debito);

            Random random = new Random();

            TransactionRequestDto = new TransactionRequestDto()
            {
                Id = 0,
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
                Category = CategoryAccountRequestDto,
                Attributes = new List<PortfolioAttributeRequestDto>()
            };

            return new PortfolioRequestDto()
            {
                Id = id,
                Name = name,
                Status = (int)StatusType.Ativo,
                Category = CategoryAccountRequestDto,
                ParentPortfolio = ParentPortfolioAccountRequestDto,
                Attributes = new List<PortfolioAttributeRequestDto>()
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
        
        protected AttributeRequestDto GenerateAttributeListOption()
        {
            List<AttributeOptionRequestDto> options = new List<AttributeOptionRequestDto>();
            var option = new AttributeOptionRequestDto
            {
                Label = Faker.Lorem.Words(2).Last(),
                IsDefault = 0,
                Status = (int)StatusType.Ativo
            };
            options.Add(option);
            
            option = new AttributeOptionRequestDto
            {
                Label = Faker.Lorem.Words(3).Last(),
                IsDefault = 1,
                Status = (int)StatusType.Ativo
            };
            
            options.Add(option);
            
            return new AttributeRequestDto
            {
                Name = Faker.Lorem.Words(1).First(),
                Description = Faker.Lorem.Sentence(),
                Status = (int)StatusType.Ativo,
                DataType = (int)AttributeDataType.ListOptions,
                Options = options
            };
        }

        protected void AddAttributeInPortfolio(
            PortfolioRequestDto portfolioRequestDto,
            AttributeRequestDto attributeRequestDto,
            AttributeOptionRequestDto attributeOptionRequestDto)
        {
            List<PortfolioAttributeRequestDto> portfolioAttributes = new List<PortfolioAttributeRequestDto>();
            var portfolioAttribute = new PortfolioAttributeRequestDto
            {
                ActionType = (int)AttributeActionType.Informed,
                Status = (int)StatusType.Ativo,
                Attribute = attributeRequestDto,
                AttributeOption = attributeOptionRequestDto
            };
            
            portfolioAttributes.Add(portfolioAttribute);
            portfolioRequestDto.Attributes = portfolioAttributes;
        }

        protected void AddTransactionImpact(
            PortfolioRequestDto portfolioRequestDto,
            PortfolioAttributeRequestDto portfolioAttributeRequestDto,
            int? portfolioId = null)
        {

            List<TransactionImpactRequestDto> transactionImpacts = new List<TransactionImpactRequestDto>();
            var transactionImpact = new TransactionImpactRequestDto
            {
                Portfolio = portfolioRequestDto,
                PortfolioAttribute = new PortfolioAttributeRequestDto { Id = portfolioAttributeRequestDto.Id },
                ImpactType = (int)TransactionImpactType.Incidence,
                OperationType = (int)TransactionImpactOperationType.Addition,
                Value = 10
            };

            transactionImpacts.Add(transactionImpact);
            TransactionRequestDto.Impacts = transactionImpacts;
        }
    }
}
