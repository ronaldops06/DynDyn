using Api.Domain.Dtos.Operation;
using Api.Domain.Dtos.Category;
using Api.Domain.Enums;
using Domain.Helpers;

namespace Api.Integration.Test.Operation
{
    public class BaseTestOperation : BaseIntegration
    {
        protected class CategoryBase
        {
            public int CategoryId { get; set; }
            public string CategoryNome { get; set; }
            public int CategoryTipo { get; set; }
            public int CategoryStatus { get; set; }
        }

        protected class OperationBase
        {
            public int OperationId { get; set; }
            public string OperationName { get; set; }
            public bool OperationRecurrent { get; set; }
            public int OperationType { get; set; }
            public int OperationStatus { get; set; }
            public CategoryBase OperationCategory { get; set; }
        }

        protected OperationRequestDto OperationRequestDto;
        protected CategoryRequestDto CategoryRequestDto;
        protected OperationBase OperationBaseDto;
        protected PageParams PageParams;

        protected BaseTestOperation()
        {
            PageParams = new PageParams()
            {
                Tipo = 2,
                PageNumber = 1,
                PageSize = 3
            };

            var categoryBase = new CategoryBase
            {
                CategoryId = 1,
                CategoryNome = "Diversos",
                CategoryTipo = (int)CategoryType.Operação,
                CategoryStatus = (int)StatusType.Ativo
            };

            OperationBaseDto = new OperationBase
            {
                OperationId = 2,
                OperationName = "Compra de carro",
                OperationRecurrent = false,
                OperationType = (int)Api.Domain.Enums.OperationType.Debito,
                OperationStatus = (int)StatusType.Ativo,
                OperationCategory = categoryBase,
            };

            OperationRequestDto = new OperationRequestDto
            {
                Name = "",
                Type = 4,
                Status = 3,
                Category = null
            };
        }

        protected void GenerateRequestDto()
        {
            CategoryRequestDto = new CategoryRequestDto()
            {
                Id = OperationBaseDto.OperationCategory.CategoryId,
                Name = OperationBaseDto.OperationCategory.CategoryNome,
                Status = OperationBaseDto.OperationCategory.CategoryStatus,
                Type = OperationBaseDto.OperationCategory.CategoryTipo
            };

            OperationRequestDto = new OperationRequestDto
            {
                Id = OperationBaseDto.OperationId,
                Name = OperationBaseDto.OperationName,
                Recurrent = OperationBaseDto.OperationRecurrent,
                Type = OperationBaseDto.OperationType,
                Status = OperationBaseDto.OperationStatus,
                Category = CategoryRequestDto
            };
        }
    }
}